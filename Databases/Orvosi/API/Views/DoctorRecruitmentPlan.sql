

CREATE VIEW [API].[DoctorRecruitmentPlan] 
AS 

SELECT tf.PK_Date
	, tf.[Sequence]
	, tf.[Year]
	, tf.[Month]
	, tf.Month_Of_Year
	, ConfigValue = CASE WHEN conf.ConfigValue IS NULL THEN def.ConfigValue ELSE conf.ConfigValue END
	, DerivedFrom = CASE WHEN conf.ConfigValue IS NULL THEN 'Inherited from default' ELSE 'Explicitly set' END
FROM dbo.Timeframe tf
CROSS JOIN dbo.Configuration def
LEFT JOIN dbo.Configuration conf ON tf.[Sequence] = conf.[Sequence]
WHERE def.ConfigurationTypeID = 1
	AND def.[Sequence] IS NULL