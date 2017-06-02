CREATE PROCEDURE gpd_UserAuthenticate
       @P_EMAIL nvarchar(150),
	   @P_PASSWORD nvarchar(150)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT distinct 
		u.user_id
	FROM gpd_user_details u
	WHERE LOWER(u.email) = LOWER(@P_EMAIL)
		AND u.password = @P_PASSWORD
		AND u.active = 1;
END;