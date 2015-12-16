
CREATE PROCEDURE [API].[PhysicianInsurance_Insert]
	 @PhysicianId nvarchar(128)
	,@Insurer nvarchar(256)
	,@PolicyNumber nvarchar(128)
	,@ExpiryDate date
	,@DocumentId smallint
	,@ModifiedUser nvarchar(100)
AS

DECLARE @Now DATETIME
SET @Now = GETDATE()

INSERT INTO dbo.[PhysicianInsurance]
(
	 [PhysicianId]
	,[Insurer]
	,[PolicyNumber]
	,[ExpiryDate]
	,[DocumentId]
	,[ModifiedUser]
)
VALUES 
(
	 @PhysicianId
	,@Insurer
	,@PolicyNumber
	,@ExpiryDate
	,@DocumentId
	,@ModifiedUser
)

DECLARE @Id INT
SELECT @Id = SCOPE_IDENTITY()

SELECT * FROM API.PhysicianInsurance WHERE Id = @Id