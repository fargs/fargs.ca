CREATE PROCEDURE [API].[GetServiceCatalogueRate]
	@ServiceProviderGuid uniqueidentifier,
	@CustomerGuid uniqueidentifier
AS

DECLARE @NoShowRate DECIMAL(18,2), @LateCancellationRate DECIMAL(18,2)
DECLARE @ParentId INT, @ParentGuid UNIQUEIDENTIFIER

SELECT @ParentId = ParentId FROM dbo.Company WHERE ObjectGuid = @CustomerGuid

SELECT @ParentGuid = ObjectGuid FROM dbo.Company WHERE Id = @ParentId

-- Get the rates of the selected company
SELECT @NoShowRate = r.NoShowRate
	, @LateCancellationRate = r.LateCancellationRate
FROM dbo.ServiceCatalogueRate r
WHERE r.ServiceProviderGuid = @ServiceProviderGuid AND r.CustomerGuid = @CustomerGuid

-- If the rates for the selected company have not been set, then use the parent company rates
IF @ParentGuid IS NOT NULL BEGIN
	SELECT @NoShowRate = ISNULL(@NoShowRate, r.NoShowRate)
		, @LateCancellationRate = ISNULL(@LateCancellationRate, r.LateCancellationRate)
	FROM dbo.ServiceCatalogueRate r
	WHERE r.ServiceProviderGuid = @ServiceProviderGuid AND r.CustomerGuid = @ParentGuid
END

-- Finally if the rates have still not been set, use the service provider defaults.
SELECT @NoShowRate = ISNULL(@NoShowRate, r.NoShowRate)
	, @LateCancellationRate = ISNULL(@LateCancellationRate, r.LateCancellationRate)
FROM dbo.ServiceCatalogueRate r
WHERE r.ServiceProviderGuid = @ServiceProviderGuid AND r.CustomerGuid IS NULL

SELECT NoShowRate = ISNULL(@NoShowRate, 0.0)
	, LateCancellationRate = ISNULL(@LateCancellationRate, 0.0)

RETURN 0

