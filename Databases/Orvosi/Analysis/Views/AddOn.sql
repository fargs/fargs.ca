CREATE VIEW [Analysis].[AddOn]
AS
SELECT
	 sr.Id
	,sr.ClaimantName
	,sr.DueDate
	,[Service] = s.Name
	,[Company] = c.Name
	,sr.CancelledDate
	,IsBillable = CASE WHEN sr.CancelledDate IS NULL THEN 1 ELSE 0 END
FROM [dbo].[ServiceRequest] sr 
LEFT JOIN [dbo].[ServiceRequestStatus] st ON sr.ServiceRequestStatusId = st.Id
LEFT JOIN [dbo].[Service] s ON sr.ServiceId = s.Id
LEFT JOIN [dbo].[Company] c ON sr.CompanyId = c.Id
LEFT JOIN [dbo].[Address] aa ON sr.AddressId = aa.Id
LEFT JOIN [dbo].[City] ci ON aa.CityId = ci.Id
LEFT JOIN [dbo].[Province] pr ON ci.ProvinceId = pr.Id
WHERE sr.AppointmentDate IS NULL