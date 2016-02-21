

CREATE PROC [API].[Invoice_Insert]
	 @InvoiceNumber nvarchar(128)
	,@InvoiceDate datetime
	,@Currency nvarchar(128)
	,@Terms nvarchar(128)
	,@DueDate datetime
	,@CompanyGuid uniqueidentifier
	,@CompanyName nvarchar(128)
	,@Email nvarchar(128)
	,@PhoneNumber nvarchar(128)
	,@Address1 nvarchar(128)
	,@Address2 nvarchar(128)
	,@Address3 nvarchar(128)
	,@BillToGuid uniqueidentifier
	,@BillToName nvarchar(128)
	,@BillToAddress1 nvarchar(128)
	,@BillToAddress2 nvarchar(128)
	,@BillToAddress3 nvarchar(128)
	,@BillToEmail nvarchar(128)
	,@SubTotal decimal
	,@TaxRateHst decimal
	,@Discount decimal
	,@Total decimal
	,@PaymentReceivedDate datetime
	,@ModifiedUser nvarchar(100)
AS

DECLARE @Now DATETIME
SET @Now = GETDATE()

INSERT INTO dbo.[Invoice]
(
	 [InvoiceNumber]
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
)
VALUES
(
	 @InvoiceNumber
	,@InvoiceDate
	,@Currency
	,@Terms
	,@DueDate
	,@CompanyGuid
	,@CompanyName
	,@Email
	,@PhoneNumber
	,@Address1
	,@Address2
	,@Address3
	,@BillToGuid
	,@BillToName
	,@BillToAddress1
	,@BillToAddress2
	,@BillToAddress3
	,@BillToEmail
	,@SubTotal
	,@TaxRateHst
	,@Discount
	,@Total
	,@PaymentReceivedDate
	,@Now
	,@ModifiedUser
)

SELECT Id = SCOPE_IDENTITY()