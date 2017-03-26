CREATE TABLE [dbo].[gpd_client_details] (
    [client_id]           INT             IDENTITY (100, 1) NOT NULL,
    [source_client]       NVARCHAR (30)   NOT NULL,
    [site_url]            NVARCHAR (300)  NOT NULL,
    [short_description]   NVARCHAR (150)  NULL,
    [description]         NVARCHAR (1000) NOT NULL,
    [active]              BIT             NOT NULL,
    [xml_client_metadata] XML             NULL,
    [create_date]         DATETIME        NOT NULL,
    [update_date]         DATETIME        NULL,
    CONSTRAINT [PK_gpd_client_details] PRIMARY KEY CLUSTERED ([client_id] ASC),
    CONSTRAINT [IX_gpd_client_details] UNIQUE NONCLUSTERED ([source_client] ASC)
);

