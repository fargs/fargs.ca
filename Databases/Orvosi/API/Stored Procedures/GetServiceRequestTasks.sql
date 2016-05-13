CREATE PROC API.GetServiceRequestTasks
	@Now DATETIME
	, @ServiceRequestIds NVARCHAR(255)
AS

WITH Tasks
AS (
	SELECT t1.Id
		, t1.AssignedTo
		, t1.ServiceRequestId
		, t1.TaskId 
		, t1.TaskName
		, t1.CompletedDate
		, t1.IsObsolete
		, t1.DueDateBase
		, t1.DueDateDiff
		, t1.IsDependentOnExamDate
		, sr.AppointmentDate
		, sr.DueDate
		, PreviousId = LAG(t1.Id, 1, NULL) OVER(PARTITION BY t1.ServiceRequestId, t1.AssignedTo ORDER BY t1.[Sequence])
		, RowNum = ROW_NUMBER() OVER(PARTITION BY t1.ServiceRequestId, t1.AssignedTo, CASE WHEN t1.CompletedDate IS NULL THEN 1 ELSE 0 END ORDER BY t1.[Sequence])
	FROM dbo.ServiceRequestTask t1
	LEFT JOIN dbo.ServiceRequest sr ON t1.ServiceRequestId = sr.Id
	OUTER APPLY dbo.Split(@ServiceRequestIds, ',') ids
	WHERE sr.Id = ids.[Data]
), 
TasksWithStatusId
AS (
	SELECT t1.*
		, TaskStatusId = dbo.GetTaskStatusId(t1.CompletedDate, t1.IsObsolete, t2.CompletedDate, t2.IsObsolete, @Now, t1.AppointmentDate, t1.IsDependentOnExamDate)
	FROM Tasks t1
	LEFT JOIN dbo.ServiceRequestTask t2 ON t1.PreviousId = t2.Id
)
SELECT t1.Id
	, t1.ServiceRequestId
	, t1.TaskName
	, AssignedToInitials = dbo.GetInitials(u.FirstName, u.LastName)
	, AssignedToDisplayName = dbo.GetDisplayName(u.FirstName, u.LastName, u.Title)
	, AssignedToColorCode = u.ColorCode
	, TaskDueDate = dbo.GetTaskDueDate(t1.AppointmentDate, t1.DueDate, t1.DueDateBase, t1.DueDateDiff) 
	, TaskStatusName = ts.[Text]
FROM TasksWithStatusId t1
LEFT JOIN dbo.AspNetUsers u ON t1.AssignedTo = u.Id
LEFT JOIN dbo.LookupItem ts ON ts.Id = t1.TaskStatusId
WHERE t1.TaskStatusId NOT IN (40, 47) -- Obsolete, Done
	AND t1.RowNum = 1