CREATE TABLE [dbo].[gpd_partner_api_access] (
    [partner_id]  UNIQUEIDENTIFIER NOT NULL,
    [api_key_id]  NVARCHAR (50)    NOT NULL,
    [start_date]  DATETIME         NULL,
    [exp_date]    DATETIME         NULL,
    [create_date] DATETIME         NOT NULL,
    [update_date] DATETIME         NULL,
    CONSTRAINT [FK_gpd_partner_api_access_gpd_partner_details] FOREIGN KEY ([partner_id]) REFERENCES [dbo].[gpd_partner_details] ([partner_id])
);

