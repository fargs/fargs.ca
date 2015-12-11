
CREATE PROCEDURE [API].[ServiceRequest_Insert]
	 @CompanyReferenceId nvarchar(128)
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

INSERT INTO dbo.[ServiceRequest]
(
	 [CompanyReferenceId]
	,[ServiceCatalogueId]
	,[HarvestProjectId]
	,[Title]
	,[Body]
	,[RequestedDate]
	,[RequestedBy]
	,[CancelledDate]
	,[AssignedTo]
	,[StatusId]
	,[DueDate]
	,[StartTime]
	,[EndTime]
	,[Price]
	,[ModifiedUser]
)
VALUES 
(
	 @CompanyReferenceId
	,@ServiceCatalogueId
	,@HarvestProjectId
	,@Title
	,@Body
	,@RequestedDate
	,@RequestedBy
	,@CancelledDate
	,@AssignedTo
	,@StatusId
	,@DueDate
	,@StartTime
	,@EndTime
	,@Price
	,@ModifiedUser
)

DECLARE @Id INT
SELECT @Id = SCOPE_IDENTITY()

SELECT * FROM API.ServiceRequest WHERE Id = @Id