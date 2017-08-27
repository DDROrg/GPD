CREATE PROCEDURE [dbo].[gpd_GetUserFullProfile]
	@P_USER_ID int
AS
BEGIN
	DECLARE @v_partner_id uniqueidentifier
	
	SELECT [last_name]
      ,[first_name]
      ,[email]
      ,[job_title]
      ,[business_phone]
      ,[home_phone]
      ,[mobile_phone]
      ,[fax_number]
      ,[address_line_1]
      ,[address_line_2]
      ,[city]
      ,[state_province]
      ,[zip_postal_code]
      ,[country]
	FROM gpd_user_details
	WHERE user_id = @P_USER_ID;

	SELECT [firm_id]
	  ,[name]
      ,[url]
      ,[logo]
      ,[address_line_1]
      ,[address_line_2]
      ,[city]
      ,[state_province]
      ,[zip_postal_code]
      ,[country]
	  ,p.value('.', 'nvarchar(250)') AS [DefaultIndustry]
	FROM gpd_firm_profile
		CROSS APPLY xml_firm_metadata.nodes('/list/item[@name="defaultIndustry"]') t(p)
	WHERE [firm_id] = (
		SELECT [firm_id]
		FROM gpd_user_details
		WHERE user_id = @P_USER_ID
	)
	AND [active] = 1;
END;