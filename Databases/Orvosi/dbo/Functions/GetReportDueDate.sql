




-- =============================================
-- Author:		Les Farago
-- Create date: 
-- Description:	
-- =============================================
CREATE FUNCTION [dbo].[GetReportDueDate] 
(
	-- Add the parameters for the function here
	@ReportDueDate datetime
	,@AppointmentDate datetime
	,@DueDateDiff smallint
)
RETURNS datetime
AS
BEGIN
	IF @DueDateDiff IS NULL BEGIN
		SET @DueDateDiff = 7
	END

	-- Declare the return variable here
	IF @ReportDueDate IS NULL BEGIN
		SELECT @ReportDueDate = DATEADD(d, 7, @AppointmentDate)
	END

	-- Return the result of the function
	RETURN @ReportDueDate

END