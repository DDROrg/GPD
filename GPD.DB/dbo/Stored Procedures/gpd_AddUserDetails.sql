CREATE PROCEDURE [dbo].[gpd_AddUserDetails]
	@P_XML XML,
	@P_IpAddress VARCHAR(50),
	@P_Return_UserId INT OUT,
	@P_Return_ErrorCode INT OUT,
	@P_Return_Message VARCHAR(1024) OUT
AS 
BEGIN
	SET NOCOUNT ON;
	/******************************
	*  Variable Declarations
	*******************************/
	DECLARE @V_UserEmail VARCHAR(150);
	DECLARE @V_FirmId INT = -1;
	DECLARE @V_FirmName VARCHAR(150);
	DECLARE @V_FirmUrl VARCHAR(250);	

	/******************************
	*  Initialize Variables
	*******************************/
	SET @P_Return_ErrorCode = @@ERROR;
	SET @P_Return_Message = '';
	
	BEGIN TRY
		SET @P_Return_UserId = -1;

		-- user email address
		WITH XMLNAMESPACES(DEFAULT 'http://www.gpd.com', 'http://www.w3.org/2001/XMLSchema-instance' AS i)
			SELECT  @V_UserEmail = M.value('(email)[1]', 'NVARCHAR(150)'),
				@V_FirmName = M.value('(company/name)[1]', 'NVARCHAR(150)'),
				@V_FirmUrl = M.value('(company/website)[1]', 'NVARCHAR(250)')
			FROM @P_XML.nodes('/UserDetails') M(M);

		IF EXISTS(SELECT 1 FROM GPD_USER_DETAILS WHERE EMAIL = @V_UserEmail)
			BEGIN
				SET @P_Return_ErrorCode = 0;
				SET @P_Return_Message = 'the provided e-mail address is already registered';
			END
		ELSE
		 
		    BEGIN

			   IF(LEN(@V_FirmName) > 0)
			      BEGIN
				     Exec gpd_AddFirmProfile @P_XML, @V_FirmId OUTPUT, @P_Return_ErrorCode OUTPUT, @P_Return_Message OUTPUT;
				  END;
			   ELSE
			      BEGIN
				     SET @V_FirmId = NULL;
				  END;

				-- USER DATA
				WITH XMLNAMESPACES(DEFAULT 'http://www.gpd.com', 'http://www.w3.org/2001/XMLSchema-instance' AS i)
				INSERT INTO GPD_USER_DETAILS
					([last_name]
					,[first_name]
					,[full_name]
					,[email]
					,[password]
 					,[firm_id]
					,[manufacture_id]
 					,[job_title]
 					,[business_phone]
 					,[home_phone]
 					,[mobile_phone]
 					,[fax_number]
 					,[address_line_1]
 					,[address_line_2]
 					,[city]
 					,[state_province]
 					,[zip_postal_code]
 					,[country]
 					,[ip_address]
 					,[active]
 					,[xml_user_metadata]
 					,[create_date]
 					,[update_date])
				SELECT			
					M.value('(lastName)[1]', 'NVARCHAR(150)')
					,M.value('(firstName)[1]', 'NVARCHAR(150)')
					,M.value('(firstName)[1]', 'NVARCHAR(150)') + ' ' + M.value('(lastName)[1]', 'NVARCHAR(150)')
					,M.value('(email)[1]', 'NVARCHAR(150)')					
					,M.value('(password)[1]', 'NVARCHAR(150)')
					,@V_FirmId
					,NULL
					,M.value('(jobTitle)[1]', 'NVARCHAR(150)')
					
					--,M.value('(BusinessPhone)[1]', 'NVARCHAR(50)')
					,NULL

					--,M.value('(HomePhone)[1]', 'NVARCHAR(50)')
					,NULL

					--,M.value('(MobilePhone)[1]', 'NVARCHAR(50)')
					,NULL

					--,M.value('(Fax)[1]', 'NVARCHAR(50)')
					,NULL

					--,M.value('(AddressLine1)[1]', 'NVARCHAR(150)')
					,NULL

					--,M.value('(AddressLine2)[1]', 'NVARCHAR(150)')
					,NULL

					--,M.value('(City)[1]', 'NVARCHAR(150)')
					,NULL

					--,M.value('(State)[1]', 'NVARCHAR(50)')
					,NULL

					--,M.value('(Zip)[1]', 'NVARCHAR(50)')
					,NULL

					--,M.value('(Country)[1]', 'NVARCHAR(50)')
					,NULL

					,@P_IpAddress
					,1
					,M.query('declare default element namespace "http://www.gpd.com";/UserDetails/additional-data')
					,getdate()
					,NULL
				FROM @P_XML.nodes('/UserDetails') M(M);
				SET @P_Return_UserId = SCOPE_IDENTITY()
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