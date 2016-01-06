
CREATE PROCEDURE [API].[ServiceRequestTask_Delete]
	 @Id int
AS

DECLARE @Now DATETIME
SET @Now = GETDATE()

DELETE FROM dbo.[ServiceRequestTask]
WHERE 
	Id = @Id