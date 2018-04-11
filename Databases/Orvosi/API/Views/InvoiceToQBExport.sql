



CREATE VIEW [API].[InvoiceToQBExport]
AS

WITH Intake
AS (
	SELECT * 
	FROM (
		SELECT DisplayName = u.FirstName + ' ' + u.LastName
			, srr.ServiceRequestId
			, RowNum = ROW_NUMBER() OVER(PARTITION BY srr.ServiceRequestId ORDER BY srr.CreatedDate)
		FROM dbo.ServiceRequestResource srr
		INNER JOIN dbo.AspNetUsers u ON srr.UserId = u.Id
		WHERE srr.RoleId = '9DD582A0-CF86-4FC0-8894-477266068C12'
	) t WHERE RowNum = 1
)
, DocReviewer
AS (
	SELECT * 
	FROM (
		SELECT DisplayName = u.FirstName + ' ' + u.LastName
			, srr.ServiceRequestId
			, RowNum = ROW_NUMBER() OVER(PARTITION BY srr.ServiceRequestId ORDER BY srr.CreatedDate)
		FROM dbo.ServiceRequestResource srr
		INNER JOIN dbo.AspNetUsers u ON srr.UserId = u.Id
		WHERE srr.RoleId = '22B5C8AC-2C96-4A74-8057-976914031A7E'
	) t WHERE t.RowNum = 1
)
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
	, [Service] = sv.[Name]
	, [City] = ci.[Name]
	, [Province] = pr.[ProvinceName]
	, sr.IsNoShow
	, sr.IsLateCancellation
	, Intake = it.DisplayName
	, DocReviewer = dr.DisplayName
FROM dbo.Invoice i
LEFT JOIN (
	SELECT id.InvoiceId, id.[Description], id.ServiceRequestId
		, RowNum = ROW_NUMBER() OVER(PARTITION BY id.InvoiceId ORDER BY id.Id)
	FROM InvoiceDetail id
) id ON i.Id = id.InvoiceId
LEFT JOIN dbo.ServiceRequest sr ON id.ServiceRequestId = sr.Id
LEFT JOIN dbo.[Service] sv ON sr.ServiceId = sv.Id
LEFT JOIN dbo.[Address] ad ON sr.AddressId = ad.Id
LEFT JOIN dbo.[City] ci ON ad.CityId = ci.Id
LEFT JOIN dbo.[Province] pr ON ci.ProvinceId = pr.Id
LEFT JOIN Intake it ON sr.Id = it.ServiceRequestId
LEFT JOIN DocReviewer dr ON sr.Id = dr.ServiceRequestId
WHERE id.RowNum = 1