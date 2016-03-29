/*SELECT sr.Id
INTO #sr
FROM API.ServiceRequest sr
WHERE sr.ServiceRequestStatusId = 11 -- Closed


-- Add Task 30 where it does not exist
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
	,[DependsOn]
	,[DueDateBase]
	,[DueDateDiff]
	,[ShortName]
	,[IsCriticalPath]
)
SELECT sr.Id
	, st.Id
	, st.TaskName
	, st.TaskPhaseId
	, st.TaskPhaseName
	, st.Guidance
	, st.ResponsibleRoleId
	, st.ResponsibleRoleName
	, st.[Sequence]
	, st.IsBillable
	, 'd579d0a4-11ce-46f2-97ec-4c2bfc4dc704'
	, st.HourlyRate
	, st.EstimatedHours
	, GETDATE()
	, SUSER_NAME()
	, st.DependsOn
	, st.DueDateBase
	, st.DueDateDiff
	, st.ShortName
	, st.IsCriticalPath
FROM API.[Task] st, dbo.ServiceRequest sr
WHERE st.Id IN (28,30)

-- Mark the complete ones as complete
UPDATE srt SET CompletedDate = GETDATE()
FROM dbo.ServiceRequestTask srt
WHERE srt.ServiceRequestId IN (
	SELECT Id FROM #sr
) AND 
srt.TaskId IN (28,30)

-- Update existing tasks
UPDATE srt SET srt.DependsOn = t.DependsOn, srt.DueDateBase = t.DueDateBase, srt.DueDateDiff = t.DueDateDiff, srt.ShortName = t.ShortName, srt.IsCriticalPath = t.IsCriticalPath
FROM dbo.ServiceRequestTask srt 
INNER JOIN dbo.Task t ON srt.TaskId = t.Id

DELETE FROM dbo.ServiceRequestTask WHERE TaskId = 20 AND CompletedDate IS NULL

DELETE FROM dbo.ServiceRequestTask WHERE TaskId = 21 AND CompletedDate IS NULL AND ServiceRequestId NOT IN (
	SELECT ServiceRequestID FROM dbo.ServiceRequestTask WHERE TaskId = 20
)

UPDATE dbo.ServiceRequestTask SET TaskName = 'Complete intake sections' WHERE TaskName = 'Conduct the intake interview'

*/
SELECT * FROM dbo.Task ORDER BY [Sequence]
SELECT * FROM ServiceRequestTask WHERE TaskId IN (20,21) ORDER BY [SEquence]