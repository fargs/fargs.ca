
CREATE VIEW API.AvailableDay
AS 
SELECT 
	 ad.[Id]
	,ad.[PhysicianId] 
	,ad.[Day]
	,ad.[CompanyId]
	,ad.[LocationId]
	,ad.[ModifiedDate]
	,ad.[ModifiedUser]
	,CompanyName = c.Name
	,LocationName = l.Name
	,LocationOwnerDisplayName = l.EntityDisplayName
	,LocationOwnerId = l.EntityId
FROM dbo.AvailableDay ad
LEFT JOIN API.Company c ON ad.CompanyId = c.Id
LEFT JOIN API.Location l ON ad.LocationId = l.Id