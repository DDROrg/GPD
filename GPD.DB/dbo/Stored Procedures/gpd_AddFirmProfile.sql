CREATE PROCEDURE [dbo].[gpd_AddFirmProfile]
	@P_XML XML,
	@P_Return_FirmId INT OUT,
	@P_Return_FirmAdmin BIT OUT,
	@P_Return_ErrorCode INT OUT,
	@P_Return_Message VARCHAR(1024) OUT
AS 
BEGIN
	SET NOCOUNT ON;
	/******************************
	*  Variable Declarations
	*******************************/
	DECLARE @V_FirmName VARCHAR(150);
	DECLARE @V_FirmUrl VARCHAR(250);

	/******************************
	*  Initialize Variables
	*******************************/
	SET @P_Return_ErrorCode = @@ERROR
	SET @P_Return_Message = ''
	SET @P_Return_FirmId = -1
	SET @P_Return_FirmAdmin = 0
	
	BEGIN TRY	

		-- user email address
		SELECT
			@V_FirmName = M.value('(./*[local-name()="name"])[1]', 'NVARCHAR(150)'),
			@V_FirmUrl = M.value('(./*[local-name()="website"])[1]', 'NVARCHAR(250)')
		FROM @P_XML.nodes('//*[local-name()="company"]') M(M);
			

		IF EXISTS(SELECT 1 FROM GPD_FIRM_PROFILE F WHERE F.name = @V_FirmName AND F.url = @V_FirmUrl)
			BEGIN
				SELECT @P_Return_FirmId = F.firm_id FROM GPD_FIRM_PROFILE F 
				WHERE F.name = @V_FirmName AND F.url = @V_FirmUrl;
				
				SET @P_Return_ErrorCode = 0;
			END
		ELSE
		 
		    BEGIN

			   -- USER DATA
				WITH XMLNAMESPACES(DEFAULT 'http://www.gpd.com', 'http://www.w3.org/2001/XMLSchema-instance' AS i)
				INSERT INTO gpd_firm_profile(
					[name]
					,[url]
					,[logo]
					,[address_line_1]
					,[address_line_2]
					,[city]
					,[state_province]
					,[zip_postal_code]
					,[country]
					,[active]
					,[xml_firm_metadata]
					,[create_date]
					,[update_date])
				SELECT			
					M.value('(./*[local-name()="name"])[1]', 'NVARCHAR(150)')
					,M.value('(./*[local-name()="website"])[1]', 'NVARCHAR(250)')
					,NULL
					,M.value('(./*[local-name()="address"])[1]', 'NVARCHAR(250)')
					,M.value('(./*[local-name()="address2"])[1]', 'NVARCHAR(250)')
					,M.value('(./*[local-name()="city"])[1]', 'NVARCHAR(250)')
					,M.value('(./*[local-name()="state"])[1]', 'NVARCHAR(250)')
					,M.value('(./*[local-name()="postalCode"])[1]', 'NVARCHAR(250)')
					,M.value('(./*[local-name()="country"])[1]', 'NVARCHAR(250)')
					
					,1
					,'<list><item name="defaultIndustry">' + M.value('(./*[local-name()="defaultIndustry"])[1]', 'NVARCHAR(150)') + '</item></list>'
					,getdate()
					,NULL
				FROM @P_XML.nodes('//*[local-name()="company"]') M(M);

				SET @P_Return_FirmId = SCOPE_IDENTITY();
				SET @P_Return_FirmAdmin = 1;
				SET NOCOUNT OFF
			END

	END TRY
	
	BEGIN CATCH
		SELECT @P_Return_ErrorCode = ERROR_NUMBER(), 
		@P_Return_Message = cast(ERROR_NUMBER() as varchar(20)) + ' line: '
			+ CAST(ERROR_LINE() as varchar(20)) + ' ' 
			+ ERROR_MESSAGE() + ' > ' 
			+ ERROR_PROCEDURE();
	END CATCH
END;