
CREATE VIEW [API].[Company]
AS
SELECT 
	Id = ISNULL([Id], 0)
	,[Name]
	,[ParentId]
	,[LogoCssClass]
	,[MasterBookingPageByPhysician]
	,[MasterBookingPageByTime]
	,[MasterBookingPageTeleconference]
	,[ModifiedDate] = NULLIF([ModifiedDate], -1)
	,[ModifiedUser] = NULLIF([ModifiedUser], -1)
FROM dbo.Company c