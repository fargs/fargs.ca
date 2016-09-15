


CREATE VIEW [API].[ServiceRequest]
AS
WITH Tasks
AS (
	SELECT ServiceRequestId
		, TotalTasks = COUNT(TaskId)
		, ClosedTasks = COUNT(CompletedDate)
		, OpenTasks = SUM(CASE WHEN CompletedDate IS NOT NULL THEN 0 ELSE 1 END)
	FROM dbo.ServiceRequestTask t
	WHERE IsObsolete = 0 AND TaskType IS NULL
	GROUP BY ServiceRequestId
),
NextTask
AS (
	SELECT ServiceRequestId, Id, TaskName, AssignedTo, AssignedToName
	FROM (
		SELECT t.ServiceRequestId
			, t.Id
			, t.TaskName
			, t.TaskPhaseId
			, t.TaskPhaseName
			, t.ResponsibleRoleId
			, t.ResponsibleRoleName
			, t.AssignedTo
			, AssignedToName = dbo.GetDisplayName(u.FirstName, u.LastName, u.Title)
			, RowNum = ROW_NUMBER() OVER(PARTITION BY t.ServiceRequestId ORDER BY t.[Sequence])
		FROM dbo.ServiceRequestTask t
		LEFT JOIN dbo.AspNetUsers u ON t.AssignedTo = u.Id
		WHERE t.CompletedDate IS NULL AND IsObsolete = 0
	) t
	WHERE RowNum = 1
),
ServicePhaseTasks
AS (
	SELECT ServiceRequestId
		, TotalTasks = COUNT(TaskId)
		, ClosedTasks = COUNT(CompletedDate)
		, OpenTasks = COUNT(CASE WHEN CompletedDate IS NOT NULL THEN NULL ELSE '2000-01-01' END)
	FROM dbo.ServiceRequestTask t
	WHERE t.TaskPhaseId = 34 AND IsObsolete = 0
	GROUP BY t.ServiceRequestId
)
SELECT
	 sr.[Id] 
	,sr.[ObjectGuid]
	,sr.[CompanyReferenceId]
	,sr.[ClaimantName]
	,sr.[ServiceCatalogueId]
	,sr.[Body]
	,sr.[AddressId]
	,sr.[RequestedDate]
	,sr.[RequestedBy]
	,sr.[CancelledDate]
	,sr.[AvailableSlotId]
	,sr.[DueDate]
	,sr.CaseCoordinatorId
	,sr.IntakeAssistantId
	,sr.DocumentReviewerId
	,ServiceRequestPrice = sr.[Price]
	,sr.Notes
	,sr.CompanyId
	,sr.IsNoShow
	,sr.IsLateCancellation
	,sr.NoShowRate
	,sr.LateCancellationRate
	,sr.[ModifiedDate]
	,sr.[ModifiedUser]
	,sr.ServiceId
	,sr.AppointmentDate
	,sr.StartTime
	,Duration = NULL -- Obsolete
	,sr.EndTime
	,[Title] = dbo.GetServiceRequestTitle(s.Id, sr.Id, sr.AppointmentDate, sr.DueDate, sr.StartTime, l.ShortText, s.Code, c.Code, p.UserName, sr.ClaimantName)
	,ServiceRequestStatusId = dbo.GetServiceRequestStatusId(ts.OpenTasks)
	,ServiceRequestStatusText = lisr.[Text]
	,ServiceStatusId = dbo.GetServiceStatusId(sr.IsLateCancellation, sr.CancelledDate, sr.IsNoShow)
	,ServiceStatusText = lis.[Text]
	,ServiceName = s.Name
	,ServiceCode = s.Code
	,ServicePrice = s.Price
	,s.ServiceCategoryId
	,sr.PhysicianId
	,PhysicianDisplayName = dbo.GetDisplayName(p.FirstName, p.LastName, p.Title)
	,PhysicianUserName = p.UserName
	,PhysicianInitials = dbo.GetInitials(p.FirstName, p.LastName)
	,PhysicianColorCode = p.ColorCode
	,CompanyGuid = c.ObjectGuid
	,CompanyName = c.Name
	,ParentCompanyName = cp.Name
	,ServiceCataloguePrice = sr.ServiceCataloguePrice
	,EffectivePrice = COALESCE(sr.Price, sr.Price, s.Price)
	,RequestedByName = dbo.GetDisplayName(rb.FirstName, rb.LastName, rb.Title)
	,AddressName = a.Name
	,AddressTypeId = a.AddressTypeId
	,AddressTypeName = a.AddressTypeName
	,Address1 = a.[Address1]
	,Address2 = a.[Address2]
	,City = a.[City]
	,PostalCode = a.PostalCode
	,ProvinceId = a.ProvinceId
	,ProvinceName = a.ProvinceCode
	,CountryId = a.CountryId
	,CountryName = a.CountryCode
	,LocationId = a.LocationId
	,CaseCoordinatorName = dbo.GetDisplayName(cc.FirstName, cc.LastName, cc.Title)
	,CaseCoordinatorInitials = dbo.GetInitials(cc.FirstName, cc.LastName)
	,CaseCoordinatorColorCode = cc.ColorCode
	,CaseCoordinatorUserName = cc.UserName
	,IntakeAssistantName = dbo.GetDisplayName(ia.FirstName, ia.LastName, ia.Title)
	,IntakeAssistantInitials = dbo.GetInitials(ia.FirstName, ia.LastName)
	,IntakeAssistantColorCode = ia.ColorCode
	,IntakeAssistantUserName = ia.UserName
	,IntakeAssistantBoxCollaborationId = sr.IntakeAssistantBoxCollaborationId
	,IntakeAssistantBoxUserId = ia.BoxUserId
	,DocumentReviewerName = dbo.GetDisplayName(dr.FirstName, dr.LastName, dr.Title)
	,DocumentReviewerInitials = dbo.GetInitials(dr.FirstName, dr.LastName)
	,DocumentReviewerColorCode = dr.ColorCode
	,DocumentReviewerUserName = dr.UserName
	,DocumentReviewerBoxCollaborationId = sr.DocumentReviewerBoxCollaborationId
	,DocumentReviewerBoxUserId = dr.BoxUserId
	,CalendarEventTitle = a.LocationShortName + ': ' + sr.ClaimantName + ' (' + s.Code + ') ' + c.Code + '-' + CONVERT(nvarchar(10), sr.Id)
	,ts.TotalTasks
	,ts.ClosedTasks
	,ts.OpenTasks
	,NextTaskId = nt.Id
	,NextTaskName = nt.TaskName
	,NextTaskAssignedTo = nt.AssignedTo
	,NextTaskAssignedtoName = nt.AssignedToName
	,BoxCaseFolderId
	,BoxPhysicianFolderId = p.BoxFolderId
	,ServiceCategoryName = CONVERT(varchar(2),null)
	,DocumentFolderLink = CONVERT(varchar(2),null)
	,ServicePortfolioName = CONVERT(varchar(2),null)
	,LocationName = CONVERT(varchar(2),null)
