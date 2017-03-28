CREATE PROCEDURE [dbo].[gpd_AddProject]
	@P_XML XML,
	@P_SOURCE_CLIENT NVARCHAR (30),
	@P_Return_ProjectId int = -1 OUT,
	@P_Return_ErrorCode int OUT,
	@P_Return_Message VARCHAR(1024) = ''  OUT
AS 
BEGIN
 SET NOCOUNT ON;

/******************************
*  Variable Declarations
*******************************/  
 DECLARE @ErrorStep  varchar(200);
 DECLARE @INSERTED_IDS TABLE ([ID] INT);

 DECLARE @i int = 0;
 DECLARE @count int = 0; -- length of query results
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
 SELECT P_Return_ErrorCode = @@ERROR
  
 BEGIN TRY
	BEGIN TRAN
		
		-- PROJECT DATA
		INSERT INTO gpd_project
		OUTPUT INSERTED.project_id INTO @INSERTED_IDS
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
		SELECT TOP(1) @P_Return_ProjectId = [ID] FROM @INSERTED_IDS;

		-- INDENTIFIER DATA
		INSERT INTO gpd_project_identifier
		SELECT			
			@P_Return_ProjectId,
			M.value('(identifier)[1]', 'NVARCHAR(250)'),
			M.value('(system-name)[1]', 'NVARCHAR(150)'), 
			getdate(), null
		FROM @P_XML.nodes('/project/identifiers') M(M);

		-- LOCATION DATA
		INSERT gpd_project_location
		SELECT
			@P_Return_ProjectId,
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
			@P_Return_ProjectId,
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

		select @count = count(*) from @TempItem;

		WHILE (@i < @count)
			BEGIN
				DELETE FROM @INSERTED_IDS;

				INSERT INTO gpd_project_item
				OUTPUT INSERTED.item_id INTO @INSERTED_IDS
				SELECT			
					@P_Return_ProjectId, [type], [currency], [family], 
					[product_id], [product_image_url], [product_manufacturer], [product_model], [product_name], [product_url],
					null, getdate(), null
				FROM @TempItem
				WHERE [id] = (@i + 1) -- row number is 1-based

				SELECT TOP(1) @V_Item_Id = [ID] FROM @INSERTED_IDS;

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
				FROM @P_XML.nodes('/project/items/item[sql:variable("@i") + 1]/materials/material') M(M)

				SET @i = @i + 1;
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

 RETURN @P_Return_ErrorCode

END;