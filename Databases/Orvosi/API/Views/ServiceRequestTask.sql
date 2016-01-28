







CREATE VIEW [API].[ServiceRequestTask]
AS
SELECT 
	 st.[Id]
	,st.[ObjectGuid]
	,st.[ServiceRequestId]
	,st.[TaskId]
	,st.[TaskName]
	,st.[Guidance]
	,st.[ResponsibleRoleId]
	,st.[ResponsibleRoleName]
	,st.[Sequence]
	,st.[AssignedTo]
	,st.[IsBillable]
	,st.[HourlyRate]
	,st.[EstimatedHours]
	,st.[ActualHours]
	,st.[CompletedDate]
	,st.[Notes]
	,st.[InvoiceItemId]
	,st.[ModifiedDate]
	,st.[ModifiedUser]
	,st.[TaskPhaseId]
	,st.[TaskPhaseName]
	,AssignedToDisplayName = dbo.GetDisplayName(u.FirstName, u.LastName, u.Title)
	,UserId = u.Id
	,StaffHourlyRate = u.HourlyRate
	,Cost = ActualHours * u.HourlyRate
FROM dbo.ServiceRequestTask st
LEFT JOIN dbo.AspNetUsers u ON st.AssignedTo = u.Id