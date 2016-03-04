
CREATE PROCEDURE [API].[ServiceRequest_Update]
	 @Id int
	,@CompanyReferenceId nvarchar(128)
	,@ClaimantName nvarchar(128)
	,@AddressId int
	,@Body nvarchar(max)
	,@RequestedDate datetime
	,@RequestedBy uniqueidentifier
	,@CancelledDate datetime
	,@CaseCoordinatorId uniqueidentifier
	,@IntakeAssistantId uniqueidentifier
	,@DocumentReviewerId uniqueidentifier
	,@AvailableSlotId smallint
	,@AppointmentDate date
	,@StartTime time
	,@EndTime time
	,@DueDate date
	,@Price decimal(18,2)
	,@Notes nvarchar(128)
	,@DocumentFolderLink nvarchar(2000)
	,@CompanyId smallint
	,@IsLateCancellation bit
	,@IsNoShow bit
	,@NoShowRate decimal(18,2)
	,@LateCancellationRate decimal(18,2)
	,@ModifiedUser nvarchar(100)
AS

DECLARE @Now DATETIME
SET @Now = GETDATE()

UPDATE dbo.[ServiceRequest]
SET
	[CompanyReferenceId] = @CompanyReferenceId
	,[ClaimantName] = @ClaimantName
	,[AddressId] = @AddressId
	,[Body] = @Body
	,[RequestedDate] = @RequestedDate
	,[RequestedBy] = @RequestedBy
	,[CancelledDate] = @CancelledDate
	,[CaseCoordinatorId] = @CaseCoordinatorId
	,[IntakeAssistantId] = @IntakeAssistantId
	,[DocumentReviewerId] = @DocumentReviewerId
	,[AvailableSlotId] = @AvailableSlotId
	,[AppointmentDate] = @AppointmentDate
	,[StartTime] = @StartTime
	,[EndTime] = @EndTime
	,[DueDate] = @DueDate
	,[Price] = @Price
	,[Notes] = @Notes
	,[DocumentFolderLink] = @DocumentFolderLink
	,[CompanyId] = @CompanyId
	,[IsLateCancellation] = @IsLateCancellation
	,[IsNoShow] = @IsNoShow
	,[NoShowRate] = @NoShowRate
	,[LateCancellationRate] = @LateCancellationRate
	,[ModifiedDate] = @Now
	,[ModifiedUser] = @ModifiedUser
WHERE 
	Id = @Id

UPDATE dbo.ServiceRequestTask
SET AssignedTo = @CaseCoordinatorId
WHERE ResponsibleRoleId = dbo.RoleId_CaseCoordinator()
	AND ServiceRequestId = @Id

UPDATE dbo.ServiceRequestTask
SET AssignedTo = @IntakeAssistantId
WHERE ResponsibleRoleId = dbo.RoleId_IntakeAssistant()
	AND ServiceRequestId = @Id

UPDATE dbo.ServiceRequestTask
SET AssignedTo = @DocumentReviewerId
WHERE ResponsibleRoleId = dbo.RoleId_DocumentReviewer()
	AND ServiceRequestId = @Id