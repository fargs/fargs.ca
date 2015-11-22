CREATE PROC [API].[Job_Insert]
	@ExternalProjectId nvarchar(128)
	,@Name nvarchar(256)
	,@Code nvarchar(256)
	,@PhysicianId int
	,@CompanyId smallint
	,@ServiceId int
	,@DueDate date
	,@StartTime time
	,@EndTime time
	,@Price decimal
	,@FileId smallint
	,@ModifiedUser nvarchar(100)
AS
	
	INSERT INTO dbo.Job (
		[ExternalProjectId]
		,[Name]
		,[Code]
		,[PhysicianId]
		,[CompanyId]
		,[ServiceId]
		,[DueDate]
		,[StartTime]
		,[EndTime]
		,[Price]
		,[FileId]
		,[ModifiedUser]
	) VALUES (
		@ExternalProjectId
		,@Name
		,@Code
		,@PhysicianId
		,@CompanyId
		,@ServiceId
		,@DueDate
		,@StartTime
		,@EndTime
		,@Price
		,@FileId
		,@ModifiedUser
	)