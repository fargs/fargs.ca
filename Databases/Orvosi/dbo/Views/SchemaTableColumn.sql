



CREATE VIEW [dbo].[SchemaTableColumn]
AS
SELECT object_id AS ObjectID
	, column_id AS ColumnID
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
	--, [MS_Description] AS ColumnDescription
	--, [cst_Metadata], [cst_MigrationAggregate], [cst_FormPageID], [cst_LookupID], [cst_RegExMessage], [cst_RegEx], [cst_DisplayText_en], [cst_DisplayText_fr]
FROM         (
	SELECT cols.object_id, cols.column_id, ic.TABLE_NAME, ic.COLUMN_NAME, ic.DATA_TYPE, ic.COLUMN_DEFAULT, 
		ic.TABLE_SCHEMA, ic.CHARACTER_MAXIMUM_LENGTH, ic.IS_NULLABLE, ic.NUMERIC_PRECISION, ic.NUMERIC_SCALE, ic.ORDINAL_POSITION
        --,CAST(ex.name AS VARCHAR(1000)) AS ext_prop_name, CAST(ex.value AS VARCHAR(1000)) AS ext_prop_value
	FROM sys.columns cols 
	LEFT OUTER JOIN INFORMATION_SCHEMA.COLUMNS ic ON OBJECT_NAME(object_id) = ic.TABLE_NAME AND cols.[name] = ic.COLUMN_NAME 
	--LEFT OUTER JOIN sys.extended_properties ex ON ex.major_id = cols.object_id AND ex.minor_id = cols.column_id
) p 
WHERE TABLE_NAME IS NOT NULL
--PIVOT (
--	MIN(ext_prop_value) 
--	FOR ext_prop_name 
--	IN ([cst_Metadata], [cst_MigrationAggregate], [cst_FormPageID], [MS_Description], [cst_LookupID], [cst_RegExMessage], [cst_RegEx], [cst_DisplayText_en], [cst_DisplayText_fr])
--) AS pt