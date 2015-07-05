-- =============================================
-- Author:		Les Farago
-- Create date: 
-- Description:	
-- =============================================
CREATE FUNCTION [dbo].[CalculateBiWeeklyPay]
(
	-- Add the parameters for the function here
	@HourlyRate decimal(18,2)
)
RETURNS decimal(18,2)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @Result decimal(18,2)

	-- Add the T-SQL statements to compute the return value here
	SELECT @Result = (@HourlyRate * 37.5) * 2

	-- Return the result of the function
	RETURN @Result

END