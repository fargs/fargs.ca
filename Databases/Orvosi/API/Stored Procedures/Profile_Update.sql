﻿
CREATE PROC [API].[Profile_Update]
	 @Id nvarchar(128)
	,@Title nvarchar(50)
	,@FirstName nvarchar(128)
	,@LastName nvarchar(128)
	,@LogoCssClass nvarchar(128)
	,@EmployeeId nvarchar(50)
	,@ModifiedUser nvarchar(256)
	,@IsTestRecord bit
	,@ColorCode nvarchar(50)
AS
	
	UPDATE dbo.AspNetUsers
	SET  [Id] = @Id
		,[Title] = @Title
		,[FirstName] = @FirstName
		,[LastName] = @LastName
		,[LogoCssClass] = @LogoCssClass
		,[EmployeeId] = @EmployeeId
		,[ModifiedUser] = @ModifiedUser
		,[IsTestRecord] = @IsTestRecord
		,[ColorCode] = @ColorCode
	WHERE [Id] = @Id