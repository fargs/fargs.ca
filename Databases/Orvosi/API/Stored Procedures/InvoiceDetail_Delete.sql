CREATE PROC [API].[InvoiceDetail_Delete]
	 @Id int
AS

DELETE FROM dbo.InvoiceDetail
WHERE Id = @Id