
CREATE PROCEDURE [API].[Document_Delete]
	 @Id smallint
AS

DECLARE @Now DATETIME
SET @Now = GETDATE()

DELETE FROM dbo.[Document]
WHERE 
	Id = @Id