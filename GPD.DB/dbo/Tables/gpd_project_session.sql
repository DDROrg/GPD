CREATE TABLE [dbo].[gpd_project_session] (
    [project_id]                INT         NULL,
    [type]                      NCHAR (100) NULL,
    [platform]                  NCHAR (150) NULL,
    [application_build]         NCHAR (150) NULL,
    [application_name]          NCHAR (150) NULL,
    [application_plugin_build]  NCHAR (150) NULL,
    [application_plugin_source] NCHAR (150) NULL,
    [application_version]       NCHAR (150) NULL,
    [application_type]          NCHAR (100) NULL,
    [create_date]               DATE        NOT NULL,
    [update_date]               DATE        NULL,
    CONSTRAINT [FK_gpd_project_session_gpd_project] FOREIGN KEY ([project_id]) REFERENCES [dbo].[gpd_project] ([project_id])
);

