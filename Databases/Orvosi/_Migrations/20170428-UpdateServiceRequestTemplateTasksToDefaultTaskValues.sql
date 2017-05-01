SELECT * FROM ServiceRequestTemplate WHERE Id IN (2,8,10,11,13,16)
GO 

/*
Update all the non default templates with values from the default templates.
*/
WITH DefaultIME
AS (
	SELECT TaskId, IsCriticalPath, IsBaselineDate, DueDateDurationFromBaseline, EffectiveDateDurationFromBaseline
	FROM ServiceRequestTemplateTask srtt
	WHERE srtt.ServiceRequestTemplateId IN (1)
)
--UPDATE srt SET DueDateDurationFromBaseline = t.DueDateDurationFromBaseline, EffectiveDateDurationFromBaseline = t.EffectiveDateDurationFromBaseline, IsBaselineDate = t.IsBaselineDate, IsCriticalPath = t.IsCriticalPath
SELECT srt.Id, srt.TaskId, tt.Name, t.DueDateDurationFromBaseline, t.EffectiveDateDurationFromBaseline, t.IsBaselineDate, t.IsCriticalPath
FROM ServiceRequestTemplateTask srt 
LEFT JOIN Task tt ON tt.Id = srt.TaskId 
LEFT JOIN DefaultIME t ON srt.TaskId = t.TaskId
WHERE srt.ServiceRequestTemplateId IN (3,4,7,9,12,14,15)
AND srt.TaskId != 134

WITH DefaultAddOn
AS (
	SELECT TaskId, IsCriticalPath, IsBaselineDate, DueDateDurationFromBaseline, EffectiveDateDurationFromBaseline
	FROM ServiceRequestTemplateTask srtt
	WHERE srtt.ServiceRequestTemplateId IN (2)
)
--UPDATE srt SET DueDateDurationFromBaseline = t.DueDateDurationFromBaseline, EffectiveDateDurationFromBaseline = t.EffectiveDateDurationFromBaseline, IsBaselineDate = t.IsBaselineDate, IsCriticalPath = t.IsCriticalPath
SELECT srt.Id, srt.TaskId, tt.Name, t.DueDateDurationFromBaseline, t.EffectiveDateDurationFromBaseline, t.IsBaselineDate, t.IsCriticalPath
FROM ServiceRequestTemplateTask srt 
LEFT JOIN Task tt ON tt.Id = srt.TaskId 
LEFT JOIN DefaultAddON t ON srt.TaskId = t.TaskId
WHERE srt.ServiceRequestTemplateId IN (8,10,11,13,16)

