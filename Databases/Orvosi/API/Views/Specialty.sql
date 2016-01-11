







CREATE VIEW [API].[Specialty]
AS
SELECT *
FROM API.LookupItem l 
WHERE l.LookupId = dbo.LookupId_Specialty()