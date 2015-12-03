
CREATE PROC [API].[Company_Update]
	 @Id smallint
	,@ObjectGuid uniqueidentifier
	,@Name nvarchar(128)
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

UPDATE dbo.[Company]
SET 
	 [Name] = @Name
	,[Code] = @Code
	,[IsParent] = @IsParent
	,[ParentId] = @ParentId
	,[LogoCssClass] = @LogoCssClass
	,[MasterBookingPageByPhysician] = @MasterBookingPageByPhysician
	,[MasterBookingPageByTime] = @MasterBookingPageByTime
	,[MasterBookingPageTeleconference] = @MasterBookingPageTeleconference
	,[ModifiedDate] = @Now
	,[ModifiedUser] = @ModifiedUser
WHERE [Id] = @Id OR [ObjectGuid] = @ObjectGuid