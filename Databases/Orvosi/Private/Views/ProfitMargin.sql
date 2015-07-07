CREATE VIEW [Private].ProfitMargin
AS
SELECT j.*
	, Profit = j.Price - jt.Cost
	, jt.Cost
	, ProfitMargin = ((j.Price - jt.Cost) / j.Price) * 100
FROM [Private].Job j
INNER JOIN [Private].JobTask jt ON j.JobId = jt.JobId
WHERE jt.IsBillable = 1