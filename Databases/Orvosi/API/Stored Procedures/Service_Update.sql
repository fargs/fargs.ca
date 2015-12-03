CREATE PROC [API].[Service_Update]
	 @Id smallint
	,@ObjectGuid uniqueidentifier
	,@Name nvarchar(128)
	,@Description nvarchar(2000)
	,@Code nvarchar(10)
	,@Price decimal(18,2)
	,@ServiceCategoryId smallint
	,@ServicePortfolioId smallint
	,@ModifiedUser nvarchar(100)
AS

DECLARE @Now DATETIME
SET @Now = GETDATE()

UPDATE dbo.[Service]
SET 
	 [Name] = @Name
	,[Description] = @Description
	,[Code] = @Code
	,[ServiceCategoryId] = @ServiceCategoryId
	,[ServicePortfolioId] = @ServicePortfolioId
	,[ModifiedDate] = @Now
	,[ModifiedUser] = @ModifiedUser
WHERE [Id] = @Id OR [ObjectGuid] = @ObjectGuid

MERGE INTO dbo.Price AS target
USING (SELECT @ObjectGuid, @Price) AS source (ObjectGuid, Price)
ON (target.ObjectGuid = source.ObjectGuid)
WHEN MATCHED THEN
	UPDATE SET Price = source.Price
WHEN NOT MATCHED THEN
	INSERT (ObjectGuid, Price)
	VALUES (source.ObjectGuid, source.Price);