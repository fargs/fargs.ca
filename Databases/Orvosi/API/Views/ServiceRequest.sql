
















CREATE VIEW API.ServiceRequest
AS
WITH Tasks
AS (
	SELECT ServiceRequestId
		, TotalTasks = COUNT(TaskId)
		, ClosedTasks = COUNT(CompletedDate)
		, OpenTasks = COUNT(CASE WHEN CompletedDate IS NOT NULL THEN NULL ELSE '2000-01-01' END)
	FROM dbo.ServiceRequestTask t
	WHERE IsObsolete = 0
	GROUP BY ServiceRequestId
),
NextTask
AS (
	SELECT *
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
--NextPrepTask
--AS (
--	SELECT *
--	FROM (
--		SELECT t.ServiceRequestId
--			, t.Id
--			, t.TaskName
--			, t.TaskPhaseId
--			, t.TaskPhaseName
--			, t.ResponsibleRoleId
--			, t.ResponsibleRoleName
--			, t.AssignedTo
--			, AssignedToName = dbo.GetDisplayName(u.FirstName, u.LastName, u.Title)
--			, RowNum = ROW_NUMBER() OVER(PARTITION BY t.ServiceRequestId ORDER BY t.[Sequence])
--		FROM dbo.ServiceRequestTask t
--		LEFT JOIN dbo.AspNetUsers u ON t.AssignedTo = u.Id
--		WHERE t.CompletedDate IS NULL AND t.TaskPhaseId = 33
--	) t
--	WHERE RowNum = 1
--),
--NextServiceTask
--AS (
--	SELECT *
--	FROM (
--		SELECT t.ServiceRequestId
--			, t.Id
--			, t.TaskName
--			, t.TaskPhaseId
--			, t.TaskPhaseName
--			, t.ResponsibleRoleId
--			, t.ResponsibleRoleName
--			, t.AssignedTo
--			, AssignedToName = dbo.GetDisplayName(u.FirstName, u.LastName, u.Title)
--			, RowNum = ROW_NUMBER() OVER(PARTITION BY t.ServiceRequestId ORDER BY t.[Sequence])
--		FROM dbo.ServiceRequestTask t
--		LEFT JOIN dbo.AspNetUsers u ON t.AssignedTo = u.Id
--		WHERE t.CompletedDate IS NULL AND t.TaskPhaseId = 34
--	) t
--	WHERE RowNum = 1
--),
--NextCloseoutTask
--AS (
--	SELECT *
--	FROM (
--		SELECT t.ServiceRequestId
--			, t.Id
--			, t.TaskName
--			, t.TaskPhaseId
--			, t.TaskPhaseName
--			, t.ResponsibleRoleId
--			, t.ResponsibleRoleName
--			, t.AssignedTo
--			, AssignedToName = dbo.GetDisplayName(u.FirstName, u.LastName, u.Title)
--			, RowNum = ROW_NUMBER() OVER(PARTITION BY t.ServiceRequestId ORDER BY t.[Sequence])
--		FROM dbo.ServiceRequestTask t
--		LEFT JOIN dbo.AspNetUsers u ON t.AssignedTo = u.Id
--		WHERE t.CompletedDate IS NULL AND t.TaskPhaseId = 35
--	) t
--	WHERE RowNum = 1
--)
SELECT
	 sr.[Id] 
	,sr.[ObjectGuid]
	,sr.[CompanyReferenceId]
	,sr.[ClaimantName]
	,sr.[ServiceCatalogueId]
	,sr.[HarvestProjectId]
	,[Title] = dbo.FormatDateTime(ad.[Day], 'yy-MM-dd') + ' ' + LEFT(REPLACE(CONVERT(nvarchar(10), sl.StartTime), ':', ''),4) + ' - ' + a.LocationShortName + ' ' + sr.ClaimantName + ' - ' + s.Code + ' ' + p.UserName + ' ' + ISNULL(c.Code, '') + ' - ' + CONVERT(nvarchar(10), sr.Id)
	,sr.[Body]
	,sr.[AddressId]
	,sr.[RequestedDate]
	,sr.[RequestedBy]
	,sr.[CancelledDate]
	,ServiceRequestStatusId = dbo.GetServiceRequestStatusId(ts.OpenTasks)
	,ServiceRequestStatusText = lisr.[Text]
	,ServiceStatusId = dbo.GetServiceStatusId(sr.IsLateCancellation, sr.CancelledDate, sr.IsNoShow, spt.OpenTasks)
	,ServiceStatusText = lis.[Text]
	,sr.[AvailableSlotId]
	,sr.[DueDate]
	,sr.CaseCoordinatorId
	,sr.IntakeAssistantId
	,sr.DocumentReviewerId
	,ServiceRequestPrice = sr.[Price]
	,sr.Notes
	,sr.DocumentFolderLink
	,CompanyId = ISNULL(sr.CompanyId, sc.CompanyId)
	,sr.IsNoShow
	,sr.IsLateCancellation
	,sr.NoShowRate
	,sr.LateCancellationRate
	,sr.[ModifiedDate]
	,sr.[ModifiedUser]
	,sc.ServiceId
	,ServiceName = s.Name
	,ServiceCode = s.Code
	,s.ServiceCategoryName
	,s.ServicePortfolioName
	,sc.PhysicianId
	,PhysicianDisplayName = p.DisplayName
	,PhysicianUserName = p.UserName
	,CompanyGuid = c.ObjectGuid
	,CompanyName = c.Name
	,ParentCompanyName = c.ParentName
	,ServicePrice = s.DefaultPrice
	,ServiceCataloguePrice = sc.Price
	,EffectivePrice = COALESCE(sr.Price, sc.Price, s.DefaultPrice)
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
	,LocationName = a.LocationName
	,CaseCoordinatorName = dbo.GetDisplayName(cc.FirstName, cc.LastName, cc.Title)
	,IntakeAssistantName = dbo.GetDisplayName(ia.FirstName, ia.LastName, ia.Title)
	,DocumentReviewerName = dbo.GetDisplayName(dr.FirstName, dr.LastName, dr.Title)
	,AppointmentDate = ad.[Day]
	,sl.StartTime
	,sl.Duration
	,sl.EndTime
	,CalendarEventTitle = '(' + sr.ClaimantName + ') ' + s.Code + '-' + c.Name + '-' + CONVERT(nvarchar(10), sr.Id)
	,ts.TotalTasks
	,ts.ClosedTasks
	,ts.OpenTasks
	,NextTaskId = nt.Id
	,NextTaskName = nt.TaskName
	,NextTaskAssignedTo = nt.AssignedTo
	,NextTaskAssignedtoName = nt.AssignedToName
	--,NextPrepTaskId = npt.Id
	--,NextPrepTaskName = npt.TaskName
	--,NextPrepTaskAssignedTo = npt.AssignedTo
	--,NextPrepTaskAssignedtoName = npt.AssignedToName
	--,NextServiceTaskId = nst.Id
	--,NextServiceTaskName = nst.TaskName
	--,NextServiceTaskAssignedTo = nst.AssignedTo
	--,NextServiceTaskAssignedtoName = nst.AssignedToName
	--,NextCloseoutTaskId = nct.Id
	--,NextCloseoutTaskName = nct.TaskName
	--,NextCloseoutTaskAssignedTo = nct.AssignedTo
	--,NextCloseoutTaskAssignedtoName = nct.AssignedToName
FROM dbo.ServiceRequest sr
LEFT JOIN dbo.ServiceCatalogue sc ON sc.Id = sr.ServiceCatalogueId
INNER JOIN API.[Service] s ON s.Id = sc.ServiceId
INNER JOIN API.Physician p ON sc.PhysicianId = p.Id
LEFT JOIN API.Company c ON ISNULL(sr.CompanyId, sc.CompanyId) = c.Id
LEFT JOIN dbo.[AspNetUsers] cc ON cc.Id = sr.CaseCoordinatorId
LEFT JOIN dbo.[AspNetUsers] ia ON ia.Id = sr.IntakeAssistantId
LEFT JOIN dbo.[AspNetUsers] dr ON dr.Id = sr.DocumentReviewerId
LEFT JOIN dbo.[AspNetUsers] rb ON rb.Id = sr.RequestedBy
LEFT JOIN API.[Location] a ON sr.AddressId = a.Id
LEFT JOIN dbo.AvailableSlot sl ON sr.AvailableSlotId = sl.Id
LEFT JOIN dbo.AvailableDay ad ON sl.AvailableDayId = ad.Id
LEFT JOIN Tasks ts ON sr.Id = ts.ServiceRequestId
LEFT JOIN ServicePhaseTasks spt ON sr.Id = spt.ServiceRequestId
LEFT JOIN NextTask nt ON sr.Id = nt.ServiceRequestId
LEFT JOIN dbo.LookupItem lisr ON dbo.GetServiceRequestStatusId(ts.OpenTasks) = lisr.Id
LEFT JOIN dbo.LookupItem lis ON dbo.GetServiceStatusId(sr.IsLateCancellation, sr.CancelledDate, sr.IsNoShow, spt.OpenTasks) = lis.Id
--LEFT JOIN NextPrepTask npt ON sr.Id = npt.ServiceRequestId
--LEFT JOIN NextServiceTask nst ON sr.Id = nst.ServiceRequestId
--LEFT JOIN NextCloseoutTask nct ON sr.Id = nct.ServiceRequestId