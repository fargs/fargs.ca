



CREATE VIEW [API].[CompanyAssessorPackage]
AS
SELECT 
	 DocumentTemplateId = d.[Id]
	,DocumentTemplateName = d.[Name]
	,CompanyObjectGuid = d.[OwnedByObjectGuid]
	,d.[ModifiedDate]
	,d.[ModifiedUser]
	,CompanyName = c.Name
FROM [dbo].[DocumentTemplate] d
INNER JOIN dbo.Company c ON c.ObjectGuid = d.OwnedByObjectGuid