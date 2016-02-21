
-- =============================================
-- Author:		Les Farago
-- Create date: 
-- Description:	
-- =============================================
CREATE FUNCTION [dbo].[GetInvoiceDetailAmount] 
(
	-- Add the parameters for the function here
	@Price decimal(18, 2),
	@IsNoShow bit,
	@NoShowRate decimal(10,2),
	@IsLateCancellation bit,
	@LateCancellationRate decimal(10,2)
)
RETURNS decimal(18, 2)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @Result decimal(18, 2)

	-- Add the T-SQL statements to compute the return value here
	SET @Result = @Price

	IF (@IsNoShow = 1)
		SET @Result = @Price * @NoShowRate
	ELSE IF (@IsLateCancellation = 1)
		SET @Result = @Price * @LateCancellationRate

	-- Return the result of the function
	RETURN @Result

END