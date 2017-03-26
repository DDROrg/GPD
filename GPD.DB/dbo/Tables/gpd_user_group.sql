CREATE TABLE [dbo].[gpd_user_group] (
    [group_id]           INT            IDENTITY (100, 1) NOT NULL,
    [name]               NVARCHAR (30)  NOT NULL,
    [description]        NVARCHAR (200) NOT NULL,
    [active]             BIT            NOT NULL,
    [xml_group_metadata] XML            NULL,
    [create_date]        DATETIME       NOT NULL,
    [update_date]        DATETIME       NULL,
    CONSTRAINT [PK_gpd_user_group] PRIMARY KEY CLUSTERED ([group_id] ASC)
);


