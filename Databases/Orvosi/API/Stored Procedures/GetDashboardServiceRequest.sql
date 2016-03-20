CREATE PROC API.GetDashboardServiceRequest
	@ServiceProviderId uniqueidentifier,
	@Now DATETIME
AS

DECLARE @weekday INT
SELECT @weekday = DATEPART(WEEKDAY, @Now);
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
		, ServiceName
		, ClaimantName
	FROM API.ServiceRequest sr, DateRanges dr
	WHERE AppointmentDate BETWEEN dr.WeekStart AND dr.WeekEnd
		AND @ServiceProviderId IN (sr.PhysicianId, sr.CaseCoordinatorId, sr.DocumentReviewerId, sr.IntakeAssistantId)
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
		, ServiceName
		, ClaimantName
	FROM API.ServiceRequest sr, DateRanges dr
	WHERE AppointmentDate BETWEEN dr.NextWeekStart AND dr.NextWeekEnd		
		AND @ServiceProviderId IN (sr.PhysicianId, sr.CaseCoordinatorId, sr.DocumentReviewerId, sr.IntakeAssistantId)
)
SELECT * FROM ServiceRequests