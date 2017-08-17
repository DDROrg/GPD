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
				organization_description = m.value('(organization-description)[1]', 'NVARCHAR(250)')
			FROM @P_XML.nodes('/project') M(m)
			WHERE project_id = @P_ProjectId;

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