




CREATE VIEW [API].[Clients]
AS

SELECT 
	 c.[Id]
	,c.[Name]
	,c.[Code]
	,c.[LogoCssClass]
	,c.BillingEmail
	,c.ReportsEmail
	,c.Phone
	,ParentName = p.Name
	,AddressTypeName = at.Name
	,AddressName = a.[Name]
	,a.[Attention]
	,a.[Address1]
	,a.[Address2]
	,City = ci.[Name]
	,a.[PostalCode]
	,CountryName = ct.Name
	,CountryCode = ct.ISO3CountryCode
	,ProvinceName = pv.ProvinceName
	,ProvinceCode = pv.ProvinceCode
FROM dbo.Company c
LEFT JOIN dbo.Company p ON c.ParentId = p.Id
LEFT JOIN dbo.[Address] a ON a.OwnerGuid = c.ObjectGuid
LEFT JOIN dbo.[AddressType] at ON a.AddressTypeId = at.Id
LEFT JOIN dbo.City ci ON a.CityId = ci.Id
LEFT JOIN dbo.Country ct ON a.CountryId = ct.Id
LEFT JOIN dbo.Province pv ON a.ProvinceId = pv.Id
WHERE a.AddressTypeID = 4