

CREATE VIEW [Private].[OpenServiceRequestIds]
AS
SELECT DISTINCT ServiceRequestId
FROM dbo.ServiceRequestTask srt
WHERE IsObsolete = 0 AND CompletedDate IS NULL AND srt.TaskType IS NULL