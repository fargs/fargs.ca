CREATE PROC RunSimulation
AS
SELECT Id
	, Name = 'Company ' + CONVERT(NVARCHAR(10), Id)
	, ExpectedAssessmentCount = 10
FROM dbo.[Sequence] s
WHERE s.Id <= 10

SELECT Id, 'Doctor ' + CONVERT(NVARCHAR(10), Id) 
FROM dbo.[Sequence] s
WHERE s.Id <= 10

SELECT Id, 'Assessment ' + CONVERT(NVARCHAR(10), Id) 
FROM dbo.[Sequence] s
WHERE s.Id <= 100