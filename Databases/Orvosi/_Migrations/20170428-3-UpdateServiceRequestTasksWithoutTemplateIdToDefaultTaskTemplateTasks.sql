/*
Update all the existing service request tasks without a link to a template to the appropriate task from the default templates.
*/
DECLARE @Now DATETIME
SET @Now = GETDATE()
DECLARE @UserName NVARCHAR(50)
SET @UserName = 'lfarago@orvosi.ca'
UPDATE srt SET ServiceRequestTemplateTaskId = tt.Id, ModifiedDate = @Now, ModifiedUser = @UserName
--SELECT srt.ServiceRequestTemplateTaskId, tt.Id
FROM ServiceRequestTask srt
INNER JOIN Task t ON srt.TaskId = t.Id
INNER JOIN (
	SELECT * FROM ServiceRequestTemplateTask WHERE ServiceRequestTemplateId = 1
) tt ON srt.TaskId = tt.TaskId
INNER JOIN ServiceRequest sr ON srt.ServiceRequestId = sr.Id
WHERE srt.ServiceRequestTemplateTaskId IS NULL AND sr.AppointmentDate IS NOT NULL
