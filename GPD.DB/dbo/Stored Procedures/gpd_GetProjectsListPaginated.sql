CREATE PROCEDURE [dbo].[gpd_GetProjectsListPaginated]
       @P_PartnerName nvarchar(30),
	   @P_StartRowIndex int,
       @P_PageSize int
AS
BEGIN
       -- SET NOCOUNT ON added to prevent extra result sets from
       -- interfering with SELECT statements. 
       SET NOCOUNT ON;

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
				LEFT OUTER JOIN GPD_PROJECT_LOCATION L ON P.PROJECT_ID = L.PROJECT_ID
				WHERE P.active = 1
				ORDER BY P.create_date DESC
				OFFSET @P_StartRowIndex ROWS FETCH NEXT @P_PageSize ROWS ONLY;
				
				-- get the total count of the records
				SELECT COUNT(project_id) AS TotalCount  FROM GPD_PROJECT;
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
				LEFT OUTER JOIN GPD_PROJECT_LOCATION L ON P.PROJECT_ID = L.PROJECT_ID
				WHERE P.PARTNER_NAME = @P_PartnerName
				AND P.active = 1
				ORDER BY P.create_date DESC
				OFFSET @P_StartRowIndex ROWS FETCH NEXT @P_PageSize ROWS ONLY
				
				-- get the total count of the records
				SELECT COUNT(project_id) AS TotalCount  
				FROM GPD_PROJECT
				WHERE PARTNER_NAME = @P_PartnerName;
	   	   END
END;