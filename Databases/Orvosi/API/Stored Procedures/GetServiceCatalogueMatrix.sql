

CREATE PROC [API].[GetServiceCatalogueMatrix]
	@PhysicianId NVARCHAR(128),
	@CompanyId SMALLINT
AS

SELECT 
	 ServiceId = s.Id
	,ServiceName = s.Name
	,ServicePrice = s.Price
	,LocationId = l.ItemId
	,LocationName = l.ItemText
INTO #ServiceLocations
FROM dbo.[Service] s
CROSS JOIN API.LocationArea l 

IF @CompanyId IS NULL BEGIN
	SELECT 
		 sl.*
		,sc.Price
		,ServiceCatalogueId = sc.Id
	FROM #ServiceLocations sl
	LEFT JOIN dbo.ServiceCatalogue sc ON sl.ServiceId = sc.ServiceId AND sl.LocationId = sc.LocationId
	WHERE sc.PhysicianId = @PhysicianId AND sc.CompanyId IS NULL
END
ELSE BEGIN
WITH ServiceCatalogue
AS (
	SELECT 
		 ServiceId = s.Id
		,ServiceName = s.Name
		,LocationId = l.ItemId
		,LocationName = l.ItemText
		,sc.Price
		,[Rank] = 2
		,sc.CompanyId
		,ServiceCatalogueId = sc.Id
	FROM dbo.ServiceCatalogue sc
	INNER JOIN dbo.[Service] s ON sc.ServiceId = s.Id
	LEFT JOIN API.LocationArea l ON sc.LocationId = l.ItemId
	WHERE sc.CompanyId = @CompanyId AND sc.PhysicianId = @PhysicianId
)
, ServiceList
AS
(
	SELECT 
		 ServiceID = s.Id
		,ServiceName = s.Name
		,LocationId = l.ItemId
		,LocationName = l.ItemText
		,PriceFromService = s.Price
		,ServiceCatalogueId = sc.ServiceCatalogueId
		,PriceFromServiceCatalogue = sc.Price
		,EffectivePrice = COALESCE(sc.Price, s.Price)
	FROM dbo.[Service] s
	CROSS JOIN API.LocationArea l
	LEFT JOIN ServiceCatalogue sc ON s.Id = sc.ServiceId AND (sc.LocationId IS NULL OR sc.LocationId = l.ItemId)
	WHERE s.ServicePortfolioId = 2 /* Physician */
)
SELECT 
	 ServiceId 
	,ServiceName
	,LocationId
	,LocationName
	,ServiceCatalogueData = (
		SELECT 
			 t2.ServiceCatalogueId
			,t2.ServiceId
			,t2.LocationId
			,t2.PriceFromService
			,t2.PriceFromServiceCatalogue
			,t2.EffectivePrice
		FROM ServiceList t2
		WHERE t1.ServiceId = t2.ServiceId AND (t1.LocationId IS NULL OR t1.LocationId = t2.LocationId)
		FOR XML PATH ('ServiceCatalogue')
	)
FROM ServiceList t1

END