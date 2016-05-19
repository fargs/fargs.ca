CREATE PROC API.GetCompanyProvince
	@CompanyId INT
AS

SELECT DISTINCT a.ProvinceID, p.ProvinceName
FROM dbo.Company c
LEFT JOIN dbo.[Address] a ON c.ObjectGuid = a.OwnerGuid
INNER JOIN dbo.Province p ON a.ProvinceID = p.Id
WHERE c.Id = @CompanyId