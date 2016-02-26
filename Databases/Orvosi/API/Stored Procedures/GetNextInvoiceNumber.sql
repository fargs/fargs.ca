
CREATE PROCEDURE [API].[GetNextInvoiceNumber]
AS
	SELECT NextInvoiceNumber = NEXT VALUE FOR dbo.InvoiceNumberSequence
RETURN 0