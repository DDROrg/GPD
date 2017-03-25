CREATE TABLE [dbo].[gpd_project] (
    [project_id]               INT         IDENTITY (100, 1) NOT NULL,
    [source_client]            NCHAR (30)  NULL,
    [author]                   NCHAR (250) NULL,
    [building_name]            NCHAR (250) NULL,
    [client]                   NCHAR (250) NULL,
    [filename]                 NCHAR (250) NULL,
    [name]                     NCHAR (250) NULL,
    [number]                   NCHAR (250) NULL,
    [organization_description] NCHAR (250) NULL,
    [organization_name]        NCHAR (250) NULL,
    [status]                   NCHAR (250) NULL,
    [xml_metadata]             XML         NULL,
    [create_date]              DATE        NOT NULL,
    [update_date]              DATE        NULL,
    CONSTRAINT [PK_gpd_project] PRIMARY KEY CLUSTERED ([project_id] ASC)
);

