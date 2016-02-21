
CREATE VIEW API.Invoice
AS

SELECT 
	 [Id]
	,[InvoiceNumber]
	,[InvoiceDate]
	,[Currency]
	,[Terms]
	,[DueDate]
	,[CompanyGuid]
	,[CompanyName]
	,[Email]
	,[PhoneNumber]
	,[Address1]
	,[Address2]
	,[Address3]
	,[BillToGuid]
	,[BillToName]
	,[BillToAddress1]
	,[BillToAddress2]
	,[BillToAddress3]
	,[BillToEmail]
	,[SubTotal]
	,[TaxRateHst]
	,[Discount]
	,[Total]
	,[PaymentReceivedDate]
	,[ModifiedDate]
	,[ModifiedUser]
FROM dbo.Invoice i