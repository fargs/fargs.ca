






CREATE VIEW [API].[Physician]
AS
SELECT 
	 u.*
	,p.Designations
	,p.PrimaryAddressId
	,p.SpecialtyId
	,p.OtherSpecialties
	,p.Pediatrics
	,p.Adolescents
	,p.Adults
	,p.Geriatrics
	,la.LocationName
	,AddressName = la.Name
	,PrimarySpecialtyName = s.ItemText
FROM API.[User] u
LEFT JOIN dbo.Physician p ON u.Id = p.Id
LEFT JOIN API.Location la ON p.PrimaryAddressId = la.Id
LEFT JOIN API.Specialty s ON p.SpecialtyId = s.ItemId
WHERE RoleId = '8359141f-e423-4e48-8925-4624ba86245a' -- Role = Physician