

-- =============================================
-- Author:		Les Farago
-- Create date: 
-- Description:	
-- =============================================
CREATE FUNCTION [dbo].[GetServiceStatusId] 
(
	-- Add the parameters for the function here
	@IsLateCancellation bit,
	@CancelledDate datetime,
	@IsNoShow bit
)
RETURNS tinyint
AS
BEGIN
	-- Declare the return variable here
	DECLARE @Result tinyint

	-- Add the T-SQL statements to compute the return value here
	SELECT @Result = CASE WHEN @IsLateCancellation = 1 THEN 32 WHEN @CancelledDate IS NOT NULL THEN 31 WHEN @IsNoShow = 1 THEN 30 END

	-- Return the result of the function
	RETURN @Result

END