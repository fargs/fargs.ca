
CREATE PROCEDURE [API].[PhysicianLicense_Insert]
	 @PhysicianId uniqueidentifier
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

INSERT INTO dbo.[PhysicianLicense]
(
	 [PhysicianId]
	,[CollegeName]
	,[LongName]
	,[ExpiryDate]
	,[MemberName]
	,[CertificateClass]
	,[IsPrimary]
	,[Preference]
	,[DocumentId]
	,[CountryId]
	,[ProvinceId]
	,[ModifiedUser]
)
VALUES 
(
	 @PhysicianId
	,@CollegeName
	,@LongName
	,@ExpiryDate
	,@MemberName
	,@CertificateClass
	,@IsPrimary
	,@Preference
	,@DocumentId
	,@CountryId
	,@ProvinceId
	,@ModifiedUser
)

DECLARE @Id INT
SELECT @Id = SCOPE_IDENTITY()

SELECT * FROM API.PhysicianLicense WHERE Id = @Id