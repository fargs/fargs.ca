
CREATE PROC [Migrate].[PopulateTaskStatusId]
AS
UPDATE srt SET TaskStatusId = CASE WHEN CompletedDate IS NOT NULL THEN 3
	WHEN IsObsolete = 1 THEN 4
	ELSE 2
	END
FROM ServiceRequestTask srt

DECLARE @Now DATETIME
SET @Now = GETDATE()

UPDATE srt SET TaskStatusId = CASE WHEN sr.AppointmentDate <= @Now THEN 3 ELSE 1 END
--SELECT TaskStatusId = CASE WHEN sr.AppointmentDate <= @Now THEN 3 ELSE 1 END, *
FROM ServiceRequestTask srt
LEFT JOIN ServiceRequest sr ON srt.ServiceRequestId = sr.Id
WHERE TaskId = 133 AND TaskStatusId = 2