

CREATE PROC [Migrate].[UpdateAssessmentDayTaskState]
	@Now DATETIME,
	@UserName NVARCHAR(128)
AS

UPDATE srt SET TaskStatusId = 3, CompletedDate = @Now, srt.ModifiedUser = @UserName, srt.ModifiedDate = @Now
FROM dbo.ServiceRequestTask srt
LEFT JOIN dbo.ServiceRequest sr ON sr.Id = srt.ServiceRequestId
WHERE TaskId = 133 
	AND sr.AppointmentDate < @Now 
	AND IsObsolete = 0