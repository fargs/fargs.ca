





CREATE VIEW [API].[ServiceRequest]
AS

SELECT
	 sr.[Id] 
	,sr.[ObjectGuid]
	,sr.[CompanyReferenceId]
	,sr.[ClaimantName]
	,sr.[ServiceCatalogueId]
	,sr.[HarvestProjectId]
	,[Title] = dbo.FormatDateTime(ad.[Day], 'yyyyMMdd') + '-' + LEFT(REPLACE(CONVERT(nvarchar(10), sl.StartTime), ':', ''),4) + '-' + s.Code + '-' + SUBSTRING(p.Email, 0, CHARINDEX('@', p.Email, 0)) + '-' + c.Name + '-' + CONVERT(nvarchar(10), sr.Id)
	,sr.[Body]
	,sr.[AddressId]
	,sr.[RequestedDate]
	,sr.[RequestedBy]
	,sr.[CancelledDate]
	,sr.[StatusId]
	,sr.[AvailableSlotId]
	,sr.[DueDate]
	,sr.CaseCoordinatorId
	,sr.IntakeAssistantId
	,sr.DocumentReviewerId
	,ServiceRequestPrice = sr.[Price]
	,sr.DocumentFolderLink
	,CompanyId = ISNULL(sr.CompanyId, sc.CompanyId)
	,sr.[ModifiedDate]
	,sr.[ModifiedUser]
	,sc.ServiceId
	,ServiceName = s.Name
	,ServiceCode = s.Code
	,s.ServiceCategoryName
	,s.ServicePortfolioName
	,sc.PhysicianId
	,PhysicianDisplayName = p.DisplayName
	,CompanyName = c.Name
	,ParentCompanyName = c.ParentName
	,ServicePrice = s.DefaultPrice
	,ServiceCataloguePrice = sc.Price
	,EffectivePrice = COALESCE(sr.Price, sc.Price, s.DefaultPrice)
	,RequestedByName = dbo.GetDisplayName(rb.FirstName, rb.LastName, rb.Title)
	,StatusName = li.[Text]
	,AddressName = a.Name
	,AddressTypeId = a.AddressTypeId
	,AddressTypeName = a.AddressTypeName
	,Address1 = a.[Address1]
	,Address2 = a.[Address2]
	,City = a.[City]
	,PostalCode = a.PostalCode
	,ProvinceId = a.ProvinceId
	,ProvinceName = a.ProvinceCode
	,CountryId = a.CountryId
	,CountryName = a.CountryCode
	,LocationId = a.LocationId
	,LocationName = a.LocationName
	,CaseCoordinatorName = dbo.GetDisplayName(cc.FirstName, cc.LastName, cc.Title)
	,IntakeAssistantName = dbo.GetDisplayName(ia.FirstName, ia.LastName, ia.Title)
	,DocumentReviewerName = dbo.GetDisplayName(dr.FirstName, dr.LastName, dr.Title)
	,AppointmentDate = ad.[Day]
	,sl.StartTime
	,sl.Duration
	,sl.EndTime
	,CalendarEventTitle = s.Code + '-' + c.Name + '-' + CONVERT(nvarchar(10), sr.Id)
FROM dbo.ServiceRequest sr
LEFT JOIN dbo.ServiceCatalogue sc ON sc.Id = sr.ServiceCatalogueId
INNER JOIN API.[Service] s ON s.Id = sc.ServiceId
INNER JOIN API.Physician p ON sc.PhysicianId = p.Id
LEFT JOIN API.Company c ON ISNULL(sr.CompanyId, sc.CompanyId) = c.Id
LEFT JOIN dbo.[AspNetUsers] cc ON cc.Id = sr.CaseCoordinatorId
LEFT JOIN dbo.[AspNetUsers] ia ON ia.Id = sr.IntakeAssistantId
LEFT JOIN dbo.[AspNetUsers] dr ON dr.Id = sr.DocumentReviewerId
LEFT JOIN dbo.[AspNetUsers] rb ON rb.Id = sr.RequestedBy
LEFT JOIN dbo.LookupItem li ON li.Id = sr.StatusId
LEFT JOIN API.[Location] a ON sr.AddressId = a.Id
LEFT JOIN dbo.AvailableSlot sl ON sr.AvailableSlotId = sl.Id
LEFT JOIN dbo.AvailableDay ad ON sl.AvailableDayId = ad.Id