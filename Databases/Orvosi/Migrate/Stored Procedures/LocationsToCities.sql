CREATE PROC Migrate.LocationsToCities
AS

INSERT INTO City ([Name], [Code], ProvinceId, ModifiedDate, ModifiedUser) VALUES ('Windsor', 'WIN', 9, '2017-04-13', 'lfarago@orvosi.ca')

--UPDATE ServiceCatalogue SET LocationId = c.Id
SELECT sc.PhysicianId, sc.LocationId, li.[Text], c.Id, c.[Name] 
FROM ServiceCatalogue sc
LEFT JOIN LookupItem li ON sc.LocationId = li.Id
LEFT JOIN City c ON li.[Text] = c.[Name]
WHERE sc.LocationId IS NOT NULL
--ORDER BY sc.PhysicianId

--DELETE FROM ServiceCatalogue WHERE PhysicianId IS NULL