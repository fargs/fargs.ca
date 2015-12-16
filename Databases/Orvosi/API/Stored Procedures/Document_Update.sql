
CREATE PROCEDURE [API].[Document_Update]
	 @Id smallint
	,@ObjectGuid uniqueidentifier
	,@OwnedByObjectGuid uniqueidentifier
	,@DocumentTemplateId smallint
	,@EffectiveDate datetime
	,@ExpiryDate datetime
	,@Path nvarchar(2000)
	,@Extension nvarchar(10)
	,@Name nvarchar(256)
	,@Content varbinary(max)
	,@ContentType nvarchar(50)
	,@ModifiedUser nvarchar(100)
AS

DECLARE @Now DATETIME
SET @Now = GETDATE()

UPDATE dbo.[Document]
SET
	 [ObjectGuid] = @ObjectGuid
	,[OwnedByObjectGuid] = @OwnedByObjectGuid
	,[DocumentTemplateId] = @DocumentTemplateId
	,[EffectiveDate] = @EffectiveDate
	,[ExpiryDate] = @ExpiryDate
	,[Path] = @Path
	,[Extension] = @Extension
	,[Name] = @Name
	,[Content] = @Content
	,[ContentType] = @ContentType
	,[ModifiedDate] = @Now
	,[ModifiedUser] = @ModifiedUser
WHERE 
	Id = @Id