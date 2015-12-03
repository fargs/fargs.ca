



CREATE VIEW [API].[User]
AS

WITH PrimaryRole
AS (
	SELECT UserId, RoleId, RoleName, RoleCategoryId
	FROM (
		SELECT ur.UserId, ur.RoleId, RoleName = r.Name, r.RoleCategoryId
			, RowNum = ROW_NUMBER() OVER(PARTITION BY UserId ORDER BY RoleId)
		FROM dbo.AspNetUserRoles ur
		INNER JOIN dbo.AspNetRoles r ON ur.RoleId = r.Id
	) t 
	WHERE RowNum = 1
)
SELECT u.[Id]
      ,u.[Email]
      ,u.[EmailConfirmed]
      ,u.[PasswordHash]
      ,u.[SecurityStamp]
      ,u.[PhoneNumber]
      ,u.[PhoneNumberConfirmed]
      ,u.[TwoFactorEnabled]
      ,u.[LockoutEndDateUtc]
      ,u.[LockoutEnabled]
      ,u.[AccessFailedCount]
      ,u.[UserName]
      ,u.[Title]
      ,u.[FirstName]
      ,u.[LastName]
	  ,DisplayName = dbo.GetDisplayName(u.FirstName,u.LastName,u.Title)
      ,u.[EmployeeId]
      ,u.[CompanyId]
      ,CompanySubmittedName = u.[CompanyName]
      ,u.[ModifiedDate]
      ,u.[ModifiedUser]
      ,u.[LastActivationDate]
      ,u.[IsTestRecord]
	  ,r.RoleId
	  ,r.RoleName
	  ,CompanyName = c.Name
	  ,r.RoleCategoryId
	  ,RoleCategoryName = rc.Name
FROM [dbo].[AspNetUsers] u
LEFT JOIN PrimaryRole r ON r.UserId = u.Id
LEFT JOIN dbo.RoleCategory rc ON r.RoleCategoryId = rc.Id
LEFT JOIN dbo.Company c ON u.CompanyId = c.Id