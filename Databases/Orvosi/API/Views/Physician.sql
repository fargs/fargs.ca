










CREATE VIEW [API].[Physician]
AS
SELECT 
	 u.[Id]
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
      ,u.[DisplayName]
      ,u.[EmployeeId]
      ,u.[CompanyId]
      ,u.[CompanySubmittedName]
      ,u.[ModifiedDate]
      ,u.[ModifiedUser]
      ,u.[LastActivationDate]
      ,u.[IsTestRecord]
      ,u.[RoleId]
      ,u.[RoleName]
      ,u.[CompanyName]
      ,u.[RoleCategoryId]
      ,u.[RoleCategoryName]
      ,u.[HourlyRate]
	  ,u.[LogoCssClass]
	,p.Designations
	,p.PrimaryAddressId
	,p.SpecialtyId
	,p.OtherSpecialties
	,p.Pediatrics
	,p.Adolescents
	,p.Adults
	,p.Geriatrics
	,la.LocationName
	,AddressName = la.Name
	,PrimarySpecialtyName = s.ItemText
	,u.ColorCode
	,u.BoxFolderId
	,u.BoxUserId
	,p.BoxAddOnTemplateFolderId
	,p.BoxCaseTemplateFolderId
FROM API.[User] u
LEFT JOIN dbo.Physician p ON u.Id = p.Id
LEFT JOIN API.Location la ON p.PrimaryAddressId = la.Id
LEFT JOIN API.Specialty s ON p.SpecialtyId = s.ItemId
WHERE RoleId = '8359141f-e423-4e48-8925-4624ba86245a' -- Role = Physician