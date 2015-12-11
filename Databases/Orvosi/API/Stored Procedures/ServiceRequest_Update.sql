
CREATE PROCEDURE [API].[ServiceRequest_Update]
	 @Id int
	,@ObjectGuid uniqueidentifier
	,@CompanyReferenceId nvarchar(128)
	,@ServiceCatalogueId smallint
	,@HarvestProjectId bigint
	,@Title nvarchar(256)
	,@Body nvarchar(max)
	,@RequestedDate datetime
	,@RequestedBy uniqueidentifier
	,@CancelledDate datetime
	,@AssignedTo uniqueidentifier
	,@StatusId tinyint
	,@DueDate date
	,@StartTime time
	,@EndTime time
	,@Price decimal(18,2)
	,@ModifiedUser nvarchar(100)
AS

DECLARE @Now DATETIME
SET @Now = GETDATE()

UPDATE dbo.[ServiceRequest]
SET
	 [ObjectGuid] = @ObjectGuid
	,[CompanyReferenceId] = @CompanyReferenceId
	,[ServiceCatalogueId] = @ServiceCatalogueId
	,[HarvestProjectId] = @HarvestProjectId
	,[Title] = @Title
	,[Body] = @Body
	,[RequestedDate] = @RequestedDate
	,[RequestedBy] = @RequestedBy
	,[CancelledDate] = @CancelledDate
	,[AssignedTo] = @AssignedTo
	,[StatusId] = @StatusId
	,[DueDate] = @DueDate
	,[StartTime] = @StartTime
	,[EndTime] = @EndTime
	,[Price] = @Price
	,[ModifiedDate] = @Now
	,[ModifiedUser] = @ModifiedUser
WHERE 
	Id = @Id