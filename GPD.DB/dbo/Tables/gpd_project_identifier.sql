CREATE TABLE [dbo].[gpd_project_identifier] (
    [project_id]  INT         NOT NULL,
    [identifier]  NCHAR (250) NOT NULL,
    [system]      NCHAR (150) NULL,
    [create_date] DATE        NOT NULL,
    [update_date] NCHAR (10)  NULL,
    CONSTRAINT [FK_gpd_project_identifier_gpd_project] FOREIGN KEY ([project_id]) REFERENCES [dbo].[gpd_project] ([project_id])
);

