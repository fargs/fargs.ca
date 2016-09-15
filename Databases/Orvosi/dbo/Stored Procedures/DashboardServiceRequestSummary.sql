CREATE PROC [dbo].[DashboardServiceRequestSummary]
	@AssignedTo uniqueidentifier,
	@PhysicianId uniqueidentifier,
	@DateRangeStart datetime,
	@DateRangeEnd datetime
AS

IF @PhysicianId IS NULL
	SET @PhysicianId = @AssignedTo

IF @DateRangeStart IS NULL
	SET @DateRangeStart = '1900-01-01'

IF @DateRangeEnd IS NULL
	SET @DateRangeEnd = '2100-01-01';

WITH Requests
AS (
	SELECT t.ServiceRequestId
		, AppointmentDate
		, ServiceCategoryId
		, ServiceRequestStatusId = t.TaskStatusId
	FROM (	
		SELECT ServiceRequestId = sr.Id
			, TaskStatusId
			, AppointmentDate = ISNULL(sr.AppointmentDate, sr.DueDate)
			, s.ServiceCategoryId
			, RowNum = ROW_NUMBER() OVER(PARTITION BY sr.Id ORDER BY t.TaskStatusId)
		FROM [Private].TaskStatus t
		LEFT JOIN dbo.ServiceRequestTask srt ON srt.Id = t.Id
		LEFT JOIN dbo.ServiceRequest sr ON srt.ServiceRequestId = sr.Id
		LEFT JOIN dbo.[Service] s ON s.Id = sr.ServiceId
		WHERE t.RowNum = 1
			AND srt.AssignedTo = @AssignedTo
			AND sr.PhysicianId = @PhysicianId
			AND sr.AppointmentDate >= @DateRangeStart
			AND sr.AppointmentDate <= @DateRangeEnd
	) t
	WHERE t.RowNum = 1
)
SELECT r.AppointmentDate
	, r.ServiceRequestStatusId
	, r.ServiceCategoryId
	, IsClosed = CONVERT(BIT, CASE WHEN o.ServiceRequestId IS NULL THEN 1 ELSE 0 END)
	, DayCount = COUNT(r.ServiceRequestId)
FROM Requests r
LEFT JOIN [Private].OpenServiceRequestIds o ON r.ServiceRequestId = o.ServiceRequestId
GROUP BY r.AppointmentDate, r.ServiceRequestStatusId, r.ServiceCategoryId, o.ServiceRequestId