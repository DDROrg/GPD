CREATE TABLE [dbo].[gpd_project] (
    [project_id]               INT         IDENTITY (100, 1) NOT NULL,
    [source_client]            NVARCHAR(30)  NULL,
    [author]                   nvarchar (250) NULL,
    [building_name]            nvarchar (250) NULL,
    [client]                   nvarchar (250) NULL,
    [filename]                 nvarchar (250) NULL,
    [name]                     nvarchar (250) NULL,
    [number]                   nvarchar (250) NULL,
    [organization_description] nvarchar (250) NULL,
    [organization_name]        nvarchar (250) NULL,
    [status]                   NVARCHAR(250) NULL,
    [xml_metadata]             XML         NULL,
    [create_date]              DATETIME        NOT NULL,
    [update_date]              DATETIME        NULL,
    CONSTRAINT [PK_gpd_project] PRIMARY KEY CLUSTERED ([project_id] ASC)
);

