﻿

CREATE PROC [API].[GetServiceRequest]
	@ServiceRequestId INT
	, @Now DATETIME
AS
WITH Requests 
AS (
	SELECT t.Id
		, t.AssignedTo
		, t.ServiceRequestId
		, t.TaskId
		, t.TaskName
		, t.CompletedDate
		, t.IsObsolete
		, t.IsDependentOnExamDate
		, t.[Sequence]
		, DependsOnCSV = t.[DependsOn]
		, LTRIM(RTRIM(m.n.value('.[1]','varchar(8000)'))) AS DependsOn
	FROM
	(
		SELECT Id
			, AssignedTo
			, ServiceRequestId
			, TaskId 
			, TaskName
			, CompletedDate
			, IsObsolete
			, IsDependentOnExamDate
			, [Sequence]
			, [DependsOn]
			, CAST('<XMLRoot><RowData>' + REPLACE(CASE WHEN DependsOn IS NULL THEN '' ELSE DependsOn END,',','</RowData><RowData>') + '</RowData></XMLRoot>' AS XML) AS x
		FROM dbo.ServiceRequestTask
		WHERE ServiceRequestId = @ServiceRequestId
	) t
	CROSS APPLY x.nodes('/XMLRoot/RowData')m(n)
) 
, Tasks
AS (
SELECT t.Id
	, t.AssignedTo
	, t.ServiceRequestId
	, t.TaskId
	, t.TaskName
	, TaskStatusId = dbo.GetTaskStatusId(t.CompletedDate, t.IsObsolete, srt.CompletedDate, srt.IsObsolete, @Now, sr.AppointmentDate, t.IsDependentOnExamDate)
	, t.IsObsolete
	, t.[Sequence]
	, sr.AppointmentDate
	, sr.DueDate
	, sr.StartTime
	, sr.CompanyId
	, sr.ServiceId
	, sr.ClaimantName
	, sr.PhysicianId
	, sr.AddressId
	, sr.BoxCaseFolderId
	, t.DependsOnCSV
	, t.CompletedDate
	, t.IsDependentOnExamDate
FROM Requests t
LEFT JOIN dbo.ServiceRequestTask srt 
	ON t.ServiceRequestId = srt.ServiceRequestId
		AND CASE WHEN t.DependsOn = 'ExamDate' THEN NULL ELSE t.DependsOn END = srt.TaskId
LEFT JOIN dbo.ServiceRequest sr ON t.ServiceRequestId = sr.Id
)
, TasksWithStatus
AS
(
	SELECT 
	  Id
	, AssignedTo
	, ServiceRequestId
	, TaskId
	, TaskName
	, CompletedDate
	, IsObsolete
	, AppointmentDate
	, DueDate
	, StartTime
	, CompanyId
	, ServiceId
	, ClaimantName
	, PhysicianId
	, AddressId
	, TaskStatusId = MIN(TaskStatusId)
	, [Sequence]
	, BoxCaseFolderId
	, DependsOnCSV
FROM Tasks
GROUP BY 
	  Id
	, AssignedTo
	, ServiceRequestId
	, TaskId
	, TaskName
	, CompletedDate
	, IsObsolete
	, AppointmentDate
	, DueDate
	, StartTime
	, CompanyId
	, ServiceId
	, ClaimantName
	, PhysicianId
	, AddressId
	, [Sequence]
	, BoxCaseFolderId
	, DependsOnCSV
)
SELECT t.Id
	, t.ServiceRequestId
	, ReportDueDate = t.DueDate
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
	, s.ServiceCategoryId
	, t.ClaimantName
	, t.AssignedTo
	, AssignedToDisplayName = dbo.GetDisplayName(ast.FirstName, ast.LastName, ast.Title)
	, AssignedToColorCode = ast.ColorCode
	, AssignedToInitials = dbo.GetInitials(ast.FirstName, ast.LastName)
	, t.AddressId
	, AddressName = a.Name
	, City = ci.Name
	, TaskSequence = t.[Sequence]
	, BoxCaseFolderId
	, [Title] = dbo.GetServiceRequestTitle(s.Id, t.ServiceRequestId, t.AppointmentDate, t.DueDate, t.StartTime, ci.Code, s.Code, c.Code, p.UserName, t.ClaimantName)
	, DependsOnCSV
	, ServiceCode = s.Code
	, ServiceColorCode = s.ColorCode
	, bc.BoxCollaborationId
	, t.CompletedDate
FROM TasksWithStatus t
LEFT JOIN dbo.Company c ON t.CompanyId = c.Id
LEFT JOIN dbo.[Service] s ON t.ServiceId = s.Id
LEFT JOIN dbo.[Address] a ON t.AddressId = a.Id
LEFT JOIN dbo.[City] ci ON a.CityId = ci.Id
LEFT JOIN dbo.LookupItem li ON t.TaskStatusId = li.Id
LEFT JOIN dbo.[AspNetUsers] p ON t.PhysicianId = p.Id
LEFT JOIN dbo.[AspNetUSers] ast ON t.AssignedTo = ast.Id
LEFT JOIN dbo.[ServiceRequestBoxCollaboration] bc ON t.ServiceRequestId = bc.ServiceRequestId AND ast.Id = bc.UserId