
CREATE PROCEDURE [dbo].[gpd_GetPartners]
AS
BEGIN	
	SELECT 
		partner_id,
		name,
		site_url,
		short_description,
		description,
		active
	FROM gpd_partner_details;
END;

