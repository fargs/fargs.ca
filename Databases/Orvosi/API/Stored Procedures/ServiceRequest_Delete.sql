
CREATE PROCEDURE [API].[ServiceRequest_Delete]
	 @Id int
AS

DECLARE @Now DATETIME
SET @Now = GETDATE()

DELETE FROM dbo.[ServiceRequest]
WHERE 
	Id = @Id