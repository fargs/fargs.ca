/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

:r .\Data\Script.Data.PostDeployment.Static.sql
:r .\Data\Script.Data.PostDeployment.Config.sql
:r .\Data\Script.Data.PostDeployment.Test.sql

-- =================================================
-- Create User as DBO template for Windows Azure SQL Database
-- =================================================
-- For login orvosi_webagent, create a user in the database
CREATE USER orvosi_webagent
	FOR LOGIN orvosi_webagent
	WITH DEFAULT_SCHEMA = dbo
GO

-- Add user to the database owner role
EXEC sp_addrolemember N'db_owner', N'orvosi_webagent'
GO