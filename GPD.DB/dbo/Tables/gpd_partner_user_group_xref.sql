CREATE TABLE [dbo].[gpd_partner_user_group_xref] (
    [partner_id]  UNIQUEIDENTIFIER NOT NULL,
    [user_id]     INT              NOT NULL,
    [group_id]    INT              NOT NULL,
    [description] NVARCHAR (200)   NULL,
    [active]      BIT              NOT NULL,
    [create_date] DATETIME         NOT NULL,
    [update_date] DATETIME         NULL,
    CONSTRAINT [PK_gpd_partner_user_group_xref] PRIMARY KEY CLUSTERED ([partner_id] ASC, [user_id] ASC, [group_id] ASC),
    CONSTRAINT [FK_gpd_partner_user_group_xref_gpd_partner_details] FOREIGN KEY ([partner_id]) REFERENCES [dbo].[gpd_partner_details] ([partner_id]),
    CONSTRAINT [FK_gpd_partner_user_group_xref_gpd_user_details] FOREIGN KEY ([user_id]) REFERENCES [dbo].[gpd_user_details] ([user_id]),
    CONSTRAINT [FK_gpd_partner_user_group_xref_gpd_user_group] FOREIGN KEY ([group_id]) REFERENCES [dbo].[gpd_user_group] ([group_id])
);



