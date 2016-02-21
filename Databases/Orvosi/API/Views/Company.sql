﻿




CREATE VIEW [API].[Company]
AS

SELECT 
	 c.[Id]
	,c.ObjectGuid
	,c.[Name]
	,c.[Code]
	,c.[IsParent]
	,c.[ParentId]
	,c.[InvoiceEmails]
	,c.[LogoCssClass]
	,c.[MasterBookingPageByPhysician]
	,c.[MasterBookingPageByTime]
	,c.[MasterBookingPageTeleconference]
	,c.[ModifiedDate]
	,c.[ModifiedUser]
	,ParentName = p.Name
FROM dbo.Company c
LEFT JOIN dbo.Company p ON c.ParentId = p.Id