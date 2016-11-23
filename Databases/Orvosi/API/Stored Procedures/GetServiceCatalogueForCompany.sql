

CREATE PROC [API].[GetServiceCatalogueForCompany]
	@PhysicianId uniqueidentifier,
	@CompanyId SMALLINT
AS

WITH ServiceLocations
AS (
	SELECT 
		 ServiceId = s.Id
		,ServiceName = s.Name
		,ServiceCategoryId = s.ServiceCategoryId
		,LocationId = l.ItemId
		,LocationName = l.ItemText
		,ServicePrice = s.Price
	FROM dbo.[Service] s
	CROSS JOIN (SELECT * FROM API.LocationArea UNION ALL SELECT 4, 'Locations', 0, NULL, NULL) l
	WHERE s.ServicePortfolioId = 2 /* Physician */ 
)
, ServiceCatalogue
AS (
	SELECT ServiceCatalogueId = s.Id
		,s.ServiceId
		,s.LocationId
		,s.Price
		,Precedence = 1
	FROM dbo.[ServiceCatalogue] s
	WHERE CompanyId IS NULL AND s.PhysicianId = @PhysicianId
	UNION ALL
	SELECT ServiceCatalogueId = s.Id
		,s.ServiceId
		,s.LocationId
		,s.Price
		,Precedence = 2
	FROM dbo.[ServiceCatalogue] s
	WHERE s.CompanyId = (SELECT ParentId FROM dbo.Company WHERE Id = @CompanyId) 
		AND s.PhysicianId = @PhysicianId
	UNION ALL
	SELECT ServiceCatalogueId = s.Id
		,s.ServiceId
		,s.LocationId
		,s.Price
		,Precedence = 3
	FROM dbo.[ServiceCatalogue] s
	WHERE s.CompanyId = @CompanyId 
		AND s.PhysicianId = @PhysicianId
)
, ServiceCatalogueOrdered
AS (
	SELECT * 
		,RowNum = ROW_NUMBER() OVER(PARTITION BY s.ServiceId, s.LocationId ORDER BY Precedence DESC)
	FROM ServiceCatalogue s
)
, ServiceCatalogueForCompany
AS (
	SELECT *
	FROM ServiceCatalogueOrdered
	WHERE RowNum = 1
)
, ServiceCatalogueMatrix
AS (
	SELECT 
		 sl.*
		,ServiceCatalogueId = sc.ServiceCatalogueId
		,sc.Price
	FROM ServiceLocations sl
	LEFT JOIN ServiceCatalogueForCompany sc ON sl.ServiceId = sc.ServiceId AND (sc.LocationId IS NULL OR sc.LocationId = sl.LocationId)
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