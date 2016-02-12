

CREATE PROCEDURE [API].[AvailableSlot_Delete]
	 @Id smallint
AS

DECLARE @Now DATETIME
SET @Now = GETDATE()

DELETE FROM dbo.[AvailableSlot]
WHERE 
	Id = @Id