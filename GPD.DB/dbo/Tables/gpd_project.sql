CREATE TABLE [dbo].[gpd_project] (
    [project_id]               INT            IDENTITY (100, 1) NOT NULL,
    [source_client]            NVARCHAR (30)  NULL,
    [author]                   NVARCHAR (250) NULL,
    [building_name]            NVARCHAR (250) NULL,
    [client]                   NVARCHAR (250) NULL,
    [filename]                 NVARCHAR (250) NULL,
    [name]                     NVARCHAR (250) NULL,
    [number]                   NVARCHAR (250) NULL,
    [organization_description] NVARCHAR (250) NULL,
    [organization_name]        NVARCHAR (250) NULL,
    [status]                   NVARCHAR (250) NULL,
    [xml_project_metadata]     XML            NULL,
    [create_date]              DATETIME       NOT NULL,
    [update_date]              DATETIME       NULL,
    CONSTRAINT [PK_gpd_project] PRIMARY KEY CLUSTERED ([project_id] ASC)
);



