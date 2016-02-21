


CREATE VIEW [API].[PhysicianToCompanyServiceRequestInvoicePreview]
AS

SELECT 
	 [ServiceRequestId] = sr.Id
	,[InvoiceDate] = GETDATE()
	,[Currency] = 'CAD'
	,[Terms] = NULL
	,[DueDate] = DATEADD(d, 14, GETDATE())
	,[CompanyGuid] = sr.PhysicianId
	,[CompanyName] = p.CompanyName
	,[Email] = p.Email
	,[PhoneNumber] = p.PhoneNumber
	,[Address1] = pa.Address1
	,[Address2] = pa.City + ', ' + pa.ProvinceName
	,[Address3] = pa.PostalCode
	,[BillToGuid] = sr.ObjectGuid
	,[BillToName] = sr.CompanyName
	,[BillToAddress1] = ca.Address1
	,[BillToAddress2] = ca.City + ', ' + ca.ProvinceName
	,[BillToAddress3] = ca.PostalCode
	,[BillToEmail] = c.InvoiceEmails
	,[DescriptionLine1] = sr.ServiceName
	,[DescriptionLine2] = sr.City + ', ' + sr.AddressName
	,[DescriptionLine3] = sr.ClaimantName + ', Ref# ' + sr.CompanyReferenceId
	,sr.IsNoShow
	,NoShowRate = .5
	,sr.CancelledDate
	,sr.IsLateCancellation
	,LateCancellationRate = .3
	,[SubTotal] = dbo.[GetInvoiceDetailAmount](sr.EffectivePrice, sr.IsNoShow, .5, sr.IsLateCancellation, .3)
	,[TaxRateHst] = 0.13
	,[Discount] = NULL
	,[Total] = CONVERT(DECIMAL(10,2), dbo.[GetInvoiceDetailAmount](sr.EffectivePrice, sr.IsNoShow, .5, sr.IsLateCancellation, .3) * 1.13)
FROM API.ServiceRequest sr
LEFT JOIN dbo.AspNetUsers p ON sr.PhysicianId = p.Id
LEFT JOIN API.Company c ON sr.CompanyId = c.Id
LEFT JOIN API.[Location] ca ON c.ObjectGuid = ca.OwnerGuid AND ca.AddressTypeID = 3
LEFT JOIN API.[Location] pa ON p.Id = pa.OwnerGuid AND pa.AddressTypeID = 3