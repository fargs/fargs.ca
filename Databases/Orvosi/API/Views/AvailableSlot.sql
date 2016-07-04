
CREATE VIEW [API].[AvailableSlot]
AS
SELECT 
	 a.Id
	,a.AvailableDayId
	,a.StartTime
	,a.EndTime
	,a.Duration
	,a.ModifiedDate
	,a.ModifiedUser
	,ServiceRequestId = sr.Id
	,Title = CASE WHEN sr.Id IS NULL THEN 'Available' ELSE ISNULL(sr.ServiceName, '[Service Not Set]') + ' - ' + ISNULL(sr.CompanyName, '[Company Not Set]') + ' - ' + ISNULL(sr.AddressName, '[Location Not Set]') END
FROM [dbo].[AvailableSlot] a
LEFT JOIN [API].[ServiceRequest] sr ON sr.AvailableSlotId = a.Id