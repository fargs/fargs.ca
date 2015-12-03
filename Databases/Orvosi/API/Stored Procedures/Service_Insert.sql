CREATE PROC [API].[Service_Insert]
	 @Name nvarchar(128)
	,@Description nvarchar(2000)
	,@Code nvarchar(10)
	,@Price decimal(18,2)
	,@ServiceCategoryId smallint
	,@ServicePortfolioId smallint
	,@ModifiedUser nvarchar(100)
AS

DECLARE @Now DATETIME
SET @Now = GETDATE()

INSERT INTO dbo.[Service]
(
	 [Name]
	,[Description]
	,[Code]
	,[ServiceCategoryId]
	,[ServicePortfolioId]
	,[ModifiedDate]
	,[ModifiedUser]
)
VALUES 
(
	 @Name
	,@Description
	,@Code
	,@ServiceCategoryId
	,@ServicePortfolioId
	,@Now
	,@ModifiedUser
)

DECLARE @ObjectGuid uniqueidentifier
SELECT @ObjectGuid = ObjectGuid
FROM dbo.[Service] 
WHERE Id = SCOPE_IDENTITY()

INSERT INTO dbo.Price 
(
	 [ObjectGuid]
	,[Price]
	,[ModifiedUser] 
)
VALUES 
(
	 @ObjectGuid
	,@Price
	,@ModifiedUser
)