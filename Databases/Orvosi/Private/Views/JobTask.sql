
CREATE VIEW [Private].[JobTask]
AS
SELECT 
	  UserId = u.Id
	, u.UserName
	, FullName = u.FirstName + ' ' + u.LastName
	, RoleName = r.Name
	, jt.JobId
	, jt.IsMilestone
	, DueDate = ISNULL(jt.DueDate, DATEADD(d, jt.DurationFromDueDateInDays, j.DueDate))
	, jt.StartTime
	, jt.EndTime
	, jt.IsBillable
	, b.[Hours]
	, rl.HourlyRate
	, Cost = b.[Hours] * rl.HourlyRate
	, [Status] = s.Name
FROM dbo.AspNetUsers u
INNER JOIN dbo.AspNetUserRoles ur ON u.Id = ur.UserId
INNER JOIN dbo.AspNetRoles r ON ur.RoleId = r.Id
INNER JOIN dbo.[JobTask] jt ON u.Id = jt.EmployeeId
INNER JOIN dbo.[Job] j ON jt.JobId = j.Id
LEFT JOIN dbo.BillableHourCategory b ON jt.BillableHourCategoryId = b.Id
LEFT JOIN dbo.RoleLevel rl ON u.RoleLevelId = rl.Id
LEFT JOIN dbo.[Status] s ON jt.StatusId = s.Id