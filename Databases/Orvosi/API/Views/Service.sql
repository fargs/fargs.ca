

CREATE VIEW [API].[Service]
AS

SELECT 
	s.[Id]
	,s.[ObjectGuid]
	,s.[Name]
	,s.[Description]
	,s.[Code]
	,DefaultPrice = p.[Price]
	,s.[ServiceCategoryId]
	,s.[ServicePortfolioId]
	,s.[ModifiedDate]
	,s.[ModifiedUser]
	,ServiceCategoryName = sc.Name
	,ServicePortfolioName = sp.Name
FROM dbo.[Service] s
LEFT JOIN dbo.ServiceCategory sc ON s.ServiceCategoryId = sc.Id
LEFT JOIN dbo.ServicePortfolio sp ON s.ServicePortfolioId = sp.Id
LEFT JOIN dbo.Price p ON s.ObjectGuid = p.ObjectGuid