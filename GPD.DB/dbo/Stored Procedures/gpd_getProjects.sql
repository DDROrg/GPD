CREATE PROCEDURE [dbo].[gpd_GetProjects]
	@P_USER VARCHAR(100)
AS
BEGIN
	SELECT p.project_id,
		p.source_client,
		p.author,
		p.building_name,
		p.client,
		p.[filename],
		p.name,
		p.number,
		p.organization_description,
		p.organization_name,
		p.[status]
	FROM gpd_project p;
END
