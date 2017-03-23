CREATE PROC Migrate.UpdateTaskDueDates
AS
UPDATE srt SET srt.DueDate = t.DueDate  
--SELECT srt.Id, t.DueDate
FROM dbo.ServiceRequestTask srt
LEFT JOIN ( 
	SELECT srt.Id
		, srt.TaskId 
		, TaskName = t.[Name]
		, t.ResponsibleRoleId
		, t.IsCriticalPath
		, ResponsibleRoleName = r.[Name]
		, TaskStatusId
		, TaskPhaseName
		, DueDate = CASE 
			WHEN srt.TaskId = 133 THEN sr.AppointmentDate
			WHEN srt.TaskId = 19 THEN sr.DueDate
			WHEN t.DueDateBase = 1 THEN DATEADD(d, t.DueDateDiff, sr.AppointmentDate) 
			WHEN t.DueDateBase = 2 THEN DATEADD(d, t.DueDateDiff, sr.DueDate)
			WHEN t.DueDateBase = 3 THEN DATEADD(d, t.DueDateDiff, srt.ModifiedDate)
			END 
		, t.DueDateBase
		, t.DueDateDiff
		, CompletedDate
		, CompletedBy = u.Email
	FROM ServiceRequestTask srt
	LEFT JOIN Task t ON srt.TaskId = t.Id
	LEFT JOIN AspNetRoles r ON t.ResponsibleRoleId = r.Id
	LEFT JOIN AspNetUsers u ON srt.CompletedBy = u.Id
	LEFT JOIN ServiceRequest sr ON srt.ServiceRequestId = sr.Id
) t ON srt.Id = t.Id