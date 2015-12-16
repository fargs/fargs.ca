
CREATE PROCEDURE [API].[PhysicianInsurance_Update]
	 @Id smallint
	,@PhysicianId nvarchar(128)
	,@Insurer nvarchar(256)
	,@PolicyNumber nvarchar(128)
	,@ExpiryDate date
	,@DocumentId smallint
	,@ModifiedUser nvarchar(100)
AS

DECLARE @Now DATETIME
SET @Now = GETDATE()

UPDATE dbo.[PhysicianInsurance]
SET
	 [PhysicianId] = @PhysicianId
	,[Insurer] = @Insurer
	,[PolicyNumber] = @PolicyNumber
	,[ExpiryDate] = @ExpiryDate
	,[DocumentId] = @DocumentId
	,[ModifiedDate] = @Now
	,[ModifiedUser] = @ModifiedUser
WHERE 
	Id = @Id