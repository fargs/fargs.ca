

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
	@IsNoShow bit,
	@OpenServiceTasks tinyint
)
RETURNS tinyint
AS
BEGIN
	-- Declare the return variable here
	DECLARE @Result tinyint

	-- Add the T-SQL statements to compute the return value here
	SELECT @Result = CASE WHEN @IsLateCancellation = 1 THEN 32 WHEN @CancelledDate IS NOT NULL THEN 31 WHEN @IsNoShow = 1 THEN 30 WHEN @OpenServiceTasks = 0 THEN 36 ELSE NULL END

	-- Return the result of the function
	RETURN @Result

END