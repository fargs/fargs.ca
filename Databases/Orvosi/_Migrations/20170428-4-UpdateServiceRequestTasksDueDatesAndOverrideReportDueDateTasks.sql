UPDATE srt SET 
	srt.DueDate = t.DueDate
	, srt.DueDateDurationFromBaseline = t.DueDateDurationFromBaseline
	, srt.EffectiveDate = t.EffectiveDate
	, srt.EffectiveDateDurationFromBaseline = t.EffectiveDateDurationFromBaseline
	, srt.IsCriticalPath = ISNULL(t.IsCriticalPath, 0)
--SELECT 
--    srt.Id
--	, t.DueDate
--	, t.DueDateDurationFromBaseline
--	, t.EffectiveDate
--	, t.EffectiveDateDurationFromBaseline
--	, t.IsCriticalPath
FROM dbo.ServiceRequestTask srt
LEFT JOIN ( 
	SELECT
		srt.Id
		, t.TaskId
		, t.IsCriticalPath
		, srt.TaskName
		, sr.AppointmentDate
		, ReportDueDate = sr.DueDate
		, DueDate = CASE 
			WHEN srt.TaskId = 133 AND sr.AppointmentDate IS NOT NULL THEN sr.AppointmentDate
			WHEN srt.TaskId = 19 AND sr.AppointmentDate IS NULL THEN sr.DueDate
			--WHEN srt.TaskId IN (21,28,132,134) THEN srt.ModifiedDate
			WHEN sr.AppointmentDate IS NOT NULL THEN DATEADD(d, t.DueDateDurationFromBaseline, sr.AppointmentDate) 
			WHEN sr.AppointmentDate IS NULL THEN DATEADD(d, t.DueDateDurationFromBaseline, sr.DueDate)
			END 
		, EffectiveDate = CASE 
			WHEN sr.AppointmentDate IS NOT NULL AND t.EffectiveDateDurationFromBaseline IS NOT NULL THEN DATEADD(d, t.EffectiveDateDurationFromBaseline, sr.AppointmentDate) 
			WHEN sr.AppointmentDate IS NULL AND t.EffectiveDateDurationFromBaseline IS NOT NULL THEN DATEADD(d, t.EffectiveDateDurationFromBaseline, sr.DueDate)
			END 
		, t.DueDateDurationFromBaseline
		, t.EffectiveDateDurationFromBaseline
	FROM ServiceRequestTask srt
	INNER JOIN ServiceRequestTemplateTask t ON srt.ServiceRequestTemplateTaskId = t.Id
	LEFT JOIN AspNetRoles r ON t.ResponsibleRoleId = r.Id
	LEFT JOIN AspNetUsers u ON srt.CompletedBy = u.Id
	LEFT JOIN ServiceRequest sr ON srt.ServiceRequestId = sr.Id
) t ON srt.Id = t.Id

/*
This overrides the calculated due date for Approve Report, Submit Report and Close Case tasks to match the Due Date set on the service request.
*/
UPDATE srt SET DueDate = sr.DueDate
--SELECT srt.DueDate
--	, sr.DueDate
FROM ServiceRequestTask srt
LEFT JOIN ServiceRequest sr ON srt.ServiceRequestId = sr.Id
WHERE srt.TaskId IN (19,9,30) AND sr.DueDate IS NOT NULL

