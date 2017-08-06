CREATE PROCEDURE [dbo].[gpd_GetFirmProfile]
	@P_FIRM_ID int
AS
BEGIN
	SELECT
		F.*,
		F2.DefaultIndustry
	FROM gpd_firm_profile F
	LEFT JOIN (
		SELECT FP.firm_id, p.value('.', 'nvarchar(250)') AS [DefaultIndustry]
		FROM gpd_firm_profile FP
			CROSS APPLY FP.xml_firm_metadata.nodes('/list/item[@name="defaultIndustry"]') t(p)
		WHERE FP.firm_id = @P_FIRM_ID
	) F2 ON F2.firm_id = F.firm_id
	WHERE F.firm_id = @P_FIRM_ID;
END;