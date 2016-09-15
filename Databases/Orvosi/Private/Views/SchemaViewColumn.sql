




CREATE VIEW [Private].[SchemaViewColumn]
AS
SELECT c.TableObjectId AS ObjectID
	, c.ColumnId AS ColumnID
	, ic.TABLE_SCHEMA AS SchemaName
	, ic.TABLE_NAME AS TableName
	, ic.COLUMN_NAME AS ColumnName
	, ic.DATA_TYPE AS DataType
	, ic.COLUMN_DEFAULT AS ColumnDefault
	, ic.CHARACTER_MAXIMUM_LENGTH AS [MaxLength]
	, ic.IS_NULLABLE AS IsNullable
	, ic.NUMERIC_PRECISION AS NumericPrecision
	, ic.NUMERIC_SCALE AS NumericScale
	, ic.ORDINAL_POSITION AS OrdinalPosition
	, DataTypeDisplayName = 
		CASE WHEN ic.DATA_TYPE IN ('nvarchar', 'varchar') 
		THEN ic.DATA_TYPE + '(' + CASE WHEN CHARACTER_MAXIMUM_LENGTH = -1 THEN 'max' ELSE CONVERT(VARCHAR(50), CHARACTER_MAXIMUM_LENGTH) END + ')' 
		WHEN ic.DATA_TYPE IN ('decimal', 'numeric')
		THEN ic.DATA_TYPE + '(' + CONVERT(VARCHAR(2), NUMERIC_PRECISION) + ',' + CONVERT(VARCHAR(2), NUMERIC_SCALE) + ')' 
		ELSE ic.DATA_TYPE END
	, ParameterDisplayName = '@' + ic.COLUMN_NAME
	, ColumnDisplayName = '[' + ic.COLUMN_NAME + ']'
	, c.is_computed
	, c.is_identity
FROM (
	SELECT TableObjectId = t.object_id, TableName = t.name, SchemaId = s.SCHEMA_ID, SchemaName = s.name, ColumnName = c.name, ColumnId = c.column_id
		, c.is_ansi_padded
		, c.is_computed
		, c.is_identity
		, c.is_filestream
		, c.is_nullable
		, c.is_rowguidcol
		, c.is_column_set
		, c.is_sparse
		, c.is_xml_document
		, c.max_length
		, c.[precision]
		, c.scale
	FROM sys.views t
	LEFT JOIN sys.schemas s ON t.schema_id = s.schema_id
	LEFT JOIN sys.columns c ON c.object_id = t.object_id
) c
LEFT OUTER JOIN INFORMATION_SCHEMA.COLUMNS ic ON c.TableName = ic.TABLE_NAME AND c.ColumnName = ic.COLUMN_NAME AND ic.TABLE_SCHEMA = c.SchemaName