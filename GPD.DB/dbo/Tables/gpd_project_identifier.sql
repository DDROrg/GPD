CREATE TABLE [dbo].[gpd_project_identifier] (
    [project_id]  UNIQUEIDENTIFIER         NOT NULL,
    [identifier]  UNIQUEIDENTIFIER NOT NULL,
    [system]      nvarchar (150) NULL,
    [create_date] DATETIME        NOT NULL,
    [update_date] DATETIME  NULL,
    CONSTRAINT [FK_gpd_project_identifier_gpd_project] FOREIGN KEY ([project_id]) REFERENCES [dbo].[gpd_project] ([project_id])
);

