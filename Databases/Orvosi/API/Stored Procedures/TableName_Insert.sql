
CREATE PROC [API].[TableName_Insert]
	 @Id tinyint
AS

DECLARE @Now DATETIME
SET @Now = GETDATE()

INSERT INTO dbo.[Temp]
(
	[Id]
)
VALUES
(
	@Id
)