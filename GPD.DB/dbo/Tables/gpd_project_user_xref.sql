﻿CREATE TABLE [dbo].[gpd_project_user_xref] (
    [project_id]  UNIQUEIDENTIFIER NOT NULL,
    [user_id]     INT              NOT NULL,
    [create_date] DATETIME         NOT NULL,
    [update_date] DATETIME         NULL,
    CONSTRAINT [FK_gpd_project_user_xref_gpd_project] FOREIGN KEY ([project_id]) REFERENCES [dbo].[gpd_project] ([project_id])
);





