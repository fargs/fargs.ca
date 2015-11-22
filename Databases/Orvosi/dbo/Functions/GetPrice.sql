
-- =============================================
-- Author:		Les Farago
-- Create date: 
-- Description:	
-- =============================================
CREATE FUNCTION [dbo].[GetPrice] 
(
	-- Add the parameters for the function here
	@ServiceDefault decimal(18, 2),
	@CompanyOverride decimal(18, 2),
	@JobOverride decimal(18, 2)
)
RETURNS decimal(18, 2)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @Result decimal(18, 2)

	-- Add the T-SQL statements to compute the return value here
	SET @Result = @ServiceDefault

	IF (@CompanyOverride IS NOT NULL)
		SET @Result = @CompanyOverride

	IF (@JobOverride IS NOT NULL)
		SET @Result = @JobOverride

	-- Return the result of the function
	RETURN @Result

END