
CREATE PROC [dbo].[CodeGenerationHelpers]
AS

-- Parameters
SELECT 
	',@' + ColumnName + ' ' + CASE WHEN DataType IN ('nvarchar', 'varchar') THEN DataType + '(' + CONVERT(VARCHAR(50), [MaxLength]) + ')' ELSE DataType END
FROM [dbo].[SchemaTableColumn]
WHERE TableName = 'SpecialRequest'
ORDER BY OrdinalPosition

-- Select Columns
SELECT 
	',[' + ColumnName + ']'
FROM [dbo].[SchemaTableColumn]
WHERE TableName = 'SpecialRequest'
ORDER BY OrdinalPosition

-- Parameter List
SELECT 
	',@' + ColumnName
FROM [dbo].[SchemaTableColumn]
WHERE TableName = 'SpecialRequest'
ORDER BY OrdinalPosition

-- Update
SELECT 
	',[' + ColumnName + '] = @' + ColumnName
FROM [dbo].[SchemaTableColumn]
WHERE TableName = 'SpecialRequest'
ORDER BY OrdinalPosition

SELECT 
	',[' + ColumnName + '] ' + CASE WHEN DataType IN ('nvarchar', 'varchar') THEN DataType + '(' + CONVERT(VARCHAR(50), [MaxLength]) + ')' ELSE DataType END
FROM [dbo].[SchemaTableColumn]
WHERE TableName = 'SpecialRequest'
ORDER BY OrdinalPosition