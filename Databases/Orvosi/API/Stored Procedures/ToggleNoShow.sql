
CREATE PROC [API].[ToggleNoShow]
	@Id INT
AS

DECLARE @IsNoShow BIT

SELECT @IsNoShow = CASE WHEN IsNoShow = 1 THEN 0 ELSE 1 END
FROM dbo.ServiceRequest
WHERE Id = @Id

UPDATE dbo.ServiceRequest SET IsNoShow = @IsNoShow
WHERE Id = @Id

UPDATE dbo.ServiceRequestTask SET IsObsolete = CASE WHEN @IsNoShow = 1 THEN 1 ELSE 0 END
WHERE ServiceRequestId = @Id 
	AND TaskId <> 24 
	AND TaskId <> 30
	AND CompletedDate IS NULL