CREATE VIEW [Analysis].[Availability]
AS

SELECT
	 a.Id
	,ad.[Day]
	,a.StartTime
	,ad.PhysicianId
	,[State] = CASE WHEN sr.Id IS NULL THEN 'Unfilled' ELSE 'Filled' END
FROM [dbo].[AvailableSlot] a
LEFT JOIN [dbo].[AvailableDay] ad ON a.AvailableDayId = ad.Id
LEFT JOIN (
	-- Exlude deleted or
	SELECT sr.Id, sr.AvailableSlotId FROM dbo.ServiceRequest sr WHERE sr.CancelledDate IS NULL
) sr ON sr.AvailableSlotId = a.Id