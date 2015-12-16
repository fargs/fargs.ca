



CREATE VIEW [API].[PhysicianLocation]
AS
SELECT 
	 t.[Id]
	,t.[PhysicianId]
	,t.[LocationId]
	,t.[IsPrimary]
	,t.[Preference]
	,t.[ModifiedDate]
	,t.[ModifiedUser]
FROM [dbo].[PhysicianLocation] t