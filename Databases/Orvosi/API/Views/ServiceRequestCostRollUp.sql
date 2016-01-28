CREATE VIEW API.ServiceRequestCostRollUp
AS
WITH CostRollUp
AS (
	SELECT	CASE WHEN (GROUPING(ServiceRequestId) = 1) THEN 0
				ELSE ISNULL(ServiceRequestId, NULL)
				END AS ServiceRequestId
			, CASE WHEN (GROUPING(UserId) = 1) THEN 'ALL'
				ELSE ISNULL(UserId, 'UNKNOWN')
				END AS UserId
			, TaskId
			, SUM(Cost) AS TotalCost
	FROM API.ServiceRequestTask
	WHERE UserId IN (
		SELECT Id FROM API.[User] WHERE RoleCategoryId = 3 -- Staff
	)
	AND IsBillable = 1
	GROUP BY ServiceRequestId, UserId, TaskId WITH ROLLUP
)
SELECT Id = ISNULL(ROW_NUMBER() OVER(ORDER BY c.ServiceRequestId, ISNULL(u.DisplayName, 'zTotal'), ISNULL(t.Name, 'z')),0)
	, c.ServiceRequestId
	, DisplayName = ISNULL(u.DisplayName, 'Total')
	, u.RoleName
	, u.RoleCategoryName
	, TaskName = ISNULL(t.Name, 'Total')
	, c.TotalCost
FROM CostRollUp c
LEFT JOIN API.[User] u ON c.UserId = u.Id
LEFT JOIN dbo.Task t ON c.TaskId = t.Id