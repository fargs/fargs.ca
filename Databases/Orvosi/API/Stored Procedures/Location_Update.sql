
CREATE PROCEDURE [API].[Location_Update]
	 @Id int
	,@OwnerGuid uniqueidentifier
	,@AddressTypeID tinyint
	,@Name nvarchar(256)
	,@Attention nvarchar(255)
	,@Address1 nvarchar(255)
	,@Address2 nvarchar(255)
	,@City nvarchar(50)
	,@PostalCode nvarchar(50)
	,@CountryID smallint
	,@ProvinceID smallint
	,@LocationId smallint
	,@ModifiedUser nvarchar(256)
AS

DECLARE @Now DATETIME
SET @Now = GETDATE()

UPDATE dbo.[Address]
SET
	 [OwnerGuid] = @OwnerGuid
	,[AddressTypeID] = @AddressTypeID
	,[Name] = @Name
	,[Attention] = @Attention
	,[Address1] = @Address1
	,[Address2] = @Address2
	,[City] = @City
	,[PostalCode] = @PostalCode
	,[CountryID] = @CountryID
	,[ProvinceID] = @ProvinceID
	,[LocationId] = @LocationId
	,[ModifiedDate] = @Now
	,[ModifiedUser] = @ModifiedUser
WHERE 
	Id = @Id