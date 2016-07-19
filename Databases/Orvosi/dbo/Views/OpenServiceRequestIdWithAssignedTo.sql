CREATE VIEW dbo.OpenServiceRequestIdWithAssignedTo
AS
SELECT DISTINCT ServiceRequestId, AssignedTo
FROM dbo.ServiceRequestTask srt
WHERE IsObsolete = 0 AND CompletedDate IS NULL