FROM dbo.ServiceRequest sr
INNER JOIN dbo.[Service] s ON s.Id = sr.ServiceId
INNER JOIN dbo.[AspNetUsers] p ON sr.PhysicianId = p.Id
INNER JOIN dbo.Company c ON sr.CompanyId = c.Id
LEFT JOIN dbo.Company cp ON c.ParentId = cp.Id
LEFT JOIN dbo.[AspNetUsers] cc ON cc.Id = sr.CaseCoordinatorId
LEFT JOIN dbo.[AspNetUsers] ia ON ia.Id = sr.IntakeAssistantId
LEFT JOIN dbo.[AspNetUsers] dr ON dr.Id = sr.DocumentReviewerId
LEFT JOIN dbo.[AspNetUsers] rb ON rb.Id = sr.RequestedBy
LEFT JOIN API.[Address] a ON sr.AddressId = a.Id
LEFT JOIN dbo.LookupItem l ON a.LocationId = l.Id
LEFT JOIN Tasks ts ON sr.Id = ts.ServiceRequestId
LEFT JOIN ServicePhaseTasks spt ON sr.Id = spt.ServiceRequestId
LEFT JOIN NextTask nt ON sr.Id = nt.ServiceRequestId
LEFT JOIN dbo.LookupItem lisr ON dbo.GetServiceRequestStatusId(ts.OpenTasks) = lisr.Id
LEFT JOIN dbo.LookupItem lis ON dbo.GetServiceStatusId(sr.IsLateCancellation, sr.CancelledDate, sr.IsNoShow) = lis.Id