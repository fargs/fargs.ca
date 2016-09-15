




-- =============================================
-- Author:		Les Farago
-- Create date: 
-- Description:	
-- =============================================
CREATE FUNCTION [dbo].[GetTaskStatusId2] 
(
	-- Add the parameters for the function here
	@CompletedDate datetime,
	@IsObsolete bit,
	@TaskType varchar(20),
	@DependentCompletedDate datetime,
	@DependentIsObsolete bit,
	@DependentTaskType varchar(20),
	@Now DATETIME,
	@AppointmentDate DATETIME
)
RETURNS tinyint
AS
BEGIN
	-- Declare the return variable here
	DECLARE @Result tinyint

	-- Add the T-SQL statements to compute the return value here
	SELECT @Result 
		= CASE 
			WHEN (@CompletedDate IS NOT NULL) 
					OR (@TaskType = 'EVENT' AND @Now >= @AppointmentDate) THEN 3 -- Done
			WHEN @IsObsolete = 1 THEN 4 -- Is Obsolete
			WHEN (@DependentTaskType IS NULL AND @DependentCompletedDate IS NULL AND @DependentIsObsolete = 0)
					OR (@DependentTaskType = 'EVENT' AND @Now < @AppointmentDate)
					OR (@TaskType = 'EVENT' AND @Now < @AppointmentDate)
				THEN 1 -- Waiting
			ELSE 2 -- ToDo
		END

	-- Return the result of the function
	RETURN @Result

END