﻿
CREATE PROCEDURE [API].[ServiceRequestTask_Insert]
	 @ServiceRequestId int
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

INSERT INTO dbo.[ServiceRequestTask]
(
	 [ServiceRequestId]
	,[TaskId]
	,[TaskName]
	,[ResponsibleRoleId]
	,[ResponsibleRoleName]
	,[Sequence]
	,[AssignedTo]
	,[IsBillable]
	,[HourlyRate]
	,[EstimatedHours]
	,[ActualHours]
	,[CompletedDate]
	,[Notes]
	,[InvoiceItemId]
	,[ModifiedUser]
)
VALUES 
(
	 @ServiceRequestId
	,@TaskId
	,@TaskName
	,@ResponsibleRoleId
	,@ResponsibleRoleName
	,@Sequence
	,@AssignedTo
	,@IsBillable
	,@HourlyRate
	,@EstimatedHours
	,@ActualHours
	,@CompletedDate
	,@Notes
	,@InvoiceItemId
	,@ModifiedUser
)

DECLARE @Id INT
SELECT @Id = SCOPE_IDENTITY()

SELECT @Id