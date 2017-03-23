
CREATE PROC [Migrate].[DependentTasks]
AS

SELECT 'Already executed'

--INSERT INTO dbo.ServiceRequestTask (ObjectGuid, ServiceRequestId, TaskId, TaskName, ResponsibleRoleId, [Sequence], TaskPhaseId, Guidance, DependsOn, DueDateBase, DueDateDiff, ShortName, t.[Workload])
--SELECT newid(), sr.Id, t.Id, t.Name, t.ResponsibleRoleId, t.[Sequence], t.TaskPhaseId, t.Guidance, t.DependsOn, t.DueDateBase, t.DueDateDiff, t.ShortName, t.[Workload]
--FROM dbo.Task t, dbo.ServiceRequest sr
--WHERE t.TaskType = 'EVENT' AND sr.AppointmentDate IS NOT NULL;

--WITH Requests 
--AS (
--	SELECT t.Id
--		, t.AssignedTo
--		, t.ServiceRequestId
--		, t.TaskId
--		, t.TaskName
--		, t.CompletedDate
--		, t.IsObsolete
--		, t.IsDependentOnExamDate
--		, t.[Sequence]
--		, DependsOnCSV = t.[DependsOn]
--		, ResponsibleRoleId
--		, [Workload]
--		, LTRIM(RTRIM(m.n.value('.[1]','varchar(8000)'))) AS DependsOn
--	FROM
--	(
--		SELECT Id
--			, AssignedTo
--			, ServiceRequestId
--			, TaskId 
--			, TaskName
--			, CompletedDate
--			, IsObsolete
--			, IsDependentOnExamDate
--			, [Sequence]
--			, [DependsOn]
--			, [ResponsibleRoleId]
--			, [Workload]
--			, CAST('<XMLRoot><RowData>' + REPLACE(CASE WHEN DependsOn IS NULL THEN '' ELSE DependsOn END,',','</RowData><RowData>') + '</RowData></XMLRoot>' AS XML) AS x
--		FROM dbo.ServiceRequestTask
--		WHERE ISNULL(DependsOn, '') <> ''
--	) t
--	CROSS APPLY x.nodes('/XMLRoot/RowData')m(n)
--)
--INSERT INTO dbo.ServiceRequestTaskDependent (ParentId, ChildId) 
--SELECT r1.Id
--	, r2.Id
--FROM Requests r1
--LEFT JOIN dbo.ServiceRequestTask r2 ON r2.ServiceRequestId = r1.ServiceRequestId AND r2.TaskId = CONVERT(SMALLINT, r1.DependsOn)
--WHERE r2.TaskId IS NOT NULL

--UPDATE dbo.ServiceRequestTask SET TaskType = 'EVENT', [Sequence] = 35 WHERE TaskId = 133

---- Map existing tasks to cleaned up task list.

----Create case Folder 32 to 16
--UPDATE dbo.ServiceRequestTask SET TaskId = 16 WHERE TaskId = 32
----Save medbrief 33 to 18
--UPDATE dbo.ServiceRequestTask SET TaskId = 18 WHERE TaskId = 33
----Draft the report 34 to 8
--UPDATE dbo.ServiceRequestTask SET TaskId = 8 WHERE TaskId = 34
----Approve the report 35 to 9
--UPDATE dbo.ServiceRequestTask SET TaskId = 9 WHERE TaskId = 35
----Submit the report 36 to 19
--UPDATE dbo.ServiceRequestTask SET TaskId = 19 WHERE TaskId = 36
----Close the case 131 to 30
--UPDATE dbo.ServiceRequestTask SET TaskId = 30 WHERE TaskId = 131

----Case preparation - delete all 1
--DELETE FROM ServiceRequestTask WHERE TaskId = 1
----Waiting for QA confirmation - delete all 20
--DELETE FROM ServiceRequestTask WHERE TaskId = 20

---- Delete Available Days for non physicians
--DELETE FROM ad 
--FROM AvailableDay ad
--LEFT JOIN Physician p ON ad.PhysicianId = p.Id
--WHERE p.Id IS NULL