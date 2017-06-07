CREATE PROCEDURE [dbo].[gpd_GetUserRole]
       @P_EMAIL nvarchar(150)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT distinct 
		u.user_id, 
		u.first_name, 
		u.last_name, 
		p.name as PartnerName,
		g.name as GroupName
	FROM gpd_user_details u
	INNER JOIN gpd_partner_user_group_xref x
		ON u.user_id = x.user_id
			AND LOWER(u.email) = LOWER(@P_EMAIL)
			AND u.active = 1	
	INNER JOIN gpd_partner_details p
		ON x.partner_id = p.partner_id
	INNER JOIN gpd_user_group g
		ON x.group_id = g.group_id
	ORDER BY p.name;
END;