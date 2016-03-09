








CREATE VIEW [API].[Address]
AS
SELECT 
	 a.[Id]
	,[EntityGuid] = a.[ObjectGuid]
	,a.OwnerGuid
	,a.[AddressTypeID]
	,AddressTypeName = at.Name
	,a.[Name]
	,a.[Attention]
	,a.[Address1]
	,a.[Address2]
	,a.[City]
	,a.[PostalCode]
	,a.[CountryID]
	,a.[ProvinceID]
	,a.[ModifiedDate]
	,a.[ModifiedUser]
	,CountryName = c.Name
	,CountryCode = c.ISO3CountryCode
	,ProvinceName = p.ProvinceName
	,ProvinceCode = p.ProvinceCode
FROM [dbo].[Address] a
LEFT JOIN dbo.[AddressType] at ON a.AddressTypeId = at.Id
LEFT JOIN dbo.Country c ON a.CountryId = c.Id
LEFT JOIN dbo.Province p ON a.ProvinceId = p.Id