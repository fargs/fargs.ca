
CREATE VIEW [Private].[ServiceRequestTaskDependsOnCSV]
AS
SELECT ServiceRequestTaskId = p1.ParentId
       , DependsOnCSV = STUFF( (SELECT ','+CONVERT(varchar(10),p2.ChildId)
               FROM dbo.ServiceRequestTaskDependent p2
               WHERE p2.ParentId = p1.ParentId
               FOR XML PATH(''), TYPE).value('.', 'varchar(max)')
            ,1,1,'')
FROM dbo.ServiceRequestTaskDependent p1
GROUP BY ParentId ;