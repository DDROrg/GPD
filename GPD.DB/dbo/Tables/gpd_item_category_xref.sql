CREATE TABLE [dbo].[gpd_item_category_xref] (
    [item_id]     INT  NOT NULL,
    [category_id] INT  NOT NULL,
    [create_date] DATETIME NOT NULL,
    [update_date] DATETIME NULL,
    CONSTRAINT [PK_gpd_item_category_xref] PRIMARY KEY CLUSTERED ([item_id] ASC, [category_id] ASC),
    CONSTRAINT [FK_gpd_item_category_xref_gpd_categories] FOREIGN KEY ([category_id]) REFERENCES [dbo].[gpd_categories] ([category_id]),
    CONSTRAINT [FK_gpd_item_category_xref_gpd_project_item] FOREIGN KEY ([item_id]) REFERENCES [dbo].[gpd_project_item] ([item_id])
);

