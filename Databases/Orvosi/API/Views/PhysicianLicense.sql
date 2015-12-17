





CREATE VIEW [API].[PhysicianLicense]
AS
SELECT 
	 t.[Id]
	,t.[PhysicianId]
	,t.[CollegeName]
	,t.[LongName]
	,t.[ExpiryDate]
	,t.[MemberName]
	,t.[CertificateClass]
	,t.[IsPrimary]
	,t.[Preference]
	,t.[DocumentId]
	,t.[CountryId]
	,t.[ProvinceId]
	,t.[ModifiedDate]
	,t.[ModifiedUser]
	,d.Content
	,d.ContentType
	,CountryName = c.Name
	,CountryCode = c.ISO3CountryCode
	,ProvinceName = p.ProvinceName
	,ProvinceCode = p.ProvinceCode
FROM dbo.PhysicianLicense t
LEFT JOIN dbo.Document d ON t.DocumentId = d.Id AND t.PhysicianId = OwnedByObjectGuid
LEFT JOIN dbo.Country c ON t.CountryId = c.Id
LEFT JOIN dbo.Province p ON t.ProvinceId = p.Id