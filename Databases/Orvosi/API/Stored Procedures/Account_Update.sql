
CREATE PROC [API].[Account_Update]
	 @Id uniqueidentifier
	,@Email nvarchar(256)
	,@EmailConfirmed bit
	,@PhoneNumber nvarchar(128)
	,@PhoneNumberConfirmed bit
	,@TwoFactorEnabled bit
	,@LockoutEndDateUtc datetime
	,@LockoutEnabled bit
	,@AccessFailedCount int
	,@UserName nvarchar(256)
	,@CompanyId smallint
	,@ModifiedUser nvarchar(256)
	,@LastActivationDate datetime
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
		,[CompanyId] = @CompanyId
		,[ModifiedUser] = @ModifiedUser
		,[LastActivationDate] = @LastActivationDate
	WHERE [Id] = @Id