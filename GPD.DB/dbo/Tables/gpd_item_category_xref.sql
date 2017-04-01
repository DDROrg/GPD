CREATE TABLE [dbo].[gpd_item_category_xref] (
	[project_item_id] UNIQUEIDENTIFIER  NOT NULL,
    [category_id] UNIQUEIDENTIFIER  NOT NULL,
    [create_date] DATETIME NOT NULL,
    [update_date] DATETIME NULL,
    CONSTRAINT [PK_gpd_item_category_xref] PRIMARY KEY CLUSTERED ([project_item_id], [category_id]),
    CONSTRAINT [FK_gpd_item_category_xref_gpd_categories] FOREIGN KEY ([category_id]) REFERENCES [dbo].[gpd_category] ([category_id]),
    CONSTRAINT [FK_gpd_item_category_xref_gpd_project_item] FOREIGN KEY ([project_item_id]) REFERENCES [dbo].[gpd_project_item] ([project_item_id])
);

