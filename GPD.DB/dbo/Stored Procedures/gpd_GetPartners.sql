
CREATE PROCEDURE [dbo].[gpd_GetPartners]
AS
BEGIN	
	SELECT 
		p.partner_id,
		p.name,
		p.site_url,
		p.short_description,
		p.description,
		p.active
	FROM gpd_partner_details p;
END;

