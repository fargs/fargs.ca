
CREATE PROCEDURE [API].[ServiceRequestTask_Update]
	 @Id int
	,@ServiceRequestId int
	,@TaskId smallint
	,@TaskName nvarchar(128)
	,@ResponsibleRoleId nvarchar(128)
	,@ResponsibleRoleName nvarchar(128)
	,@Sequence smallint
	,@AssignedTo nvarchar(128)
	,@IsBillable bit
	,@HourlyRate decimal(18,2)
	,@EstimatedHours decimal(18,2)
	,@ActualHours decimal(18,2)
	,@CompletedDate datetime
	,@Notes nvarchar(2000)
	,@InvoiceItemId smallint
	,@ModifiedUser nvarchar(100)
AS

DECLARE @Now DATETIME
SET @Now = GETDATE()

UPDATE dbo.[ServiceRequestTask]
SET
	 [ServiceRequestId] = @ServiceRequestId
	,[TaskId] = @TaskId
	,[TaskName] = @TaskName
	,[ResponsibleRoleId] = @ResponsibleRoleId
	,[ResponsibleRoleName] = @ResponsibleRoleName
	,[Sequence] = @Sequence
	,[AssignedTo] = @AssignedTo
	,[IsBillable] = @IsBillable
	,[HourlyRate] = @HourlyRate
	,[EstimatedHours] = @EstimatedHours
	,[ActualHours] = @ActualHours
	,[CompletedDate] = @CompletedDate
	,[Notes] = @Notes
	,[InvoiceItemId] = @InvoiceItemId
	,[ModifiedDate] = @Now
	,[ModifiedUser] = @ModifiedUser
WHERE 
	Id = @Id