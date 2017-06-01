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
DELETE FROM gpd_partner_user_group_xref;
DELETE FROM gpd_project_user_xref;
DELETE FROM gpd_project_item_material;
DELETE FROM gpd_project_item;
DELETE FROM gpd_category;
DELETE FROM gpd_project_identifier;
DELETE FROM gpd_project_location;
DELETE FROM gpd_project_session;
DELETE FROM gpd_project;
DELETE FROM gpd_partner_details;
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

INSERT INTO gpd_user_details (user_id, last_name, first_name, full_name, email, password, company, job_title, 
	business_phone, home_phone, mobile_phone, fax_number, address_line_1, address_line_2, city, state_province, 
	zip_postal_code, country, ip_address, active, xml_user_metadata, create_date, update_date)
VALUES('C0C9E96E-6796-4BB1-9E44-60229165C0E2', NULL, NULL, 'Site Admin', 'gpd@noemail.com', 'Pass@1234', 
	'GPD', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, NULL, getdate(),  NULL);

INSERT INTO gpd_partner_details (partner_id, name, site_url, short_description, description, active, xml_partner_metadata, create_date, update_date)
VALUES('029882E7-FFFB-4FC3-92AE-323799525099', 'sweets', 'http://sweets.cnstruction.com', 'N/A', 'N/A', 1, NULL, getdate(), NULL);
INSERT INTO gpd_partner_details (partner_id, name, site_url, short_description, description, active, xml_partner_metadata, create_date, update_date)
VALUES('60F47B0E-6DEA-474B-9882-A93DCC6A59D1', 'TEST', 'N/A', 'N/A', 'N/A', 1, NULL, getdate(), NULL);
INSERT INTO gpd_partner_details (partner_id, name, site_url, short_description, description, active, xml_partner_metadata, create_date, update_date)
VALUES('484330C5-1B83-4D37-93EA-D2049BFD3DD5', 'ALL', 'N/A', 'N/A', 'N/A', 1, NULL, getdate(), NULL);


INSERT INTO gpd_partner_user_group_xref (partner_id, user_id, group_id, description, active, create_date, update_date)
VALUES ('484330C5-1B83-4D37-93EA-D2049BFD3DD5', 'C0C9E96E-6796-4BB1-9E44-60229165C0E2', 105, 'Site Admin', 1, getdate(), NULL);



