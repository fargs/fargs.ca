
CREATE VIEW [API].[ServiceRequestTemplateTask]
AS
SELECT * FROM dbo.ServiceRequestTemplateTask
--SELECT te.ServiceCategoryId, ServiceCategoryName = sc.Name
--	, tet.Id
--	, tet.TaskName
--	, tet.Guidance
--	, tet.EstimatedHours
--	, tet.HourlyRate
--	, tet.[Sequence]
--	, tet.IsBillable
--	, tet.ResponsibleRoleId
--	, ResponsibleRoleName = r.Name
--	, tet.TaskPhaseId
--	, TaskPhaseName = li.[Text]
--FROM dbo.ServiceRequestTemplate te 
--INNER JOIN dbo.ServiceRequestTemplateTask tet ON tet.ServiceRequestTemplateId = te.Id
--INNER JOIN dbo.ServiceCategory sc ON te.ServiceCategoryId = sc.Id
--INNER JOIN dbo.AspNetRoles r ON tet.ResponsibleRoleId = r.Id
--LEFT JOIN dbo.LookupItem li ON tet.TaskPhaseId = li.Id