CREATE TABLE [dbo].[gpd_project_item] (
    [item_id]              INT         IDENTITY (100, 1) NOT NULL,
    [project_id]           INT         NOT NULL,
    [type]                 NVARCHAR (100) NULL,
    [currency]             NVARCHAR (100) NULL,
    [family]               NVARCHAR (250) NULL,
    [product_id]           INT         NULL,
    [product_image_url]    NVARCHAR (500) NULL,
    [product_manufacturer] NVARCHAR (250) NULL,
    [product_model]        NVARCHAR (250) NULL,
    [product_name]         NVARCHAR (250) NULL,
    [product_url]          NVARCHAR (500) NULL,
    [xml_item_metadata]    XML         NULL,
    [create_date]          DATETIME        NOT NULL,
    [update_date]          DATETIME        NULL,
    CONSTRAINT [PK_gpd_project_item] PRIMARY KEY CLUSTERED ([item_id] ASC),
    CONSTRAINT [FK_gpd_project_item_gpd_project] FOREIGN KEY ([project_id]) REFERENCES [dbo].[gpd_project] ([project_id])
);

