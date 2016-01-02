
CREATE PROCEDURE [API].[Location_Delete]
	 @Id int
AS

DECLARE @Now DATETIME
SET @Now = GETDATE()

DELETE FROM dbo.[Address]
WHERE 
	Id = @Id