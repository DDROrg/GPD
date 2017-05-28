CREATE PROCEDURE [dbo].[gpd_GetProjectItems]
	@P_PROJECT_ID uniqueidentifier
AS
BEGIN
	SELECT
		[project_item_id],
		[product_manufacturer],
		[product_model],
		[family],
		[type],
		[quantity],
		[quantity_unit]
	FROM gpd_project_item
	WHERE project_id = @P_PROJECT_ID;

	SELECT 
		M.project_item_id,
		M.product_manufacturer,
		M.product_modeL,
		M.product_name,
		M.type_name
	FROM gpd_project_item PI, gpd_project_item_material M
	WHERE PI.project_id = @P_PROJECT_ID
	AND M.project_item_id = PI.project_item_id;
END