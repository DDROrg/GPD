CREATE TABLE [dbo].[gpd_project_location] (
    [project_id]     INT         NOT NULL,
    [type]           NVARCHAR (100) NOT NULL,
    [address_line_1] NVARCHAR (250) NULL,
    [address_line_2] NVARCHAR (250) NULL,
    [city]           NVARCHAR (250) NULL,
    [state]          NVARCHAR (250) NULL,
    [zip]            NVARCHAR (150) NULL,
    [country]        NVARCHAR (150) NULL,
    [create_date]    DATETIME        NOT NULL,
    [update_date]    DATETIME        NULL,
    CONSTRAINT [FK_gpd_project_location_gpd_project] FOREIGN KEY ([project_id]) REFERENCES [dbo].[gpd_project] ([project_id])
);

