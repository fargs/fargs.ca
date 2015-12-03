





CREATE VIEW [dbo].[SchemaTableColumn]
AS
SELECT c.TableObjectId AS ObjectID
	, ColumnId AS ColumnID
	, TABLE_SCHEMA AS SchemaName
	, TABLE_NAME AS TableName
	, COLUMN_NAME AS ColumnName
	, DATA_TYPE AS DataType
	, COLUMN_DEFAULT AS ColumnDefault
	, CHARACTER_MAXIMUM_LENGTH AS [MaxLength]
	, IS_NULLABLE AS IsNullable
	, NUMERIC_PRECISION AS NumericPrecision
	, NUMERIC_SCALE AS NumericScale
	, ORDINAL_POSITION AS OrdinalPosition
FROM (
	SELECT TableObjectId = t.object_id, TableName = t.name, SchemaId = s.SCHEMA_ID, SchemaName = s.name, ColumnName = c.name, ColumnId = c.column_id
	FROM sys.tables t
	LEFT JOIN sys.schemas s ON t.schema_id = s.schema_id
	LEFT JOIN sys.columns c ON c.object_id = t.object_id
) c
LEFT OUTER JOIN INFORMATION_SCHEMA.COLUMNS ic ON c.TableName = ic.TABLE_NAME AND c.ColumnName = ic.COLUMN_NAME AND ic.TABLE_SCHEMA = c.SchemaName