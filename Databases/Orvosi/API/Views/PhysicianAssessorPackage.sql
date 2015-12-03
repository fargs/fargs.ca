


CREATE VIEW [API].[PhysicianAssessorPackage]
AS
SELECT 
	 d.[Id]
	,d.[ObjectGuid]
	,d.[PhysicianId]
	,d.[CompanyId]
	,d.[DocumentTemplateId]
	,d.[EffectiveDate]
	,d.[ExpiryDate]
	,d.[Path]
	,d.[Extension]
	,d.[Name]
	,d.[ModifiedUser]
	,d.PhysicianDisplayName 
	,d.DocumentTemplateName 
	,d.CompanyName 
FROM [Private].[Document] d
WHERE CompanyId IS NOT NULL