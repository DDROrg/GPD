﻿CREATE TABLE [dbo].[gpd_project] (
    [project_id]               UNIQUEIDENTIFIER NOT NULL,
    [partner_name]             NVARCHAR (30)    NULL,
    [author]                   NVARCHAR (250)   NULL,
    [building_name]            NVARCHAR (250)   NULL,
    [client]                   NVARCHAR (250)   NULL,
    [filename]                 NVARCHAR (250)   NULL,
    [name]                     NVARCHAR (250)   NULL,
    [number]                   NVARCHAR (250)   NULL,
    [organization_description] NVARCHAR (250)   NULL,
    [organization_name]        NVARCHAR (250)   NULL,
    [status]                   NVARCHAR (250)   NULL,
	[active]				   BIT              NOT NULL DEFAULT 1,
	[deleted]				   BIT              NOT NULL DEFAULT 0,
    [xml_project_metadata]     XML              NULL,
    [create_date]              DATETIME         NOT NULL,
    [update_date]              DATETIME         NULL,
    CONSTRAINT [PK_gpd_project] PRIMARY KEY CLUSTERED ([project_id] ASC)
);





