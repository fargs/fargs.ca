
CREATE PROC [Migrate].[UpdateAllTaskStatuses]
AS
	UPDATE srt SET srt.TaskStatusId = t1.NewTaskStatusId
	--SELECT srt.Id, srt.TaskStatusId, t1.NewTaskStatusId
	FROM ServiceRequestTask srt
	INNER JOIN (
		SELECT srt.Id, sr.AppointmentDate, srt.CompletedDate
			, srt.TaskStatusId, srt.TaskName
			, NewTaskStatusId = (CASE 
				WHEN EXISTS(
					SELECT NULL AS [EMPTY]
					FROM [ServiceRequestTask] AS [t0]
					INNER JOIN [ServiceRequestTaskDependent] [t1] ON [t1].ChildId = [t0].Id
					WHERE srt.Id = [t1].ParentId AND [t0].CompletedDate IS NULL AND [t0].IsObsolete = 0
					) THEN 1
				WHEN CompletedDate IS NOT NULL THEN 3
				WHEN IsObsolete = 1 THEN 4
				WHEN srt.TaskId = 133 THEN 1
				ELSE 2
			 END) 
		FROM ServiceRequestTask srt
		INNER JOIN ServiceRequest sr ON srt.ServiceRequestId = sr.Id
	) t1 ON srt.Id = t1.Id
	WHERE ISNULL(srt.TaskStatusId,0) <> 3