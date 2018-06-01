SELECT sr.Id, sr.ClaimantName, s.ServiceRequestId, s.ClaimantName, s.[Law Firm / Insurance], sr.SourceCompany
--UPDATE sr SET sr.SourceCompany = s.[Law Firm / Insurance]
FROM dbo.Sheet1 s
INNER JOIN dbo.ServiceRequest sr ON s.ServiceRequestId = sr.Id
WHERE s.[Law Firm / Insurance] IS NOT NULL

SELECT sr.Id, sr.ClaimantName, s.ServiceRequestId, s.ClaimantName, s.[Service], s.[Medicolegal Type]
--UPDATE sr SET sr.MedicolegalTypeId = s.[Medicolegal Type]
FROM dbo.Sheet1 s
INNER JOIN dbo.ServiceRequest sr ON s.ServiceRequestId = sr.Id
WHERE s.[Medicolegal Type] IS NOT NULL AND sr.Id = 2572

SELECT MAX(ServiceRequestId) FROM dbo.Sheet1

SELECT MAX(Id) FROM dbo.ServiceRequest


SELECT ServiceRequestId,
	ClaimantName,
	AppointmentDate,
	StartTime,
	City,
	Province,
	Company,
	ReferralSource,
	Physician,
	[Service],
	MedicolegalType,
	[Status]
FROM Analysis.ServiceRequest WHERE ServiceRequestId > 4408

SELECT * FROM Analysis.ServiceRequest
