CREATE PROCEDURE [dbo].[gpd_ActivateProjectList]
	@P_Is_Active bit,
	@P_XML XML,	
	@P_Return_ErrorCode INT OUT,
	@P_Return_Message VARCHAR(1024) = '' OUT
AS 
BEGIN
 SET NOCOUNT ON;
 	/******************************
	*  Initialize Variables
	*******************************/
	SET @P_Return_ErrorCode = @@ERROR;

	BEGIN TRY  
		UPDATE gpd_project
		SET [active] = @P_Is_Active
			,[update_date] = getdate()
		WHERE project_id IN (
			SELECT doc.col.value('.', 'uniqueidentifier')
			FROM @P_XML.nodes('//project-list/project') doc(col)
		)
	END TRY

	BEGIN CATCH  
		SELECT   
			@P_Return_ErrorCode = ERROR_NUMBER(),
			@P_Return_Message = cast(ERROR_NUMBER() as varchar(20)) + ' line: '
				+ CAST(ERROR_LINE() as varchar(20)) + ' ' 
				+ ERROR_MESSAGE() + ' > ' 
				+ ERROR_PROCEDURE();
	END CATCH
END;