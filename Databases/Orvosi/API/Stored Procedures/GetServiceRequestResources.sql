﻿CREATE PROC API.GetServiceRequestResources
	@ServiceRequestId INT
AS
WITH Resources
AS (
SELECT DISTINCT u.Id
	, u.UserName
	, u.Email
	, DisplayName = dbo.GetDisplayName(u.FirstName, u.LastName, u.Title)
	, Initials = dbo.GetInitials(u.FirstName, u.LastName)
	, u.ColorCode
	, u.BoxUserId
	, Roles = STUFF((	
		SELECT DISTINCT '|' + ResponsibleRoleName
		FROM dbo.ServiceRequestTask srt2 
		WHERE u.Id = srt2.AssignedTo
			AND srt2.ServiceRequestId = @ServiceRequestId
		FOR XML PATH('')), 1, 1, '')
FROM dbo.AspNetUsers u
INNER JOIN dbo.ServiceRequestTask srt ON u.Id = srt.AssignedTo
WHERE srt.ServiceRequestId = @ServiceRequestId
GROUP BY u.Id
	, u.UserName
	, u.Email
	, u.FirstName
	, u.LastName
	, u.Title
	, u.ColorCode
	, u.BoxUserId
	, u.BoxFolderId
)
SELECT r.*
	, srbc.BoxCollaborationId
FROM Resources r 
LEFT JOIN dbo.ServiceRequestBoxCollaboration srbc ON r.Id = srbc.UserId AND srbc.ServiceRequestId = @ServiceRequestId