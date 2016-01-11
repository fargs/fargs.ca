



CREATE VIEW [API].[AvailableDay]
AS 
SELECT 
	 ad.[Id]
	,ad.[PhysicianId] 
	,ad.[Day]
	,ad.[IsPrebook]
	,ad.[CompanyId]
	,ad.[LocationId]
	,ad.[ModifiedDate]
	,ad.[ModifiedUser]
	,CompanyName = c.Name
	,CompanyIsParent = c.IsParent
	,LocationName = l.Name
	,LocationOwnerDisplayName = l.EntityDisplayName
	,LocationOwnerId = l.EntityId
	,PhysicianName = dbo.GetDisplayName(u.FirstName, u.LastName, u.Title)
FROM dbo.AvailableDay ad
LEFT JOIN API.Company c ON ad.CompanyId = c.Id
LEFT JOIN API.Location l ON ad.LocationId = l.Id
LEFT JOIN dbo.AspNetUsers u ON ad.PhysicianId = u.Id