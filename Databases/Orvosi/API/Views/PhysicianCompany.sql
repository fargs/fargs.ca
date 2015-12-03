



CREATE VIEW [API].[PhysicianCompany]
AS
SELECT 
	  PhysicianId = p.Id
	 ,CompanyId = c.Id
	 ,CompanyName = c.Name
	 ,c.LogoCssClass
	 ,c.ParentId
	 ,ParentCompanyName = c.ParentName
	 ,RelationshipStatusId = ISNULL(pc.RelationshipStatusId, 1)
	 ,RelationshipStatusName = li.[Text]
FROM API.[Physician] p, API.Company c 
LEFT JOIN dbo.PhysicianCompany pc ON c.Id = pc.CompanyId
LEFT JOIN dbo.LookupItem li ON ISNULL(pc.RelationshipStatusId, 1) = li.Id