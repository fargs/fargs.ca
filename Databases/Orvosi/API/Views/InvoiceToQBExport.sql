

CREATE VIEW [API].[InvoiceToQBExport]
AS
SELECT InvoiceNumber
	, InvoiceDate
	, i.DueDate
	, ServiceProviderName
	, CustomerName
	, CustomerProvince
	, SubTotal
	, TaxRateHst
	, Hst
	, Total 
	, [BillingEmail] = CustomerEmail
	, [ItemDescription] = id.[Description]
	, i.IsDeleted
	, i.SentDate
FROM dbo.Invoice i
LEFT JOIN (
	SELECT id.InvoiceId, id.[Description]
		, RowNum = ROW_NUMBER() OVER(PARTITION BY id.InvoiceId ORDER BY id.Id)
	FROM InvoiceDetail id
) id ON i.Id = id.InvoiceId
WHERE RowNum = 1