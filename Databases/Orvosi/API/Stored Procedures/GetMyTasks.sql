

CREATE PROC API.GetMyTasks
	@AssignedTo UNIQUEIDENTIFIER
	, @Now DATETIME
AS
WITH Requests 
AS (
	SELECT t.AssignedTo
		, t.ServiceRequestId
		, t.TaskId
		, t.TaskName
		, t.CompletedDate
		, t.IsObsolete
		, t.IsDependentOnExamDate
		, DueDateBase
		, DueDateDiff
		, LTRIM(RTRIM(m.n.value('.[1]','varchar(8000)'))) AS DependsOn
	FROM
	(
		SELECT AssignedTo
			, ServiceRequestId
			, TaskId 
			, TaskName
			, CompletedDate
			, IsObsolete
			, IsDependentOnExamDate
			, DueDateBase
			, DueDateDiff
			, CAST('<XMLRoot><RowData>' + REPLACE(DependsOn,',','</RowData><RowData>') + '</RowData></XMLRoot>' AS XML) AS x
		FROM   dbo.ServiceRequestTask
		WHERE IsObsolete = 0 AND CompletedDate IS NULL -- Because this is a stored proc, I want to control the amount of records returned here, not in the service layer.
			AND AssignedTo = @AssignedTo
	)t
	CROSS APPLY x.nodes('/XMLRoot/RowData')m(n)
) 
, Tasks
AS (
SELECT t.AssignedTo
	, t.ServiceRequestId
	, t.TaskId
	, t.TaskName
	, TaskStatusId = dbo.GetTaskStatusId(t.CompletedDate, t.IsObsolete, srt.CompletedDate, srt.IsObsolete, @Now, sr.AppointmentDate, t.IsDependentOnExamDate)
	, t.IsObsolete
	, DependentCompletedDate = srt.CompletedDate
	, DependentIsObsolete = srt.IsObsolete
	, sr.AppointmentDate
	, ReportDueDate = dbo.GetReportDueDate(sr.DueDate, sr.AppointmentDate, 7)
	, DueDate = dbo.GetTaskDueDate(sr.AppointmentDate
									, dbo.GetReportDueDate(sr.DueDate, sr.AppointmentDate, 7)
									, t.DueDateBase
									, t.DueDateDiff)
	, sr.StartTime
	, sr.CompanyId
	, sr.ServiceId
FROM Requests t
LEFT JOIN dbo.ServiceRequestTask srt 
	ON t.ServiceRequestId = srt.ServiceRequestId
		AND CASE WHEN t.DependsOn = 'ExamDate' THEN NULL ELSE t.DependsOn END = srt.TaskId
LEFT JOIN dbo.ServiceRequest sr ON t.ServiceRequestId = sr.Id
)
SELECT t.ServiceRequestId
	, t.ReportDueDate
	, t.AppointmentDate
	, t.StartTime
	, t.TaskId
	, t.TaskName
	, t.DueDate
	, t.TaskStatusId
	, TaskStatusName = li.[Text]
	, t.IsObsolete
	, t.CompanyId
	, CompanyName = c.Name
	, t.ServiceId
	, ServiceName = s.Name
FROM Tasks t
LEFT JOIN dbo.Company c ON t.CompanyId = c.Id
LEFT JOIN dbo.[Service] s ON t.ServiceId = s.Id
LEFT JOIN dbo.LookupItem li ON t.TaskStatusId = li.Id