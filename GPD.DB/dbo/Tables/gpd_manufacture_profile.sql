CREATE TABLE [dbo].[gpd_manufacture_profile] (
    [manufacture_id]           INT            IDENTITY (100, 1) NOT NULL,
    [name]                     NVARCHAR (150) NULL,
    [url]                      NVARCHAR (250) NULL,
    [logo]                     NVARCHAR (250) NULL,
    [address_line_1]           NVARCHAR (150) NULL,
    [address_line_2]           NVARCHAR (150) NULL,
    [city]                     NVARCHAR (150) NULL,
    [state_province]           NVARCHAR (50)  NULL,
    [zip_postal_code]          NVARCHAR (50)  NULL,
    [country]                  NVARCHAR (50)  NULL,
    [active]                   BIT            NOT NULL,
    [xml_manufacture_metadata] XML            NULL,
    [create_date]              DATETIME       NOT NULL,
    [update_date]              DATETIME       NULL,
    CONSTRAINT [PK_gpd_manufacture_profile] PRIMARY KEY CLUSTERED ([manufacture_id] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_gpd_manufacture_profile_name]
    ON [dbo].[gpd_manufacture_profile]([name] ASC);

