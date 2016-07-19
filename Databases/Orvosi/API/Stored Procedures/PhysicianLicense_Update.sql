
CREATE PROCEDURE [API].[PhysicianLicense_Update]
	 @Id smallint
	,@PhysicianId uniqueidentifier
	,@CollegeName nvarchar(128)
	,@LongName nvarchar(2000)
	,@ExpiryDate date
	,@MemberName nvarchar(256)
	,@CertificateClass nvarchar(128)
	,@IsPrimary bit
	,@Preference tinyint
	,@DocumentId smallint
	,@CountryId smallint
	,@ProvinceId smallint
	,@ModifiedUser nvarchar(100)
AS

DECLARE @Now DATETIME
SET @Now = GETDATE()

UPDATE dbo.[PhysicianLicense]
SET
	 [PhysicianId] = @PhysicianId
	,[CollegeName] = @CollegeName
	,[LongName] = @LongName
	,[ExpiryDate] = @ExpiryDate
	,[MemberName] = @MemberName
	,[CertificateClass] = @CertificateClass
	,[IsPrimary] = @IsPrimary
	,[Preference] = @Preference
	,[DocumentId] = @DocumentId
	,[CountryId] = @CountryId
	,[ProvinceId] = @ProvinceId
	,[ModifiedDate] = @Now
	,[ModifiedUser] = @ModifiedUser
WHERE 
	Id = @Id