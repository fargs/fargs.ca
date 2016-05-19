CREATE PROC API.SaveBoxTokens
	@AccessToken nvarchar(128)
	, @RefreshToken nvarchar(128)
	, @UserId nvarchar(128)
AS
UPDATE dbo.AspNetUsers 
SET BoxAccessToken = @AccessToken
	, BoxRefreshToken = @RefreshToken
WHERE Id = @UserId