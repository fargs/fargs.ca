
CREATE PROC [API].[Company_Insert]
	 @Name nvarchar(128)
	,@Code nvarchar(50)
	,@IsParent bit
	,@ParentId int
	,@LogoCssClass nvarchar(50)
	,@MasterBookingPageByPhysician nvarchar(50)
	,@MasterBookingPageByTime nvarchar(50)
	,@MasterBookingPageTeleconference nvarchar(50)
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
	,[MasterBookingPageByPhysician]
	,[MasterBookingPageByTime]
	,[MasterBookingPageTeleconference]
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
	,@MasterBookingPageByPhysician
	,@MasterBookingPageByTime
	,@MasterBookingPageTeleconference
	,@Now
	,@ModifiedUser
)

SELECT Id = Id
	, ObjectGuid = ObjectGuid
	, ModifiedDate = ModifiedDate
FROM API.Company
WHERE Id = SCOPE_IDENTITY()