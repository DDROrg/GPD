CREATE PROCEDURE [dbo].[gpd_GetPartnerListAccessTo]
	   @P_UserId int,
	   @P_ApiKeyId nvarchar(50),
	   @P_PartnerName nvarchar(30)
AS
BEGIN

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements. 
	SET NOCOUNT ON;
	
	DECLARE @V_ALL_PARTNER_NAME varchar(30) = 'ALL';
	DECLARE @V_ADMIN_GROUP_NAME varchar(30) = 'GPD Admin';
	DECLARE @t_Records TABLE (partner_name varchar(50));

	IF(@P_ApiKeyId IS NOT NULL)
		BEGIN
			IF EXISTS(SELECT 1
					FROM gpd_partner_api_access PAA, gpd_partner_details P
					WHERE PAA.api_key_id = @P_ApiKeyId
					AND (PAA.[start_date] IS NULL OR PAA.[start_date] < getdate())
					AND (PAA.[exp_date] IS NULL OR PAA.[exp_date] > getdate())
					AND P.partner_id = PAA.partner_id AND P.active = 1 AND P.name = 'ALL')
				BEGIN
					INSERT INTO @t_Records
						SELECT [name]
						FROM gpd_partner_details P
						WHERE P.active = 1;
				END
			ELSE
				BEGIN
					INSERT INTO @t_Records
						SELECT [name]
						FROM gpd_partner_api_access PAA, gpd_partner_details P
						WHERE PAA.api_key_id = @P_ApiKeyId
						AND (PAA.[start_date] IS NULL OR PAA.[start_date] < getdate())
						AND (PAA.[exp_date] IS NULL OR PAA.[exp_date] > getdate())
						AND P.partner_id = PAA.partner_id
						AND P.active = 1;
				END
		END

	ELSE IF(@P_UserId IS NOT NULL)
		BEGIN
			IF EXISTS(SELECT 1
					FROM gpd_partner_user_group_xref X, gpd_partner_details P, gpd_user_group G
					WHERE X.user_id = @P_UserId AND X.active = 1
					-- partner_details
					AND P.partner_id = X.partner_id AND P.name = 'ALL' AND P.active = 1
					-- user_group
					AND G.group_id  = X.group_id AND G.name = 'GPD Admin' AND G.active = 1)
				BEGIN
					INSERT INTO @t_Records
						SELECT [name]
						FROM gpd_partner_details P
						WHERE P.active = 1;
			END
		ELSE
			BEGIN
				INSERT INTO @t_Records
					SELECT [name]
					FROM gpd_partner_user_group_xref X, gpd_partner_details P
					WHERE X.user_id = @P_UserId AND X.active = 1
					AND P.partner_id = X.partner_id 
					AND X.active = 1;
			END
	END

	SELECT partner_name 
	FROM @t_Records
	WHERE (@P_PartnerName = 'ALL' OR partner_name = @P_PartnerName);

END;