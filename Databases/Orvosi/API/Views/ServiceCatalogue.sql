



CREATE VIEW [API].[ServiceCatalogue]
AS
SELECT sc.Id
	, sc.CompanyId
	, sc.PhysicianId
	, sc.ServiceId
	, sc.LocationId
	, ServiceCataloguePriceOverride = sc.Price
	, ServiceName = s.Name
	, ServicePrice = s.DefaultPrice
	, PhysicianDisplayName = p.DisplayName
	, CompanyName = c.Name
	, LocationName = li.[Text]
	, sc.ModifiedUser
	, sc.ModifiedDate
	, sc.NoShowRate
	, sc.LateCancellationRate
FROM dbo.ServiceCatalogue sc
INNER JOIN API.[Service] s ON s.Id = sc.ServiceId
LEFT JOIN API.[Physician] p ON p.Id = sc.PhysicianId
LEFT JOIN API.[Company] c ON c.Id = sc.CompanyId
LEFT JOIN dbo.LookupItem li ON sc.LocationId = li.Id