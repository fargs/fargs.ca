
-- =============================================
-- Author:		Les Farago
-- Create date: 
-- Description:	
-- =============================================
CREATE FUNCTION [dbo].[GetServiceRequestTitle] 
(
	@ServiceId tinyint,
	@ServiceRequestId int,
	@AppointmentDate date,
	@DueDate date,
	@StartTime time,
	@LocationShortName nvarchar(128),
	@ServiceCode nvarchar(128),
	@CompanyCode nvarchar(128),
	@PhysicianUserName nvarchar(128),
	@ClaimantName nvarchar(128)
)
RETURNS nvarchar(210)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @Result nvarchar(210)

	-- Add the T-SQL statements to compute the return value here
	IF (@ServiceId IN (16,17)) -- Paper Review or Addendum
		SET @Result = CONVERT(VARCHAR(10), @DueDate, 126) + ' ' + @ClaimantName + ' (' + @ServiceCode + '-' + @PhysicianUserName + ') ' + @CompanyCode + '-' + CONVERT(nvarchar(10), @ServiceRequestId)
	ELSE
		SET @Result = CONVERT(VARCHAR(10), @AppointmentDate, 126) + '(' + LEFT(REPLACE(CONVERT(nvarchar(10), @StartTime), ':', ''),4) + ')-' + @LocationShortName + '-' + @ClaimantName + ' (' + @ServiceCode + '-' + @PhysicianUserName + ') ' + @CompanyCode + '-' + CONVERT(nvarchar(10), @ServiceRequestId)

	-- Return the result of the function
	RETURN @Result

END