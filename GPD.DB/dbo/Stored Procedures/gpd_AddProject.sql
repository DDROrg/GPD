CREATE PROCEDURE [dbo].[gpd_AddProject]
	@P_XML XML,
	@P_SOURCE_CLIENT NVARCHAR (30) = 'N/A',	
	@P_Return_ErrorCode INT OUT,
	@P_Return_Message VARCHAR(1024) = '' OUT
AS 
BEGIN
 SET NOCOUNT ON;
 	/******************************
	*  Variable Declarations
	*******************************/   
	DECLARE @V_ProjectId UNIQUEIDENTIFIER;
	DECLARE @TempCategories TABLE (
		ID UNIQUEIDENTIFIER,
		TAXONOMY NVARCHAR(150),
		TITLE NVARCHAR(250),
		IS_NEW BIT
	);
	
	/******************************
	*  Initialize Variables
	*******************************/
	SET @P_Return_ErrorCode = @@ERROR;

	WITH XMLNAMESPACES(DEFAULT 'http://www.gpd.com',
		'http://www.w3.org/2001/XMLSchema-instance' AS i)
	SELECT @V_ProjectId = m.value('(id)[1]', 'UNIQUEIDENTIFIER')
		FROM   @P_XML.nodes('/project') M(m);

	BEGIN TRY
		BEGIN TRAN;
			-- PROJECT DATA
			WITH XMLNAMESPACES(DEFAULT 'http://www.gpd.com',
				'http://www.w3.org/2001/XMLSchema-instance' AS i)
			INSERT INTO gpd_project 
					(project_id, 
					 source_client, 
					 author, 
					 building_name, 
					 client, 
					 [filename], 
					 name, 
					 number, 
					 organization_description, 
					 organization_name, 
					 [status], 
					 xml_project_metadata, 
					 create_date, 
					 update_date) 
			SELECT @V_ProjectId, 
					@P_SOURCE_CLIENT,
					m.value('(author)[1]', 'NVARCHAR(250)'), 
					m.value('(building-name)[1]', 'NVARCHAR(250)'), 
					m.value('(client)[1]', 'NVARCHAR(250)'), 
					m.value('(filename)[1]', 'NVARCHAR(250)'), 
					m.value('(name)[1]', 'NVARCHAR(250)'), 
					m.value('(number)[1]', 'NVARCHAR(250)'), 
					m.value('(organization-description)[1]', 'NVARCHAR(250)'), 
					m.value('(organization-name)[1]', 'NVARCHAR(250)'), 
					m.value('(status)[1]', 'NVARCHAR(250)'), 
					NULL, 
					Getdate(), 
					NULL 
			FROM   @P_XML.nodes('/project') M(m);

			-- INDENTIFIER DATA
			WITH XMLNAMESPACES(DEFAULT 'http://www.gpd.com',
				'http://www.w3.org/2001/XMLSchema-instance' AS i)
			INSERT INTO gpd_project_identifier
				(project_id,
				identifier,
				[system],
				create_date,
				update_date)
			SELECT			
				@V_ProjectId,
				M.value('(identifier)[1]', 'NVARCHAR(50)'),
				M.value('(system-name)[1]', 'NVARCHAR(150)'), 
				getdate(), null
			FROM @P_XML.nodes('/project/identifiers') M(M);		

			-- LOCATION DATA
			WITH XMLNAMESPACES(DEFAULT 'http://www.gpd.com',
				'http://www.w3.org/2001/XMLSchema-instance' AS i)
			INSERT gpd_project_location(
				project_id,
				[type],
				address_line_1,
				address_line_2,
				city,
				[state],
				zip,
				country,
				create_date,
				update_date)
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
			WITH XMLNAMESPACES(DEFAULT 'http://www.gpd.com',
				'http://www.w3.org/2001/XMLSchema-instance' AS i)
			INSERT INTO gpd_project_session(
				project_id,
				[type],
				[platform],				
				application_build,
				application_client_ip,
				application_name,
				application_plugin_build,
				application_plugin_source,
				application_type,
				application_username,
				application_version,				
				create_date,
				update_date)
			SELECT
				@V_ProjectId,
				M.value('(type)[1]', 'NVARCHAR(100)'),
				M.value('(platform)[1]', 'NVARCHAR(150)'),
				M.value('(application/build)[1]', 'NVARCHAR(150)'),
				M.value('(application/client-ip)[1]', 'NVARCHAR(50)'),
				M.value('(application/name)[1]', 'NVARCHAR(150)'),
				M.value('(application/plugin-build)[1]', 'NVARCHAR(150)'),
				M.value('(application/plugin-source)[1]', 'NVARCHAR(150)'),
				M.value('(application/type)[1]', 'NVARCHAR(100)'),
				M.value('(application/username)[1]', 'NVARCHAR(250)'),
				M.value('(application/version)[1]', 'NVARCHAR(150)'),
				getdate(), null
			FROM @P_XML.nodes('/project/session') M(M);
			
			--Item
			WITH XMLNAMESPACES(DEFAULT 'http://www.gpd.com',
				'http://www.w3.org/2001/XMLSchema-instance' AS i)
			INSERT INTO gpd_project_item(
				project_item_id,
				project_id,
				item_id,
				[type],
				currency,
				family,
				quantity,
				quantity_unit,
				product_id,
				product_image_url,
				product_manufacturer,
				product_model,
				product_name,
				product_url,
				xml_item_metadata,
				create_date,
				update_date)
			SELECT			
				M.value('(./@guid)[1]', 'UNIQUEIDENTIFIER'),
				@V_ProjectId, 
				M.value('(id)[1]', 'INT'),
				M.value('(type)[1]', 'NVARCHAR(100)'),
				M.value('(currency)[1]', 'NVARCHAR(100)'),
				M.value('(family)[1]', 'NVARCHAR(250)'),
				M.value('(quantity)[1]', 'NVARCHAR(50)'),
				M.value('(quantity-unit)[1]', 'NVARCHAR(50)'),
				M.value('(product/id)[1]', 'INT'),
				M.value('(product/image-url)[1]', 'NVARCHAR(500)'),
				M.value('(product/manufacturer)[1]', 'NVARCHAR(250)'),
				M.value('(product/model)[1]', 'NVARCHAR(250)'),
				M.value('(product/name)[1]', 'NVARCHAR(250)'),
				M.value('(product/url)[1]', 'NVARCHAR(500)'),
				null, getdate(), null
			FROM @P_XML.nodes('/project/items/item') M(M);

			-- MATERIAL DATA
			WITH XMLNAMESPACES(DEFAULT 'http://www.gpd.com',
				'http://www.w3.org/2001/XMLSchema-instance' AS i)
			INSERT INTO gpd_project_item_material(
				project_item_id,
				material_id,
				product_manufacturer,
				product_model,
				product_name,
				[type_name],
				xml_material_metadata,
				create_date,
				update_date)
			SELECT DISTINCT
				M.value('(../../@guid)[1]', 'UNIQUEIDENTIFIER'),
				M.value('(id)[1]', 'INT'),
				M.value('(product/manufacturer)[1]', 'NVARCHAR(250)'),
				M.value('(product/model)[1]', 'NVARCHAR(250)'),
				M.value('(product/name)[1]', 'NVARCHAR(250)'),
				M.value('(type/name)[1]', 'NVARCHAR(250)'),
				null, getdate(), null
			FROM @P_XML.nodes('/project/items/item/materials/material') M(M);

			-- CATEGORIES DATA
			WITH XMLNAMESPACES(DEFAULT 'http://www.gpd.com',
				'http://www.w3.org/2001/XMLSchema-instance' AS i)
			INSERT INTO @TempCategories
			SELECT DISTINCT	
					m.value('(@guid)[1]', 'UNIQUEIDENTIFIER'),
					m.value('(taxonomy)[1]', 'NVARCHAR(150)'), 
					m.value('(title)[1]', 'NVARCHAR(250)'),
					1
			FROM   @P_XML.nodes('/project/items/item/categories/category') M(m);
			
			UPDATE @TempCategories
			SET ID = C.CATEGORY_ID, IS_NEW = 0
			FROM @TempCategories TEMP
			INNER JOIN GPD_CATEGORY C 
				ON TEMP.TAXONOMY = C.TAXONOMY
				AND TEMP.TITLE = C.TITLE;

			INSERT INTO gpd_category
			   (category_id,
			   taxonomy,
			   title,
			   active,
			   xml_taxonomy_metadata,
			   create_date,
			   update_date)
			SELECT ID, TAXONOMY, TITLE, 1, NULL, GETDATE(), NULL
			FROM @TempCategories
			WHERE IS_NEW = 1;

			-- ITEM-CATEGORIES XREF DATA
			WITH XMLNAMESPACES(DEFAULT 'http://www.gpd.com',
				'http://www.w3.org/2001/XMLSchema-instance' AS i)
			INSERT INTO gpd_item_category_xref
			   (project_item_id,
			   category_id)
			SELECT DISTINCT I.PROJECT_ITEM_ID, C.ID
			FROM @TempCategories C
			JOIN (
				SELECT 	
					m.value('(../../@guid)[1]', 'NVARCHAR(150)') AS PROJECT_ITEM_ID, 
					m.value('(taxonomy)[1]', 'NVARCHAR(150)') AS TAXONOMY, 
					m.value('(title)[1]', 'NVARCHAR(250)') AS TITLE
				FROM   @P_XML.nodes('/project/items/item/categories/category') M(m)) I
			ON C.TAXONOMY = I.TAXONOMY
				AND C.TITLE = I.TITLE;

	COMMIT TRAN;
    SELECT @P_Return_ErrorCode = 0, @P_Return_Message = '';
 END TRY

 BEGIN CATCH
	IF @@TRANCOUNT > 0 ROLLBACK;
	SELECT @P_Return_ErrorCode = ERROR_NUMBER(), 
		@P_Return_Message = cast(ERROR_NUMBER() as varchar(20)) + ' line: '
			+ CAST(ERROR_LINE() as varchar(20)) + ' ' 
			+ ERROR_MESSAGE() + ' > ' 
			+ ERROR_PROCEDURE();
 END CATCH
END;