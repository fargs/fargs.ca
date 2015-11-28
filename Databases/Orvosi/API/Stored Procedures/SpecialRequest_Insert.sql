
CREATE PROC [API].[SpecialRequest_Insert]
	 @PhysicianId nvarchar(128)
	,@ServiceId int
	,@Timeframe nvarchar(128)
	,@AdditionalNotes nvarchar(2000)
	,@ModifiedDate datetime
	,@ModifiedUserName nvarchar(128)
	,@ModifiedUserId nvarchar(128)
AS
	
	INSERT INTO dbo.SpecialRequest (
		 [PhysicianId]
		,[ServiceId]
		,[Timeframe]
		,[AdditionalNotes]
		,[ModifiedDate]
		,[ModifiedUserName]
		,[ModifiedUserId]
	) VALUES (
		 @PhysicianId
		,@ServiceId
		,@Timeframe
		,@AdditionalNotes
		,@ModifiedDate
		,@ModifiedUserName
		,@ModifiedUserId
	)