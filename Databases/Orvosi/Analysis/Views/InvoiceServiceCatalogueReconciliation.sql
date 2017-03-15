
CREATE VIEW [Analysis].[InvoiceServiceCatalogueReconciliation]
AS
SELECT *
FROM Analysis.ServiceRequest
WHERE Amount <> ServiceCataloguePriceCurrent
--ORDER BY Physician, City, [Service], Company