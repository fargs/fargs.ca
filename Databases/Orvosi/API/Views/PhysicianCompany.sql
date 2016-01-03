







CREATE VIEW [API].[PhysicianCompany]
AS
WITH FullList
AS (
	SELECT PhysicianId = p.Id
		,CompanyId = c.Id
		,CompanyName = c.Name
		,c.LogoCssClass
		,c.ParentId
		,ParentCompanyName = c.ParentName
		,PhysicianDisplayName = p.DisplayName
	FROM API.[Physician] p, API.Company c 
)
, Companies
AS (
SELECT 
	  t.PhysicianId
	 ,t.PhysicianDisplayName
	 ,t.CompanyId
	 ,t.CompanyName
	 ,t.LogoCssClass
	 ,t.ParentId
	 ,t.ParentCompanyName
	 ,RelationshipStatusId = CONVERT(tinyint, CASE WHEN pc.RelationshipStatusId IS NULL AND parent.RelationshipStatusId IS NULL THEN 1
			WHEN pc.RelationshipStatusId IS NULL AND parent.RelationshipStatusId IS NOT NULL THEN parent.RelationshipStatusId
			WHEN pc.RelationshipStatusId IS NOT NULL THEN pc.RelationshipStatusId
			ELSE 1 
			END)
FROM FullList t
LEFT JOIN dbo.PhysicianCompany pc ON t.CompanyId = pc.CompanyId AND t.PhysicianId = pc.PhysicianId
LEFT JOIN dbo.PhysicianCompany parent ON t.ParentId = parent.CompanyId AND t.PhysicianId = parent.PhysicianId
)
SELECT *
	,RelationshipStatusName = li.[Text]
FROM Companies pc
LEFT JOIN dbo.LookupItem li ON pc.RelationshipStatusId = li.Id