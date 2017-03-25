CREATE TABLE [dbo].[gpd_project_location] (
    [project_id]     INT         NOT NULL,
    [type]           NCHAR (100) NOT NULL,
    [address_line_1] NCHAR (250) NULL,
    [address_line_2] NCHAR (250) NULL,
    [city]           NCHAR (250) NULL,
    [state]          NCHAR (250) NULL,
    [zip]            NCHAR (150) NULL,
    [country]        NCHAR (150) NULL,
    [create_date]    DATE        NOT NULL,
    [update_date]    DATE        NULL,
    CONSTRAINT [FK_gpd_project_location_gpd_project] FOREIGN KEY ([project_id]) REFERENCES [dbo].[gpd_project] ([project_id])
);

