CREATE VIEW API.Entity
AS
SELECT EntityId = ObjectGuid
	, DisplayName = Name
	, EntityType = 'SERVICE'
FROM dbo.[Service]
UNION
SELECT ObjectGuid
	, Name
	, 'COMPANY'
FROM dbo.Company
UNION
SELECT ObjectGuid = Id
	, dbo.GetDisplayName(FirstName, LastName, Title)
	, 'USER'
FROM dbo.AspNetUsers