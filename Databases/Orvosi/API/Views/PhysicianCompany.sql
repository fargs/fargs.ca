





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
SELECT 
	  t.PhysicianId
	 ,t.PhysicianDisplayName
	 ,t.CompanyId
	 ,t.CompanyName
	 ,t.LogoCssClass
	 ,t.ParentId
	 ,t.ParentCompanyName
	 ,RelationshipStatusId = ISNULL(pc.RelationshipStatusId, 1)
	 ,RelationshipStatusName = li.[Text]
FROM FullList t
LEFT JOIN dbo.PhysicianCompany pc ON t.CompanyId = pc.CompanyId AND t.PhysicianId = pc.PhysicianId
LEFT JOIN dbo.LookupItem li ON ISNULL(pc.RelationshipStatusId, 1) = li.Id