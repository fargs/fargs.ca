-- Data Migration

-- Update all the task statuses via the SysAdmin Update All Task Status button

-- Update the TaskStatusChangedBy and TaskStatusChangedDate values with CompletedBy and CompletedDate

-- Update the IsCriticalPath column on the ServiceRequestTask table (ensure it is being set properly)
CREATE PROC Migrate.TaskIsCriticalPath
AS
UPDATE srt SET srt.IsCriticalPath = t.IsCriticalPath
FROM ServiceRequestTask srt
INNER JOIN Task t ON srt.TaskId = t.Id
WHERE t.IsCriticalPath = 1 AND srt.IsCriticalPath = 0

UPDATE ServiceRequestTask SET IsCriticalPath = 0 WHERE TaskId = 24
UPDATE ServiceRequestTask SET IsCriticalPath = 1 WHERE TaskId = 133