
CREATE PROC [dbo].[CodeGenerationHelpers]
AS

SELECT 
	',[' + ColumnName + ']'
FROM [dbo].[SchemaTableColumn]
WHERE TableName = 'Job'

SELECT 
	',[' + ColumnName + '] ' + CASE WHEN DataType IN ('nvarchar', 'varchar') THEN DataType + '(' + CONVERT(VARCHAR(50), [MaxLength]) + ')' ELSE DataType END
FROM [dbo].[SchemaTableColumn]
WHERE TableName = 'Job'