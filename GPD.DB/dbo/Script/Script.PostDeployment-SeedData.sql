/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/
DELETE FROM gpd_item_category_xref;
DELETE FROM gpd_client_user_group_xref;
DELETE FROM gpd_project_user_xref;
DELETE FROM gpd_project_item_material;
DELETE FROM gpd_project_item;
DELETE FROM gpd_category;
DELETE FROM gpd_project_identifier;
DELETE FROM gpd_project_location;
DELETE FROM gpd_project_session;
DELETE FROM gpd_project;
DELETE FROM gpd_client_details;
DELETE FROM gpd_user_details;
DELETE FROM gpd_user_group;

INSERT INTO [dbo].[gpd_user_group] (group_id, name, description, active, xml_group_metadata, create_date, update_date) 
VALUES (100, 'USER', 'Can publish projects; can view published projects; can export project details by complete or by category', 1, NULL, getdate(), null);
INSERT INTO [dbo].[gpd_user_group] (group_id, name, description, active, xml_group_metadata, create_date, update_date) 
VALUES (101, 'FIRMS ADMINISTRATOR', 'Can create/edit firm; add/remove user to firm; can control access to firm project information', 1, NULL, getdate(), null);
INSERT INTO [dbo].[gpd_user_group] (group_id, name, description, active, xml_group_metadata, create_date, update_date) 
VALUES (102, 'CLIENT USER', 'Can view project data based on access assign by the Client_Administrator and categories linked to the Client account', 1, NULL, getdate(), null);
INSERT INTO [dbo].[gpd_user_group] (group_id, name, description, active, xml_group_metadata, create_date, update_date) 
VALUES (103, 'CLIENT ADMINISTATOR', 'Admin client information and User access to data by region, state or zip code; can export project list and individual project details to xls, pdf or csv', 1, NULL, getdate(), null);
INSERT INTO [dbo].[gpd_user_group] (group_id, name, description, active, xml_group_metadata, create_date, update_date) 
VALUES (104, 'ADMINISTRATORS', 'Site Admin Modirators - Can edit/delete all Clients, Client_admins, Firms_Admins and User information and access to specific data, Can edit project data', 1, NULL, getdate(), null);
INSERT INTO [dbo].[gpd_user_group] (group_id, name, description, active, xml_group_metadata, create_date, update_date) 
VALUES (105, 'ADMINISTRATOR', 'Site	Main Site Admins - Can edit everything', 1, NULL, getdate(), null);
