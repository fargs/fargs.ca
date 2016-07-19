



-- =============================================
-- Author:		Les Farago
-- Create date: 
-- Description:	
-- =============================================
CREATE FUNCTION [dbo].[GetTaskStatusId] 
(
	-- Add the parameters for the function here
	@CompletedDate datetime,
	@IsObsolete bit,
	@DependentCompletedDate datetime,
	@DependentIsObsolete bit,
	@Now DATETIME,
	@AppointmentDate DATETIME,
	@IsDependentOnExamDate bit
)
RETURNS tinyint
AS
BEGIN
	-- Declare the return variable here
	DECLARE @Result tinyint

	-- Add the T-SQL statements to compute the return value here
	SELECT @Result 
		= CASE 
			WHEN @CompletedDate IS NOT NULL THEN 101 -- Completed
			WHEN @IsObsolete = 1 THEN 103 -- Is Obsolete
			WHEN @IsDependentOnExamDate = 1 AND @Now < @AppointmentDate
				THEN 100 -- Waiting
			WHEN @DependentCompletedDate IS NULL AND @DependentIsObsolete = 0
				THEN 100 -- Waiting
			ELSE 102 -- Active
		END

	-- Return the result of the function
	RETURN @Result

END