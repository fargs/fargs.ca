

CREATE VIEW [API].[Entity]
AS
SELECT EntityId = CONVERT(NVARCHAR(128), Id)
	, EntityGuid = ObjectGuid
	, DisplayName = Name
	, EntityType = 'SERVICE'
FROM dbo.[Service]
UNION
SELECT CONVERT(NVARCHAR(128), Id)
	, ObjectGuid
	, Name
	, 'COMPANY'
FROM dbo.Company
UNION
SELECT Id
	, Id
	, dbo.GetDisplayName(FirstName, LastName, Title)
	, 'PHYSICIAN'
FROM API.Physician
UNION
SELECT Id
	, Id
	, dbo.GetDisplayName(FirstName, LastName, Title)
	, 'USER'
FROM API.[User] u
WHERE u.RoleCategoryId <> 1