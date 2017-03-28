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
 DECLARE @ErrorStep  varchar(200)

 DECLARE @INSERTED_IDS TABLE ([ID] INT);
 
 DECLARE @TempProject TABLE (
	AUTHOR NVARCHAR(250),
	BUILDING_NAME NVARCHAR(250),
	CLIENT NVARCHAR(250),
	[FILENAME] NVARCHAR(250),
	[NAME] NVARCHAR(250),
	[NUMBER] NVARCHAR(250),
	ORGANIZATION_DESCRIPTION NVARCHAR(250),
	ORGANIZATION_NAME NVARCHAR(250),
	[STATUS] NVARCHAR(250)
 );
 
 DECLARE @TempIdentifier TABLE (
	[identifier] NVARCHAR(250),
	[system] NVARCHAR(150)
 );

 DECLARE @TempLocation TABLE (
	[type] NVARCHAR(100),
	[address_line_1] NVARCHAR(250),
	[address_line_2] NVARCHAR(250),
	[city] NVARCHAR(250),
	[state] NVARCHAR(250),
	[zip] NVARCHAR(150),
	[country] NVARCHAR(150)
 );

 DECLARE @TempSession TABLE (
	[type] NVARCHAR(100),
	[platform] NVARCHAR(150),
	[application_build] NVARCHAR(150),
	[application_name] NVARCHAR(150),
	[application_plugin_build] NVARCHAR(150),
	[application_plugin_source] NVARCHAR(150),
	[application_version] NVARCHAR(150),
	[application_type] NVARCHAR(100)
 );

/******************************
*  Initialize Variables
*******************************/
 SELECT P_Return_ErrorCode = @@ERROR
  
 BEGIN TRY
	BEGIN TRAN
		
		-- PROJECT DATA
		INSERT @TempProject
		SELECT
			M.value('(author)[1]', 'NVARCHAR(250)'),
			M.value('(building-name)[1]', 'NVARCHAR(250)'),
			M.value('(client)[1]', 'NVARCHAR(250)'),
			M.value('(filename)[1]', 'NVARCHAR(250)'),
			M.value('(name)[1]', 'NVARCHAR(250)'),
			M.value('(number)[1]', 'NVARCHAR(250)'),
			M.value('(organization-description)[1]', 'NVARCHAR(250)'),
			M.value('(organization-name)[1]', 'NVARCHAR(250)'),
			M.value('(status)[1]', 'NVARCHAR(250)')
		FROM @P_XML.nodes('/project') M(M);

		BEGIN
			INSERT INTO gpd_project
			OUTPUT INSERTED.project_id INTO @INSERTED_IDS
			SELECT			
				-1,
				AUTHOR,
				BUILDING_NAME,
				CLIENT,
				[FILENAME],
				[NAME],
				[NUMBER],
				ORGANIZATION_DESCRIPTION,
				ORGANIZATION_NAME,
				[STATUS],
				null, getdate(), null
			FROM
				@TempProject

			SELECT TOP(1) @P_Return_ProjectId = [ID] FROM @INSERTED_IDS;
		END

		-- INDENTIFIER DATA
		INSERT @TempIdentifier
		SELECT
			M.value('(identifier)[1]', 'NVARCHAR(250)'),
			M.value('(system-name)[1]', 'NVARCHAR(150)')
		FROM @P_XML.nodes('/project/identifiers') M(M);

		BEGIN
			INSERT INTO gpd_project_identifier
			SELECT			
				@P_Return_ProjectId, identifier, [system], getdate(), null
			FROM
				 @TempIdentifier
		END

		-- LOCATION DATA
		INSERT @TempLocation
		SELECT
			M.value('(type)[1]', 'NVARCHAR(100)'),
			M.value('(address1)[1]', 'NVARCHAR(250)'),
			M.value('(address2)[1]', 'NVARCHAR(250)'),
			M.value('(city)[1]', 'NVARCHAR(250)'),
			M.value('(state)[1]', 'NVARCHAR(250)'),
			M.value('(zip)[1]', 'NVARCHAR(150)'),
			M.value('(country)[1]', 'NVARCHAR(150)')
		FROM @P_XML.nodes('/project/location') M(M);

		BEGIN
			INSERT INTO gpd_project_location
			SELECT			
				@P_Return_ProjectId, 
				ISNULL([type], 'N/A'), 
				[address_line_1], [address_line_2], [city], [state], [zip], [country], getdate(), null
			FROM
				 @TempLocation
		END
		
		-- SESSION DATA
		INSERT @TempSession
		SELECT
			M.value('(type)[1]', 'NVARCHAR(100)'),
			M.value('(platform)[1]', 'NVARCHAR(150)'),
			M.value('(application/build)[1]', 'NVARCHAR(150)'),
			M.value('(application/name)[1]', 'NVARCHAR(150)'),
			M.value('(application/plugin-build)[1]', 'NVARCHAR(150)'),
			M.value('(application/plugin-source)[1]', 'NVARCHAR(150)'),
			M.value('(application/version)[1]', 'NVARCHAR(150)'),
			M.value('(application/type)[1]', 'NVARCHAR(100)')
		FROM @P_XML.nodes('/project/session') M(M);

		BEGIN
			INSERT INTO gpd_project_session
			SELECT			
				@P_Return_ProjectId, [type], [platform], [application_build], [application_name], [application_plugin_build],
				[application_plugin_source], [application_version], [application_type], getdate(), null
			FROM
				 @TempSession
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