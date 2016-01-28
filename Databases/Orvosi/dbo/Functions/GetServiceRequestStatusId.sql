

-- =============================================
-- Author:		Les Farago
-- Create date: 
-- Description:	
-- =============================================
CREATE FUNCTION [dbo].[GetServiceRequestStatusId] 
(
	-- Add the parameters for the function here
	@OpenTasks tinyint
)
RETURNS tinyint
AS
BEGIN
	-- Declare the return variable here
	DECLARE @Result tinyint

	-- Add the T-SQL statements to compute the return value here
	SELECT @Result = CASE WHEN @OpenTasks = 0 THEN 11 ELSE 10 END

	-- Return the result of the function
	RETURN @Result

END