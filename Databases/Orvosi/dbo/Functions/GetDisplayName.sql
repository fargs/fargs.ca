-- =============================================
-- Author:		Les Farago
-- Create date: 
-- Description:	
-- =============================================
CREATE FUNCTION [dbo].[GetDisplayName] 
(
	-- Add the parameters for the function here
	@FirstName nvarchar(100),
	@LastName nvarchar(100),
	@Title nvarchar(10)
)
RETURNS nvarchar(210)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @Result nvarchar(210)

	-- Add the T-SQL statements to compute the return value here
	IF (@Title <> '')
		SET @Result = @Title

	IF (@FirstName <> '')
		SET @Result = @Result + ' ' + @FirstName

	IF (@LastName <> '')
		SET @Result = @Result + ' ' + @LastName

	-- Return the result of the function
	RETURN @Result

END