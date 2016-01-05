
CREATE PROCEDURE [API].[ServiceRequest_Insert]
	 @CompanyReferenceId nvarchar(128)
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
	,@DocumentFolderLink nvarchar(2000)
	,@CompanyId smallint
	,@ModifiedUser nvarchar(100)
AS

DECLARE @Now DATETIME
SET @Now = GETDATE()

INSERT INTO dbo.[ServiceRequest]
(
	 [CompanyReferenceId]
	,[ClaimantName]
	,[ServiceCatalogueId]
	,[AddressId]
	,[HarvestProjectId]
	,[Title]
	,[Body]
	,[RequestedDate]
	,[RequestedBy]
	,[CancelledDate]
	,[CaseCoordinatorId]
	,[IntakeAssistantId]
	,[DocumentReviewerId]
	,[AvailableSlotId]
	,[AppointmentDate]
	,[StartTime]
	,[EndTime]
	,[DueDate]
	,[Price]
	,[DocumentFolderLink]
	,[CompanyId]
	,[ModifiedUser]
)
VALUES 
(
	 @CompanyReferenceId
	,@ClaimantName
	,@ServiceCatalogueId
	,@AddressId
	,@HarvestProjectId
	,@Title
	,@Body
	,@RequestedDate
	,@RequestedBy
	,@CancelledDate
	,@CaseCoordinatorId
	,@IntakeAssistantId
	,@DocumentReviewerId
	,@AvailableSlotId
	,@AppointmentDate
	,@StartTime
	,@EndTime
	,@DueDate
	,@Price
	,@DocumentFolderLink
	,@CompanyId
	,@ModifiedUser
)

DECLARE @Id INT
SELECT @Id = SCOPE_IDENTITY()

SELECT * FROM API.ServiceRequest WHERE Id = @Id