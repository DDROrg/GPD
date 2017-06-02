CREATE PROCEDURE [dbo].[gpd_GetProjects]
	@P_USER VARCHAR(100)
AS
BEGIN
	SELECT p.project_id,
		p.partner_name,
		p.author,
		p.building_name,
		p.client,
		p.[filename],
		p.name,
		p.number,
		p.organization_description,
		p.organization_name,
		p.[status],
		p.create_date
	FROM gpd_project p
	ORDER BY P.create_date DESC;
END
