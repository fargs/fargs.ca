






CREATE VIEW [API].[LocationArea]
AS
SELECT *
FROM API.LookupItem l 
WHERE l.LookupId = [dbo].[LookupId_Location]()