CREATE PROCEDURE [dbo].[gpd_UpdateProject]
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
			WITH XMLNAMESPACES(DEFAULT 'http://www.gpd.com', 'http://www.w3.org/2001/XMLSchema-instance' AS i)
			UPDATE gpd_project
			SET [name] = m.value('(name)[1]', 'NVARCHAR(250)'), 
				[client] = m.value('(client)[1]', 'NVARCHAR(250)'),
				[status] = m.value('(status)[1]', 'NVARCHAR(250)'),
				organization_name = m.value('(organization-name)[1]', 'NVARCHAR(250)'),
				[author] = m.value('(author)[1]', 'NVARCHAR(250)'),
				[filename] = m.value('(filename)[1]', 'NVARCHAR(250)'),
				building_name = m.value('(building-name)[1]', 'NVARCHAR(250)'), 
				organization_description = m.value('(organization-description)[1]', 'NVARCHAR(250)'),
				[update_date] = getdate()
			FROM @P_XML.nodes('/project') M(m)
			WHERE project_id = @P_ProjectId;

			-- PROJECT SESSION
			WITH XMLNAMESPACES(DEFAULT 'http://www.gpd.com', 'http://www.w3.org/2001/XMLSchema-instance' AS i)
			UPDATE gpd_project_session
			SET [type] = m.value('(type)[1]', 'NVARCHAR(100)'),
				[platform] = m.value('(platform)[1]', 'NVARCHAR(150)'),
				[application_name] = m.value('(application[1]/name)[1]', 'NVARCHAR(150)'),
				[application_build] = m.value('(application[1]/build)[1]', 'NVARCHAR(150)'),
				[application_type] = m.value('(application[1]/type)[1]', 'NVARCHAR(150)'),
				[application_plugin_build] = m.value('(application[1]/plugin-build)[1]', 'NVARCHAR(150)'),
				[application_plugin_source] = m.value('(application[1]/plugin-source)[1]', 'NVARCHAR(150)'),
				[application_plugin_name] = m.value('(application[1]/plugin-name)[1]', 'NVARCHAR(150)'),
				[application_client_ip] = m.value('(application[1]/client-ip)[1]', 'NVARCHAR(150)'),
				[application_version] = m.value('(application[1]/version)[1]', 'NVARCHAR(150)'),
				[user_info_email] = m.value('(session-user-info/Email)[1]', 'NVARCHAR(250)'),
				[user_info_fname] = m.value('(session-user-info/FName)[1]', 'NVARCHAR(150)'),
				[user_info_lname] = m.value('(session-user-info/LName)[1]', 'NVARCHAR(150)'),
				[update_date] = getdate()
			FROM @P_XML.nodes('/project/session') M(m)
			WHERE project_id = @P_ProjectId;

			-- PROJECT LOCATION
			IF EXISTS (SELECT 1 FROM gpd_project_location L WHERE project_id = @P_ProjectId)
				BEGIN
					WITH XMLNAMESPACES(DEFAULT 'http://www.gpd.com', 'http://www.w3.org/2001/XMLSchema-instance' AS i)
					UPDATE gpd_project_location
					SET [address_line_1] = m.value('(address1)[1]', 'NVARCHAR(250)'),
						[address_line_2] = m.value('(address2)[1]', 'NVARCHAR(250)'),
						[city] = m.value('(city)[1]', 'NVARCHAR(250)'),						
						[state] = m.value('(state)[1]', 'NVARCHAR(250)'),
						[country] = m.value('(country)[1]', 'NVARCHAR(150)'),				
						[zip] = m.value('(zip)[1]', 'NVARCHAR(150)'),
						[update_date] = getdate()
					FROM @P_XML.nodes('/project/location') M(m)
					WHERE project_id = @P_ProjectId;
				END;
			ELSE
				BEGIN
					WITH XMLNAMESPACES(DEFAULT 'http://www.gpd.com', 'http://www.w3.org/2001/XMLSchema-instance' AS i)
					INSERT INTO gpd_project_location
					([project_id]
						,[type]
						,[address_line_1]
						,[address_line_2]
						,[city]
						,[state]
						,[zip]
						,[country]
						,[create_date]
						,[update_date])
					SELECT
						@P_ProjectId,
						'N/A',
						m.value('(address1)[1]', 'NVARCHAR(250)'),
						m.value('(address2)[1]', 'NVARCHAR(250)'),
						m.value('(city)[1]', 'NVARCHAR(250)'),
						m.value('(state)[1]', 'NVARCHAR(250)'),
						m.value('(zip)[1]', 'NVARCHAR(150)'),
						m.value('(country)[1]', 'NVARCHAR(150)'),
						getdate(),
						NULL
					FROM @P_XML.nodes('/project/location') M(m);

				END;

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