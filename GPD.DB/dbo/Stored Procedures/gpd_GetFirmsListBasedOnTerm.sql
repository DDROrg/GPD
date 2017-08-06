CREATE PROCEDURE [dbo].[gpd_GetFirmsListBasedOnTerm]
	@P_FIRM_NAME_TERM nvarchar(150)
AS
BEGIN
	SELECT firm_id, name, url
	FROM gpd_firm_profile
	WHERE name like '%' + @P_FIRM_NAME_TERM + '%'
	AND active = 1
	ORDER BY name;
END;