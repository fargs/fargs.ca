


CREATE PROC [API].[AvailableSlot_Insert]
	@AvailableDayId SMALLINT
	,@StartTime TIME(7)
	,@EndTime TIME(7)
	,@Duration SMALLINT
	,@ModifiedUser NVARCHAR(128)
AS
DECLARE @Now DATETIME, @Id SMALLINT
SELECT @Now = GETDATE()

INSERT INTO dbo.AvailableSlot ([AvailableDayId], StartTime, EndTime, Duration, ModifiedUser, ModifiedDate)
VALUES (@AvailableDayId, @StartTime, @EndTime, @Duration, @ModifiedUser, @Now)

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