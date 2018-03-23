
CREATE VIEW [Analysis].[Physician]
AS
SELECT u.Id
	,u.Title
	,u.FirstName
	,u.LastName
FROM dbo.AspNetUsers u
INNER JOIN (
	SELECT ur.UserId, r.[Name]
	FROM AspNetUserRoles ur
	INNER JOIN AspNetRoles r ON ur.RoleId = r.Id
	WHERE r.Id = '8359141F-E423-4E48-8925-4624BA86245A'
) r ON u.Id = r.UserId