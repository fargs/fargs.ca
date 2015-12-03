


CREATE VIEW [API].[PhysicianDocument]
AS
SELECT 
	 d.[Id]
	,d.[ObjectGuid]
	,d.[PhysicianId]
	,d.[DocumentTemplateId]
	,d.[EffectiveDate]
	,d.[ExpiryDate]
	,d.[Path]
	,d.[Extension]
	,d.[Name]
	,d.[ModifiedUser]
	,d.PhysicianDisplayName 
	,d.DocumentTemplateName 
FROM [Private].[Document] d
WHERE CompanyId IS NULL