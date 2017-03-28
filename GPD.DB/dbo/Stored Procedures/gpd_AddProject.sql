CREATE PROCEDURE [dbo].[gpd_AddProject]
	@P_XML XML,
	@P_SOURCE_CLIENT NVARCHAR (30),	
	@P_Return_ErrorCode INT OUT,
	@P_Return_Message VARCHAR(1024) = '' OUT
AS 
BEGIN
 SET NOCOUNT ON;

/******************************
*  Variable Declarations
*******************************/   
 DECLARE @V_ProjectId INT;
 DECLARE @V_Inserted_Ids TABLE ([ID] INT);
 DECLARE @V_I int = 0;
 DECLARE @V_COUNT int = 0; -- length of query results
 DECLARE @V_Item_Id int = 0;

 DECLARE @TempItem TABLE (
	[id] INT IDENTITY(1, 1),
	[type] NVARCHAR(100),
	[currency] NVARCHAR(100),
	[family] NVARCHAR(250),
	[product_id] INT,
	[product_image_url] NVARCHAR(500),
	[product_manufacturer] NVARCHAR(250),
	[product_model] NVARCHAR(250),
	[product_name] NVARCHAR(250),
	[product_url] NVARCHAR(500)
 );

/******************************
*  Initialize Variables
*******************************/
 SELECT @P_Return_ErrorCode = @@ERROR;
 SELECT @V_ProjectId = -1; 
  
 BEGIN TRY
	BEGIN TRAN
		
		-- PROJECT DATA
		INSERT INTO gpd_project
		OUTPUT INSERTED.project_id INTO @V_Inserted_Ids
		SELECT			
			-1,
			M.value('(author)[1]', 'NVARCHAR(250)'),
			M.value('(building-name)[1]', 'NVARCHAR(250)'),
			M.value('(client)[1]', 'NVARCHAR(250)'),
			M.value('(filename)[1]', 'NVARCHAR(250)'),
			M.value('(name)[1]', 'NVARCHAR(250)'),
			M.value('(number)[1]', 'NVARCHAR(250)'),
			M.value('(organization-description)[1]', 'NVARCHAR(250)'),
			M.value('(organization-name)[1]', 'NVARCHAR(250)'),
			M.value('(status)[1]', 'NVARCHAR(250)'),
			null, getdate(), null
		FROM @P_XML.nodes('/project') M(M);

		-- GET PROJECT ID
		SELECT TOP(1) @V_ProjectId = [ID] FROM @V_Inserted_Ids;

		-- INDENTIFIER DATA
		INSERT INTO gpd_project_identifier
		SELECT			
			@V_ProjectId,
			M.value('(identifier)[1]', 'NVARCHAR(250)'),
			M.value('(system-name)[1]', 'NVARCHAR(150)'), 
			getdate(), null
		FROM @P_XML.nodes('/project/identifiers') M(M);

		-- LOCATION DATA
		INSERT gpd_project_location
		SELECT
			@V_ProjectId,
			ISNULL(M.value('(type)[1]', 'NVARCHAR(100)'), 'N/A'),
			M.value('(address1)[1]', 'NVARCHAR(250)'),
			M.value('(address2)[1]', 'NVARCHAR(250)'),
			M.value('(city)[1]', 'NVARCHAR(250)'),
			M.value('(state)[1]', 'NVARCHAR(250)'),
			M.value('(zip)[1]', 'NVARCHAR(150)'),
			M.value('(country)[1]', 'NVARCHAR(150)'),
			getdate(), null
		FROM @P_XML.nodes('/project/location') M(M);
		
		-- SESSION DATA
		INSERT INTO gpd_project_session
		SELECT
			@V_ProjectId,
			M.value('(type)[1]', 'NVARCHAR(100)'),
			M.value('(platform)[1]', 'NVARCHAR(150)'),
			M.value('(application/build)[1]', 'NVARCHAR(150)'),
			M.value('(application/name)[1]', 'NVARCHAR(150)'),
			M.value('(application/plugin-build)[1]', 'NVARCHAR(150)'),
			M.value('(application/plugin-source)[1]', 'NVARCHAR(150)'),
			M.value('(application/version)[1]', 'NVARCHAR(150)'),
			M.value('(application/type)[1]', 'NVARCHAR(100)'),
			getdate(), null
		FROM @P_XML.nodes('/project/session') M(M);

		-- ITEM DATA
		INSERT @TempItem
		SELECT
			M.value('(type)[1]', 'NVARCHAR(100)'),
			M.value('(currency)[1]', 'NVARCHAR(100)'),
			M.value('(family)[1]', 'NVARCHAR(250)'),
			M.value('(product/id)[1]', 'INT'),
			M.value('(product/image-url)[1]', 'NVARCHAR(500)'),
			M.value('(product/manufacturer)[1]', 'NVARCHAR(250)'),
			M.value('(product/model)[1]', 'NVARCHAR(250)'),
			M.value('(product/name)[1]', 'NVARCHAR(250)'),
			M.value('(product/url)[1]', 'NVARCHAR(500)')
		FROM @P_XML.nodes('/project/items/item') M(M);

		-- NUMBER OF "ITEMS" 
		SELECT @V_COUNT = COUNT(*) FROM @TempItem;

		WHILE (@V_I < @V_COUNT)
			BEGIN
				DELETE FROM @V_Inserted_Ids;

				INSERT INTO gpd_project_item
				OUTPUT INSERTED.item_id INTO @V_Inserted_Ids
				SELECT			
					@V_ProjectId, [type], [currency], [family], 
					[product_id], [product_image_url], [product_manufacturer], [product_model], [product_name], [product_url],
					null, getdate(), null
				FROM @TempItem
				WHERE [id] = (@V_I + 1) -- row number is 1-based

				-- GET "ITEM" Id
				SELECT TOP(1) @V_Item_Id = [ID] FROM @V_Inserted_Ids;

				-- MATERIAL DATA
				INSERT gpd_materials
				SELECT
					M.value('(id)[1]', 'INT'),
					@V_Item_Id,
					M.value('(product/manufacturer)[1]', 'NVARCHAR(250)'),
					M.value('(product/model)[1]', 'NVARCHAR(250)'),
					M.value('(product/name)[1]', 'NVARCHAR(250)'),
					M.value('(type/name)[1]', 'NVARCHAR(250)'),
					null, getdate(), null
				FROM @P_XML.nodes('/project/items/item[sql:variable("@V_I") + 1]/materials/material') M(M)

				-- LOOP Index
				SET @V_I = @V_I + 1;
			END

	COMMIT TRAN
    SELECT @P_Return_ErrorCode = 0, @P_Return_Message = 'no errors'

 END TRY

 BEGIN CATCH
	IF @@TRANCOUNT > 0 ROLLBACK

	SELECT @P_Return_ErrorCode = ERROR_NUMBER(), 
		@P_Return_Message = cast(ERROR_NUMBER() as varchar(20)) + ' line: '
			+ cast(ERROR_LINE() as varchar(20)) + ' ' 
			+ ERROR_MESSAGE() + ' > ' 
			+ ERROR_PROCEDURE()	

 END CATCH

 RETURN @V_ProjectId

END;