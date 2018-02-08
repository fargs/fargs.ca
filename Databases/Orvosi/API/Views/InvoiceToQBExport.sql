

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
	, i.CreatedDate
	, i.SentDate
	, i.IsDeleted
	, i.DeletedDate
	, sr.Id
	, sr.ClaimantName
	, sr.IsNoShow
	, sr.IsLateCancellation
FROM dbo.Invoice i
LEFT JOIN (
	SELECT id.InvoiceId, id.[Description], id.ServiceRequestId
		, RowNum = ROW_NUMBER() OVER(PARTITION BY id.InvoiceId ORDER BY id.Id)
	FROM InvoiceDetail id
) id ON i.Id = id.InvoiceId
LEFT JOIN dbo.ServiceRequest sr ON id.ServiceRequestId = sr.Id
WHERE RowNum = 1