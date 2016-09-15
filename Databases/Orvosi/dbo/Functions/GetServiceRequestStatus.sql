
-- =============================================
-- Author:		Les Farago
-- Create date: 
-- Description:	
-- =============================================
CREATE FUNCTION [dbo].[GetServiceRequestStatus] 
(	
	-- Add the parameters for the function here
	@Now datetime,
	@SelectedServiceRequests SelectedServiceRequestTVP READONLY,
	@AssignedTo uniqueidentifier
)
RETURNS 
@Records TABLE 
(
	-- Add the column definitions for the TABLE variable here
	Id int NOT NULL, 
	StatusId tinyint NOT NULL,
	IsClosed bit NOT NULL
)
AS
BEGIN
	-- Add the SELECT statement with parameter references here
	INSERT INTO @Records
	SELECT t.Id
		, t.StatusId
		, t.IsClosed
	FROM (	
		SELECT sr.Id
			, StatusId
			, IsClosed = CASE WHEN o.ServiceRequestId IS NULL THEN 1 ELSE 0 END
			, RowNum = ROW_NUMBER() OVER(PARTITION BY sr.Id ORDER BY ts.ServiceRequestPrecedence)
		FROM [dbo].GetServiceRequestTaskStatus(@Now, @SelectedServiceRequests, @AssignedTo) t
		INNER JOIN dbo.ServiceRequestTask srt ON srt.Id = t.Id
		LEFT JOIN dbo.TaskStatus ts ON t.StatusId = ts.Id
		LEFT JOIN dbo.ServiceRequest sr ON srt.ServiceRequestId = sr.Id
		LEFT JOIN [Private].OpenServiceRequestIds o ON srt.ServiceRequestId = o.ServiceRequestId
	) t
	WHERE t.RowNum = 1

	RETURN
END