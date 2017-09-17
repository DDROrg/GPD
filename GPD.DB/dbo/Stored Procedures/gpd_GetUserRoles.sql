CREATE PROCEDURE [dbo].[gpd_GetUserRoles]
	@P_USER_EMAIL nvarchar(150)
AS
BEGIN
	DECLARE @v_partner_id uniqueidentifier
	
	SELECT u.user_id, 
		u.first_name, 
		u.last_name 
	FROM gpd_user_details u
	WHERE u.email = @P_USER_EMAIL;

	IF EXISTS (
		SELECT gref.*
		FROM [gpd_user_details] u, 
			[gpd_partner_user_group_xref] gref,
			[gpd_partner_details] p
		WHERE u.email = @P_USER_EMAIL
		AND gref.user_id = u.user_id
		AND gref.group_id = 100
		AND gref.active = 1
		AND p.partner_id = gref.partner_id
		AND p.name = 'ALL'
	)
		BEGIN
		    SELECT TOP 1 
				p.partner_id,
				p.name  PartnerName,
				p.[xml_partner_metadata].value('(/metadata/item)[1]', 'varchar(300)') AS "PartnerImageUrl",
				100 AS group_id,
				(SELECT TOP 1 g.name FROM [gpd_user_group] g WHERE g.group_id = 100) AS GroupName,
				0 AS sort_order
			FROM [dbo].[gpd_partner_details] P
			WHERE p.name = 'ALL'
			UNION
			SELECT
				p.partner_id,
				p.name  PartnerName,
				p.[xml_partner_metadata].value('(/metadata/item)[1]', 'varchar(300)') AS "PartnerImageUrl",
				100 AS group_id,
				(SELECT TOP 1 g.name FROM [gpd_user_group] g WHERE g.group_id = 100) AS GroupName,
				1 AS sort_order
			FROM [dbo].[gpd_partner_details] P
			WHERE p.name <> 'ALL'
			AND p.active = 1
			ORDER by sort_order, PartnerName
		END
	ELSE
		BEGIN
			SELECT p.partner_id,
				p.name  PartnerName,
				p.[xml_partner_metadata].value('(/metadata/item)[1]', 'varchar(300)') AS "PartnerImageUrl",
				g.group_id,
				g.name AS GroupName,
				p.partner_id
			FROM [gpd_user_details] u, 
				[gpd_partner_user_group_xref] gref,
				[gpd_partner_details] p,
				[gpd_user_group] g
			WHERE u.email = @P_USER_EMAIL
			AND gref.user_id = u.user_id
			AND gref.group_id <> 100
			AND p.partner_id = gref.partner_id
			AND g.group_id = gref.group_id
			ORDER BY p.partner_id;
		END
END;