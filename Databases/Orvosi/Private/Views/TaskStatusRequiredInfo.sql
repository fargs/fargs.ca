
CREATE VIEW [Private].[TaskStatusRequiredInfo]
AS
SELECT PrimaryId = t.Id
	, PrimaryCompletedDate = t.CompletedDate
	, PrimaryIsObsolete = t.IsObsolete
	, PrimaryTaskType = tt.TaskType
	, PrimaryAssignedTo = t.AssignedTo
	, DependentCompletedDate = c.CompletedDate
	, DependentIsObsolete = c.IsObsolete
	, DependentTaskType = ct.TaskType
	, ServiceRequestAppointmentDate = sr.AppointmentDate
FROM dbo.ServiceRequest sr
LEFT JOIN dbo.ServiceRequestTask t ON t.ServiceRequestId = sr.Id
LEFT JOIN dbo.Task tt ON tt.Id = t.TaskId
LEFT JOIN dbo.ServiceRequestTaskDependent d ON t.Id = d.ParentId
LEFT JOIN dbo.ServiceRequestTask c ON c.Id = d.ChildId
LEFT JOIN dbo.Task ct ON ct.Id = c.TaskId