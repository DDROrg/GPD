﻿
CREATE PROCEDURE [dbo].[gpd_GetProjectById]
	@P_PROJECTID UNIQUEIDENTIFIER
AS
BEGIN	
	SELECT 
		P.PROJECT_ID,
		P.partner_name,
		P.AUTHOR,
		P.BUILDING_NAME,
		P.CLIENT,
		P.[FILENAME],
		P.NAME,
		P.NUMBER,
		P.ORGANIZATION_DESCRIPTION,
		P.ORGANIZATION_NAME,
		P.[STATUS],	
		L.ADDRESS_LINE_1,
		L.CITY,
		L.[STATE],
		L.ZIP,
		S.[TYPE],
		S.[PLATFORM],
		S.APPLICATION_BUILD,
		S.APPLICATION_NAME,
		S.APPLICATION_PLUGIN_BUILD,
		S.APPLICATION_PLUGIN_SOURCE,
		S.APPLICATION_VERSION,
		S.APPLICATION_TYPE	
	FROM GPD_PROJECT P
	LEFT JOIN GPD_PROJECT_LOCATION L ON P.PROJECT_ID = L.PROJECT_ID 
	LEFT JOIN GPD_PROJECT_SESSION S ON P.PROJECT_ID = S.PROJECT_ID
	WHERE P.PROJECT_ID = @P_PROJECTID;

	SELECT I.IDENTIFIER, I.[SYSTEM]
	FROM GPD_PROJECT_IDENTIFIER I WHERE I.PROJECT_ID = @P_PROJECTID;

	SELECT
		I.PROJECT_ITEM_ID, 
	    I.PROJECT_ID,
		I.ITEM_ID,
		I.[TYPE],
		I.CURRENCY,
		I.FAMILY,
		I.QUANTITY,
		I.QUANTITY_UNIT,
		I.PRODUCT_ID,
		I.PRODUCT_IMAGE_URL,
		I.PRODUCT_MANUFACTURER,
		I.PRODUCT_MODEL,
		I.PRODUCT_NAME,
		I.PRODUCT_URL,
		I.product_image_url
	FROM GPD_PROJECT_ITEM I WHERE I.PROJECT_ID = @P_PROJECTID;

	SELECT
		I.PROJECT_ITEM_ID,
		I.PROJECT_ID,
		I.ITEM_ID,
		M.MATERIAL_ID,
		M.PRODUCT_MANUFACTURER,
		M.PRODUCT_MODEL,
		M.PRODUCT_NAME,
		M.[TYPE_NAME]
	FROM GPD_PROJECT_ITEM I
	INNER JOIN GPD_PROJECT_ITEM_MATERIAL M ON I.project_item_id = M.project_item_id
	WHERE I.PROJECT_ID = @P_PROJECTID;
	
	SELECT 
		I.PROJECT_ID,
		I.ITEM_ID,
		C.taxonomy,
		C.title
	FROM GPD_PROJECT_ITEM I
	INNER JOIN gpd_item_category_xref X ON I.project_item_id = X.project_item_id
	INNER JOIN gpd_category C ON X.category_id = C.category_id AND c.active = 1
	WHERE I.PROJECT_ID = @P_PROJECTID;
END
