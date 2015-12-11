CREATE PROC API.Location_Select_PhysicianAndCompany
	@PhysicianId nvarchar(128)
	, @CompanyId smallint
AS

SELECT l.Id, l.EntityType, l.EntityDisplayName, l.Name, l.LocationName
FROM API.Location l
WHERE EntityId = @PhysicianId
UNION
SELECT l.Id, l.EntityType, l.EntityDisplayName, l.Name, l.LocationName
FROM API.Location l
WHERE EntityId = @CompanyId AND EntityType = 'COMPANY'