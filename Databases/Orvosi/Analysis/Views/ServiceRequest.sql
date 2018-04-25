






CREATE VIEW [Analysis].[ServiceRequest]
AS
	WITH InvoiceDetailCte
	AS (
		SELECT InvoiceDetailId = id.Id
			, id.ServiceRequestId
			, id.InvoiceId
			, InvoiceDetailRate = id.Rate
			, InvoiceDetailAmount = id.Amount
			, InvoiceDetailTotal = id.Total
			, InvoiceDetailCount = c.DetailCount
			, i.InvoiceNumber
			, InvoiceSubTotal = i.SubTotal
			, InvoiceHst = i.Hst
			, InvoiceTotal = i.Total
		FROM dbo.InvoiceDetail id
		INNER JOIN dbo.Invoice i ON id.InvoiceId = i.Id
		LEFT JOIN (
			SELECT id.InvoiceId, DetailCount = COUNT(id.Id)
			FROM dbo.InvoiceDetail id
			GROUP BY id.InvoiceId
		) c ON i.Id = c.InvoiceId
		WHERE id.ServiceRequestId IS NOT NULL 
			AND i.IsDeleted = 0
	),
	ServiceRequestCte
	AS (
		SELECT ServiceRequestId = sr.Id
			, sr.ClaimantName
			, sr.AvailableSlotId
			, [AppointmentDate] = sr.AppointmentDate
			, StartTime = sr.StartTime
			, DueDate = sr.DueDate
			, [Service] = s.Name
			, ServiceCode = s.Code
			, ServiceCategory = sc.Name
			, [Status] = lis.[Text]
			, Physician = p.LastName
			, Company = c.Name
			, ParentCompany = cp.Name
			, City = ci.Name
			, Province = pr.ProvinceName
			, HasAppointment = CASE WHEN sr.AppointmentDate IS NOT NULL THEN 1 ELSE 0 END
			, HasAddress = CASE WHEN sr.AddressId IS NOT NULL THEN 1 ELSE 0 END
			, ReferralSource = SourceCompany
			, MedicolegalType = mt.Name
		FROM dbo.ServiceRequest sr 
		LEFT JOIN dbo.[Service] s ON s.Id = sr.ServiceId
		LEFT JOIN dbo.ServiceCategory sc ON s.ServiceCategoryId = sc.Id
		LEFT JOIN dbo.LookupItem lis ON dbo.GetServiceStatusId2(sr.IsLateCancellation, sr.CancelledDate, sr.IsNoShow) = lis.Id
		LEFT JOIN dbo.Company c ON sr.CompanyId = c.Id
		LEFT JOIN dbo.Company cp ON c.ParentId = cp.Id
		LEFT JOIN dbo.[Address] a ON sr.AddressId = a.Id
		LEFT JOIN dbo.City ci ON a.CityId = ci.Id
		LEFT JOIN dbo.Province pr ON pr.Id = ci.ProvinceId
		LEFT JOIN dbo.AspNetUsers p ON sr.PhysicianId = p.Id
		LEFT JOIN dbo.MedicolegalType mt ON sr.MedicolegalTypeId = mt.Id
	),
	AvailableSlotCte
	AS (
		SELECT AvailableSlotId = a.Id
			, AvailableSlotStartTime = a.StartTime
			, AvailableSlotRequestCount = RequestCount
			, AvailableDay = ad.Day
			, AvailableDayPhysician = p.LastName
			, AvailableDayCompany = c.Name
			, AvailableDayCity = ci.Name
			, AvailableDayProvince = pr.ProvinceName
		FROM dbo.AvailableDay ad
		INNER JOIN dbo.AvailableSlot a ON ad.Id = a.AvailableDayId
		LEFT JOIN dbo.AspNetUsers p ON ad.PhysicianId = p.Id
		LEFT JOIN dbo.AspNetUserRoles ur ON p.Id = ur.UserId
		LEFT JOIN dbo.Company c ON ad.CompanyId = c.Id
		LEFT JOIN dbo.Company cp ON c.ParentId = cp.Id
		LEFT JOIN dbo.[Address] adr ON ad.LocationId = adr.Id
		LEFT JOIN dbo.City ci ON adr.CityId = ci.Id
		LEFT JOIN dbo.Province pr ON ci.ProvinceId = pr.Id
		LEFT JOIN (
			SELECT sr.AvailableSlotId, RequestCount = COUNT(sr.Id)
			FROM dbo.ServiceRequest sr
			WHERE CancelledDate IS NULL
			GROUP BY sr.AvailableSlotId
		) src ON a.Id = src.AvailableSlotId
	)
	SELECT DateSlicer = COALESCE(sr.AppointmentDate, a.AvailableDay, sr.DueDate)
		, a.AvailableSlotId
		, a.AvailableSlotStartTime
		, a.AvailableDay
		, a.AvailableDayPhysician
		, a.AvailableDayCompany
		, a.AvailableDayCity
		, a.AvailableDayProvince
		, a.AvailableSlotRequestCount
		, AvailableSlotStatus = CASE WHEN a.AvailableSlotRequestCount IS NOT NULL AND a.AvailableSlotRequestCount > 0 THEN 'Filled' ELSE 'Not Filled' END
		, sr.ServiceRequestId
		, sr.ClaimantName
		, sr.AppointmentDate
		, sr.StartTime
		, sr.DueDate
		, sr.City
		, sr.Province
		, sr.Company
		, sr.ParentCompany
		, sr.Physician
		, sr.Service
		, sr.ServiceCode
		, sr.ServiceCategory
		, sr.HasAppointment
		, sr.HasAddress
		, sr.Status
		, id.InvoiceDetailId
		, id.InvoiceDetailAmount
		, id.InvoiceDetailRate
		, id.InvoiceDetailTotal
		, id.InvoiceDetailCount
		, id.InvoiceId
		, id.InvoiceNumber
		, id.InvoiceSubTotal
		, id.InvoiceHst
		, id.InvoiceTotal
		, sr.ReferralSource
		, sr.MedicolegalType
	FROM AvailableSlotCte a
	FULL OUTER JOIN ServiceRequestCte sr ON a.AvailableSlotId = sr.AvailableSlotId
	LEFT JOIN InvoiceDetailCte id ON sr.ServiceRequestId = id.ServiceRequestId