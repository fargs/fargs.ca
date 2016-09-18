



CREATE VIEW [Analysis].[ServiceRequest]
AS

	SELECT sr.Id
		, [Day] = sr.DueDate
		, sr.StartTime
		, [Service] = s.Name
		, ServiceCode = s.Code
		, ServiceCategory = sc.Name
		, [State] = lis.[Text]
		, Physician = dbo.GetDisplayName(p.FirstName, p.LastName, p.Title)
		, CompanyName = c.Name
		, ParentCompanyName = cp.Name
		, a.City
		, id.Rate
		, id.Amount
		, id.Total
	FROM dbo.ServiceRequest sr 
	LEFT JOIN dbo.[Service] s ON s.Id = sr.ServiceId
	LEFT JOIN dbo.ServiceCategory sc ON s.ServiceCategoryId = sc.Id
	LEFT JOIN dbo.LookupItem lis ON dbo.GetServiceStatusId(sr.IsLateCancellation, sr.CancelledDate, sr.IsNoShow) = lis.Id
	LEFT JOIN dbo.Company c ON sr.CompanyId = c.Id
	LEFT JOIN dbo.Company cp ON c.ParentId = cp.Id
	LEFT JOIN dbo.[Address] a ON sr.AddressId = a.Id
	LEFT JOIN dbo.InvoiceDetail id ON sr.Id = id.ServiceRequestId
	LEFT JOIN dbo.AspNetUsers p ON sr.PhysicianId = p.Id
	WHERE ServiceId IN (16,17)
	UNION ALL
	SELECT 
		  sr.Id
		, [Day] = COALESCE(sr.[AppointmentDate], ad.[Day])
		, StartTime = COALESCE(sr.StartTime, a.StartTime)
		, sr.[Service]
		, sr.ServiceCode
		, sr.ServiceCategory
		, [State] = CASE WHEN sr.Id IS NULL THEN 'Unused' ELSE sr.[State] END
		, Physician = COALESCE(sr.Physician, dbo.GetDisplayName(p.FirstName, p.LastName, p.Title))
		, Company = COALESCE(sr.Company, c.Name)
		, ParentCompany = COALESCE(sr.ParentCompany, cp.Name)
		, City = COALESCE(sr.City, adr.City)
		, sr.Rate
		, sr.Amount
		, sr.Total
	FROM dbo.AvailableDay ad
	LEFT JOIN dbo.AvailableSlot a ON ad.Id = a.AvailableDayId
	LEFT JOIN dbo.AspNetUsers p ON ad.PhysicianId = p.Id
	LEFT JOIN dbo.AspNetUserRoles ur ON p.Id = ur.UserId
	LEFT JOIN dbo.Company c ON ad.CompanyId = c.Id
	LEFT JOIN dbo.Company cp ON c.ParentId = cp.Id
	LEFT JOIN dbo.[Address] adr ON ad.LocationId = adr.Id
	LEFT JOIN (
		SELECT sr.Id, sr.AppointmentDate, sr.StartTime, [Service] = s.Name, ServiceCode = s.Code
			, Physician = dbo.GetDisplayName(p.FirstName, p.LastName, p.Title)
			, ServiceCategory = sc.Name
			, [State] = lis.[Text]
			, Company = c.Name
			, ParentCompany = cp.Name
			, a.City
			, id.Rate
			, id.Amount
			, id.Total
			, sr.AvailableSlotId
		FROM dbo.ServiceRequest sr 
		LEFT JOIN dbo.[Service] s ON s.Id = sr.ServiceId
		LEFT JOIN dbo.ServiceCategory sc ON s.ServiceCategoryId = sc.Id
		LEFT JOIN dbo.LookupItem lis ON dbo.GetServiceStatusId(sr.IsLateCancellation, sr.CancelledDate, sr.IsNoShow) = lis.Id
		LEFT JOIN dbo.Company c ON sr.CompanyId = c.Id
		LEFT JOIN dbo.Company cp ON c.ParentId = cp.Id
		LEFT JOIN dbo.[Address] a ON sr.AddressId = a.Id
		LEFT JOIN dbo.InvoiceDetail id ON sr.Id = id.ServiceRequestId
		LEFT JOIN dbo.AspNetUsers p ON sr.PhysicianId = p.Id
		WHERE ServiceId NOT IN (16,17)
	) sr ON sr.AvailableSlotId = a.Id
	WHERE ur.RoleId = '8359141f-e423-4e48-8925-4624ba86245a' -- Include physicians only
	-- NOTE: There are service requests with the same available slot id. This accounts for the more requests than available slots.