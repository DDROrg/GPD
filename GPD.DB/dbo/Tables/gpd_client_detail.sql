CREATE TABLE [dbo].[gpd_client_detail]
(
	[client_id] INT NOT NULL  IDENTITY(100, 1), 
    [source_client] NVARCHAR(50) NOT NULL, 
    [site_url] NVARCHAR(400) NULL, 
    [short_description] NVARCHAR(200) NULL, 
    [description] NVARCHAR(500) NULL,
	[active]           BIT NOT NULL,
    [xml_client_metadata] XML NULL,
    [create_date]    DATETIME        NOT NULL,
    [update_date]    DATETIME        NULL, 
    CONSTRAINT [PK_gpd_client_detail] PRIMARY KEY ([client_id])
)

