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
INSERT INTO [dbo].[gpd_user_group] VALUES (101, 'Admin', 'n/a description', 1, NULL, getdate(), null);
INSERT INTO [dbo].[gpd_user_group] VALUES (102, 'Moderator', 'n/a description', 1, NULL, getdate(), null);
INSERT INTO [dbo].[gpd_user_group] VALUES (103, 'Site Client', 'n/a description', 1, NULL, getdate(), null);
INSERT INTO [dbo].[gpd_user_group] VALUES (104, 'User', 'n/a description', 1, NULL, getdate(), null);
INSERT INTO [dbo].[gpd_user_group] VALUES (105, 'Site Admin', 'n/a description', 1, NULL, getdate(), null);
INSERT INTO [dbo].[gpd_user_group] VALUES (106, 'Manufacturer', 'n/a description', 1, NULL, getdate(), null);
INSERT INTO [dbo].[gpd_user_group] VALUES (107, 'Firm', 'n/a description', 1, NULL, getdate(), null);

