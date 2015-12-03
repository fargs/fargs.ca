
CREATE PROC [API].[AspNetUsers_Update]
	 @Id nvarchar(128)
	,@Email nvarchar(256)
	,@EmailConfirmed bit
	,@PhoneNumber nvarchar(128)
	,@PhoneNumberConfirmed bit
	,@TwoFactorEnabled bit
	,@LockoutEndDateUtc datetime
	,@LockoutEnabled bit
	,@AccessFailedCount int
	,@UserName nvarchar(256)
	,@Title nvarchar(50)
	,@FirstName nvarchar(128)
	,@LastName nvarchar(128)
	,@EmployeeId nvarchar(50)
	,@CompanyId smallint
	,@CompanyName nvarchar(200)
	,@ModifiedUser nvarchar(256)
	,@LastActivationDate datetime
	,@IsTestRecord bit
AS
	
	UPDATE dbo.AspNetUsers
	SET  [Id] = @Id
		,[Email] = @Email
		,[EmailConfirmed] = @EmailConfirmed
		,[PhoneNumber] = @PhoneNumber
		,[PhoneNumberConfirmed] = @PhoneNumberConfirmed
		,[TwoFactorEnabled] = @TwoFactorEnabled
		,[LockoutEndDateUtc] = @LockoutEndDateUtc
		,[LockoutEnabled] = @LockoutEnabled
		,[AccessFailedCount] = @AccessFailedCount
		,[UserName] = @UserName
		,[Title] = @Title
		,[FirstName] = @FirstName
		,[LastName] = @LastName
		,[EmployeeId] = @EmployeeId
		,[CompanyId] = @CompanyId
		,[CompanyName] = @CompanyName
		,[ModifiedUser] = @ModifiedUser
		,[LastActivationDate] = @LastActivationDate
		,[IsTestRecord] = @IsTestRecord
	WHERE [Id] = @Id