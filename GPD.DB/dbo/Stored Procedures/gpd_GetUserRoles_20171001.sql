CREATE PROCEDURE [dbo].[gpd_GetUserRoles_20171001]
	@P_USER_EMAIL nvarchar(150)
AS
BEGIN
	DECLARE @v_partner_id uniqueidentifier
	
	SELECT u.user_id, 
		u.first_name, 
		u.last_name 
	FROM gpd_user_details u
	WHERE LOWER(u.email) = LOWER(@P_USER_EMAIL);

	Select @v_partner_id = partner_id FROM gpd_partner_details p WHERE p.name = 'ALL' AND p.active = 1;

	IF EXISTS (
		SELECT u.user_id FROM gpd_user_details u, gpd_partner_user_group_xref xref
		WHERE u.email = 'test@test.com'
		AND u.active = 1
		AND xref.user_id = u.user_id
		AND partner_id = @v_partner_id
		AND xref.group_id in (104, 105)
	)
		BEGIN
		    SELECT a.partner_id,
				a.PartnerName,
				a.PartnerImageUrl,
				105 AS group_id,
				'ADMINISTRATOR' as GroupName
			FROM (
				SELECT partner_id, p.name as PartnerName, 
					[xml_partner_metadata].value('(/metadata/item)[1]', 'varchar(300)') AS "PartnerImageUrl",
					0 item_index
				FROM gpd_partner_details p 
				WHERE p.name = 'ALL' AND p.active = 1
				UNION
				SELECT p.partner_id, p.name as PartnerName, 
					[xml_partner_metadata].value('(/metadata/item)[1]', 'varchar(300)') AS "PartnerImageUrl",
					1
				FROM gpd_partner_details p
				WHERE p.active = 1
				AND p.partner_id <> @v_partner_id
			) a
			ORDER BY a.item_index, a.PartnerName
		END
	ELSE
		BEGIN
			SELECT p.partner_id,
				p.name as PartnerName,
				g.group_id,
				g.name as GroupName
			FROM gpd_user_details u
			INNER JOIN gpd_partner_user_group_xref x
				ON u.user_id = x.user_id
			AND LOWER(u.email) = LOWER(@P_USER_EMAIL)
			AND u.active = 1	
			INNER JOIN gpd_partner_details p
				ON x.partner_id = p.partner_id
			INNER JOIN gpd_user_group g
				ON x.group_id = g.group_id
			ORDER BY p.name;
		END
END;