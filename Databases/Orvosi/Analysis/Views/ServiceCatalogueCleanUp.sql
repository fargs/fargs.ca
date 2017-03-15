CREATE VIEW Analysis.ServiceCatalogueCleanUp
AS
SELECT sc.Id 
	, u.LastName
	, Company = c.Name
	, [Service] = s.Name
	, [Location] = l.ItemText
	, sc.Price
FROM dbo.ServiceCatalogue sc
LEFT JOIN dbo.AspNetUsers u ON sc.PhysicianId = u.Id
LEFT JOIN dbo.[Service] s ON sc.ServiceId = s.Id
LEFT JOIN dbo.[Company] c ON sc.CompanyId = c.Id
LEFT JOIN API.LocationArea l ON sc.LocationId = l.ItemId 
WHERE sc.Price <= 100