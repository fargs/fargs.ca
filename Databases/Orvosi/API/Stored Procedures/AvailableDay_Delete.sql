

CREATE PROCEDURE [API].[AvailableDay_Delete]
	 @Id smallint
AS

DECLARE @Now DATETIME
SET @Now = GETDATE()

DELETE FROM dbo.[AvailableDay]
WHERE 
	Id = @Id