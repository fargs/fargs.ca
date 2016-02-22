




CREATE VIEW [API].[BillableEntity]
AS

WITH Entity 
AS (
	SELECT EntityId = CONVERT(NVARCHAR(128), Id)
		, EntityGuid = ObjectGuid
		, EntityName = Name
		, EntityType = 'COMPANY'
		, LogoCssClass
	FROM dbo.Company
	UNION
	SELECT Id
		, Id
		, u.MyCompanyName
		, 'PHYSICIAN'
		, u.LogoCssClass
	FROM API.[User] u
	WHERE u.RoleCategoryId = 1
)
SELECT e.EntityGuid
	, e.EntityName
	, e.EntityType
	, e.EntityId
	, e.LogoCssClass
	, AddressName = a.Name
	, a.Attention
	, a.Address1
	, a.Address2
	, a.City
	, a.PostalCode
	, a.ProvinceName
	, a.CountryName
	, a.Email
	, a.Phone
	, a.Fax
FROM Entity e
LEFT JOIN API.[Address] a ON e.EntityGuid = a.OwnerGuid AND a.AddressTypeID = 4