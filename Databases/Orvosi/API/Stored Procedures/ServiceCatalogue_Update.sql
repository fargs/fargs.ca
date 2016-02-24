
CREATE PROCEDURE [API].[ServiceCatalogue_Update]
	 @Id int
	,@PhysicianId nvarchar(128)
	,@ServiceId smallint
	,@CompanyId smallint
	,@LocationId smallint
	,@Price decimal
	,@ModifiedUser nvarchar(100)
	,@NoShowRate decimal
	,@LateCancellationRate decimal
AS

DECLARE @Now DATETIME
SET @Now = GETDATE()

UPDATE dbo.[ServiceCatalogue] SET
 [PhysicianId] = @PhysicianId
,[ServiceId] = @ServiceId
,[CompanyId] = @CompanyId
,[LocationId] = @LocationId
,[Price] = @Price
,[ModifiedDate] = @Now
,[ModifiedUser] = @ModifiedUser
,[NoShowRate] = @NoShowRate
,[LateCancellationRate] = @LateCancellationRate
WHERE [Id] = @Id