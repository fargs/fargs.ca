

CREATE PROC [API].[InvoiceDetail_Insert]
	 @InvoiceId int
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

INSERT INTO dbo.InvoiceDetail (
	 [InvoiceId]
	,[Description]
	,[Quantity]
	,[Rate]
	,[Total]
	,[Discount]
	,[Amount]
	,[ModifiedDate]
	,[ModifiedUser]
) VALUES (
	 @InvoiceId
	,@Description
	,@Quantity
	,@Rate
	,@Total
	,@Discount
	,@Amount
	,@Now
	,@ModifiedUser
)

SELECT Id = SCOPE_IDENTITY()