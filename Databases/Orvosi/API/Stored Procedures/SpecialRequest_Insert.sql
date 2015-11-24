
CREATE PROC [API].[SpecialRequest_Insert]
	 @PhysicianId nvarchar(128)
	,@ServiceId int
	,@Timeframe nvarchar(128)
	,@AdditionalNotes nvarchar(2000)
	,@ModifiedDate datetime
	,@ModifiedUser nvarchar(100)
AS
	
	INSERT INTO dbo.SpecialRequest (
		 [PhysicianId]
		,[ServiceId]
		,[Timeframe]
		,[AdditionalNotes]
		,[ModifiedDate]
		,[ModifiedUser]
	) VALUES (
		 @PhysicianId
		,@ServiceId
		,@Timeframe
		,@AdditionalNotes
		,@ModifiedDate
		,@ModifiedUser
	)