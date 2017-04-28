CREATE PROC [Migrate].[UpdateTaskDueDates]
AS
UPDATE srt SET 
	srt.DueDate = t.DueDate
	, srt.DueDateDurationFromBaseline = t.DueDateDurationFromBaseline
	, srt.EffectiveDate = t.EffectiveDate
	, srt.EffectiveDateDurationFromBaseline = t.EffectiveDateDurationFromBaseline
	, srt.IsCriticalPath = ISNULL(t.IsCriticalPath, 0)
--SELECT srt.Id
--	, t.DueDate
--	, srt.DueDateDurationFromBaseline
--	, srt.EffectiveDate
--	, srt.EffectiveDateDurationFromBaseline
--	, srt.IsCriticalPath
FROM dbo.ServiceRequestTask srt
LEFT JOIN ( 
	SELECT srt.Id
		, srt.TaskId 
		, t.ResponsibleRoleId
		, t.IsCriticalPath
		, ResponsibleRoleName = r.[Name]
		, TaskStatusId
		, srt.TaskName
		, sr.AppointmentDate
		, ReportDueDate = sr.DueDate
		, DueDate = CASE 
			WHEN srt.TaskId = 133 THEN sr.AppointmentDate
			--WHEN srt.TaskId = 19 THEN sr.DueDate
			WHEN srt.TaskId IN (21,28,132,134) THEN srt.ModifiedDate
			WHEN sr.AppointmentDate IS NOT NULL THEN DATEADD(d, t.DueDateDurationFromBaseline, sr.AppointmentDate) 
			WHEN sr.AppointmentDate IS NULL THEN DATEADD(d, t.DueDateDurationFromBaseline, sr.DueDate)
			END 
		, EffectiveDate = CASE 
			WHEN sr.AppointmentDate IS NOT NULL AND t.EffectiveDateDurationFromBaseline IS NOT NULL THEN DATEADD(d, t.EffectiveDateDurationFromBaseline, sr.AppointmentDate) 
			WHEN sr.AppointmentDate IS NULL AND t.EffectiveDateDurationFromBaseline IS NOT NULL THEN DATEADD(d, t.EffectiveDateDurationFromBaseline, sr.DueDate)
			END 
		, t.DueDateDurationFromBaseline
		, t.EffectiveDateDurationFromBaseline
		, CompletedDate
		, CompletedBy = u.Id
	FROM ServiceRequestTask srt
	LEFT JOIN (SELECT * FROM ServiceRequestTemplateTask WHERE ServiceRequestTemplateId = 1) t ON srt.TaskId = t.TaskId
	LEFT JOIN AspNetRoles r ON t.ResponsibleRoleId = r.Id
	LEFT JOIN AspNetUsers u ON srt.CompletedBy = u.Id
	LEFT JOIN ServiceRequest sr ON srt.ServiceRequestId = sr.Id
) t ON srt.Id = t.Id
LEFT JOIN ServiceRequest sr ON srt.ServiceRequestId = sr.Id
WHERE sr.ServiceId IN (SELECT Id FROM Service WHERE ServiceCategoryID = 5)