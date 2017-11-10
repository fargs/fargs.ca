
CREATE VIEW [API].[Invoice]
AS
WITH ServiceRequests AS (
	SELECT sr.Id,
		sr.ClaimantName,
		sr.ServiceId,
		[Service] = s.[Name],
		sr.AddressId,
		[City] = ct.[Name],
		[Province] = p.ProvinceName,
		id.InvoiceDetailId,
		id.InvoiceId
	FROM dbo.ServiceRequest sr
	LEFT JOIN dbo.[Company] c ON sr.CompanyId = c.Id
	LEFT JOIN dbo.[Service] s ON sr.ServiceId = s.Id
	LEFT JOIN dbo.[Address] a ON sr.AddressId = a.Id
	LEFT JOIN dbo.[City] ct ON a.CityId = ct.Id
	LEFT JOIN dbo.[Province] p ON ct.ProvinceId = p.Id
	LEFT JOIN (
		SELECT id.ServiceRequestId, InvoiceDetailId = id.Id, InvoiceId = i.Id
		FROM dbo.InvoiceDetail id
		LEFT JOIN dbo.Invoice i ON id.InvoiceId = i.Id
	) id ON sr.Id = id.ServiceRequestId
)
, Invoices AS (
	SELECT
		i.Id,
		i.InvoiceNumber,
		InvoiceDate = CONVERT(DATE, i.InvoiceDate),
		i.CustomerGuid,
		i.CustomerName,
		i.CustomerEmail,
		i.ServiceProviderGuid,
		i.ServiceProviderName,
		i.SubTotal,
		i.TaxRateHst,
		i.Hst,
		i.Total
	FROM dbo.Invoice i
	LEFT JOIN dbo.InvoiceDetail id ON i.Id = id.InvoiceId
)
, InvoicesSent AS (
	SELECT InvoiceId, SentDate
	FROM (
		SELECT isl.InvoiceId, 
			isl.SentDate, 
			RowNum = ROW_NUMBER() OVER(PARTITION BY isl.InvoiceId ORDER BY SentDate DESC)
		FROM dbo.InvoiceSentLog isl
	) isl
	WHERE RowNum = 1
)
SELECT
	Id = ISNULL(ROW_NUMBER() OVER(ORDER BY i.Id, sr.Id),0),

	InvoiceId = i.Id, 
	i.InvoiceNumber,
	i.InvoiceDate,
	i.CustomerGuid, 
	i.CustomerName,
	i.CustomerEmail,
	i.ServiceProviderGuid,
	i.ServiceProviderName, 
	
	ServiceRequestId = sr.Id, 
	sr.ClaimantName, 
	sr.ServiceId,
	sr.[Service],
	sr.City,
	sr.Province,

	i.SubTotal,
	i.TaxRateHst,
	i.Hst,
	i.Total,

	[Status] = CASE WHEN isl.InvoiceId IS NULL THEN 'unsent' ELSE 'unpaid' END
FROM Invoices i
LEFT JOIN ServiceRequests sr ON i.Id = sr.InvoiceId
LEFT JOIN InvoicesSent isl ON i.Id = isl.InvoiceId