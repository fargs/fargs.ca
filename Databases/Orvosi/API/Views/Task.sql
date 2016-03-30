﻿




CREATE VIEW [API].[Task]
AS
SELECT t.ServiceCategoryId, ServiceCategoryName = sc.Name
	, t.Id
	, TaskName = t.Name
	, t.Guidance
	, t.EstimatedHours
	, t.HourlyRate
	, t.[Sequence]
	, t.IsBillable
	, t.ResponsibleRoleId
	, ResponsibleRoleName = r.Name
	, t.TaskPhaseId
	, TaskPhaseName = li.[Text]
	, t.DependsOn
	, t.DueDateBase
	, t.DueDateDiff
	, t.ShortName
	, t.IsCriticalPath
FROM dbo.Task t 
LEFT JOIN dbo.ServiceCategory sc ON t.ServiceCategoryId = sc.Id
LEFT JOIN dbo.AspNetRoles r ON t.ResponsibleRoleId = r.Id
LEFT JOIN dbo.LookupItem li ON t.TaskPhaseId = li.Id