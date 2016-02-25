
CREATE PROCEDURE [API].[ServiceCatalogue_Insert]
	 @PhysicianId nvarchar(128)
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

INSERT INTO dbo.[ServiceCatalogue]
(
	 [PhysicianId]
	,[ServiceId]
	,[CompanyId]
	,[LocationId]
	,[Price]
	,[ModifiedDate]
	,[ModifiedUser]
	,[NoShowRate]
	,[LateCancellationRate]
)
VALUES 
(
	 @PhysicianId
	,@ServiceId
	,@CompanyId
	,@LocationId
	,@Price
	,@Now
	,@ModifiedUser
	,@NoShowRate
	,@LateCancellationRate
)

DECLARE @Id INT
SELECT Id = SCOPE_IDENTITY()