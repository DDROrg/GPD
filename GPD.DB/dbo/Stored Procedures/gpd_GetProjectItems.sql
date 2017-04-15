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
	FROM GPD_PROJECT_ITEM
	WHERE PROJECT_ID = @P_PROJECT_ID;

	SELECT 
		M.project_item_id,
		M.product_manufacturer,
		M.product_modeL,
		M.product_name,
		M.type_name
	FROM GPD_PROJECT_ITEM PI, GPD_PROJECT_ITEM_MATERIAL M
	WHERE PI.PROJECT_ID = @P_PROJECT_ID
	AND M.project_item_id = PI.project_item_id;
END