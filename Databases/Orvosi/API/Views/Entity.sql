


CREATE VIEW [API].[Entity]
AS
SELECT EntityId = ObjectGuid
	, EntityGuid = ObjectGuid
	, DisplayName = Name
	, EntityType = 'SERVICE'
FROM dbo.[Service]
UNION
SELECT ObjectGuid
	, ObjectGuid
	, Name
	, 'COMPANY'
FROM dbo.Company
UNION
SELECT Id
	, Id
	, dbo.GetDisplayName(FirstName, LastName, Title)
	, 'PHYSICIAN'
FROM API.[User] u
WHERE u.RoleCategoryId = 1
UNION
SELECT Id
	, Id
	, dbo.GetDisplayName(FirstName, LastName, Title)
	, 'USER'
FROM API.[User] u
WHERE u.RoleCategoryId <> 1