DECLARE @Month DATE, @PhysicianId UNIQUEIDENTIFIER
SET @Month = '2018-07-01'
SET @PhysicianId = '8DD4E180-6E3A-4968-A00D-EEB6D2CC7F0C';

DROP TABLE #Work;
WITH W AS
(
	SELECT sr.Id
		, sr.ClaimantName
		, sr.PhysicianId
		, WorkDate = COALESCE(sr.AppointmentDate, sr.DueDate)
		, WorkMonth = CONVERT(DATE, dbo.FormatDateTime(COALESCE(sr.AppointmentDate, sr.DueDate), 'yyyy-MM-01'))
		, CancelledDate
		, IsLateCancellation
		, IsNoShow
		, [Service] = s.[Name]
	FROM dbo.ServiceRequest sr 
	LEFT JOIN dbo.[Service] s ON sr.ServiceId = s.Id
)
SELECT *
INTO #Work
FROM W

DROP TABLE #WorkInMonth
SELECT *
INTO #WorkInMonth
FROM #Work w
WHERE WorkMonth = @Month AND PhysicianId = @PhysicianId;

DROP TABLE #Invoices;

WITH I AS 
(
	SELECT i.Id
		, i.ServiceProviderGuid
		, i.InvoiceNumber
		, CONVERT(DATE, dbo.FormatDateTime(i.InvoiceDate, 'yyyy-MM-01')) AS InvoiceMonth
		, i.InvoiceDate
		, i.SentDate
		, SentMonth = CONVERT(DATE, dbo.FormatDateTime(i.SentDate, 'yyyy-MM-01'))
		, i.IsDeleted
		, i.DeletedDate
		, id.Amount AS ItemAmount
		, id.Total AS ItemTotal
		, i.SubTotal
		, sr.Id as ServiceRequestId
		, sr.ClaimantName
		, sr.AppointmentDate
		, sr.DueDate
		, sr.PhysicianId
	FROM dbo.Invoice i
	LEFT JOIN dbo.InvoiceDetail id ON i.Id = id.InvoiceId
	LEFT JOIN dbo.ServiceRequest sr ON sr.Id = id.ServiceRequestId
)
SELECT *
INTO #Invoices
FROM I i

DROP TABLE #InvoicesInMonth
-- Invoices In Month
SELECT *
INTO #InvoicesInMonth
FROM #Invoices
WHERE InvoiceMonth = @Month AND ServiceProviderGuid = @PhysicianId;


-- Review Cancelled Work
SELECT w.Id
	, w.ClaimantName
	, w.WorkDate
	, w.CancelledDate
	, w.IsLateCancellation
	, w.IsNoShow 
	, i.InvoiceNumber
	, i.DeletedDate
FROM #WorkInMonth w
LEFT JOIN #Invoices i ON w.Id = i.ServiceRequestId
WHERE w.CancelledDate IS NOT NULL AND IsLateCancellation = 0

-- Work with no invoice
SELECT w.Id
	, w.ClaimantName
	, w.WorkDate
	, w.WorkMonth
	, w.CancelledDate
	, w.IsLateCancellation
	, w.IsNoShow 
	, i.InvoiceNumber
	, i.InvoiceDate
	, i.InvoiceMonth
FROM #WorkInMonth w
LEFT JOIN #Invoices i ON w.Id = i.ServiceRequestId
WHERE i.Id IS NULL AND CancelledDate IS NULL


-- Work in different month than invoice
SELECT w.Id
	, w.ClaimantName
	, w.WorkDate
	, w.WorkMonth
	, w.CancelledDate
	, w.IsLateCancellation
	, w.IsNoShow 
	, i.InvoiceNumber
	, i.InvoiceDate
	, i.InvoiceMonth
FROM #WorkInMonth w
LEFT JOIN #Invoices i ON w.Id = i.ServiceRequestId
WHERE w.WorkMonth <> i.SentMonth

-- invoice sent before the work (potential reschedule)
SELECT *
FROM #WorkInMonth w
LEFT JOIN #Invoices i ON w.Id = i.ServiceRequestId
WHERE w.WorkDate > i.SentDate

--SELECT * FROM dbo.AspNetUsers WHERE LastName LIKE '%Dessouki%'
