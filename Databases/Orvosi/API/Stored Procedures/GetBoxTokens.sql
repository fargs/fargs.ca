CREATE PROC API.GetBoxTokens
	@UserId nvarchar(128)
AS
SELECT BoxAccessToken 
	, BoxRefreshToken
FROM dbo.AspNetUsers  
WHERE Id = @UserId