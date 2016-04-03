-- =============================================
-- Author:		Les Farago
-- Create date: 
-- Description:	
-- =============================================
CREATE FUNCTION [dbo].[GetInitials] 
(
	-- Add the parameters for the function here
	@FirstName nvarchar(100),
	@LastName nvarchar(100)
)
RETURNS nvarchar(4)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @Result nvarchar(210)

	-- Add the T-SQL statements to compute the return value here
	IF (ISNULL(@FirstName, '') <> '')
		SET @Result = LEFT(@FirstName, 1)

	IF (ISNULL(@LastName, '') <> '') 
		SET @Result = ISNULL(@Result,'') + LEFT(@LastName,1)

	-- Return the result of the function
	RETURN @Result

END