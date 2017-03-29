CREATE TABLE [dbo].[gpd_materials] (
    [item_id]               UNIQUEIDENTIFIER NOT NULL,
    [id]                    INT              NULL,
    [product_manufacturer]  NVARCHAR (250)   NULL,
    [product_model]         NVARCHAR (250)   NULL,
    [product_name]          NVARCHAR (250)   NULL,
    [type_name]             NVARCHAR (250)   NULL,
    [xml_material_metadata] XML              NULL,
    [create_date]           DATETIME         NOT NULL,
    [update_date]           DATETIME         NULL,
    CONSTRAINT [FK_gpd_materials_gpd_project_item] FOREIGN KEY ([item_id]) REFERENCES [dbo].[gpd_project_item] ([item_id])
);



