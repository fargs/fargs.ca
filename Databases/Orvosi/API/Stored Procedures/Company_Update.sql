
CREATE PROC [API].[Company_Update]
	 @Id smallint
	,@ObjectGuid uniqueidentifier
	,@Name nvarchar(128)
	,@Code nvarchar(50)
	,@IsParent bit
	,@ParentId int
	,@LogoCssClass nvarchar(50)
	,@BillingEmail nvarchar(128)
	,@ReportsEmail nvarchar(128)
	,@Phone nvarchar(50)
	,@ModifiedUser nvarchar(256)
AS

DECLARE @Now DATETIME
SET @Now = GETDATE()

UPDATE dbo.[Company]
SET 
	 [Name] = @Name
	,[Code] = @Code
	,[IsParent] = @IsParent
	,[ParentId] = @ParentId
	,[LogoCssClass] = @LogoCssClass
	,[BillingEmail] = @BillingEmail
	,[ReportsEmail] = @ReportsEmail
	,[Phone] = @Phone
	,[ModifiedDate] = @Now
	,[ModifiedUser] = @ModifiedUser
WHERE [Id] = @Id OR [ObjectGuid] = @ObjectGuid