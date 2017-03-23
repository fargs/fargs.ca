
CREATe PROC [Migrate].[FixMissingResponsibleRoleNames]
AS
UPDATE srt SET ResponsibleRoleName = t.[Name]
--SELECT srt.Id, srt.TaskName, srt.ResponsibleRoleId, srt.ResponsibleRoleName, tt.ResponsibleRoleId, t.[Name] 
FROM ServiceRequestTask srt
LEFT JOIN ServiceRequestTemplateTask tt ON srt.ServiceRequestTemplateTaskId = tt.Id
LEFT JOIN AspNetRoles t ON tt.ResponsibleRoleId = t.Id
WHERE srt.ResponsibleRoleName IS NULL