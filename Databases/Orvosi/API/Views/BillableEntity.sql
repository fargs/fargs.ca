








CREATE VIEW [API].[BillableEntity]
AS

WITH Entity 
AS (
	SELECT EntityId = CONVERT(NVARCHAR(128), Id)
		, EntityGuid = ObjectGuid
		, EntityName = Name
		, EntityType = 'COMPANY'
		, LogoCssClass
		, BillingEmail
		, Phone
		, HstNumber = NULL
	FROM dbo.Company
	UNION
	SELECT CONVERT(NVARCHAR(128), Id)
		, Id
		, u.MyCompanyName
		, 'PHYSICIAN'
		, u.LogoCssClass
		, u.Email
		, u.PhoneNumber
		, u.HstNumber
	FROM API.[User] u
	WHERE u.RoleCategoryId = 1
)
SELECT e.EntityGuid
	, e.EntityName
	, e.EntityType
	, e.EntityId
	, e.LogoCssClass
	, AddressName = a.[Name]
	, a.Attention
	, a.Address1
	, a.Address2
	, City = c.[Name]
	, a.PostalCode
	, ProvinceName = p.ProvinceName
	, CountryName = co.[Name]
	, e.BillingEmail
	, e.Phone
	, e.HstNumber
FROM Entity e
LEFT JOIN dbo.[Address] a ON e.EntityGuid = a.OwnerGuid AND a.AddressTypeID = 4
LEFT JOIN dbo.City c ON a.CityId = c.Id
LEFT JOIN dbo.Province p ON c.ProvinceId = p.Id
LEFT JOIN dbo.Country co ON p.CountryID = co.Id