
CREATE PROCEDURE [API].[GetNextInvoiceNumber]
AS
	SELECT NextInvoiceNumber = CONVERT(NVARCHAR(50), NEXT VALUE FOR dbo.InvoiceNumberSequence)
RETURN 0