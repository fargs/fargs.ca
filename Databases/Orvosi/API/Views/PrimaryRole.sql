CREATE VIEW API.PrimaryRole
AS
SELECT UserId, RoleId, RoleName, RoleCategoryId, RoleCategoryName
FROM (
	SELECT ur.UserId
		, ur.RoleId
		, RoleName = r.Name
		, r.RoleCategoryId
		, RoleCategoryName = rc.Name
		, RowNum = ROW_NUMBER() OVER(PARTITION BY UserId ORDER BY RoleId)
	FROM dbo.AspNetUserRoles ur
	INNER JOIN dbo.AspNetRoles r ON ur.RoleId = r.Id
	LEFT JOIN dbo.RoleCategory rc ON r.RoleCategoryId = rc.Id
) t 
WHERE RowNum = 1