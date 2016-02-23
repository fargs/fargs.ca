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
	,c.[LogoCssClass]
	,c.BillingEmail
	,c.ReportsEmail
	,c.Phone
	,c.[ModifiedDate]
	,c.[ModifiedUser]
	,ParentName = p.Name
FROM dbo.Company c
LEFT JOIN dbo.Company p ON c.ParentId = p.Id