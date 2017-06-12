CREATE PROCEDURE gpd_AddUserDetails
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

	/******************************
	*  Initialize Variables
	*******************************/
	SET @P_Return_ErrorCode = @@ERROR;
	SET @P_Return_Message = '';
	
	BEGIN TRY
		SET @P_Return_UserId = -1;

		-- user email address
		WITH XMLNAMESPACES(DEFAULT 'http://schemas.datacontract.org/2004/07/GPD.ServiceEntities', 'http://www.w3.org/2001/XMLSchema-instance' AS i)
			SELECT  @V_UserEmail = M.value('(Email)[1]', 'NVARCHAR(150)')
			FROM @P_XML.nodes('/UserRegistrationDTO') M(M);

		IF EXISTS(SELECT 1 FROM GPD_USER_DETAILS WHERE EMAIL = @V_UserEmail)
			BEGIN
				SET @P_Return_ErrorCode = 0;
				SET @P_Return_Message = 'the provided e-mail address is already registered';
			END
		ELSE
			BEGIN
				-- USER DATA
				WITH XMLNAMESPACES(DEFAULT 'http://schemas.datacontract.org/2004/07/GPD.ServiceEntities', 'http://www.w3.org/2001/XMLSchema-instance' AS i)
				INSERT INTO GPD_USER_DETAILS
					([last_name]
					,[first_name]
					,[full_name]
					,[email]
					,[password]
 					,[company]
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
					M.value('(LastName)[1]', 'NVARCHAR(150)')
					,M.value('(FirstName)[1]', 'NVARCHAR(150)')
					,M.value('(FirstName)[1]', 'NVARCHAR(150)') + ' ' + M.value('(LastName)[1]', 'NVARCHAR(150)')
					,M.value('(Email)[1]', 'NVARCHAR(150)')					
					,M.value('(Password)[1]', 'NVARCHAR(150)')
					,M.value('(Company)[1]', 'NVARCHAR(150)')
					,M.value('(JobTitle)[1]', 'NVARCHAR(150)')
					,M.value('(BusinessPhone)[1]', 'NVARCHAR(50)')
					,M.value('(HomePhone)[1]', 'NVARCHAR(50)')
					,M.value('(MobilePhone)[1]', 'NVARCHAR(50)')
					,M.value('(Fax)[1]', 'NVARCHAR(50)')
					,M.value('(AddressLine1)[1]', 'NVARCHAR(150)')
					,M.value('(AddressLine2)[1]', 'NVARCHAR(150)')
					,M.value('(City)[1]', 'NVARCHAR(150)')
					,M.value('(State)[1]', 'NVARCHAR(50)')
					,M.value('(Zip)[1]', 'NVARCHAR(50)')
					,M.value('(Country)[1]', 'NVARCHAR(50)')
					,@P_IpAddress
					,1
					,null
					,getdate(), null
				FROM @P_XML.nodes('/UserRegistrationDTO') M(M);
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