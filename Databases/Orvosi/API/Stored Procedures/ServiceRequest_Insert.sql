
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
	,@Notes nvarchar(2000)
	,@DocumentFolderLink nvarchar(2000)
	,@CompanyId smallint
	,@IsNoShow bit
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
	,[Notes]
	,[DocumentFolderLink]
	,[CompanyId]
	,[IsNoShow]
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
	,@Notes
	,@DocumentFolderLink
	,@CompanyId
	,@IsNoShow
	,@ModifiedUser
)

DECLARE @Id INT
SELECT @Id = SCOPE_IDENTITY();

WITH Assignments 
AS (
	SELECT UserId = @CaseCoordinatorId, RoleId = '9eab89c0-225c-4027-9f42-cc35e5656b14'
	UNION 
	SELECT @IntakeAssistantId, RoleId = '9dd582a0-cf86-4fc0-8894-477266068c12'
	UNION 
	SELECT @DocumentReviewerId, RoleId = '22B5C8AC-2C96-4A74-8057-976914031A7E'
	UNION
	SELECT PhysicianId, RoleId = '8359141f-e423-4e48-8925-4624ba86245a' FROM dbo.ServiceCatalogue WHERE Id = @ServiceCatalogueId
)
INSERT INTO dbo.ServiceRequestTask (
	 [ServiceRequestId]
	,[TaskId]
	,[TaskName]
	,[ResponsibleRoleId]
	,[ResponsibleRoleName]
	,[Sequence]
	,[IsBillable]
	,[AssignedTo]
	,[HourlyRate]
	,[EstimatedHours]
	,[ModifiedDate]
	,[ModifiedUser]
)
SELECT @Id
	, st.TaskId
	, t.Name
	, t.ResponsibleRoleId
	, r.Name
	, t.[Sequence]
	, t.IsBillable
	, a.UserId
	, st.HourlyRate
	, st.EstimatedHours
	, @Now
	, @ModifiedUser
FROM dbo.[ServiceTask] st
INNER JOIN dbo.Task t ON st.TaskId = t.Id
LEFT JOIN dbo.AspNetRoles r ON t.ResponsibleRoleId = r.Id
LEFT JOIN Assignments a ON t.ResponsibleRoleId = a.RoleId
WHERE st.ServiceId = (
	SELECT ServiceId FROM dbo.ServiceCatalogue WHERE Id = @ServiceCatalogueId
)

SELECT * FROM API.ServiceRequest WHERE Id = @Id