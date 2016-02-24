
CREATE PROCEDURE [API].[ServiceCatalogue_Delete]
	 @Id int
AS

DECLARE @Now DATETIME
SET @Now = GETDATE()

DELETE FROM dbo.[ServiceCatalogue] 
WHERE [Id] = @Id