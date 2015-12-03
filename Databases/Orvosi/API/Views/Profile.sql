




CREATE VIEW [API].[Profile]
AS

WITH PrimaryRole
AS (
	SELECT UserId, RoleId, RoleName
	FROM (
		SELECT ur.UserId, ur.RoleId, RoleName = r.Name
			, RowNum = ROW_NUMBER() OVER(PARTITION BY UserId ORDER BY RoleId)
		FROM dbo.AspNetUserRoles ur
		INNER JOIN dbo.AspNetRoles r ON ur.RoleId = r.Id
	) t 
	WHERE RowNum = 1
)
SELECT u.[Id]
      ,u.[Email]
      ,u.[PhoneNumber]
      ,u.[Title]
      ,u.[FirstName]
      ,u.[LastName]
	  ,DisplayName = dbo.GetDisplayName(u.FirstName,u.LastName,u.Title)
      ,u.[EmployeeId]
      ,u.[CompanyId]
      ,CompanySubmittedName = u.[CompanyName]
      ,u.[ModifiedDate]
      ,u.[ModifiedUser]
      ,u.[IsTestRecord]
	  ,r.RoleId
	  ,r.RoleName
	  ,CompanyName = c.Name
FROM [dbo].[AspNetUsers] u
LEFT JOIN PrimaryRole r ON r.UserId = u.Id
LEFT JOIN dbo.Company c ON u.CompanyId = c.Id