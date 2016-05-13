UPDATE Invoice SET TaxRateHST = .05 WHERE CustomerProvince = 'British Columbia'

UPDATE i SET
	SubTotal = st.SubTotal
	, Hst = i.TaxRateHst * st.SubTotal
	, Total = st.SubTotal + (i.TaxRateHst * st.SubTotal)
FROM dbo.Invoice i
INNER JOIN (
	SELECT InvoiceID, SubTotal = SUM(Total)
	FROM dbo.InvoiceDetail id
	GROUP BY InvoiceID
) st ON i.Id = st.InvoiceId

DELETE FROM Invoice WHERE Hst IS NULL
SELECT * FROM InvoiceDetail WHERE InvoiceId IN (
	SELECT Id FROM Invoice WHERE Hst IS NULL
)

