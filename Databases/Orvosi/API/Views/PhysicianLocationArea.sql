





CREATE VIEW [API].[PhysicianLocationArea]
AS
SELECT 
	 t.[Id]
	,t.[PhysicianId]
	,t.[LocationId]
	,t.[StatusId]
	,StatusText = s.[Text]
	,t.Price
	,t.PriceIncrease
	,t.[ModifiedDate]
	,t.[ModifiedUser]
	,LocationName = l.[Text]
	,PhysicianName = p.DisplayName
FROM [dbo].[PhysicianLocation] t
LEFT JOIN [dbo].[LookupItem] l ON t.LocationId = l.Id
LEFT JOIN [dbo].[LookupItem] s ON t.StatusId = s.Id
LEFT JOIN API.Physician p ON t.PhysicianId = p.Id