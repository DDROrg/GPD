CREATE TABLE [dbo].[gpd_materials] (
    [material_id]           INT         IDENTITY (100, 1) NOT NULL,
    [item_id]               INT         NOT NULL,
    [product_manufacturer]  NCHAR (250) NULL,
    [product_model]         NCHAR (250) NULL,
    [product_name]          NCHAR (250) NULL,
    [type_name]             NCHAR (250) NULL,
    [xml_material_metadata] XML         NULL,
    [create_date]           DATE        NOT NULL,
    [update_date]           DATE        NULL,
    CONSTRAINT [PK_gpd_materials] PRIMARY KEY CLUSTERED ([material_id] ASC),
    CONSTRAINT [FK_gpd_materials_gpd_project_item] FOREIGN KEY ([item_id]) REFERENCES [dbo].[gpd_project_item] ([item_id])
);

