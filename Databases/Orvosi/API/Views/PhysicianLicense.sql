



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
	,t.[ModifiedDate]
	,t.[ModifiedUser]
FROM dbo.PhysicianLicense t