
CREATE VIEW [Analysis].[User]
AS
SELECT u.Id
	,u.FirstName
	,u.LastName
	,[Role] = r.[Name]
FROM dbo.AspNetUsers u
LEFT JOIN (
	SELECT ur.UserId, r.[Name]
	FROM AspNetUserRoles ur
	INNER JOIN AspNetRoles r ON ur.RoleId = r.Id
) r ON u.Id = r.UserId