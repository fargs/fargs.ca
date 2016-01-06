CREATE VIEW API.ServiceRequestTask
AS
SELECT 
	 st.[Id]
	,st.[ObjectGuid]
	,st.[ServiceRequestId]
	,st.[TaskId]
	,st.[TaskName]
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
	,AssignedToDisplayName = dbo.GetDisplayName(u.FirstName, u.LastName, u.Title)
FROM dbo.ServiceRequestTask st
LEFT JOIN dbo.AspNetUsers u ON st.AssignedTo = u.Id