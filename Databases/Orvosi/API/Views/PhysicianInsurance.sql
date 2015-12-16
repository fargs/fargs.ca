






CREATE VIEW [API].[PhysicianInsurance]
AS
SELECT 
	 t.[Id]
	,t.[PhysicianId]
	,t.[Insurer]
	,t.[PolicyNumber]
	,t.[ExpiryDate]
	,t.[DocumentId]
	,t.[ModifiedDate]
	,t.[ModifiedUser]
	,d.Content
	,d.ContentType
FROM dbo.PhysicianInsurance t
LEFT JOIN dbo.Document d ON t.DocumentId = d.Id AND t.PhysicianId = OwnedByObjectGuid