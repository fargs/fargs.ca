CREATE VIEW API.TaskStatus
AS
SELECT t.Id
	, t.TaskStatusId
	, RowNum = ROW_NUMBER() OVER(PARTITION BY t.Id ORDER BY t.TaskStatusId)
FROM (
	SELECT t.Id
	, TaskStatusId = dbo.GetTaskStatusId(t.CompletedDate, t.IsObsolete, tt.TaskType, c.CompletedDate, c.IsObsolete, ct.TaskType, GETDATE(), sr.AppointmentDate)
	FROM dbo.ServiceRequest sr
	LEFT JOIN dbo.ServiceRequestTask t ON t.ServiceRequestId = sr.Id
	LEFT JOIN dbo.Task tt ON tt.Id = t.TaskId
	LEFT JOIN dbo.ServiceRequestTaskDependent d ON t.Id = d.ParentId
	LEFT JOIN dbo.ServiceRequestTask c ON c.Id = d.ChildId
	LEFT JOIN dbo.Task ct ON ct.Id = c.TaskId
	--WHERE t.ServiceRequestId = 531
) t