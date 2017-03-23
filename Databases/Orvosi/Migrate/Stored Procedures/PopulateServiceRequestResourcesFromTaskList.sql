
CREATE PROC [Migrate].[PopulateServiceRequestResourcesFromTaskList]
AS

DECLARE @Now DATETIME
SET @Now = GETDATE()

MERGE dbo.ServiceRequestResource AS target
USING (
	SELECT DISTINCT srt.ServiceRequestId, srt.AssignedTo, srt.ResponsibleRoleId
	FROM ServiceRequestTask srt
	WHERE AssignedTo IS NOT NULL
) AS source (ServiceRequestId, AssignedTo, ResponsibleRoleId)
ON target.ServiceRequestId = source.ServiceRequestId AND target.UserId = source.AssignedTo AND target.RoleId = source.ResponsibleRoleId
WHEN NOT MATCHED BY target THEN
	INSERT (Id, ServiceRequestId, UserId, RoleId, CreatedDate, CreatedUser, ModifiedDate, ModifiedUser)
	VALUES (NEWID(), ServiceRequestId, AssignedTo, ResponsibleRoleId, @Now, '11111111-2222-3333-4444-555555555555', @Now, '11111111-2222-3333-4444-555555555555')
WHEN NOT MATCHED BY source THEN
	DELETE;