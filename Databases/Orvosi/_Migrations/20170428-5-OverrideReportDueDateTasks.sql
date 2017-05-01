/*
This overrides the calculated due date for Approve Report, Submit Report and Close Case tasks to match the Due Date set on the service request.
*/
UPDATE srt SET DueDate = sr.DueDate
--SELECT srt.DueDate
--	, sr.DueDate
FROM ServiceRequestTask srt
LEFT JOIN ServiceRequest sr ON srt.ServiceRequestId = sr.Id
WHERE srt.TaskId IN (19,9,30) AND sr.DueDate IS NOT NULL AND sr.DueDate <> srt.DueDate
