CREATE PROCEDURE [dbo].[GetProjectsLIstPaginated]
       @P_PartnerName nvarchar(30),
	   @P_StartRowIndex int,
       @P_PageSize int,
       @P_TotalCount int OUT
AS
BEGIN
       -- SET NOCOUNT ON added to prevent extra result sets from
       -- interfering with SELECT statements. 
       SET NOCOUNT ON;

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
			  P.CREATE_DATE
       FROM GPD_PROJECT P
	   WHERE P.PARTNER_NAME = @P_PartnerName
	   ORDER BY P.create_date DESC
	   OFFSET @P_StartRowIndex ROWS FETCH NEXT @P_PageSize ROWS ONLY

       -- get the total count of the records
       SELECT @P_TotalCount = COUNT(project_id)
	   FROM GPD_PROJECT
	   WHERE PARTNER_NAME = @P_PartnerName;
END;