
CREATE PROCEDURE [API].[PhysicianLicense_Delete]
	 @Id smallint
AS

DECLARE @Now DATETIME
SET @Now = GETDATE()

DELETE FROM dbo.[PhysicianLicense]
WHERE 
	Id = @Id