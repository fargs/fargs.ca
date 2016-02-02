









CREATE VIEW [API].[DashboardTaskSummary]
AS
SELECT 
	 st.[TaskId]
	,st.[TaskName]
	,st.[TaskPhaseName]
	,AssignedToUserId = st.[AssignedTo]
	,st.[AssignedToDisplayName]
	,st.[CompletedDate]
	,st.ResponsibleRoleId
	,st.ResponsibleRoleName
	,TaskCount = COUNT(1)
FROM API.ServiceRequestTask st
GROUP BY st.TaskName, st.TaskId, st.TaskPhaseName, st.CompletedDate, st.ResponsibleRoleId, st.ResponsibleRoleName, st.[AssignedTo], st.AssignedToDisplayName