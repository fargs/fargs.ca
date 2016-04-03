




-- =============================================
-- Author:		Les Farago
-- Create date: 
-- Description:	
-- =============================================
CREATE FUNCTION [dbo].[GetTaskDueDate] 
(
	-- Add the parameters for the function here
	 @AppointmentDate datetime
	,@ReportDueDate datetime
	,@DueDateBase tinyint
	,@DueDateDiff smallint
)
RETURNS datetime
AS
BEGIN
	IF @DueDateDiff IS NULL BEGIN
		SET @DueDateDiff = 0
	END

	-- Declare the return variable here
	DECLARE @Result datetime

	IF @DueDateBase = 2 BEGIN
		SELECT @Result = DATEADD(d, @DueDateDiff, @ReportDueDate)
	END
	ELSE BEGIN
		SELECT @Result = DATEADD(d, @DueDateDiff, @AppointmentDate)
	END

	-- Return the result of the function
	RETURN @Result

END