CREATE VIEW [Analysis].[InvoiceItem]
AS

SELECT id.ServiceRequestId
	,id.Amount
	,id.Rate
	,id.Total
FROM dbo.InvoiceDetail id
LEFT JOIN dbo.Invoice i ON id.InvoiceId = i.Id
WHERE id.IsDeleted = 0 AND i.IsDeleted = 0