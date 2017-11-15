CREATE PROCEDURE [dbo].[gpd_GetUsersList_V2]
	   @P_FromDate datetime,
	   @P_ToDate datetime,
	   @P_SearchTerm nvarchar(150),
	   @P_OrderByColIndex int,
	   @P_SortingOrder nvarchar(5),
	   @P_UserGroupId int,
	   @P_StartRowIndex int,
	   @P_PageSize int
AS
BEGIN

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements. 
	SET NOCOUNT ON;
	
	DECLARE @t_RecordsCount_A TABLE (user_id int, firmName varchar(150));
	DECLARE @t_RecordsCount_B TABLE (user_id int, firmName varchar(150));

	IF (LEN(ISNULL(@P_SearchTerm, '')) > 0)
		BEGIN
			SET @P_SearchTerm = '%' + @P_SearchTerm  + '%';
		END

	INSERT INTO @t_RecordsCount_A
		SELECT U.user_id, F.name
		FROM gpd_user_details U
		LEFT JOIN gpd_firm_profile F ON U.firm_id = F.firm_id
		WHERE U.create_date BETWEEN CONVERT(DATETIME, @P_FromDate, 102) AND DATEADD(day, 1, @P_ToDate)
		AND (
			(@P_SearchTerm IS NULL OR LEN(@P_SearchTerm) = 0) 
			OR 
			(U.full_name LIKE @P_SearchTerm OR U.email LIKE @P_SearchTerm OR U.job_title LIKE @P_SearchTerm OR U.business_phone LIKE @P_SearchTerm
			OR F.name LIKE @P_SearchTerm OR F.url LIKE @P_SearchTerm OR F.country LIKE @P_SearchTerm OR F.address_line_1 LIKE @P_SearchTerm
			OR F.address_line_2 LIKE @P_SearchTerm OR F.city LIKE @P_SearchTerm OR F.state_province LIKE @P_SearchTerm OR F.zip_postal_code LIKE @P_SearchTerm)
		);

	IF (@P_UserGroupId IS NOT NULL AND @P_UserGroupId <> -1)
		BEGIN
			INSERT INTO @t_RecordsCount_B
				SELECT T.user_id, T.firmName
				FROM gpd_partner_user_group_xref R, @t_RecordsCount_A T
				WHERE R.group_id = @P_UserGroupId
				AND R.active = 1
				AND T.user_id = R.user_id;

			SELECT U.user_id,
				U.first_name,
				U.last_name,
				U.email,
				U.firm_id,
				T.firmName,
				U.create_date,
				U.active
			FROM @t_RecordsCount_B T, gpd_user_details U
			WHERE U.user_id = T.user_id
			ORDER BY
				CASE WHEN @P_SortingOrder = 'asc' THEN
					CASE 
						WHEN @P_OrderByColIndex = 1 THEN U.first_name
						WHEN @P_OrderByColIndex = 2 THEN U.last_name
						WHEN @P_OrderByColIndex = 3 THEN U.email						
						WHEN @P_OrderByColIndex = 4 THEN T.firmName						
					END
				END ASC,
				CASE WHEN @P_SortingOrder = 'desc' THEN
					CASE 
						WHEN @P_OrderByColIndex = 1 THEN U.first_name
						WHEN @P_OrderByColIndex = 2 THEN U.last_name
						WHEN @P_OrderByColIndex = 3 THEN U.email						
						WHEN @P_OrderByColIndex = 4 THEN T.firmName
					END
				END DESC,				
				CASE WHEN @P_OrderByColIndex = 5 AND @P_SortingOrder = 'acs' THEN U.create_date END ASC,
				CASE WHEN @P_OrderByColIndex = 5 AND @P_SortingOrder = 'desc' THEN U.create_date END DESC				
			OFFSET @P_StartRowIndex ROWS FETCH NEXT @P_PageSize ROWS ONLY;

			SELECT count(*) AS TotalCount FROM @t_RecordsCount_B;
		END
	ELSE
		BEGIN
			SELECT U.user_id,
				U.first_name,
				U.last_name,
				U.email,
				U.firm_id,
				T.firmName,
				U.create_date,
				U.active
			FROM @t_RecordsCount_A T, gpd_user_details U
			WHERE U.user_id = T.user_id
			ORDER BY
				CASE WHEN @P_SortingOrder = 'asc' THEN
					CASE 
						WHEN @P_OrderByColIndex = 1 THEN U.first_name
						WHEN @P_OrderByColIndex = 2 THEN U.last_name
						WHEN @P_OrderByColIndex = 3 THEN U.email						
						WHEN @P_OrderByColIndex = 4 THEN T.firmName
					END
				END ASC,
				CASE WHEN @P_SortingOrder = 'desc' THEN
					CASE 
						WHEN @P_OrderByColIndex = 1 THEN U.first_name
						WHEN @P_OrderByColIndex = 2 THEN U.last_name
						WHEN @P_OrderByColIndex = 3 THEN U.email						
						WHEN @P_OrderByColIndex = 4 THEN T.firmName
					END
				END DESC,				
				CASE WHEN @P_OrderByColIndex = 5 AND @P_SortingOrder = 'asc' THEN U.create_date END ASC,
				CASE WHEN @P_OrderByColIndex = 5 AND @P_SortingOrder = 'desc' THEN U.create_date END DESC,
				CASE WHEN @P_OrderByColIndex = 6 AND @P_SortingOrder = 'asc' THEN U.active END ASC,
				CASE WHEN @P_OrderByColIndex = 6 AND @P_SortingOrder = 'desc' THEN U.active END DESC	
						
			OFFSET @P_StartRowIndex ROWS FETCH NEXT @P_PageSize ROWS ONLY;

			SELECT count(*) AS TotalCount FROM @t_RecordsCount_A;
		END
END;