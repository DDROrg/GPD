CREATE PROCEDURE [dbo].[gpd_GetProjectsListBySearchTerm]
	   @P_PartnerName nvarchar(30),
	   @P_SearchKeyword nvarchar(30),
	   @P_ProjectIdentifier nvarchar(250),
	   @P_StartRowIndex int,
       @P_PageSize int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements. 
	SET NOCOUNT ON;
	
	DECLARE @t_RecordsCount TABLE (project_id uniqueidentifier, partner_name nvarchar(30));

	IF @P_SearchKeyword IS NULL
		BEGIN
			INSERT INTO @t_RecordsCount
				SELECT P.project_id, P.partner_name
				FROM gpd_project P, gpd_project_identifier I
				WHERE I.identifier = @P_ProjectIdentifier
				AND P.project_id = I.project_id
				AND P.active = 1
		END
	ELSE
		BEGIN
			INSERT INTO @t_RecordsCount
				SELECT P.project_id, P.partner_name
				FROM gpd_project P
				WHERE P.project_id IN (
					/* Project Name | Client | Status */
					SELECT project_id FROM gpd_project P WHERE P.active = 1
					AND (P.name LIKE '%' + @P_SearchKeyword +'%' OR P.client LIKE '%' + @P_SearchKeyword +'%' OR P.status LIKE '%' + @P_SearchKeyword +'%')
			
					UNION
					/* Zipcode */
					SELECT PL.project_id
					FROM gpd_project_location PL
					WHERE PL.zip LIKE '%' + @P_SearchKeyword +'%'
				
					UNION
					/* Material Name | Material Manufacturer | Material Model */
					SELECT I.project_id
					FROM gpd_project_item I
					WHERE I.project_item_id IN (
						SELECT M.project_item_id
						FROM gpd_project_item_material M
						WHERE M.product_name LIKE '%' + @P_SearchKeyword +'%'
						OR M.product_manufacturer LIKE '%' + @P_SearchKeyword +'%'
						OR M.product_model LIKE '%' + @P_SearchKeyword +'%'
					)
					/* Family */
					OR [family] LIKE '%' + @P_SearchKeyword +'%'
					/* Family Type */
					OR [type] LIKE '%' + @P_SearchKeyword +'%'
					/* Manufacturer Name */
					OR [product_manufacturer] LIKE '%' + @P_SearchKeyword +'%'
					/* Manufacturer Name | Model Number / Name */
					OR [product_model] LIKE '%' + @P_SearchKeyword +'%'
					OR [product_name] LIKE '%' + @P_SearchKeyword +'%'
				)
				AND P.active = 1
				AND (
					@P_ProjectIdentifier IS NULL OR p.project_id IN (SELECT I.project_id 
																FROM gpd_project_identifier I 
																WHERE I.identifier = @P_ProjectIdentifier)
				)
		END	

	IF @P_PartnerName = 'ALL'
		BEGIN
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
				P.CREATE_DATE,
				L.ADDRESS_LINE_1,
				L.CITY,
				L.STATE,
				L.ZIP
			FROM GPD_PROJECT P
			INNER JOIN @t_RecordsCount x ON P.project_id = x.project_id
			LEFT OUTER JOIN GPD_PROJECT_LOCATION L ON P.PROJECT_ID = L.PROJECT_ID
			ORDER BY P.create_date DESC
			OFFSET @P_StartRowIndex ROWS FETCH NEXT @P_PageSize ROWS ONLY;
				
			-- get the total count of the records
			SELECT count(*) FROM @t_RecordsCount;
		END
	ELSE
		BEGIN
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
				P.CREATE_DATE,
				L.ADDRESS_LINE_1,
				L.CITY,
				L.STATE,
				L.ZIP
			FROM GPD_PROJECT P
			INNER JOIN @t_RecordsCount x ON P.project_id = x.project_id
			LEFT OUTER JOIN GPD_PROJECT_LOCATION L ON P.PROJECT_ID = L.PROJECT_ID
			WHERE x.partner_name = @P_PartnerName
			ORDER BY P.create_date DESC
			OFFSET @P_StartRowIndex ROWS FETCH NEXT @P_PageSize ROWS ONLY;
				
			-- get the total count of the records
			SELECT count(*) FROM @t_RecordsCount x WHERE x.partner_name = @P_PartnerName;
		END
END;