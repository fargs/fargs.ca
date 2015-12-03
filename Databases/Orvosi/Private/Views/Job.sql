

CREATE VIEW [Private].[Job]
AS
SELECT 
	  JobId = j.Id
	, UserId = u.Id
	, u.UserName
	, FullName = u.FirstName + ' ' + u.LastName
	, RoleName = r.Name
	, [FileName] = f.Name
	, CompanyName = c.Name
	, ParentCompanyName = pc.Name
	, j.DueDate
	, j.StartTime
	, j.EndTime
	, j.Price
	, [Status] = s.Name
FROM dbo.AspNetUsers u
INNER JOIN dbo.AspNetUserRoles ur ON u.Id = ur.UserId
INNER JOIN dbo.AspNetRoles r ON ur.RoleId = r.Id
INNER JOIN dbo.[Case] f ON u.Id = f.PhysicianId
INNER JOIN dbo.Company c ON f.CompanyId = c.Id
INNER JOIN dbo.Company pc ON c.ParentId = pc.Id
INNER JOIN dbo.Job j ON j.FileId = f.Id
INNER JOIN (
	SELECT jt.JobId
		, JobStatusId = MIN(jt.StatusId)
	FROM dbo.JobTask jt
	WHERE jt.IsMilestone = 0
	GROUP BY jt.JobId
) jt ON j.Id = jt.JobId
INNER JOIN dbo.[Status] s ON jt.JobStatusId = s.Id