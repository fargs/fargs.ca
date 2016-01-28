﻿
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
	,@AvailableSlotId smallint
	,@AppointmentDate date
	,@StartTime time
	,@EndTime time
	,@DueDate date
	,@Price decimal(18,2)
	,@Notes nvarchar(2000)
	,@DocumentFolderLink nvarchar(2000)
	,@CompanyId smallint
	,@IsLateCancellation bit
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
	,[IsLateCancellation]
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
	,@IsLateCancellation
	,@IsNoShow
	,@ModifiedUser
)

DECLARE @Id INT
SELECT @Id = SCOPE_IDENTITY();

WITH Assignments 
AS (
	SELECT UserId = @CaseCoordinatorId, RoleId = dbo.RoleId_CaseCoordinator()
	UNION 
	SELECT @IntakeAssistantId, RoleId = dbo.RoleId_IntakeAssistant()
	UNION 
	SELECT @DocumentReviewerId, RoleId = dbo.RoleId_DocumentReviewer()
	UNION
	SELECT PhysicianId, RoleId = dbo.RoleId_Physician() FROM dbo.ServiceCatalogue WHERE Id = @ServiceCatalogueId
)
INSERT INTO dbo.ServiceRequestTask (
	 [ServiceRequestId]
	,[TaskId]
	,[TaskName]
	,[TaskPhaseId]
	,[TaskPhaseName]
	,[Guidance]
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
	, st.Id
	, st.TaskName
	, st.TaskPhaseId
	, st.TaskPhaseName
	, st.Guidance
	, st.ResponsibleRoleId
	, st.ResponsibleRoleName
	, st.[Sequence]
	, st.IsBillable
	, a.UserId
	, st.HourlyRate
	, st.EstimatedHours
	, @Now
	, @ModifiedUser
FROM API.[Task] st
LEFT JOIN Assignments a ON st.ResponsibleRoleId = a.RoleId
WHERE st.ServiceCategoryId = (
	SELECT ServiceCategoryId FROM dbo.ServiceCatalogue WHERE Id = @ServiceCatalogueId
)

SELECT * FROM API.ServiceRequest WHERE Id = @Id