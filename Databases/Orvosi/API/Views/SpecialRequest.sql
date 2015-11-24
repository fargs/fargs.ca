

CREATE VIEW [API].[SpecialRequest]
AS
SELECT 
	 s.[Id]
	,s.[ObjectGuid]
	,s.[PhysicianId]
	,s.[ServiceId]
	,s.[Timeframe]
	,s.[AdditionalNotes]
	,s.[ModifiedDate]
	,s.[ModifiedUser]
	,PhysicianDisplayName = dbo.GetDisplayName(p.FirstName, p.LastName, p.Title)
	,ServiceName = sv.Name
FROM dbo.[SpecialRequest] s
LEFT JOIN dbo.AspNetUsers p ON s.PhysicianId = p.Id
LEFT JOIN dbo.[Service] sv ON s.ServiceId = sv.Id