
CREATE PROCEDURE [API].[Document_Insert]
	 @ObjectGuid uniqueidentifier
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

INSERT INTO dbo.[Document]
(
	 [ObjectGuid]
	,[OwnedByObjectGuid]
	,[DocumentTemplateId]
	,[EffectiveDate]
	,[ExpiryDate]
	,[Path]
	,[Extension]
	,[Name]
	,[Content]
	,[ContentType]
	,[ModifiedUser]
)
VALUES 
(
	 @ObjectGuid
	,@OwnedByObjectGuid
	,@DocumentTemplateId
	,@EffectiveDate
	,@ExpiryDate
	,@Path
	,@Extension
	,@Name
	,@Content
	,@ContentType
	,@ModifiedUser
)

DECLARE @Id INT
SELECT @Id = SCOPE_IDENTITY()

SELECT * FROM API.Document WHERE Id = @Id