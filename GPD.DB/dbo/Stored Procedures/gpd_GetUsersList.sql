CREATE PROCEDURE [dbo].[gpd_GetUsersList]
	@P_SEARCH_VALUE VARCHAR(250)
AS
BEGIN
	SELECT TOP (1000)
		U.user_id,
		U.first_name,
		U.last_name,
		U.email,
		U.firm_id,
		F.name as "firmName",
		u.active      
	FROM gpd_user_details U
	LEFT JOIN gpd_firm_profile F ON U.firm_id = F.firm_id
	WHERE (
		(@P_SEARCH_VALUE IS NULL OR LEN(@P_SEARCH_VALUE) = 0) 
		OR 
		(u.last_name LIKE @P_SEARCH_VALUE OR U.first_name LIKE @P_SEARCH_VALUE OR U.email LIKE @P_SEARCH_VALUE)
	)
	ORDER BY U.create_date DESC;
END;