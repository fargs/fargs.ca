CREATE PROC API.GetDashboardSchedule
	@Now DATETIME,
	@UserId NVARCHAR(128),
	@IsSuperAdmin bit
AS

DECLARE @weekday INT
SELECT @weekday = DATEPART(WEEKDAY, GETDATE());
WITH DateRanges
AS (
	SELECT 
		w.Today
		, w.WeekStart
		, w.WeekEnd
		, NextWeekStart = DATEADD(d, 1, w.WeekEnd)
		, NextWeekEnd = DATEADD(d, 7, w.WeekEnd)
	FROM (
		SELECT Today = @now
			, WeekStart = DATEADD(d, (@weekday - 1) * -1, @now)
			, WeekEnd = DATEADD(d, 7 - @weekday, @now)
	) w
)
, ServiceRequests
AS (
	SELECT 1 AS WeekNumber
		, AppointmentDate
		, CompanyName = ISNULL(sr.ParentCompanyName, sr.CompanyName)
		, Id
		, AddressName
		, City
		, StartTime
		, EndTime
		, TimelineId = CASE WHEN AppointmentDate = dr.Today THEN 38 WHEN AppointmentDate < dr.Today THEN 37 ELSE 39 END
		, PhysicianId
		, CaseCoordinatorId
		, DocumentReviewerId
		, IntakeAssistantId
	FROM API.ServiceRequest sr, DateRanges dr
	WHERE AppointmentDate BETWEEN dr.WeekStart AND dr.WeekEnd
	UNION 
	SELECT 2 as WeekNumber
		, AppointmentDate
		, CompanyName = ISNULL(sr.ParentCompanyName, sr.CompanyName)
		, Id
		, AddressName
		, City
		, StartTime
		, EndTime
		, TimelineId = 39 -- Always Future
		, PhysicianId
		, CaseCoordinatorId
		, DocumentReviewerId
		, IntakeAssistantId
	FROM API.ServiceRequest sr, DateRanges dr
	WHERE AppointmentDate BETWEEN dr.NextWeekStart AND dr.NextWeekEnd		
)
SELECT WeekNumber
	, PreviousWeekNumber = LAG(WeekNumber, 1, 0) OVER(ORDER BY AppointmentDate)
	, AppointmentDate
	, TimelineId
	, RequestCount = COUNT(sr.Id)
	, AddressName = MIN(sr.AddressName)
	, City = MIN(sr.City)
	, CompanyName = MIN(sr.CompanyName)
	, StartTime = MIN(sr.StartTime)
	, EndTime = MAX(sr.EndTime)
FROM ServiceRequests sr
WHERE @IsSuperAdmin = 1 
	 OR (sr.PhysicianId = @UserId OR sr.IntakeAssistantId = @UserId OR sr.DocumentReviewerId = @UserId OR sr.CaseCoordinatorId = @UserId)
GROUP BY WeekNumber, AppointmentDate, TimelineId