
CREATE PROC [dbo].[CodeGenerationHelpers]
	 @TableName NVARCHAR(128)
	,@SchemaName NVARCHAR(128)
AS

-- Parameters
SELECT 
	[Parameters] = ',@' + ColumnName + ' ' + CASE WHEN DataType IN ('nvarchar', 'varchar') THEN DataType + '(' + CONVERT(VARCHAR(50), [MaxLength]) + ')' ELSE DataType END
FROM [dbo].[SchemaTableColumn]
WHERE TableName = @TableName AND SchemaName = @SchemaName
ORDER BY OrdinalPosition

-- Select Columns
SELECT 
	[Select] = ',[' + ColumnName + ']'
FROM [dbo].[SchemaTableColumn]
WHERE TableName = @TableName AND SchemaName = @SchemaName
ORDER BY OrdinalPosition

-- Insert List
SELECT 
	[Insert] = ',@' + ColumnName
FROM [dbo].[SchemaTableColumn]
WHERE TableName = @TableName AND SchemaName = @SchemaName
ORDER BY OrdinalPosition

-- Update
SELECT 
	[Update] = ',[' + ColumnName + '] = @' + ColumnName
FROM [dbo].[SchemaTableColumn]
WHERE TableName = @TableName AND SchemaName = @SchemaName
ORDER BY OrdinalPosition