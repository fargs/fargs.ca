



CREATE VIEW [API].[InvoiceDetail]
AS
SELECT 
	 [Id]
	,[InvoiceId] = ISNULL(InvoiceId, 0)
	,[ServiceRequestId]
	,[Description]
	,[Quantity]
	,[Rate]
	,[Total]
	,[Discount]
	,[Amount]
	,[AdditionalNotes]
	,[ModifiedDate]
	,[ModifiedUser]
FROM dbo.[InvoiceDetail] id