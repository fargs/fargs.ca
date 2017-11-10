CREATE PROC [Manual].UpdateSubmitReportTaskDueDatesToReportDueDate
AS

DECLARE @now DATETIME
DECLARE @user NVARCHAR(50)
SET @now = GETDATE()
SET @user = 'lfarago@orvosi.ca'


SELECT sr.CreatedDate, sr.Id, sr.ClaimantName, t.[Name], sr.AppointmentDate, ReportDueDate = sr.DueDate, srt.DueDate, srt.EffectiveDate
--UPDATE srt SET DueDate = sr.DueDate, ModifiedDate = @now, ModifiedUser = @user
FROM dbo.ServiceRequestTask srt
LEFT JOIN dbo.ServiceRequest sr ON srt.ServiceRequestId = sr.Id 
LEFT JOIN dbo.Task t ON srt.TaskId = t.Id
WHERE srt.TaskId IN (9, 19, 30)
	AND srt.DueDate <> sr.DueDate