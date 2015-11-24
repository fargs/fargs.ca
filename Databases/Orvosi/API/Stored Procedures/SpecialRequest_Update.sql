
CREATE PROC [API].[SpecialRequest_Update]
	 @Id smallint
	,@PhysicianId nvarchar(128)
	,@ServiceId int
	,@Timeframe nvarchar(128)
	,@AdditionalNotes nvarchar(2000)
	,@ModifiedDate datetime
	,@ModifiedUser nvarchar(100)
AS
	
	UPDATE dbo.SpecialRequest 
	SET  [PhysicianId] = @PhysicianId
		,[ServiceId] = @ServiceId
		,[Timeframe] = @Timeframe
		,[AdditionalNotes] = @AdditionalNotes
		,[ModifiedDate] = @ModifiedDate
		,[ModifiedUser] = @ModifiedUser
	WHERE [Id] = @Id