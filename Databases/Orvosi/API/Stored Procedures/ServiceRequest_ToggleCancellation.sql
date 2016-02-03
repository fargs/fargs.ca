
CREATE PROC API.ServiceRequest_ToggleCancellation
	@Id INT
	,@CancelledDate DATETIME
	,@IsLateCancellation BIT
	,@Notes NVARCHAR(2000)
AS

UPDATE dbo.ServiceRequest 
SET IsLateCancellation = @IsLateCancellation, CancelledDate = @CancelledDate, Notes = Notes + '\n' + @Notes
WHERE Id = @Id

UPDATE dbo.ServiceRequestTask SET IsObsolete = CASE WHEN @CancelledDate IS NOT NULL THEN 1 ELSE 0 END
	WHERE ServiceRequestId = @Id 
		AND CompletedDate IS NULL

IF @IsLateCancellation = 1 BEGIN
	UPDATE dbo.ServiceRequestTask SET IsObsolete = 0 WHERE ServiceRequestId = @Id AND TaskId = 24
END