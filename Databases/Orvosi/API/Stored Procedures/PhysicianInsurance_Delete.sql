
CREATE PROCEDURE [API].[PhysicianInsurance_Delete]
	 @Id smallint
AS

DECLARE @Now DATETIME
SET @Now = GETDATE()

DELETE FROM dbo.[PhysicianInsurance]
WHERE 
	Id = @Id