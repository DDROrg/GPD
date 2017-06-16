
CREATE PROCEDURE [dbo].[gpd_GetUsers]
	@P_SEARCH nvarchar(150)
AS
BEGIN	
	SELECT 
		u.user_id,
		u.last_name,
		u.first_name,
		u.full_name,
		u.email,
		u.company,
		u.job_title,
		u.business_phone,
		u.home_phone,
		u.mobile_phone,
		u.fax_number,
		u.address_line_1,
		u.address_line_2,
		u.city,
		u.state_province,
		u.zip_postal_code,
		u.country,
		u.active      
	FROM gpd_user_details U
	WHERE @P_SEARCH = '' 
		OR UPPER(u.last_name) LIKE @P_SEARCH 
		OR UPPER(u.first_name) LIKE @P_SEARCH 
		OR UPPER(u.email) LIKE @P_SEARCH;
END;

