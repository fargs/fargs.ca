
CREATE PROC [API].[Company_Insert]
	 @Name nvarchar(128)
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

INSERT INTO dbo.[Company]
(
	 [Name]
	,[Code]
	,[IsParent]
	,[ParentId]
	,[LogoCssClass]
	,[BillingEmail]
	,[ReportsEmail]
	,[Phone]
	,[ModifiedDate]
	,[ModifiedUser]
)
VALUES
(
	 @Name
	,@Code
	,@IsParent
	,@ParentId
	,@LogoCssClass
	,@BillingEmail
	,@ReportsEmail
	,@Phone
	,@Now
	,@ModifiedUser
)

SELECT Id = Id
	, ObjectGuid = ObjectGuid
	, ModifiedDate = ModifiedDate
FROM API.Company
WHERE Id = SCOPE_IDENTITY()