


CREATE VIEW [Private].[Document]
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
	,PhysicianDisplayName = dbo.GetDisplayName(u.FirstName, u.LastName, u.Title)
	,DocumentTemplateName = dt.Name
	,CompanyName = c.Name
FROM dbo.[Document] d
LEFT JOIN dbo.AspNetUsers u ON u.Id = d.PhysicianId
LEFT JOIN dbo.Company c ON c.Id = d.CompanyId
LEFT JOIN dbo.DocumentTemplate dt ON dt.Id = d.DocumentTemplateId