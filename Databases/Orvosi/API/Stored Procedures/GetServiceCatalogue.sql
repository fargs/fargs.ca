
CREATE PROC [API].[GetServiceCatalogue]
	@PhysicianId uniqueidentifier
AS

WITH ServiceLocations
AS (
	SELECT 
		 ServiceId = s.Id
		,ServiceName = s.Name
		,ServiceCategoryId = s.ServiceCategoryId
		,LocationId = l.Id
		,LocationName = l.Name
		,ServicePrice = s.Price
	FROM dbo.[Service] s
	CROSS JOIN (SELECT Id, [Name] FROM dbo.City UNION ALL SELECT 0, 'Cities') l
	WHERE s.ServicePortfolioId = 2 /* Physician */ 
)
, ServiceCatalogue
AS (
	SELECT 
		 ServiceCatalogueId = s.Id
		,s.ServiceId
		,s.LocationId
		,s.Price
	FROM dbo.[ServiceCatalogue] s
	WHERE s.PhysicianId = @PhysicianId AND s.CompanyId IS NULL
)
, ServiceCatalogueMatrix
AS (
	SELECT 
		 sl.*
		,ServiceCatalogueId = sc.ServiceCatalogueId
		,sc.Price
	FROM ServiceLocations sl
	LEFT JOIN ServiceCatalogue sc ON sl.ServiceId = sc.ServiceId AND (sc.LocationId IS NULL OR sc.LocationId = sl.LocationId)
)
SELECT sc1.*
	,ServiceCatalogueData = (
		SELECT 
			 sc2.ServiceCatalogueId
			,sc2.ServiceId
			,sc2.ServiceCategoryId
			,sc2.LocationId
			,sc2.ServicePrice
			,sc2.Price
		FROM ServiceCatalogueMatrix sc2
		WHERE sc1.ServiceId = sc2.ServiceId AND (sc1.LocationId IS NULL OR sc1.LocationId = sc2.LocationId)
		FOR XML PATH ('ServiceCatalogue')
	)
FROM ServiceCatalogueMatrix sc1