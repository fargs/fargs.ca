

CREATE PROC [API].[Invoice_Delete]
	 @Id int
AS

DELETE FROM dbo.Invoice
WHERE Id = @Id