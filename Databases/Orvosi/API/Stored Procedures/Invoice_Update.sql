

CREATE PROC [API].[Invoice_Update]
	 @Id int
	,@InvoiceNumber nvarchar(128)
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

UPDATE dbo.Invoice SET
	 [InvoiceNumber] = @InvoiceNumber
	,[InvoiceDate] = @InvoiceDate
	,[Currency] = @Currency
	,[Terms] = @Terms
	,[DueDate] = @DueDate
	,[CompanyGuid] = @CompanyGuid
	,[CompanyName] = @CompanyName
	,[Email] = @Email
	,[PhoneNumber] = @PhoneNumber
	,[Address1] = @Address1
	,[Address2] = @Address2
	,[Address3] = @Address3
	,[BillToGuid] = @BillToGuid
	,[BillToName] = @BillToName
	,[BillToAddress1] = @BillToAddress1
	,[BillToAddress2] = @BillToAddress2
	,[BillToAddress3] = @BillToAddress3
	,[BillToEmail] = @BillToEmail
	,[SubTotal] = @SubTotal
	,[TaxRateHst] = @TaxRateHst
	,[Discount] = @Discount
	,[Total] = @Total
	,[PaymentReceivedDate] = @PaymentReceivedDate
	,[ModifiedDate] = @Now
	,[ModifiedUser] = @ModifiedUser
WHERE Id = @Id