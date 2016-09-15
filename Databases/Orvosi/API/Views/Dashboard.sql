CREATE VIEW API.Dashboard
AS
SELECT t.Id
	, srt.ServiceRequestId
	, ReportDueDate = sr.DueDate
	, sr.AppointmentDate
	, sr.StartTime
	, srt.TaskId
	, srt.TaskName
	, sr.DueDate
	, t.TaskStatusId
	, TaskStatusName = li.[Text]
	, srt.IsObsolete
	, sr.CompanyId
	, CompanyName = c.Name
	, sr.ServiceId
	, ServiceName = s.Name
	, s.ServiceCategoryId
	, sr.ClaimantName
	, srt.AssignedTo
	, AssignedToDisplayName = dbo.GetDisplayName(ast.FirstName, ast.LastName, ast.Title)
	, AssignedToColorCode = ast.ColorCode
	, AssignedToInitials = dbo.GetInitials(ast.FirstName, ast.LastName)
	, sr.AddressId
	, AddressName = a.Name
	, City = ci.Name
	, TaskSequence = srt.[Sequence]
	, BoxCaseFolderId
	, [Title] = dbo.GetServiceRequestTitle(s.Id, srt.ServiceRequestId, sr.AppointmentDate, sr.DueDate, sr.StartTime, ci.Code, s.Code, c.Code, p.UserName, sr.ClaimantName)
	, ServiceCode = s.Code
	, ServiceColorCode = s.ColorCode
	, bc.BoxCollaborationId
	, srt.CompletedDate
	, sr.IsLateCancellation
	, sr.IsNoShow
	, sr.CancelledDate
	, srt.ResponsibleRoleId
	, srt.[Workload]
FROM API.TaskStatus t
LEFT JOIN dbo.ServiceRequestTask srt ON t.Id = srt.Id
LEFT JOIN dbo.ServiceRequest sr ON srt.ServiceRequestId = sr.Id
LEFT JOIN dbo.Company c ON sr.CompanyId = c.Id
LEFT JOIN dbo.[Service] s ON sr.ServiceId = s.Id
LEFT JOIN dbo.[Address] a ON sr.AddressId = a.Id
LEFT JOIN dbo.[City] ci ON a.CityId = ci.Id
LEFT JOIN dbo.LookupItem li ON t.TaskStatusId = li.Id
LEFT JOIN dbo.[AspNetUsers] p ON sr.PhysicianId = p.Id
LEFT JOIN dbo.[AspNetUSers] ast ON srt.AssignedTo = ast.Id
LEFT JOIN dbo.[ServiceRequestBoxCollaboration] bc ON srt.ServiceRequestId = bc.ServiceRequestId AND ast.Id = bc.UserId
WHERE RowNum = 1