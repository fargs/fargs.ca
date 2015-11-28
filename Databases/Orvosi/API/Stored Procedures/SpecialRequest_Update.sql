
CREATE PROC [API].[SpecialRequest_Update]
	 @Id smallint
	,@PhysicianId nvarchar(128)
	,@ServiceId int
	,@Timeframe nvarchar(128)
	,@AdditionalNotes nvarchar(2000)
	,@ModifiedDate datetime
	,@ModifiedUserName nvarchar(128)
	,@ModifiedUserId nvarchar(128)
AS
	
	UPDATE dbo.SpecialRequest 
	SET  [PhysicianId] = @PhysicianId
		,[ServiceId] = @ServiceId
		,[Timeframe] = @Timeframe
		,[AdditionalNotes] = @AdditionalNotes
		,[ModifiedDate] = @ModifiedDate
		,[ModifiedUserName] = @ModifiedUserName 
		,[ModifiedUserId] = @ModifiedUserId 
	WHERE [Id] = @Id