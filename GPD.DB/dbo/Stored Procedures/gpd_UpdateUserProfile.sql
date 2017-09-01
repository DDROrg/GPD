CREATE PROCEDURE [dbo].[gpd_UpdateUserProfile]
	@P_UserId INT,
	@P_XML XML,
	@P_Return_ErrorCode INT OUT,
	@P_Return_Message VARCHAR(1024) OUT
AS 
BEGIN
	SET NOCOUNT ON;
	/******************************
	*  Variable Declarations
	*******************************/
	DECLARE @V_firstName VARCHAR(150),
			@V_lastName VARCHAR(150),
			@P_email VARCHAR(150),
			@V_jobTitle VARCHAR(150),
			@V_phone VARCHAR(50),
			@V_password VARCHAR(300),
			@V_firmName VARCHAR(150),
			@V_firmUrl VARCHAR(250),
            @V_firmAddress1 VARCHAR(150),
            @V_firmAddress2 VARCHAR(150),
            @V_firmCity VARCHAR(150),
            @V_firmState VARCHAR(50),
            @V_firmZip VARCHAR(50),
			@V_firmDefaultIndustry VARCHAR(250);

	/******************************
	*  Initialize Variables
	*******************************/
	SET @P_Return_ErrorCode = @@ERROR;
	SET @P_Return_Message = '';
	
	BEGIN TRY
		
		WITH XMLNAMESPACES(DEFAULT 'http://www.gpd.com', 'http://www.w3.org/2001/XMLSchema-instance' AS i)
		SELECT @V_firstName = m.value('(firstName)[1]', 'NVARCHAR(150)'),
			@V_lastName = m.value('(lastName)[1]', 'NVARCHAR(150)'),
			@P_email = m.value('(email)[1]', 'NVARCHAR(150)'),
			@V_jobTitle = m.value('(jobTitle)[1][not(@*[local-name()="nil"])]', 'NVARCHAR(150)'),
			@V_phone = m.value('(phone)[1][not(@*[local-name()="nil"])]', 'NVARCHAR(50)'),
			@V_password = m.value('(password)[1][not(@*[local-name()="nil"])]', 'NVARCHAR(300)'),
			@V_firmName = m.value('(company/name)[1][not(@*[local-name()="nil"])]', 'NVARCHAR(150)'),
			@V_firmUrl = m.value('(company/website)[1][not(@*[local-name()="nil"])]', 'NVARCHAR(250)'),
            @V_firmAddress1 = m.value('(company/address)[1][not(@*[local-name()="nil"])]', 'NVARCHAR(150)'),
            @V_firmAddress2 = m.value('(company/address2)[1][not(@*[local-name()="nil"])]', 'NVARCHAR(150)'),
			@V_firmCity = m.value('(company/city)[1][not(@*[local-name()="nil"])]', 'NVARCHAR(50)'),
            @V_firmState = m.value('(company/state)[1][not(@*[local-name()="nil"])]', 'NVARCHAR(50)'),
            @V_firmZip = m.value('(company/postalCode)[1][not(@*[local-name()="nil"])]', 'NVARCHAR(50)'),
			@V_firmDefaultIndustry = m.value('(company/defaultIndustry)[1][not(@*[local-name()="nil"])]', 'NVARCHAR(250)')
		FROM @P_XML.nodes('/UserDetails') M(m);

		IF(LEN(ISNULL(@P_email, '')) > 0)
			BEGIN
				IF EXISTS(SELECT 1 FROM GPD_USER_DETAILS WHERE EMAIL = @P_email AND user_id <> @P_UserId)
					BEGIN
						SET @P_Return_ErrorCode = 0;
						SET @P_Return_Message = 'the provided e-mail address is already registered';
					END
			END;
		
		-- update user details
		IF (LEN(@P_Return_Message) = 0)
			BEGIN
				UPDATE GPD_USER_DETAILS 
				SET [first_name] = 
					CASE @V_firstName
						WHEN '' THEN [first_name]
						ELSE @V_firstName
					END,
				[last_name] = 
					CASE @V_lastName
						WHEN '' THEN [last_name]
						ELSE @V_lastName
					END,
				[email] = 
					CASE @P_email
						WHEN '' THEN [email]
						ELSE @P_email
					END,
				[job_title] = 
					CASE ISNULL(@V_jobTitle, 'NULL')
						WHEN 'NULL' THEN [job_title]
						ELSE @V_jobTitle
					END,
				[business_phone] =
					CASE ISNULL(@V_phone, 'NULL')
						WHEN 'NULL' THEN [business_phone]
						ELSE @V_phone
					END,
				[password] =
					CASE ISNULL(@V_password, 'NULL')
						WHEN 'NULL' THEN [password]
						WHEN '' THEN [password]
						ELSE @V_password
					END,
				update_date = getdate()

				WHERE user_id = @P_UserId;

				-- fullname update
				UPDATE GPD_USER_DETAILS 
				SET [full_name] = [first_name] + ' ' + [last_name],
					update_date = getdate()
				WHERE user_id = @P_UserId;
				
				IF EXISTS(SELECT firm_id
							FROM [gpd_user_details] u
								CROSS APPLY u.xml_user_metadata.nodes('//*[local-name()="additional-data"]/*[local-name()="item"][@type="firm-admin"]') AS x(r)
							WHERE u.user_id = @P_UserId
							AND firm_id IS NOT NULL AND r.value('.', 'int') = u.firm_id)
					BEGIN
						-- company update
						UPDATE [gpd_firm_profile]
						SET 
							[name] =
								CASE ISNULL(@V_firmName, 'NULL')
									WHEN 'NULL' THEN [name]
									WHEN '' THEN [name]
									ELSE @V_firmName
								END,
							[url] =
								CASE ISNULL(@V_firmUrl, 'NULL')
									WHEN 'NULL' THEN [url]
									WHEN '' THEN [url]
									ELSE @V_firmUrl
								END,
							[address_line_1] =
								CASE ISNULL(@V_firmAddress1, 'NULL')
									WHEN 'NULL' THEN [address_line_1]
									ELSE @V_firmAddress1
								END,
							[address_line_2] =
								CASE ISNULL(@V_firmAddress2, 'NULL')
									WHEN 'NULL' THEN [address_line_2]
									ELSE @V_firmAddress2
								END,
							[city] =
								CASE ISNULL(@V_firmCity, 'NULL')
									WHEN 'NULL' THEN [city]
									ELSE @V_firmCity
								END,
							[state_province] =
								CASE ISNULL(@V_firmState, 'NULL')
									WHEN 'NULL' THEN [state_province]
									ELSE @V_firmState
								END,
							[zip_postal_code] =
								CASE ISNULL(@V_firmZip, 'NULL')
									WHEN 'NULL' THEN [zip_postal_code]
									ELSE @V_firmZip
								END,
							[xml_firm_metadata] = 
								CASE ISNULL(@V_firmDefaultIndustry, 'NULL')
									WHEN 'NULL' THEN [xml_firm_metadata]
									ELSE (SELECT 'defaultIndustry' AS [item/@name], @V_firmDefaultIndustry AS [item] FOR XML PATH(''), ROOT ('list'))
									END,
							update_date = getdate()
						WHERE firm_id = (SELECT firm_id FROM GPD_USER_DETAILS WHERE user_id = @P_UserId)
					END;

			END;
	END TRY
	
	BEGIN CATCH
		SELECT @P_Return_ErrorCode = ERROR_NUMBER(), 
		@P_Return_Message = cast(ERROR_NUMBER() as varchar(20)) + ' line: '
			+ CAST(ERROR_LINE() as varchar(20)) + ' ' 
			+ ERROR_MESSAGE() + ' > ' 
			+ ERROR_PROCEDURE();
	END CATCH
END;