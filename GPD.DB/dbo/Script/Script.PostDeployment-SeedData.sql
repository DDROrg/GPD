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
INSERT INTO [dbo].[gpd_user_group] VALUES ('Admin', 'n/a description', 1, NULL, getdate(), null);
INSERT INTO [dbo].[gpd_user_group] VALUES ('Moderator', 'n/a description', 1, NULL, getdate(), null);
INSERT INTO [dbo].[gpd_user_group] VALUES ('Site Client', 'n/a description', 1, NULL, getdate(), null);
INSERT INTO [dbo].[gpd_user_group] VALUES ('User', 'n/a description', 1, NULL, getdate(), null);
INSERT INTO [dbo].[gpd_user_group] VALUES ('Site Admin', 'n/a description', 1, NULL, getdate(), null);
INSERT INTO [dbo].[gpd_user_group] VALUES ('Manufacturer', 'n/a description', 1, NULL, getdate(), null);
INSERT INTO [dbo].[gpd_user_group] VALUES ('Firm', 'n/a description', 1, NULL, getdate(), null);

