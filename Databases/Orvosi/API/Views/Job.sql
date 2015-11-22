
CREATE VIEW API.Job
AS
WITH ServiceCatalogue 
AS (
	SELECT PhysicianId
		, ServiceId = s.Id
		, ServiceName = s.Name
		, DefaultPrice = s.Price
		, CompanyId = sc.CompanyId
		, CompanyOverridePrice = sc.Price
	FROM dbo.[Service] s 
	LEFT JOIN dbo.ServiceCatalogue sc ON sc.ServiceId = s.Id
)
SELECT j.[Id]
      ,j.[ExternalProjectId]
      ,j.[Name]
      ,j.[Code]
      ,j.[PhysicianId]
      ,j.[CompanyId]
      ,j.[ServiceId]
      ,j.[DueDate]
      ,j.[StartTime]
      ,j.[EndTime]
      ,j.[FileId]
      ,j.[ModifiedDate]
      ,j.[ModifiedUser]
	  ,CompanyName = c.[Name]
	  ,PhysicianFullName = dbo.GetDisplayName(p.Title, p.FirstName, p.LastName)
	  ,ServiceName = sc.ServiceName
	  ,Price = dbo.GetPrice(sc.DefaultPrice, sc.CompanyOverridePrice, j.[Price]) --> Physician Service --> Physican Service Company --> Job
  FROM [dbo].[Job] j
  LEFT JOIN dbo.Company c ON j.CompanyId = c.Id
  LEFT JOIN dbo.Person p ON j.PhysicianId = p.Id
  LEFT JOIN ServiceCatalogue sc ON sc.PhysicianId = j.PhysicianId AND sc.CompanyId = j.CompanyId AND sc.ServiceId = j.ServiceId