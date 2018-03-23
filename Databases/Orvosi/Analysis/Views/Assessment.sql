CREATE VIEW [Analysis].[Assessment]
AS

SELECT
	 sr.Id
	,sr.ClaimantName
	,sr.AppointmentDate
	,sr.StartTime
	,sr.DueDate
	,sr.PhysicianId
	,sr.AvailableSlotId
	,[City] = ci.Name
	,[Province] = pr.ProvinceName
	,[Service] = s.Name
	,[Company] = c.Name
	,sr.IsNoShow
	,sr.CancelledDate
	,sr.IsLateCancellation
	,IsBillable = CASE WHEN sr.IsLateCancellation = 1 OR sr.CancelledDate IS NULL OR sr.IsNoShow = 1 THEN 1 ELSE 0 END
FROM [dbo].[ServiceRequest] sr 
LEFT JOIN [dbo].[ServiceRequestStatus] st ON sr.ServiceRequestStatusId = st.Id
LEFT JOIN [dbo].[Service] s ON sr.ServiceId = s.Id
LEFT JOIN [dbo].[Company] c ON sr.CompanyId = c.Id
LEFT JOIN [dbo].[Address] aa ON sr.AddressId = aa.Id
LEFT JOIN [dbo].[City] ci ON aa.CityId = ci.Id
LEFT JOIN [dbo].[Province] pr ON ci.ProvinceId = pr.Id
WHERE sr.AppointmentDate IS NOT NULL AND sr.AddressId IS NOT NULL