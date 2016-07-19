

CREATE PROC [API].[AvailableDay_Insert]
	@AvailableDay DATE
	,@UserId uniqueidentifier
	,@CompanyId SMALLINT
	,@LocationId SMALLINT
	,@IsPrebook BIT
	,@ModifiedUser NVARCHAR(128)
AS
DECLARE @Now DATETIME, @Id SMALLINT
SELECT @Now = GETDATE()

INSERT INTO dbo.AvailableDay (PhysicianId, [Day], IsPrebook, CompanyId, LocationId, ModifiedUser, ModifiedDate)
VALUES (@UserId, @AvailableDay, @IsPrebook, @CompanyId, @LocationId, @ModifiedUser, @Now)

SELECT @Id = SCOPE_IDENTITY()

--INSERT INTO dbo.AvailableSlot (AvailableDayId, StartTime, EndTime, Duration, ModifiedUser, ModifiedDate)
--VALUES (@Id, '08:00', '09:00', 60, @User, @Now)
--INSERT INTO dbo.AvailableSlot (AvailableDayId, StartTime, EndTime, Duration, ModifiedUser, ModifiedDate)
--VALUES (@Id, '09:00', '10:00', 60, @User, @Now)
--INSERT INTO dbo.AvailableSlot (AvailableDayId, StartTime, EndTime, Duration, ModifiedUser, ModifiedDate)
--VALUES (@Id, '10:00', '11:00', 60, @User, @Now)
--INSERT INTO dbo.AvailableSlot (AvailableDayId, StartTime, EndTime, Duration, ModifiedUser, ModifiedDate)
--VALUES (@Id, '11:00', '12:00', 60, @User, @Now)
--INSERT INTO dbo.AvailableSlot (AvailableDayId, StartTime, EndTime, Duration, ModifiedUser, ModifiedDate)
--VALUES (@Id, '12:00', '13:00', 60, @User, @Now)
--INSERT INTO dbo.AvailableSlot (AvailableDayId, StartTime, EndTime, Duration, ModifiedUser, ModifiedDate)
--VALUES (@Id, '13:00', '14:00', 60, @User, @Now)

SELECT Id = @Id