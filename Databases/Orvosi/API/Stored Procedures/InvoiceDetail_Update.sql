

CREATE PROC [API].[InvoiceDetail_Update]
	 @Id int
	,@InvoiceId int
	,@Description nvarchar(256)
	,@Quantity smallint
	,@Rate decimal
	,@Total decimal
	,@Discount decimal
	,@Amount decimal
	,@ModifiedUser nvarchar(100)
AS

DECLARE @Now DATETIME
SET @Now = GETDATE()

UPDATE dbo.InvoiceDetail SET 
	 [InvoiceId] = @InvoiceId
	,[Description] = @Description
	,[Quantity] = @Quantity
	,[Rate] = @Rate
	,[Total] = @Total
	,[Discount] = @Discount
	,[Amount] = @Amount
	,[ModifiedDate] = @Now
	,[ModifiedUser] = @ModifiedUser
WHERE Id = @Id