CREATE TABLE [dbo].[gpd_project_item_material] (
    [project_item_id]              UNIQUEIDENTIFIER  NOT NULL,
    [material_id]           INT         NOT NULL,
    [product_manufacturer]  NVARCHAR(250) NULL,
    [product_model]         NVARCHAR(250) NULL,
    [product_name]          NVARCHAR(250) NULL,
    [type_name]             NVARCHAR(250) NULL,
    [xml_material_metadata] XML         NULL,
    [create_date]           DATETIME        NOT NULL,
    [update_date]           DATETIME        NULL,
    CONSTRAINT [PK_gpd_project_item_material] PRIMARY KEY CLUSTERED ([project_item_id], [material_id]),
    CONSTRAINT [FK_gpd_project_item_material_gpd_project_item] FOREIGN KEY ([project_item_id]) REFERENCES [dbo].[gpd_project_item] ([project_item_id])
);

