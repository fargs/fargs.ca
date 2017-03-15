
CREATE PROC [API].[UpdateAssessmentDayTaskState]
	@now DATETIME
AS

--UPDATE srt SET TaskStatusId = 3
SELECT sr.Id
FROM dbo.ServiceRequestTask srt
LEFT JOIN dbo.ServiceRequest sr ON sr.Id = srt.ServiceRequestId
WHERE TaskId = 133 
	AND sr.AppointmentDate < @now 
	--AND TaskStatusId = 1 
	--AND sr.Id = 1364