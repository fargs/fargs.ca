CREATE PROC [API].[GetBoxTokens]
	@UserId uniqueidentifier
AS
SELECT BoxAccessToken 
	, BoxRefreshToken
FROM dbo.AspNetUsers  
WHERE Id = @UserId