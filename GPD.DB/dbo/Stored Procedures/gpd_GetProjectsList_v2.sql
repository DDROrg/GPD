CREATE PROCEDURE [dbo].[gpd_GetProjectsList_v2]
	   @P_PartnerName nvarchar(30),
	   @P_UserId int,
	   @P_FromDate nvarchar(10),
	   @P_ToDate nvarchar(10),
	   @P_SearchTerm nvarchar(100),
	   @P_ProjectIdentifier nvarchar(250),
	   @P_PartnersAccessTo XML,
	   @P_StartRowIndex int,
	   @P_PageSize int
AS
BEGIN

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements. 
	SET NOCOUNT ON;
	
	DECLARE @t_RecordsCount TABLE (project_id uniqueidentifier, create_date datetime);
	DECLARE @t_RecordsCountTemp TABLE (project_id uniqueidentifier, create_date datetime);
	DECLARE @V_IS_USER_GPD_ADMIN BIT = 0;

	IF EXISTS (
		SELECT X.user_id
		FROM gpd_partner_user_group_xref X, gpd_partner_details PD
		WHERE X.user_id = @P_UserId
		AND PD.name = 'ALL'
		AND X.partner_id = PD.partner_id
	)
		BEGIN
			SET @V_IS_USER_GPD_ADMIN = 1;
		END;

	INSERT INTO @t_RecordsCount
		SELECT P.project_id, P.create_date
		FROM gpd_project P, gpd_project_identifier I
		WHERE P.create_date BETWEEN CONVERT(DATETIME, @P_FromDate, 102) AND DATEADD(day, 1, @P_ToDate)
		AND P.deleted = 0
		AND P.project_id = I.project_id
		AND (
			(@P_ProjectIdentifier IS NULL OR LEN(@P_ProjectIdentifier) = 0) OR I.identifier = @P_ProjectIdentifier
		)
		AND (
			--@P_PartnerName = 'ALL' OR P.partner_name = @P_PartnerName
			P.partner_name in (SELECT doc.col.value('.', 'nvarchar(30)') FROM @P_PartnersAccessTo.nodes('//items-list/parner') doc(col))
		)
		AND (
			@V_IS_USER_GPD_ADMIN = 1 OR P.active = 1
		);

	IF (@P_SearchTerm IS NOT NULL AND LEN(@P_SearchTerm) > 0)
		BEGIN
	
			SET @P_SearchTerm = '%' + @P_SearchTerm + '%';

			/* Project Name | Client | Status */
			INSERT INTO @t_RecordsCountTemp
				SELECT P.project_id, P.create_date
				FROM @t_RecordsCount T, gpd_project P
				WHERE P.project_id = T.project_id
				AND (
					P.name LIKE @P_SearchTerm 
					OR P.client LIKE @P_SearchTerm 
					OR P.status LIKE @P_SearchTerm
				)

			INSERT INTO @t_RecordsCountTemp
				/* Zipcode */
				SELECT PL.project_id, T.create_date
				FROM @t_RecordsCount T, gpd_project_location PL
				WHERE PL.project_id = T.project_id
				AND PL.zip LIKE @P_SearchTerm

			INSERT INTO @t_RecordsCountTemp
				/* Product-Item: Family | Family Type | Manufacturer Name | Product Model | Product Name */
				SELECT T.project_id, T.create_date
				FROM @t_RecordsCount T, gpd_project_item I
				WHERE I.project_id = T.project_id
				AND (
					/* Family */
					I.[family] LIKE @P_SearchTerm
					/* Family Type */
					OR I.[type] LIKE @P_SearchTerm
					/* Manufacturer Name */
					OR I.[product_manufacturer] LIKE @P_SearchTerm
					/* Product Model */
					OR I.[product_model] LIKE @P_SearchTerm				
					/* Product Name */
					OR I.[product_name] LIKE @P_SearchTerm
				)

			INSERT INTO @t_RecordsCountTemp
				/* Material Name | Material Manufacturer | Material Model */
				SELECT T.project_id, T.create_date
				FROM @t_RecordsCount T, gpd_project_item I, gpd_project_item_material M
				WHERE I.project_id = T.project_id
				AND M.project_item_id = I.project_item_id
				AND (M.product_name LIKE @P_SearchTerm OR M.product_manufacturer LIKE @P_SearchTerm OR M.product_model LIKE @P_SearchTerm)
			
			DELETE FROM @t_RecordsCount;
			INSERT INTO @t_RecordsCount
				SELECT DISTINCT T.project_id, T.create_date FROM @t_RecordsCountTemp T;
		END;

	DELETE FROM @t_RecordsCountTemp;
	INSERT INTO @t_RecordsCountTemp
		SELECT x.project_id, x.create_date
		FROM @t_RecordsCount x
		ORDER BY x.create_date DESC
		OFFSET @P_StartRowIndex ROWS FETCH NEXT @P_PageSize ROWS ONLY;

	-- Get the paginated records
	SELECT P.PROJECT_ID,
		P.PARTNER_NAME,
		P.AUTHOR,
		P.BUILDING_NAME,
		P.CLIENT,
		P.[FILENAME],
		P.NAME,
		P.NUMBER,
		P.ORGANIZATION_DESCRIPTION,
		P.ORGANIZATION_NAME,
		P.[STATUS],
		P.[ACTIVE],
		P.DELETED,
		P.CREATE_DATE,
		P.UPDATE_DATE,
		L.ADDRESS_LINE_1,
		L.CITY,
		L.STATE,
		L.ZIP,
		U.EMAIL
	FROM gpd_project P
	INNER JOIN @t_RecordsCountTemp x ON P.project_id = x.project_id
	LEFT OUTER JOIN GPD_PROJECT_LOCATION L ON P.PROJECT_ID = L.PROJECT_ID
	LEFT OUTER JOIN (
		SELECT X2.project_id, US.email
		FROM @t_RecordsCountTemp X2, gpd_project_user_xref XR, gpd_user_details US 
		WHERE XR.project_id = X2.project_id
		AND US.user_id = XR.user_id
	) U
	ON P.PROJECT_ID = U.project_id
	ORDER BY P.create_date DESC;

	SELECT count(project_id) AS TotalCount FROM @t_RecordsCount;
END;