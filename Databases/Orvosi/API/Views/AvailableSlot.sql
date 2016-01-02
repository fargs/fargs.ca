CREATE VIEW API.AvailableSlot
AS
SELECT 
	 a.Id
	,a.AvailableDayId
	,a.StartTime
	,a.EndTime
	,a.Duration
	,a.ServiceRequestId
	,a.ModifiedDate
	,a.ModifiedUser
	,Title = sr.ServiceName + ' - ' + sr.LocationName + ' - ' + sr.Title
FROM [dbo].[AvailableSlot] a
LEFT JOIN [API].[ServiceRequest] sr ON a.ServiceRequestId = sr.Id