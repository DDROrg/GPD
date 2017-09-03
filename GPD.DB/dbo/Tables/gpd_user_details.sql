CREATE TABLE [dbo].[gpd_user_details] (
    [user_id]           INT            IDENTITY (100, 1) NOT NULL,
    [last_name]         NVARCHAR (150) NULL,
    [first_name]        NVARCHAR (150) NULL,
    [full_name]         NVARCHAR (310) NULL,
    [email]             NVARCHAR (150) NOT NULL,
    [password]          NVARCHAR (300) NULL,
    [firm_id]           INT            NULL,
    [manufacture_id]    INT            NULL,
    [job_title]         NVARCHAR (150) NULL,
    [business_phone]    NVARCHAR (50)  NULL,
    [home_phone]        NVARCHAR (50)  NULL,
    [mobile_phone]      NVARCHAR (50)  NULL,
    [fax_number]        NVARCHAR (50)  NULL,
    [address_line_1]    NVARCHAR (150) NULL,
    [address_line_2]    NVARCHAR (150) NULL,
    [city]              NVARCHAR (150) NULL,
    [state_province]    NVARCHAR (50)  NULL,
    [zip_postal_code]   NVARCHAR (50)  NULL,
    [country]           NVARCHAR (50)  NULL,
    [ip_address]        NVARCHAR (100) NULL,
    [active]            BIT            NOT NULL,
    [xml_user_metadata] XML            NULL,
    [create_date]       DATETIME       NOT NULL,
    [update_date]       DATETIME       NULL,
    CONSTRAINT [PK_gpd_user_details] PRIMARY KEY CLUSTERED ([user_id] ASC),
    CONSTRAINT [FK_gpd_user_details_gpd_firm_profile] FOREIGN KEY ([firm_id]) REFERENCES [dbo].[gpd_firm_profile] ([firm_id]),
    CONSTRAINT [FK_gpd_user_details_gpd_manufacture_profile] FOREIGN KEY ([manufacture_id]) REFERENCES [dbo].[gpd_manufacture_profile] ([manufacture_id]),
    CONSTRAINT [IX_gpd_user_details] UNIQUE NONCLUSTERED ([email] ASC)
);









