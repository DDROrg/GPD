using System.Text;

namespace GPD.Facade.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class S001 : DbMigration
    {
        public override void Up()
        {
            StringBuilder sb = new StringBuilder("");
            #region Drop SP
            sb.AppendLine(@"
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gpd_GetProjectsListPaginated]') AND TYPE IN (N'P'))
DROP PROCEDURE [dbo].[gpd_GetProjectsListPaginated];
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gpd_GetProjectsListBySearchKeyword]') AND TYPE IN (N'P'))
DROP PROCEDURE [dbo].[gpd_GetProjectsListBySearchKeyword];
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gpd_GetProjectItems]') AND TYPE IN (N'P'))
DROP PROCEDURE [dbo].[gpd_GetProjectItems];
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gpd_GetProjectById]') AND TYPE IN (N'P'))
DROP PROCEDURE [dbo].[gpd_GetProjectById];
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gpd_AddProject]') AND TYPE IN (N'P'))
DROP PROCEDURE [dbo].[gpd_AddProject];
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gpd_AddUserDetails]') AND TYPE IN (N'P'))
DROP PROCEDURE [dbo].[gpd_AddUserDetails];
GO
");
            #endregion

            #region Drop TABLE
            sb.AppendLine(@"
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gpd_item_category_xref]') AND TYPE IN (N'U'))
DROP TABLE [dbo].[gpd_item_category_xref];
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gpd_partner_user_group_xref]') AND TYPE IN (N'U'))
DROP TABLE [dbo].[gpd_partner_user_group_xref];
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gpd_project_user_xref]') AND TYPE IN (N'U'))
DROP TABLE [dbo].[gpd_project_user_xref];
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gpd_project_item_material]') AND TYPE IN (N'U'))
DROP TABLE [dbo].[gpd_project_item_material];
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gpd_project_item]') AND TYPE IN (N'U'))
DROP TABLE [dbo].[gpd_project_item];
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gpd_category]') AND TYPE IN (N'U'))
DROP TABLE [dbo].[gpd_category];
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gpd_project_identifier]') AND TYPE IN (N'U'))
DROP TABLE [dbo].[gpd_project_identifier];
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gpd_project_location]') AND TYPE IN (N'U'))
DROP TABLE [dbo].[gpd_project_location];
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gpd_project_session]') AND TYPE IN (N'U'))
DROP TABLE [dbo].[gpd_project_session];
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gpd_project]') AND TYPE IN (N'U'))
DROP TABLE [dbo].[gpd_project];
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gpd_partner_details]') AND TYPE IN (N'U'))
DROP TABLE [dbo].[gpd_partner_details];
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gpd_user_details]') AND TYPE IN (N'U'))
DROP TABLE [dbo].[gpd_user_details];
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gpd_user_group]') AND TYPE IN (N'U'))
DROP TABLE [dbo].[gpd_user_group];
GO
");
            #endregion

            #region Create TABLE
            sb.AppendLine(@"
CREATE TABLE [dbo].[gpd_user_group] (
    [group_id]           INT            NOT NULL,
    [name]               NVARCHAR (30)  NOT NULL,
    [description]        NVARCHAR (200) NOT NULL,
    [active]             BIT            NOT NULL,
    [xml_group_metadata] XML            NULL,
    [create_date]        DATETIME       NOT NULL,
    [update_date]        DATETIME       NULL,
    CONSTRAINT [PK_gpd_user_group] PRIMARY KEY CLUSTERED ([group_id] ASC)
);
GO

CREATE TABLE [dbo].[gpd_user_details] (
    [user_id]           INT            NOT NULL IDENTITY(100, 1),
    [last_name]         NVARCHAR (150) NULL,
    [first_name]        NVARCHAR (150) NULL,
    [full_name]         NVARCHAR (150) NULL,
    [email]             NVARCHAR (150) NOT NULL,
    [password]          NVARCHAR (300) NULL,
    [company]           NVARCHAR (150) NULL,
    [job_title]         NVARCHAR (150) NULL,
    [business_phone]    NVARCHAR (50)  NULL,
    [home_phone]        NVARCHAR (50)  NULL,
    [mobile_phone]      NVARCHAR (50)  NULL,
    [fax_number]        NVARCHAR (50)  NULL,
    [address_line_1]    NVARCHAR (150) NULL,
    [address_line_2]    NVARCHAR (150) NULL,
    [city]              NVARCHAR (150) NULL,
    [state_province]    NVARCHAR (50)  NULL,
    [zip_postal_code]   NVARCHAR (50)  NULL,
    [country]           NVARCHAR (50)  NULL,
    [ip_address]        NVARCHAR (100) NULL,
    [active]            BIT            NOT NULL,
    [xml_user_metadata] XML            NULL,
    [create_date]       DATETIME       NOT NULL,
    [update_date]       DATETIME       NULL,
    CONSTRAINT [PK_gpd_user_details] PRIMARY KEY CLUSTERED ([user_id] ASC),
    CONSTRAINT [IX_gpd_user_details] UNIQUE NONCLUSTERED ([email] ASC)
);
GO

CREATE TABLE [dbo].[gpd_partner_details] (
    [partner_id]           UNIQUEIDENTIFIER NOT NULL,
    [name]                 NVARCHAR (30)    NOT NULL,
    [site_url]             NVARCHAR (300)   NOT NULL,
    [short_description]    NVARCHAR (150)   NULL,
    [description]          NVARCHAR (1000)  NOT NULL,
    [active]               BIT              NOT NULL,
    [xml_partner_metadata] XML              NULL,
    [create_date]          DATETIME         NOT NULL,
    [update_date]          DATETIME         NULL,
    CONSTRAINT [PK_gpd_partner_details] PRIMARY KEY CLUSTERED ([partner_id] ASC),
    CONSTRAINT [IX_gpd_partner_details] UNIQUE NONCLUSTERED ([name] ASC)
);
GO

CREATE TABLE [dbo].[gpd_project] (
    [project_id]               UNIQUEIDENTIFIER NOT NULL,
    [partner_name]             NVARCHAR (30)    NULL,
    [author]                   NVARCHAR (250)   NULL,
    [building_name]            NVARCHAR (250)   NULL,
    [client]                   NVARCHAR (250)   NULL,
    [filename]                 NVARCHAR (250)   NULL,
    [name]                     NVARCHAR (250)   NULL,
    [number]                   NVARCHAR (250)   NULL,
    [organization_description] NVARCHAR (250)   NULL,
    [organization_name]        NVARCHAR (250)   NULL,
    [status]                   NVARCHAR (250)   NULL,
	[active]				   BIT              NOT NULL DEFAULT 1,
    [xml_project_metadata]     XML              NULL,
    [create_date]              DATETIME         NOT NULL,
    [update_date]              DATETIME         NULL,
    CONSTRAINT [PK_gpd_project] PRIMARY KEY CLUSTERED ([project_id] ASC)
);
GO

CREATE TABLE [dbo].[gpd_project_session] (
    [project_id]                UNIQUEIDENTIFIER NULL,
    [type]                      NVARCHAR (100)   NULL,
    [platform]                  NVARCHAR (150)   NULL,
    [application_build]         NVARCHAR (150)   NULL,
    [application_client_ip]     NVARCHAR (50)    NULL,
    [application_name]          NVARCHAR (150)   NULL,
    [application_plugin_build]  NVARCHAR (150)   NULL,
    [application_plugin_source] NVARCHAR (150)   NULL,
    [application_type]          NVARCHAR (100)   NULL,
    [application_username]      NVARCHAR (250)   NULL,
    [application_version]       NVARCHAR (150)   NULL,
    [create_date]               DATETIME         NOT NULL,
    [update_date]               DATETIME         NULL,
    CONSTRAINT [FK_gpd_project_session_gpd_project] FOREIGN KEY ([project_id]) REFERENCES [dbo].[gpd_project] ([project_id])
);
GO

CREATE TABLE [dbo].[gpd_project_location] (
    [project_id]     UNIQUEIDENTIFIER         NOT NULL,
    [type]           NVARCHAR (100) NOT NULL,
    [address_line_1] NVARCHAR (250) NULL,
    [address_line_2] NVARCHAR (250) NULL,
    [city]           NVARCHAR (250) NULL,
    [state]          NVARCHAR (250) NULL,
    [zip]            NVARCHAR (150) NULL,
    [country]        NVARCHAR (150) NULL,
    [create_date]    DATETIME        NOT NULL,
    [update_date]    DATETIME        NULL,
    CONSTRAINT [FK_gpd_project_location_gpd_project] FOREIGN KEY ([project_id]) REFERENCES [dbo].[gpd_project] ([project_id])
);
GO

CREATE TABLE [dbo].[gpd_project_identifier] (
    [project_id]  UNIQUEIDENTIFIER         NOT NULL,
    [identifier]  UNIQUEIDENTIFIER NOT NULL,
    [system]      nvarchar (150) NULL,
    [create_date] DATETIME        NOT NULL,
    [update_date] DATETIME  NULL,
    CONSTRAINT [FK_gpd_project_identifier_gpd_project] FOREIGN KEY ([project_id]) REFERENCES [dbo].[gpd_project] ([project_id])
);
GO

CREATE TABLE [dbo].[gpd_category] (
    [category_id]           UNIQUEIDENTIFIER   NOT NULL,
    [taxonomy]              NVARCHAR(150) NOT NULL,
    [title]                 NVARCHAR(250) NOT NULL,
    [active]                BIT         NOT NULL,
    [xml_taxonomy_metadata] XML         NULL,
    [create_date]           DATETIME        NOT NULL,
    [update_date]           DATETIME        NULL,
    CONSTRAINT [PK_gpd_category] PRIMARY KEY CLUSTERED ([category_id] ASC)
);
GO

CREATE TABLE [dbo].[gpd_project_item] (
	[project_item_id]              UNIQUEIDENTIFIER  NOT NULL,
    [project_id]              UNIQUEIDENTIFIER  NOT NULL,
    [item_id]           INT            NOT NULL,
    [type]                 NVARCHAR (100) NULL,
    [currency]             NVARCHAR (100) NULL,
    [family]               NVARCHAR (250) NULL,
	[quantity]               NVARCHAR (50) NULL,
	[quantity_unit]               NVARCHAR (50) NULL,
    [product_id]           INT            NULL,
    [product_image_url]    NVARCHAR (500) NULL,
    [product_manufacturer] NVARCHAR (250) NULL,
    [product_model]        NVARCHAR (250) NULL,
    [product_name]         NVARCHAR (250) NULL,
    [product_url]          NVARCHAR (500) NULL,
    [xml_item_metadata]    XML            NULL,
    [create_date]          DATETIME       NOT NULL,
    [update_date]          DATETIME       NULL,
    CONSTRAINT [FK_gpd_project_item_gpd_project] FOREIGN KEY ([project_id]) REFERENCES [gpd_project]([project_id]), 
    CONSTRAINT [PK_gpd_project_item] PRIMARY KEY ([project_item_id])
);
GO

CREATE TABLE [dbo].[gpd_project_item_material] (
    [project_item_id]       UNIQUEIDENTIFIER NOT NULL,
    [material_id]           INT              NOT NULL,
    [product_manufacturer]  NVARCHAR (250)   NULL,
    [product_model]         NVARCHAR (250)   NULL,
    [product_name]          NVARCHAR (250)   NULL,
    [type_name]             NVARCHAR (250)   NULL,
    [xml_material_metadata] XML              NULL,
    [create_date]           DATETIME         NOT NULL,
    [update_date]           DATETIME         NULL,
    CONSTRAINT [FK_gpd_project_item_material_gpd_project_item] FOREIGN KEY ([project_item_id]) REFERENCES [dbo].[gpd_project_item] ([project_item_id])
);
GO

CREATE TABLE [dbo].[gpd_project_user_xref] (
    [project_id]  UNIQUEIDENTIFIER NOT NULL,
    [user_id]     INT              NOT NULL,
    [create_date] DATETIME         NOT NULL,
    [update_date] DATETIME         NULL,
    CONSTRAINT [FK_gpd_project_user_xref_gpd_project] FOREIGN KEY ([project_id]) REFERENCES [dbo].[gpd_project] ([project_id])
);
GO

CREATE TABLE [dbo].[gpd_partner_user_group_xref] (
    [partner_id]  UNIQUEIDENTIFIER NOT NULL,
    [user_id]     INT              NOT NULL,
    [group_id]    INT              NOT NULL,
    [description] NVARCHAR (200)   NULL,
    [active]      BIT              NOT NULL,
    [create_date] DATETIME         NOT NULL,
    [update_date] DATETIME         NULL,
    CONSTRAINT [PK_gpd_partner_user_group_xref] PRIMARY KEY CLUSTERED ([partner_id] ASC, [user_id] ASC, [group_id] ASC),
    CONSTRAINT [FK_gpd_partner_user_group_xref_gpd_partner_details] FOREIGN KEY ([partner_id]) REFERENCES [dbo].[gpd_partner_details] ([partner_id]),
    CONSTRAINT [FK_gpd_partner_user_group_xref_gpd_user_details] FOREIGN KEY ([user_id]) REFERENCES [dbo].[gpd_user_details] ([user_id]),
    CONSTRAINT [FK_gpd_partner_user_group_xref_gpd_user_group] FOREIGN KEY ([group_id]) REFERENCES [dbo].[gpd_user_group] ([group_id])
);
GO

CREATE TABLE [dbo].[gpd_item_category_xref] (
	[project_item_id] UNIQUEIDENTIFIER  NOT NULL,
    [category_id] UNIQUEIDENTIFIER  NOT NULL,
    [create_date] DATETIME NOT NULL DEFAULT GETDATE(),
    [update_date] DATETIME NULL,
    CONSTRAINT [PK_gpd_item_category_xref] PRIMARY KEY CLUSTERED ([project_item_id], [category_id]),
    CONSTRAINT [FK_gpd_item_category_xref_gpd_categories] FOREIGN KEY ([category_id]) REFERENCES [dbo].[gpd_category] ([category_id]),
    CONSTRAINT [FK_gpd_item_category_xref_gpd_project_item] FOREIGN KEY ([project_item_id]) REFERENCES [dbo].[gpd_project_item] ([project_item_id])
);
GO
");
            #endregion

            #region Create SP - gpd_AddProject
            sb.AppendLine(@"
CREATE PROCEDURE [dbo].[gpd_AddProject]
	@P_PartnerName nvarchar(30),
	@P_ProjectId uniqueidentifier,
	@P_XML XML,	
	@P_Return_ErrorCode INT OUT,
	@P_Return_Message VARCHAR(1024) = '' OUT
AS 
BEGIN
 SET NOCOUNT ON;
 	/******************************
	*  Variable Declarations
	*******************************/   
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

	BEGIN TRY
		BEGIN TRAN;
			-- PROJECT DATA
			WITH XMLNAMESPACES(DEFAULT 'http://www.gpd.com',
				'http://www.w3.org/2001/XMLSchema-instance' AS i)
			INSERT INTO gpd_project 
					(project_id, 
					 partner_name, 
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
			SELECT @P_ProjectId, 
					@P_PartnerName,
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
				@P_ProjectId,
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
				@P_ProjectId,
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
				@P_ProjectId,
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
				@P_ProjectId, 
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
GO
");
            #endregion

            #region Create SP - gpd_GetProjectById
            sb.AppendLine(@"
CREATE PROCEDURE [dbo].[gpd_GetProjectById]
	@P_PROJECTID UNIQUEIDENTIFIER
AS
BEGIN	
	SELECT 
		P.PROJECT_ID,
		P.partner_name,
		P.AUTHOR,
		P.BUILDING_NAME,
		P.CLIENT,
		P.[FILENAME],
		P.NAME,
		P.NUMBER,
		P.ORGANIZATION_DESCRIPTION,
		P.ORGANIZATION_NAME,
		P.[STATUS],	
		L.ADDRESS_LINE_1,
		L.CITY,
		L.[STATE],
		L.ZIP,
		S.[TYPE],
		S.[PLATFORM],
		S.APPLICATION_BUILD,
		S.APPLICATION_NAME,
		S.APPLICATION_PLUGIN_BUILD,
		S.APPLICATION_PLUGIN_SOURCE,
		S.APPLICATION_VERSION,
		S.APPLICATION_TYPE	
	FROM GPD_PROJECT P
	LEFT JOIN GPD_PROJECT_LOCATION L ON P.PROJECT_ID = L.PROJECT_ID 
	LEFT JOIN GPD_PROJECT_SESSION S ON P.PROJECT_ID = S.PROJECT_ID
	WHERE P.PROJECT_ID = @P_PROJECTID;

	SELECT I.IDENTIFIER, I.[SYSTEM]
	FROM GPD_PROJECT_IDENTIFIER I WHERE I.PROJECT_ID = @P_PROJECTID;

	SELECT 
	    I.PROJECT_ID,
		I.ITEM_ID,
		I.[TYPE],
		I.CURRENCY,
		I.FAMILY,
		I.QUANTITY,
		I.QUANTITY_UNIT,
		I.PRODUCT_ID,
		I.PRODUCT_IMAGE_URL,
		I.PRODUCT_MANUFACTURER,
		I.PRODUCT_MODEL,
		I.PRODUCT_NAME,
		I.PRODUCT_URL,
		I.product_image_url 
	FROM GPD_PROJECT_ITEM I WHERE I.PROJECT_ID = @P_PROJECTID;

	SELECT 
		I.PROJECT_ID,
		I.ITEM_ID,
		M.MATERIAL_ID,
		M.PRODUCT_MANUFACTURER,
		M.PRODUCT_MODEL,
		M.PRODUCT_NAME,
		M.[TYPE_NAME]
	FROM GPD_PROJECT_ITEM I
	INNER JOIN GPD_PROJECT_ITEM_MATERIAL M ON I.project_item_id = M.project_item_id
	WHERE I.PROJECT_ID = @P_PROJECTID;
	
	SELECT 
		I.PROJECT_ID,
		I.ITEM_ID,
		C.taxonomy,
		C.title
	FROM GPD_PROJECT_ITEM I
	INNER JOIN gpd_item_category_xref X ON I.project_item_id = X.project_item_id
	INNER JOIN gpd_category C ON X.category_id = C.category_id AND c.active = 1
	WHERE I.PROJECT_ID = @P_PROJECTID;
END;
GO
");
            #endregion

            #region Create SP - gpd_GetProjectItems
            sb.AppendLine(@"
CREATE PROCEDURE [dbo].[gpd_GetProjectItems]
	@P_PROJECT_ID uniqueidentifier
AS
BEGIN
	SELECT
		[project_item_id],
		[product_manufacturer],
		[product_model],
		[family],
		[type],
		[quantity],
		[quantity_unit]
	FROM gpd_project_item
	WHERE project_id = @P_PROJECT_ID;

	SELECT 
		M.project_item_id,
		M.product_manufacturer,
		M.product_modeL,
		M.product_name,
		M.type_name
	FROM gpd_project_item PI, gpd_project_item_material M
	WHERE PI.project_id = @P_PROJECT_ID
	AND M.project_item_id = PI.project_item_id;
END;
GO
");
            #endregion

            #region Create SP - gpd_GetProjectsListPaginated
            sb.AppendLine(@"
CREATE PROCEDURE [dbo].[gpd_GetProjectsListPaginated]
       @P_PartnerName nvarchar(30),
	   @P_StartRowIndex int,
       @P_PageSize int
AS
BEGIN
       -- SET NOCOUNT ON added to prevent extra result sets from
       -- interfering with SELECT statements. 
       SET NOCOUNT ON;

	   IF @P_PartnerName = 'ALL'
	   	   BEGIN
				-- Get the paginated records
				SELECT P.PROJECT_ID,
					P.PARTNER_NAME,
					P.AUTHOR,
					P.BUILDING_NAME,
					P.CLIENT,
					P.[FILENAME],
					P.NAME,
					P.NUMBER,
					P.ORGANIZATION_DESCRIPTION,
					P.ORGANIZATION_NAME,
					P.[STATUS],
					P.CREATE_DATE,
					L.ADDRESS_LINE_1,
					L.CITY,
					L.STATE,
					L.ZIP
				FROM GPD_PROJECT P
				LEFT OUTER JOIN GPD_PROJECT_LOCATION L ON P.PROJECT_ID = L.PROJECT_ID
				ORDER BY P.create_date DESC
				OFFSET @P_StartRowIndex ROWS FETCH NEXT @P_PageSize ROWS ONLY;
				
				-- get the total count of the records
				SELECT COUNT(project_id) AS TotalCount  FROM GPD_PROJECT;
	   	   END
	   ELSE
	   	   BEGIN
				-- Get the paginated records
				SELECT P.PROJECT_ID,
					P.PARTNER_NAME,
					P.AUTHOR,
					P.BUILDING_NAME,
					P.CLIENT,
					P.[FILENAME],
					P.NAME,
					P.NUMBER,
					P.ORGANIZATION_DESCRIPTION,
					P.ORGANIZATION_NAME,
					P.[STATUS],
					P.CREATE_DATE,
					L.ADDRESS_LINE_1,
					L.CITY,
					L.STATE,
					L.ZIP
				FROM GPD_PROJECT P
				LEFT OUTER JOIN GPD_PROJECT_LOCATION L ON P.PROJECT_ID = L.PROJECT_ID
				WHERE P.PARTNER_NAME = @P_PartnerName
				ORDER BY P.create_date DESC
				OFFSET @P_StartRowIndex ROWS FETCH NEXT @P_PageSize ROWS ONLY
				
				-- get the total count of the records
				SELECT COUNT(project_id) AS TotalCount  
				FROM GPD_PROJECT
				WHERE PARTNER_NAME = @P_PartnerName;
	   	   END
END;
GO
");
            #endregion

            #region Create SP - gpd_GetProjectsListBySearchKeyword
            sb.AppendLine(@"
CREATE PROCEDURE [dbo].[gpd_GetProjectsListBySearchKeyword]
	@P_PartnerName nvarchar(30),
	@P_SearchKeyword nvarchar(30),
	@P_StartRowIndex int,
	@P_PageSize int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements. 
	SET NOCOUNT ON;
	
	DECLARE @t_RecordsCount TABLE (project_id uniqueidentifier, partner_name nvarchar(30));
	INSERT INTO @t_RecordsCount (project_id, partner_name)
		SELECT P.project_id, P.partner_name
		FROM gpd_project P
		WHERE P.project_id IN (
			/* Project Name | Client | Status */
			SELECT project_id FROM gpd_project P WHERE P.active = 1
			AND (P.name LIKE '%' + @P_SearchKeyword +'%' OR P.client LIKE '%' + @P_SearchKeyword +'%' OR P.status LIKE '%' + @P_SearchKeyword +'%')
				
			UNION
			/* Zipcode */
			SELECT PL.project_id
			FROM gpd_project_location PL
			WHERE PL.zip LIKE '%' + @P_SearchKeyword +'%'
				
			UNION
			/* Material Name | Material Manufacturer | Material Model */
			SELECT I.project_id
			FROM gpd_project_item I
			WHERE I.project_item_id IN (
				SELECT M.project_item_id
				FROM gpd_project_item_material M
				WHERE M.product_name LIKE '%' + @P_SearchKeyword +'%'
				OR M.product_manufacturer LIKE '%' + @P_SearchKeyword +'%'
				OR M.product_model LIKE '%' + @P_SearchKeyword +'%'
			)
			/* Family */
			OR [family] LIKE '%' + @P_SearchKeyword +'%'
			/* Family Type */
			OR [type] LIKE '%' + @P_SearchKeyword +'%'
			/* Manufacturer Name */
			OR [product_manufacturer] LIKE '%' + @P_SearchKeyword +'%'
			/* Manufacturer Name | Model Number / Name */
			OR [product_model] LIKE '%' + @P_SearchKeyword +'%'
			OR [product_name] LIKE '%' + @P_SearchKeyword +'%'
		)
		AND P.active = 1

	IF @P_PartnerName = 'ALL'
		BEGIN
			-- Get the paginated records
			SELECT P.PROJECT_ID,
				P.PARTNER_NAME,
				P.AUTHOR,
				P.BUILDING_NAME,
				P.CLIENT,
				P.[FILENAME],
				P.NAME,
				P.NUMBER,
				P.ORGANIZATION_DESCRIPTION,
				P.ORGANIZATION_NAME,
				P.[STATUS],
				P.CREATE_DATE,
				L.ADDRESS_LINE_1,
				L.CITY,
				L.STATE,
				L.ZIP
			FROM GPD_PROJECT P
			INNER JOIN @t_RecordsCount x ON P.project_id = x.project_id
			LEFT OUTER JOIN GPD_PROJECT_LOCATION L ON P.PROJECT_ID = L.PROJECT_ID
			ORDER BY P.create_date DESC
			OFFSET @P_StartRowIndex ROWS FETCH NEXT @P_PageSize ROWS ONLY;

            --get the total count of the records
			SELECT count(*) FROM @t_RecordsCount;
		END
	ELSE
		BEGIN
			-- Get the paginated records
			SELECT P.PROJECT_ID,
				P.PARTNER_NAME,
				P.AUTHOR,
				P.BUILDING_NAME,
				P.CLIENT,
				P.[FILENAME],
				P.NAME,
				P.NUMBER,
				P.ORGANIZATION_DESCRIPTION,
				P.ORGANIZATION_NAME,
				P.[STATUS],
				P.CREATE_DATE,
				L.ADDRESS_LINE_1,
				L.CITY,
				L.STATE,
				L.ZIP
			FROM GPD_PROJECT P
			INNER JOIN @t_RecordsCount x ON P.project_id = x.project_id
			LEFT OUTER JOIN GPD_PROJECT_LOCATION L ON P.PROJECT_ID = L.PROJECT_ID
			WHERE x.partner_name = @P_PartnerName
			ORDER BY P.create_date DESC
			OFFSET @P_StartRowIndex ROWS FETCH NEXT @P_PageSize ROWS ONLY;	

            --get the total count of the records
			SELECT count(*) FROM @t_RecordsCount x WHERE x.partner_name = @P_PartnerName;		
		END		
END;
GO
");
            #endregion

            #region Create SP - gpd_AddUserDetails
            sb.AppendLine(@"
CREATE PROCEDURE [dbo].[gpd_AddUserDetails]
	@P_XML XML,
	@P_IpAddress VARCHAR(50),
	@P_Return_UserId INT OUT,
	@P_Return_ErrorCode INT OUT,
	@P_Return_Message VARCHAR(1024) OUT
AS 
BEGIN
	SET NOCOUNT ON;
	/******************************
	*  Variable Declarations
	*******************************/
	DECLARE @V_UserEmail VARCHAR(150);

	/******************************
	*  Initialize Variables
	*******************************/
	SET @P_Return_ErrorCode = @@ERROR;
	SET @P_Return_Message = '';
	
	BEGIN TRY
		SET @P_Return_UserId = -1;

		-- user email address
		WITH XMLNAMESPACES(DEFAULT 'http://schemas.datacontract.org/2004/07/GPD.ServiceEntities', 'http://www.w3.org/2001/XMLSchema-instance' AS i)
			SELECT  @V_UserEmail = M.value('(Email)[1]', 'NVARCHAR(150)')
			FROM @P_XML.nodes('/UserRegistrationDTO') M(M);

		IF EXISTS(SELECT 1 FROM GPD_USER_DETAILS WHERE EMAIL = @V_UserEmail)
			BEGIN
				SET @P_Return_ErrorCode = 0;
				SET @P_Return_Message = 'the provided e-mail address is already registered';
			END
		ELSE
			BEGIN
				-- USER DATA
				WITH XMLNAMESPACES(DEFAULT 'http://schemas.datacontract.org/2004/07/GPD.ServiceEntities', 'http://www.w3.org/2001/XMLSchema-instance' AS i)
				INSERT INTO GPD_USER_DETAILS
					([last_name]
					,[first_name]
					,[full_name]
					,[email]
					,[password]
 					,[company]
 					,[job_title]
 					,[business_phone]
 					,[home_phone]
 					,[mobile_phone]
 					,[fax_number]
 					,[address_line_1]
 					,[address_line_2]
 					,[city]
 					,[state_province]
 					,[zip_postal_code]
 					,[country]
 					,[ip_address]
 					,[active]
 					,[xml_user_metadata]
 					,[create_date]
 					,[update_date])
				SELECT			
					M.value('(LastName)[1]', 'NVARCHAR(150)')
					,M.value('(FirstName)[1]', 'NVARCHAR(150)')
					,M.value('(FirstName)[1]', 'NVARCHAR(150)') + ' ' + M.value('(LastName)[1]', 'NVARCHAR(150)')
					,M.value('(Email)[1]', 'NVARCHAR(150)')					
					,M.value('(Password)[1]', 'NVARCHAR(150)')
					,M.value('(Company)[1]', 'NVARCHAR(150)')
					,M.value('(JobTitle)[1]', 'NVARCHAR(150)')
					,M.value('(BusinessPhone)[1]', 'NVARCHAR(50)')
					,M.value('(HomePhone)[1]', 'NVARCHAR(50)')
					,M.value('(MobilePhone)[1]', 'NVARCHAR(50)')
					,M.value('(Fax)[1]', 'NVARCHAR(50)')
					,M.value('(AddressLine1)[1]', 'NVARCHAR(150)')
					,M.value('(AddressLine2)[1]', 'NVARCHAR(150)')
					,M.value('(City)[1]', 'NVARCHAR(150)')
					,M.value('(State)[1]', 'NVARCHAR(50)')
					,M.value('(Zip)[1]', 'NVARCHAR(50)')
					,M.value('(Country)[1]', 'NVARCHAR(50)')
					,@P_IpAddress
					,1
					,null
					,getdate(), null
				FROM @P_XML.nodes('/UserRegistrationDTO') M(M);
				SET @P_Return_UserId = SCOPE_IDENTITY()
				SET NOCOUNT OFF
			END

	END TRY
	
	BEGIN CATCH
		SELECT @P_Return_ErrorCode = ERROR_NUMBER(), 
		@P_Return_Message = cast(ERROR_NUMBER() as varchar(20)) + ' line: '
			+ CAST(ERROR_LINE() as varchar(20)) + ' ' 
			+ ERROR_MESSAGE() + ' > ' 
			+ ERROR_PROCEDURE();
	END CATCH
END;
GO
");
            #endregion

            #region Insert Script - Master Data
            sb.AppendLine(@"
INSERT INTO [dbo].[gpd_user_group] (group_id, name, description, active, xml_group_metadata, create_date, update_date) 
VALUES (100, 'USER', 'Can publish projects; can view published projects; can export project details by complete or by category', 1, NULL, getdate(), null);
INSERT INTO [dbo].[gpd_user_group] (group_id, name, description, active, xml_group_metadata, create_date, update_date) 
VALUES (101, 'FIRMS ADMINISTRATOR', 'Can create/edit firm; add/remove user to firm; can control access to firm project information', 1, NULL, getdate(), null);
INSERT INTO [dbo].[gpd_user_group] (group_id, name, description, active, xml_group_metadata, create_date, update_date) 
VALUES (102, 'CLIENT USER', 'Can view project data based on access assign by the Client_Administrator and categories linked to the Client account', 1, NULL, getdate(), null);
INSERT INTO [dbo].[gpd_user_group] (group_id, name, description, active, xml_group_metadata, create_date, update_date) 
VALUES (103, 'CLIENT ADMINISTATOR', 'Admin client information and User access to data by region, state or zip code; can export project list and individual project details to xls, pdf or csv', 1, NULL, getdate(), null);
INSERT INTO [dbo].[gpd_user_group] (group_id, name, description, active, xml_group_metadata, create_date, update_date) 
VALUES (104, 'ADMINISTRATORS', 'Site Admin Modirators - Can edit/delete all Clients, Client_admins, Firms_Admins and User information and access to specific data, Can edit project data', 1, NULL, getdate(), null);
INSERT INTO [dbo].[gpd_user_group] (group_id, name, description, active, xml_group_metadata, create_date, update_date) 
VALUES (105, 'ADMINISTRATOR', 'Site	Main Site Admins - Can edit everything', 1, NULL, getdate(), null);
GO

INSERT INTO gpd_user_details (last_name, first_name, full_name, email, password, company, job_title, 
	business_phone, home_phone, mobile_phone, fax_number, address_line_1, address_line_2, city, state_province, 
	zip_postal_code, country, ip_address, active, xml_user_metadata, create_date, update_date)
VALUES('Admin', 'Site', 'Site Admin', 'gpd@noemail.com', '1000:4b4KyKmPV7fY0+UdqZ4csi+kVhpoPTCkTresMBFR:D/cgfZDTh0xuwFJVvQd1RtTvyFl9vH+mN55+IVp919LNrOX7k0xTaQ==', 
	'GPD', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, NULL, getdate(),  NULL);
GO

INSERT INTO gpd_partner_details (partner_id, name, site_url, short_description, description, active, xml_partner_metadata, create_date, update_date)
VALUES('029882E7-FFFB-4FC3-92AE-323799525099', 'sweets', 'http://sweets.cnstruction.com', 'N/A', 'N/A', 1, NULL, getdate(), NULL);
INSERT INTO gpd_partner_details (partner_id, name, site_url, short_description, description, active, xml_partner_metadata, create_date, update_date)
VALUES('60F47B0E-6DEA-474B-9882-A93DCC6A59D1', 'TEST', 'N/A', 'N/A', 'N/A', 1, NULL, getdate(), NULL);
INSERT INTO gpd_partner_details (partner_id, name, site_url, short_description, description, active, xml_partner_metadata, create_date, update_date)
VALUES('484330C5-1B83-4D37-93EA-D2049BFD3DD5', 'ALL', 'N/A', 'N/A', 'N/A', 1, NULL, getdate(), NULL);
GO

INSERT INTO gpd_partner_user_group_xref (partner_id, user_id, group_id, description, active, create_date, update_date)
VALUES ('484330C5-1B83-4D37-93EA-D2049BFD3DD5', 100, 105, 'Site Admin', 1, getdate(), NULL);
INSERT INTO gpd_partner_user_group_xref (partner_id, user_id, group_id, description, active, create_date, update_date)
VALUES ('60F47B0E-6DEA-474B-9882-A93DCC6A59D1', 100, 105, 'Site Admin', 1, getdate(), NULL);
GO
");
            #endregion

            this.Sql(sb.ToString());
        }

        public override void Down()
        {
            StringBuilder sb = new StringBuilder("");
            #region Drop SP
            sb.AppendLine(@"
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gpd_GetProjectsListPaginated]') AND TYPE IN (N'P'))
DROP PROCEDURE [dbo].[gpd_GetProjectsListPaginated];
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gpd_GetProjectItems]') AND TYPE IN (N'P'))
DROP PROCEDURE [dbo].[gpd_GetProjectItems];
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gpd_GetProjectById]') AND TYPE IN (N'P'))
DROP PROCEDURE [dbo].[gpd_GetProjectById];
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gpd_AddProject]') AND TYPE IN (N'P'))
DROP PROCEDURE [dbo].[gpd_AddProject];
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gpd_AddUserDetails]') AND TYPE IN (N'P'))
DROP PROCEDURE [dbo].[gpd_AddUserDetails];
GO
");
            #endregion

            #region Drop TABLE
            sb.AppendLine(@"
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gpd_item_category_xref]') AND TYPE IN (N'U'))
DROP TABLE [dbo].[gpd_item_category_xref];
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gpd_partner_user_group_xref]') AND TYPE IN (N'U'))
DROP TABLE [dbo].[gpd_partner_user_group_xref];
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gpd_project_user_xref]') AND TYPE IN (N'U'))
DROP TABLE [dbo].[gpd_project_user_xref];
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gpd_project_item_material]') AND TYPE IN (N'U'))
DROP TABLE [dbo].[gpd_project_item_material];
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gpd_project_item]') AND TYPE IN (N'U'))
DROP TABLE [dbo].[gpd_project_item];
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gpd_category]') AND TYPE IN (N'U'))
DROP TABLE [dbo].[gpd_category];
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gpd_project_identifier]') AND TYPE IN (N'U'))
DROP TABLE [dbo].[gpd_project_identifier];
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gpd_project_location]') AND TYPE IN (N'U'))
DROP TABLE [dbo].[gpd_project_location];
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gpd_project_session]') AND TYPE IN (N'U'))
DROP TABLE [dbo].[gpd_project_session];
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gpd_project]') AND TYPE IN (N'U'))
DROP TABLE [dbo].[gpd_project];
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gpd_partner_details]') AND TYPE IN (N'U'))
DROP TABLE [dbo].[gpd_partner_details];
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gpd_user_details]') AND TYPE IN (N'U'))
DROP TABLE [dbo].[gpd_user_details];
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gpd_user_group]') AND TYPE IN (N'U'))
DROP TABLE [dbo].[gpd_user_group];
GO
");
            #endregion
            this.Sql(sb.ToString());
        }
    }
}
