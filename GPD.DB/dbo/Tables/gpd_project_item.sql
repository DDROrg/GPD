CREATE TABLE [dbo].[gpd_project_item] (
    [item_id]              INT         IDENTITY (100, 1) NOT NULL,
    [project_id]           INT         NOT NULL,
    [type]                 NCHAR (100) NULL,
    [currency]             NCHAR (100) NULL,
    [family]               NCHAR (250) NULL,
    [product_id]           INT         NULL,
    [product_image_url]    NCHAR (500) NULL,
    [product_manufacturer] NCHAR (250) NULL,
    [product_model]        NCHAR (250) NULL,
    [product_name]         NCHAR (250) NULL,
    [product_url]          NCHAR (500) NULL,
    [xml_item_metadata]    XML         NULL,
    [create_date]          DATE        NOT NULL,
    [update_date]          DATE        NULL,
    CONSTRAINT [PK_gpd_project_item] PRIMARY KEY CLUSTERED ([item_id] ASC),
    CONSTRAINT [FK_gpd_project_item_gpd_project] FOREIGN KEY ([project_id]) REFERENCES [dbo].[gpd_project] ([project_id])
);

