CREATE TABLE [dbo].[gpd_partner_details] (
    [partner_id]           UNIQUEIDENTIFIER NOT NULL,
    [name]                 NVARCHAR (30)    NOT NULL,
    [site_url]             NVARCHAR (300)   NOT NULL,
    [short_description]    NVARCHAR (150)   NULL,
    [description]          NVARCHAR (1000)  NOT NULL,
    [active]               BIT              NOT NULL,
    [xml_partner_metadata] XML              NULL,
    [create_date]          DATETIME         NOT NULL,
    [update_date]          DATETIME         NULL,
    CONSTRAINT [PK_gpd_partner_details] PRIMARY KEY CLUSTERED ([partner_id] ASC),
    CONSTRAINT [IX_gpd_partner_details] UNIQUE NONCLUSTERED ([name] ASC)
);

