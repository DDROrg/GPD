CREATE PROCEDURE gpd_UserLogin
       @P_EMAIL nvarchar(150),
	   @P_PASSWORD nvarchar(150)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements. 
    SET NOCOUNT ON;

	SELECT distinct u.user_id, u.first_name, u.last_name, p.name
	FROM [gpd_user_details] u, [gpd_partner_user_group_xref] x, [gpd_partner_details] p
	WHERE LOWER(u.email) = LOWER(@P_EMAIL)
	AND u.password = @P_PASSWORD
	AND u.active = 1
	AND x.user_id = u.user_id
	AND p.partner_id = x.partner_id
	ORDER BY p.name
END;