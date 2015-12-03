
CREATE PROC [API].[TableName_Update]
	 @Id tinyint
AS

DECLARE @Now DATETIME
SET @Now = GETDATE()

UPDATE dbo.[Temp]
SET 
	Id = @Id 
WHERE [Id] = @Id