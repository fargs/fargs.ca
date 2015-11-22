
CREATE VIEW [API].[Service]
AS
SELECT 
	s.[Id]
	,s.[Name]
	,s.[Description]
	,s.[Code]
	,s.[Price]
	,s.[IsAddOn]
	,s.[ServiceCategoryId]
	,s.[ServicePortfolioId]
	,s.[ModifiedDate]
	,s.[ModifiedUser]
	,ServiceCategoryName = sc.Name
	,ServicePortfolioName = sp.Name
FROM dbo.[Service] s
LEFT JOIN dbo.ServiceCategory sc ON s.ServiceCategoryId = sc.Id
LEFT JOIN dbo.ServicePortfolio sp ON s.ServicePortfolioId = sp.Id