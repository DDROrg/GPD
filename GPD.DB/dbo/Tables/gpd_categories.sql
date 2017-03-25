CREATE TABLE [dbo].[gpd_categories] (
    [category_id]           INT         IDENTITY (100, 1) NOT NULL,
    [taxonomy]              NCHAR (150) NOT NULL,
    [title]                 NCHAR (250) NOT NULL,
    [active]                BIT         NOT NULL,
    [xml_taxonomy_metadata] XML         NULL,
    [create_date]           DATE        NOT NULL,
    [update_date]           DATE        NULL,
    CONSTRAINT [PK_gpd_categories] PRIMARY KEY CLUSTERED ([category_id] ASC)
);

