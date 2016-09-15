-- =============================================
-- Author:		Les Farago
-- Create date: 
-- Description:	
-- =============================================
CREATE FUNCTION [dbo].[GetServiceRequestTaskStatus] 
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
	ServiceRequestId int NOT NULL,
	AssignedTo uniqueidentifier NULL,
	TaskType varchar(20) NULL
)
AS
BEGIN
	-- Add the SELECT statement with parameter references here
	INSERT INTO @Records
	SELECT Id
		, TaskStatusId
		, ServiceRequestId
		, AssignedTo
		, t.TaskType
	FROM (
		SELECT t.Id
			, t.TaskStatusId
			, t.ServiceRequestId
			, t.AssignedTo
			, t.TaskType
			, RowNum = ROW_NUMBER() OVER(PARTITION BY t.Id ORDER BY ts.DependentPrecedence)
		FROM (
			SELECT t.Id
				, TaskStatusId = dbo.GetTaskStatusId2(t.CompletedDate, t.IsObsolete, tt.TaskType, c.CompletedDate, c.IsObsolete, ct.TaskType, @Now, sr.AppointmentDate)
				, t.ServiceRequestId
				, t.AssignedTo
				, t.TaskType
			FROM dbo.ServiceRequest sr
			LEFT JOIN dbo.ServiceRequestTask t ON t.ServiceRequestId = sr.Id
			LEFT JOIN dbo.Task tt ON tt.Id = t.TaskId
			LEFT JOIN dbo.ServiceRequestTaskDependent d ON t.Id = d.ParentId
			LEFT JOIN dbo.ServiceRequestTask c ON c.Id = d.ChildId
			LEFT JOIN dbo.Task ct ON ct.Id = c.TaskId
			WHERE t.ServiceRequestId IN (
				SELECT Id FROM @SelectedServiceRequests
			) AND (t.AssignedTo = @AssignedTo OR @AssignedTo IS NULL)
		) t
		LEFT JOIN dbo.TaskStatus ts ON t.TaskStatusId = ts.Id
	) t
	WHERE RowNum = 1
	OPTION (RECOMPILE)

	RETURN
END