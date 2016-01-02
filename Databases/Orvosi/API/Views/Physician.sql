





CREATE VIEW [API].[Physician]
AS
SELECT 
	 u.*
	,p.Designations
	,p.LocationAreaId
	,p.Specialties
	,p.SubSpecialties
	,p.Pediatrics
	,p.Adolescents
	,p.Adults
	,p.Geriatrics
	,la.ItemText
FROM API.[User] u
LEFT JOIN dbo.Physician p ON u.Id = p.Id
LEFT JOIN API.LocationArea la ON p.LocationAreaId = la.ItemId
WHERE RoleId = '8359141f-e423-4e48-8925-4624ba86245a' -- Role = Physician