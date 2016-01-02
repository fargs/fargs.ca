
CREATE PROCEDURE [API].[Location_Insert]
	 @OwnerGuid uniqueidentifier
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

INSERT INTO dbo.[Address]
(
	 [OwnerGuid]
	,[AddressTypeID]
	,[Name]
	,[Attention]
	,[Address1]
	,[Address2]
	,[City]
	,[PostalCode]
	,[CountryID]
	,[ProvinceID]
	,[LocationId]
	,[ModifiedUser]
)
VALUES 
(
	 @OwnerGuid
	,@AddressTypeID
	,@Name
	,@Attention
	,@Address1
	,@Address2
	,@City
	,@PostalCode
	,@CountryID
	,@ProvinceID
	,@LocationId
	,@ModifiedUser
)

DECLARE @Id INT
SELECT @Id = SCOPE_IDENTITY()

SELECT * FROM API.Location WHERE Id = @Id