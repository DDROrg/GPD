CREATE TABLE [dbo].[gpd_client_user_group_xref] (
    [client_id]   UNIQUEIDENTIFIER NOT NULL,
    [user_id]     INT              NOT NULL,
    [group_id]    INT              NOT NULL,
    [description] NVARCHAR (200)   NULL,
    [active]      BIT              NOT NULL,
    [create_date] DATETIME         NOT NULL,
    [update_date] DATETIME         NULL,
    CONSTRAINT [PK_gpd_client_user_group_xref] PRIMARY KEY CLUSTERED ([client_id] ASC, [group_id] ASC, [user_id] ASC),
    CONSTRAINT [FK_gpd_client_user_group_xref_gpd_client_details] FOREIGN KEY ([client_id]) REFERENCES [dbo].[gpd_client_details] ([client_id]),
    CONSTRAINT [FK_gpd_client_user_group_xref_gpd_user_details] FOREIGN KEY ([user_id]) REFERENCES [dbo].[gpd_user_details] ([user_id]),
    CONSTRAINT [FK_gpd_client_user_group_xref_gpd_user_group] FOREIGN KEY ([group_id]) REFERENCES [dbo].[gpd_user_group] ([group_id])
);





