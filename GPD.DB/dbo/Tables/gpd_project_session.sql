CREATE TABLE [dbo].[gpd_project_session] (
    [project_id]                UNIQUEIDENTIFIER NULL,
    [type]                      NVARCHAR (100)   NULL,
    [platform]                  NVARCHAR (150)   NULL,
    [application_build]         NVARCHAR (150)   NULL,
    [application_client_ip]     NVARCHAR (50)    NULL,
    [application_name]          NVARCHAR (150)   NULL,
    [application_plugin_build]  NVARCHAR (150)   NULL,
    [application_plugin_source] NVARCHAR (150)   NULL,
	[application_plugin_name]   NVARCHAR (150)   NULL,
    [application_type]          NVARCHAR (100)   NULL,
    [application_username]      NVARCHAR (250)   NULL,
    [application_version]       NVARCHAR (150)   NULL,
	[user_info_email]			NVARCHAR (250)	 NULL,
	[user_info_fname]			NVARCHAR (150)	 NULL,
	[user_info_lname]			NVARCHAR (150)	 NULL,
    [create_date]               DATETIME         NOT NULL,
    [update_date]               DATETIME         NULL,
    CONSTRAINT [FK_gpd_project_session_gpd_project] FOREIGN KEY ([project_id]) REFERENCES [dbo].[gpd_project] ([project_id])
);



