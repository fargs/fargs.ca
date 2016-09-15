


--EXEC API.GetAssignedServiceRequests '8e9885d8-a0f7-49f6-9a3e-ff1b4d52f6a9', '2016-07-11'

CREATE PROC [API].[GetAssignedServiceRequests]
	  @AssignedTo UNIQUEIDENTIFIER
	, @Now DATETIME
	, @ShowClosed BIT
	, @ServiceRequestId INT
AS

DECLARE @SelectedServiceRequests AS SelectedServiceRequestTVP
INSERT INTO @SelectedServiceRequests (Id)
SELECT DISTINCT ServiceRequestId 
FROM dbo.ServiceRequestTask 
WHERE (AssignedTo = @AssignedTo OR @AssignedTo IS NULL)
	AND (ServiceRequestId = @ServiceRequestId OR @ServiceRequestId IS NULL)
OPTION(RECOMPILE);

SELECT 
	  ServiceRequestId = srt.ServiceRequestId
	, ReportDueDate = sr.DueDate
	, sr.AppointmentDate
	, sr.StartTime
	, sr.DueDate
	, sr.CompanyId
	, CompanyName = c.Name
	, sr.ServiceId
	, ServiceName = s.Name
	, s.ServiceCategoryId
	, sr.ClaimantName
	, sr.AddressId
	, AddressName = a.Name
	, City = ci.Name
	, BoxCaseFolderId
	, [Title] = dbo.GetServiceRequestTitle(s.Id, sr.Id, sr.AppointmentDate, sr.DueDate, sr.StartTime, ci.Code, s.Code, c.Code, p.UserName, sr.ClaimantName)
	, ServiceCode = s.Code
	, ServiceColorCode = s.ColorCode
	, sr.IsLateCancellation
	, sr.IsNoShow
	, sr.CancelledDate
	, r.IsClosed
	, ServiceRequestStatusId = r.StatusId
	, t.Id
	, srt.TaskId
	, srt.TaskName
	, TaskSequence = srt.[Sequence]
	, TaskStatusId = t.StatusId
	, TaskStatusName = ts.[Name]
	, bc.BoxCollaborationId
	, srt.CompletedDate
	, srt.IsObsolete
	, srt.ResponsibleRoleId
	, srt.[Workload]
	, srt.AssignedTo
	, srt.TaskType
	, srt.ResponsibleRoleName
	, AssignedToDisplayName = dbo.GetDisplayName(ast.FirstName, ast.LastName, ast.Title)
	, AssignedToColorCode = ast.ColorCode
	, AssignedToInitials = dbo.GetInitials(ast.FirstName, ast.LastName)
	, AssignedToRoleId = aus.RoleId
	, AssignedToRoleName = aus.[Name]
	, csv.DependsOnCSV
FROM 
---- ServiceRequestTask
dbo.GetServiceRequestTaskStatus(@Now, @SelectedServiceRequests, NULL) t
INNER JOIN dbo.ServiceRequestTask srt ON srt.Id = t.Id
LEFT JOIN dbo.TaskStatus ts ON t.StatusId = ts.Id
LEFT JOIN dbo.[AspNetUsers] ast ON srt.AssignedTo = ast.Id
LEFT JOIN (
	SELECT RoleId, UserId, [Name]
	FROM (
		SELECT RoleId, UserId, [Name]
			, RowNum = ROW_NUMBER() OVER(PARTITION BY ur.UserId ORDER BY ur.RoleId)
		FROM dbo.AspNetUserRoles ur
		INNER JOIN dbo.AspNetRoles r ON ur.RoleId = r.Id
	) t
	WHERE t.RowNum = 1
) aus ON ast.Id = aus.UserId
LEFT JOIN [Private].ServiceRequestTaskDependsOnCSV csv ON t.Id = csv.ServiceRequestTaskId
-- ServiceRequest
LEFT JOIN dbo.GetServiceRequestStatus(@Now, @SelectedServiceRequests, @AssignedTo) r ON r.Id = srt.ServiceRequestId
LEFT JOIN dbo.ServiceRequest sr ON sr.Id = r.Id
LEFT JOIN dbo.[Service] s ON s.Id = sr.ServiceId
LEFT JOIN dbo.Company c ON sr.CompanyId = c.Id
LEFT JOIN dbo.[Address] a ON sr.AddressId = a.Id
LEFT JOIN dbo.[City] ci ON a.CityId = ci.Id
LEFT JOIN dbo.[AspNetUsers] p ON sr.PhysicianId = p.Id
LEFT JOIN dbo.[ServiceRequestBoxCollaboration] bc ON sr.Id = bc.ServiceRequestId AND ast.Id = bc.UserId
WHERE (r.IsClosed = @ShowClosed OR @ShowClosed IS NULL)
ORDER BY sr.Id, srt.[Sequence]
OPTION (RECOMPILE)

--'
--IF @ShowClosed = 1
--	SET @SQLString = @SQLString + ' OR r.IsClosed = 1'

--DECLARE @ParamDefinition NVARCHAR(200)
--SET @ParamDefinition = N'
--	  @AssignedTo UNIQUEIDENTIFIER
--	, @Now DATETIME'

--EXECUTE sp_executesql @SQLString, @ParamDefinition
--	, @AssignedTo = @AssignedTo
--    , @Now = @Now;