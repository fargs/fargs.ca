




CREATE VIEW [API].[Document]
AS
SELECT 
	 d.[Id]
	,d.[ObjectGuid]
	,d.[OwnedByObjectGuid]
	,d.[DocumentTemplateId]
	,d.[EffectiveDate]
	,d.[ExpiryDate]
	,d.[Content]
	,d.[ContentType]
	,d.[Path]
	,d.[Name]
	,d.[Extension]
	,d.[ModifiedDate]
	,d.[ModifiedUser]
	,OwnedByDisplayName = e.DisplayName
	,OwnedByEntityType = e.EntityType
	,DocumentTemplateName = dt.Name
	,DocumentTemplateOwnedByDisplayName = et.DisplayName
	,DocumentTemplateOwnedByEntityType = et.EntityType
FROM [dbo].[Document] d
LEFT JOIN [API].[Entity] e ON d.OwnedByObjectGuid = e.EntityGuid
LEFT JOIN [dbo].[DocumentTemplate] dt ON d.DocumentTemplateId = dt.Id
LEFT JOIN [API].[Entity] et ON dt.OwnedByObjectGuid = et.EntityGuid