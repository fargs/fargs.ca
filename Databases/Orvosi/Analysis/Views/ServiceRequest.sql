







CREATE VIEW [Analysis].[ServiceRequest]
AS

	SELECT sr.Id
		, sr.ClaimantName
		, [Day] = sr.DueDate
		, StartTime = NULL
		, [Service] = s.Name
		, ServiceCode = s.Code
		, ServiceCategory = sc.Name
		, [State] = lis.[Text]
		, Physician = dbo.GetDisplayName(p.FirstName, p.LastName, p.Title)
		, Company = c.Name
		, ParentCompany = cp.Name
		, a.City
		, i.InvoiceNumber
		, InvoiceDetailId = id.Id
		, id.Rate
		, id.Amount
		, id.Total
		, ServiceCataloguePriceAtTheTime = sr.ServiceCataloguePrice
		, ServiceCataloguePriceCurrent = sca.Price
	FROM dbo.ServiceRequest sr 
	LEFT JOIN dbo.[Service] s ON s.Id = sr.ServiceId
	LEFT JOIN dbo.ServiceCategory sc ON s.ServiceCategoryId = sc.Id
	LEFT JOIN dbo.LookupItem lis ON dbo.GetServiceStatusId2(sr.IsLateCancellation, sr.CancelledDate, sr.IsNoShow) = lis.Id
	LEFT JOIN dbo.Company c ON sr.CompanyId = c.Id
	LEFT JOIN dbo.Company cp ON c.ParentId = cp.Id
	LEFT JOIN dbo.[Address] a ON sr.AddressId = a.Id
	LEFT JOIN dbo.InvoiceDetail id ON sr.Id = id.ServiceRequestId
	LEFT JOIN dbo.Invoice i ON id.InvoiceId = i.Id
	LEFT JOIN dbo.AspNetUsers p ON sr.PhysicianId = p.Id
	LEFT JOIN dbo.ServiceCatalogue sca ON sr.ServiceCatalogueId = sca.Id
	WHERE sr.ServiceId IN (16,17)
	UNION ALL
	-- Includes available slots 
	SELECT 
		  sr.Id
		, sr.ClaimantName
		, [Day] = COALESCE(sr.[AppointmentDate], ad.[Day])
		, [StartTime] = COALESCE(sr.[StartTime], a.[StartTime])
		, sr.[Service]
		, sr.ServiceCode
		, sr.ServiceCategory
		, [State] = CASE WHEN sr.Id IS NULL THEN 'Unused' ELSE sr.[State] END
		, Physician = COALESCE(sr.Physician, dbo.GetDisplayName(p.FirstName, p.LastName, p.Title))
		, Company = COALESCE(sr.Company, c.Name)
		, ParentCompany = COALESCE(sr.ParentCompany, cp.Name)
		, City = COALESCE(sr.City, adr.City)
		, sr.InvoiceNumber
		, sr.InvoiceDetailId
		, sr.Rate
		, sr.Amount
		, sr.Total
		, sr.ServiceCataloguePriceAtTheTime
		, sr.ServiceCataloguePriceCurrent
	FROM dbo.AvailableDay ad
	LEFT JOIN dbo.AvailableSlot a ON ad.Id = a.AvailableDayId
	LEFT JOIN dbo.AspNetUsers p ON ad.PhysicianId = p.Id
	LEFT JOIN dbo.AspNetUserRoles ur ON p.Id = ur.UserId
	LEFT JOIN dbo.Company c ON ad.CompanyId = c.Id
	LEFT JOIN dbo.Company cp ON c.ParentId = cp.Id
	LEFT JOIN dbo.[Address] adr ON ad.LocationId = adr.Id
	LEFT JOIN (
		SELECT sr.Id
			, sr.ClaimantName
			, sr.AppointmentDate
			, sr.StartTime
			, [Service] = s.Name
			, ServiceCode = s.Code
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
			, InvoiceDetailId = id.Id
			, ServiceCataloguePriceAtTheTime = sr.ServiceCataloguePrice
			, ServiceCataloguePriceCurrent = sca.Price
			, sr.LocationId
			, li.[Text]
			, sr.AddressId
			, i.InvoiceNumber
		FROM dbo.ServiceRequest sr 
		LEFT JOIN dbo.[Service] s ON s.Id = sr.ServiceId
		LEFT JOIN dbo.ServiceCategory sc ON s.ServiceCategoryId = sc.Id
		LEFT JOIN dbo.LookupItem lis ON dbo.GetServiceStatusId2(sr.IsLateCancellation, sr.CancelledDate, sr.IsNoShow) = lis.Id
		LEFT JOIN dbo.Company c ON sr.CompanyId = c.Id
		LEFT JOIN dbo.Company cp ON c.ParentId = cp.Id
		LEFT JOIN dbo.[Address] a ON sr.AddressId = a.Id
		LEFT JOIN dbo.InvoiceDetail id ON sr.Id = id.ServiceRequestId
		LEFT JOIN dbo.Invoice i ON id.InvoiceId = i.Id
		LEFT JOIN dbo.AspNetUsers p ON sr.PhysicianId = p.Id
		LEFT JOIN dbo.ServiceCatalogue sca ON sr.ServiceCatalogueId = sca.Id
		LEFT JOIN dbo.LookupItem li ON sr.LocationId = li.Id
		WHERE sr.ServiceId NOT IN (16,17)
	) sr ON sr.AvailableSlotId = a.Id
	WHERE ur.RoleId = '8359141f-e423-4e48-8925-4624ba86245a' -- Include physicians only
	-- NOTE: There are service requests with the same available slot id. This accounts for the more requests than available slots.