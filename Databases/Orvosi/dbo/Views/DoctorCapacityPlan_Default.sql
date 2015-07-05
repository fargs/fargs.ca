





CREATE VIEW [dbo].[DoctorCapacityPlan_Default] 
AS 

WITH Config
AS
(	
	SELECT UserId, EntityTypeID, ConfigurationTypeID, [DatePart], [Sequence], ConfigValue 
	FROM (
		SELECT ur.UserId
			, c.EntityTypeID
			, c.EntityID
			, c.ConfigurationTypeID
			, c.[DatePart]
			, c.[Sequence]
			, c.ConfigValue
			, ROW_NUMBER() OVER(PARTITION BY ur.UserId, c.ConfigurationTypeID ORDER BY c.EntityTypeID DESC) AS RowNum
		FROM dbo.AspNetUserRoles ur
		CROSS JOIN dbo.Configuration c
		WHERE ur.RoleId = '8359141f-e423-4e48-8925-4624ba86245a' -- DOCTOR
			AND (
				c.EntityTypeID = 1 
				OR (c.EntityTypeID = 2 AND ur.UserId = c.EntityID) 
			)
	) t
	WHERE RowNum = 1
)
SELECT c.UserId
	, tf.PK_Date
	, tf.[Sequence]
	, tf.[Year]
	, tf.[Month]
	, tf.Month_Of_Year
	, c.ConfigValue
FROM dbo.Timeframe tf
CROSS JOIN Config c
WHERE c.[Sequence] IS NULL 
	AND c.ConfigurationTypeID = 2