CREATE TABLE [dbo].[gpd_category] (
    [category_id]           UNIQUEIDENTIFIER   NOT NULL,
    [taxonomy]              NVARCHAR(150) NOT NULL,
    [title]                 NVARCHAR(250) NOT NULL,
    [active]                BIT         NOT NULL,
    [xml_taxonomy_metadata] XML         NULL,
    [create_date]           DATETIME        NOT NULL,
    [update_date]           DATETIME        NULL,
    CONSTRAINT [PK_gpd_category] PRIMARY KEY CLUSTERED ([category_id] ASC)
);

