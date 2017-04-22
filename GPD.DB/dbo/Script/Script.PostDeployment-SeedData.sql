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



INSERT INTO [dbo].[gpd_user_group] (group_id, name, description, active, xml_group_metadata, create_date, update_date) VALUES (101, 'Admin', 'n/a description', 1, NULL, getdate(), null);
INSERT INTO [dbo].[gpd_user_group] (group_id, name, description, active, xml_group_metadata, create_date, update_date) VALUES (102, 'Moderator', 'n/a description', 1, NULL, getdate(), null);
INSERT INTO [dbo].[gpd_user_group] (group_id, name, description, active, xml_group_metadata, create_date, update_date) VALUES (103, 'Site Client', 'n/a description', 1, NULL, getdate(), null);
INSERT INTO [dbo].[gpd_user_group] (group_id, name, description, active, xml_group_metadata, create_date, update_date) VALUES (104, 'User', 'n/a description', 1, NULL, getdate(), null);
INSERT INTO [dbo].[gpd_user_group] (group_id, name, description, active, xml_group_metadata, create_date, update_date) VALUES (105, 'Site Admin', 'n/a description', 1, NULL, getdate(), null);
INSERT INTO [dbo].[gpd_user_group] (group_id, name, description, active, xml_group_metadata, create_date, update_date) VALUES (106, 'Manufacturer', 'n/a description', 1, NULL, getdate(), null);
INSERT INTO [dbo].[gpd_user_group] (group_id, name, description, active, xml_group_metadata, create_date, update_date)VALUES (107, 'Firm', 'n/a description', 1, NULL, getdate(), null);

