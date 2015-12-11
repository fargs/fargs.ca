
CREATE VIEW [API].[ServiceCatalogue]
AS
SELECT sc.Id
	, sc.CompanyId
	, sc.PhysicianId
	, sc.ServiceId
	, ServiceCataloguePriceOverride = sc.Price
	, ServiceName = s.Name
	, ServicePrice = s.DefaultPrice
	, PhysicianDisplayName = p.DisplayName
	, CompanyName = c.Name
FROM dbo.ServiceCatalogue sc
INNER JOIN API.[Service] s ON s.Id = sc.ServiceId
LEFT JOIN API.[Physician] p ON p.Id = sc.PhysicianId
LEFT JOIN API.[Company] c ON c.Id = sc.CompanyId