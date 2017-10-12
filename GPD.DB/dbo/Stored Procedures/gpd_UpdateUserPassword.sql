CREATE PROCEDURE [dbo].[gpd_UpdateUserPassword]
	@P_USER_EMAIL VARCHAR(150),
	@P_USER_PASSWORD VARCHAR(300)
AS 
BEGIN
 SET NOCOUNT ON;
 	/******************************
	*  Variable Declarations
	*******************************/   
	DECLARE @V_USER_ID INT;

	IF EXISTS (SELECT 1 FROM gpd_user_details U WHERE U.email = @P_USER_EMAIL)
		BEGIN
			UPDATE gpd_user_details
			SET password = @P_USER_PASSWORD, update_date = getdate()
			WHERE email = @P_USER_EMAIL;

			SELECT U.user_id, U.first_name, U.last_name, U.email
			FROM gpd_user_details U
			WHERE email = @P_USER_EMAIL;
		END;
END;