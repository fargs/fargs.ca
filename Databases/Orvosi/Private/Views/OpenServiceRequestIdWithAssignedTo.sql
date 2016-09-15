


CREATE VIEW [Private].[OpenServiceRequestIdWithAssignedTo]
AS

SELECT srt.ServiceRequestId, srt.AssignedTo
FROM dbo.ServiceRequestTask srt
INNER JOIN [Private].OpenServiceRequestIds o ON srt.ServiceRequestId = o.ServiceRequestId
GROUP BY srt.ServiceRequestId, srt.AssignedTo