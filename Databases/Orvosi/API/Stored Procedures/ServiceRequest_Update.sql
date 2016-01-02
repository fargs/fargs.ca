
CREATE PROCEDURE [API].[ServiceRequest_Update]
	 @Id int
	,@ObjectGuid uniqueidentifier
	,@CompanyReferenceId nvarchar(128)
	,@ClaimantName nvarchar(128)
	,@ServiceCatalogueId smallint
	,@AddressId int
	,@HarvestProjectId bigint
	,@Title nvarchar(256)
	,@Body nvarchar(max)
	,@RequestedDate datetime
	,@RequestedBy uniqueidentifier
	,@CancelledDate datetime
	,@CaseCoordinatorId uniqueidentifier
	,@IntakeAssistantId uniqueidentifier
	,@DocumentReviewerId uniqueidentifier
	,@StatusId tinyint
	,@AvailableSlotId smallint
	,@AppointmentDate date
	,@StartTime time
	,@EndTime time
	,@DueDate date
	,@Price decimal(18,2)
	,@ModifiedUser nvarchar(100)
AS

DECLARE @Now DATETIME
SET @Now = GETDATE()

UPDATE dbo.[ServiceRequest]
SET
	 [ObjectGuid] = @ObjectGuid
	,[CompanyReferenceId] = @CompanyReferenceId
	,[ClaimantName] = @ClaimantName
	,[ServiceCatalogueId] = @ServiceCatalogueId
	,[AddressId] = @AddressId
	,[HarvestProjectId] = @HarvestProjectId
	,[Title] = @Title
	,[Body] = @Body
	,[RequestedDate] = @RequestedDate
	,[RequestedBy] = @RequestedBy
	,[CancelledDate] = @CancelledDate
	,[CaseCoordinatorId] = @CaseCoordinatorId
	,[IntakeAssistantId] = @IntakeAssistantId
	,[DocumentReviewerId] = @DocumentReviewerId
	,[StatusId] = @StatusId
	,[AvailableSlotId] = @AvailableSlotId
	,[AppointmentDate] = @AppointmentDate
	,[StartTime] = @StartTime
	,[EndTime] = @EndTime
	,[DueDate] = @DueDate
	,[Price] = @Price
	,[ModifiedDate] = @Now
	,[ModifiedUser] = @ModifiedUser
WHERE 
	Id = @Id