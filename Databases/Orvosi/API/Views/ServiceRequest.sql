




CREATE VIEW [API].[ServiceRequest]
AS

SELECT
	 sr.[Id] 
	,sr.[ObjectGuid]
	,sr.[CompanyReferenceId]
	,sr.[ServiceCatalogueId]
	,sr.[HarvestProjectId]
	,sr.[Title]
	,sr.[Body]
	,sr.[AddressId]
	,sr.[RequestedDate]
	,sr.[RequestedBy]
	,sr.[CancelledDate]
	,sr.[AssignedTo]
	,sr.[StatusId]
	,sr.[DueDate]
	,sr.[StartTime]
	,sr.[EndTime]
	,ServiceRequestPriceOverride = sr.[Price]
	,sr.[ModifiedDate]
	,sr.[ModifiedUser]
	,ServiceName = s.Name
	,s.ServiceCategoryName
	,s.ServicePortfolioName
	,s.DefaultPrice
	,PhysicianDisplayName = p.DisplayName
	,CompanyName = c.Name
	,ParentCompanyName = c.ParentName
	,ServiceDefaultPrice = s.DefaultPrice
	,ServiceCataloguePriceOverride = sc.Price
	,EffectivePrice = COALESCE(sr.Price, sc.Price, s.DefaultPrice)
	,AssignedToDisplayName = u.DisplayName
	,RequestedByDisplayName = rb.DisplayName
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
FROM dbo.ServiceRequest sr
LEFT JOIN dbo.ServiceCatalogue sc ON sc.Id = sr.ServiceCatalogueId
INNER JOIN API.[Service] s ON s.Id = sc.ServiceId
INNER JOIN API.Physician p ON sc.PhysicianId = p.Id
LEFT JOIN API.Company c ON sc.CompanyId = c.Id
LEFT JOIN API.[User] u ON u.Id = sr.AssignedTo
LEFT JOIN API.[User] rb ON rb.Id = sr.RequestedBy
LEFT JOIN dbo.LookupItem li ON li.Id = sr.StatusId
LEFT JOIN API.[Location] a ON sr.AddressId = a.Id