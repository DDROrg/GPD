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
			@V_password VARCHAR(300);

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
			@V_password = m.value('(password)[1][not(@*[local-name()="nil"])]', 'NVARCHAR(300)')
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
					CASE @V_jobTitle
						WHEN NULL THEN [job_title]
						ELSE @V_jobTitle
					END,
				[business_phone] =
					CASE @V_phone
						WHEN NULL THEN [business_phone]
						ELSE @V_phone
					END,
				[password] =
					CASE @V_password
						WHEN NULL THEN [password]
						ELSE @V_password
					END,
				update_date = getdate()

				WHERE user_id = @P_UserId;

				-- fullname update
				UPDATE GPD_USER_DETAILS 
				SET [full_name] = [first_name] + ' ' + [last_name],
					update_date = getdate()
				WHERE user_id = @P_UserId;

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