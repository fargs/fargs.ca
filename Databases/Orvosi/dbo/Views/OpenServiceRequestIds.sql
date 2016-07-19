CREATE VIEW dbo.OpenServiceRequestIds
AS
SELECT DISTINCT ServiceRequestId
FROM dbo.ServiceRequestTask srt
WHERE IsObsolete = 0 AND CompletedDate IS NULL