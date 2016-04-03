/*
This script was created by Visual Studio on 2016-04-02 at 10:52 PM.
Run this script on orvosi.database.windows.net.Orvosi (orvosi-sqladmin) to make it the same as (localdb)\ProjectsV12.Orvosi (DESKTOP-PJSAQN9\faragol).
This script performs its actions in the following order:
1. Disable foreign-key constraints.
2. Perform DELETE commands. 
3. Perform UPDATE commands.
4. Perform INSERT commands.
5. Re-enable foreign-key constraints.
Please back up your target database before running this script.
*/
SET NUMERIC_ROUNDABORT OFF
GO
SET XACT_ABORT, ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
/*Pointer used for text / image updates. This might not be needed, but is declared here just in case*/
DECLARE @pv binary(16)
BEGIN TRANSACTION
ALTER TABLE [dbo].[Document] DROP CONSTRAINT [FK_Document_DocumentTemplate]
ALTER TABLE [dbo].[Address] DROP CONSTRAINT [FK_Address_Countries]
ALTER TABLE [dbo].[Address] DROP CONSTRAINT [FK_Address_Provinces]
ALTER TABLE [dbo].[AspNetUserLogins] DROP CONSTRAINT [FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId]
ALTER TABLE [dbo].[AspNetUserClaims] DROP CONSTRAINT [FK_dbo.AspNetUserClaims_dbo.AspNetUsers_UserId]
ALTER TABLE [dbo].[AspNetUserRoles] DROP CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId]
ALTER TABLE [dbo].[AspNetUserRoles] DROP CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId]
UPDATE [dbo].[Task] SET [IsBillable]=0 WHERE [Id]=1
UPDATE [dbo].[Task] SET [Name]=N'Draft report', [DependsOn]=N'11,12,15', [DueDateBase]=2, [DueDateDiff]=-4, [ShortName]=N'Draft', [IsCriticalPath]=1 WHERE [Id]=8
UPDATE [dbo].[Task] SET [Name]=N'Approve report', [DependsOn]=N'8', [DueDateBase]=2, [DueDateDiff]=-1, [ShortName]=N'Approve', [IsCriticalPath]=1 WHERE [Id]=9
UPDATE [dbo].[Task] SET [Name]=N'Complete document review', [Sequence]=30, [DependsOn]=N'18', [DueDateBase]=1, [DueDateDiff]=-1, [ShortName]=N'DocReview', [IsCriticalPath]=1 WHERE [Id]=11
UPDATE [dbo].[Task] SET [Name]=N'Complete intake sections', [DependsOn]=N'ExamDate', [DueDateBase]=2, [DueDateDiff]=-6, [ShortName]=N'Intake', [IsCriticalPath]=1 WHERE [Id]=12
UPDATE [dbo].[Task] SET [DependsOn]=N'ExamDate', [DueDateBase]=2, [DueDateDiff]=-6, [ShortName]=N'Physician', [IsCriticalPath]=1 WHERE [Id]=15
UPDATE [dbo].[Task] SET [Name]=N'Create case folder', [DueDateBase]=1, [DueDateDiff]=-30 WHERE [Id]=16
UPDATE [dbo].[Task] SET [Sequence]=16, [DueDateBase]=1, [DueDateDiff]=-30 WHERE [Id]=17
UPDATE [dbo].[Task] SET [Name]=N'Save med brief', [Sequence]=20, [DueDateBase]=1, [DueDateDiff]=-7, [ShortName]=N'MedBrief', [IsCriticalPath]=1 WHERE [Id]=18
UPDATE [dbo].[Task] SET [Name]=N'Submit report', [DependsOn]=N'9', [DueDateBase]=2, [DueDateDiff]=-1, [ShortName]=N'Submit', [IsCriticalPath]=1 WHERE [Id]=19
UPDATE [dbo].[Task] SET [ServiceCategoryId]=NULL WHERE [Id]=20
UPDATE [dbo].[Task] SET [ServiceCategoryId]=NULL, [DependsOn]=N'19' WHERE [Id]=21
UPDATE [dbo].[Task] SET [Name]=N'Submit invoice', [Sequence]=40, [DependsOn]=N'ExamDate', [DueDateBase]=1, [DueDateDiff]=0, [ShortName]=N'Invoice', [IsCriticalPath]=1 WHERE [Id]=24
UPDATE [dbo].[Task] SET [ObjectGuid]=N'db8bc6f1-9251-4f7e-b684-5ba57fd5fe10', [Name]=N'Create calendar event', [TaskPhaseId]=33, [ResponsibleRoleId]=N'9eab89c0-225c-4027-9f42-cc35e5656b14', [HourlyRate]=25.00, [EstimatedHours]=0.25, [Sequence]=13, [IsMilestone]=0, [ModifiedDate]='20151023 16:18:55.733', [DueDateBase]=1, [DueDateDiff]=-30 WHERE [Id]=25
UPDATE [dbo].[LookupItem] SET [Text]=N'Waiting' WHERE [Id]=16
UPDATE [dbo].[LookupItem] SET [Text]=N'Reporting' WHERE [Id]=34
UPDATE [dbo].[LookupItem] SET [Text]=N'Follow-up' WHERE [Id]=35
UPDATE [dbo].[LookupItem] SET [Value]=5 WHERE [Id]=40
UPDATE [dbo].[AspNetUsers] SET [ColorCode]=N'#550400' WHERE [Id]=N'0538a347-fddc-4f03-97b1-a1e816e888ed'
UPDATE [dbo].[AspNetUsers] SET [ColorCode]=N'#8080bf' WHERE [Id]=N'07db877f-0ebc-47ee-aee7-145dac77f733'
UPDATE [dbo].[AspNetUsers] SET [ColorCode]=N'#ae00ae' WHERE [Id]=N'1c5dce4d-d236-4fde-9db1-3cffc97ac77a'
UPDATE [dbo].[AspNetUsers] SET [ColorCode]=N'#d7006b' WHERE [Id]=N'2f133d64-507e-4ec1-b6e0-ad0a06de77bc'
UPDATE [dbo].[AspNetUsers] SET [ColorCode]=N'#cccc00' WHERE [Id]=N'7c8f47bd-fcb1-443e-a703-cdeefb3b69bb'
UPDATE [dbo].[AspNetUsers] SET [ColorCode]=N'#ea7500' WHERE [Id]=N'8ad861fa-5cc2-4342-89da-b3709b1e59b5'
UPDATE [dbo].[AspNetUsers] SET [ColorCode]=N'#400080' WHERE [Id]=N'8dd4e180-6e3a-4968-a00d-eeb6d2cc7f0c'
UPDATE [dbo].[AspNetUsers] SET [ColorCode]=N'#0080ff' WHERE [Id]=N'8e9885d8-a0f7-49f6-9a3e-ff1b4d52f6a9'
UPDATE [dbo].[AspNetUsers] SET [ColorCode]=N'#008000' WHERE [Id]=N'd579d0a4-11ce-46f2-97ec-4c2bfc4dc704'
UPDATE [dbo].[AspNetUsers] SET [ColorCode]=N'#009f9f' WHERE [Id]=N'f268453d-c393-4f5b-aea9-e1c057d4ae1a'
UPDATE [dbo].[AspNetUsers] SET [ColorCode]=N'#004080' WHERE [Id]=N'f61e8eee-cc97-4fe1-9140-0fc219b05f65'
INSERT INTO [dbo].[LookupItem] ([Id], [LookupId], [Text], [Value], [ModifiedDate], [ModifiedUser], [ShortText]) VALUES (47, 3, N'Done', 4, '20160402 06:38:08.820', N'DESKTOP-PJSAQN9\faragol', NULL)
SET IDENTITY_INSERT [dbo].[Task] ON
INSERT INTO [dbo].[Task] ([Id], [ObjectGuid], [ServiceCategoryId], [ServiceId], [Name], [Guidance], [TaskPhaseId], [ResponsibleRoleId], [IsBillable], [HourlyRate], [EstimatedHours], [Sequence], [IsMilestone], [NodeId], [ModifiedDate], [ModifiedUser], [DependsOn], [DueDateBase], [DueDateDiff], [ShortName], [IsCriticalPath]) VALUES (26, N'9f492060-41c1-4aa3-a599-fef8b789e6d8', 5, NULL, N'Save NOA', NULL, 33, N'9eab89c0-225c-4027-9f42-cc35e5656b14', 0, 25.00, 0.25, 23, 0, NULL, '20151023 16:18:55.733', N'orvosi-sqladmin', NULL, 1, -7, NULL, 0)
INSERT INTO [dbo].[Task] ([Id], [ObjectGuid], [ServiceCategoryId], [ServiceId], [Name], [Guidance], [TaskPhaseId], [ResponsibleRoleId], [IsBillable], [HourlyRate], [EstimatedHours], [Sequence], [IsMilestone], [NodeId], [ModifiedDate], [ModifiedUser], [DependsOn], [DueDateBase], [DueDateDiff], [ShortName], [IsCriticalPath]) VALUES (27, N'5df8fa32-e16b-4e82-b911-47e964016c0d', 5, NULL, N'Save LOI', NULL, 33, N'9eab89c0-225c-4027-9f42-cc35e5656b14', 0, 25.00, 0.25, 25, 0, NULL, '20151023 16:18:55.733', N'orvosi-sqladmin', NULL, 1, -7, NULL, 0)
INSERT INTO [dbo].[Task] ([Id], [ObjectGuid], [ServiceCategoryId], [ServiceId], [Name], [Guidance], [TaskPhaseId], [ResponsibleRoleId], [IsBillable], [HourlyRate], [EstimatedHours], [Sequence], [IsMilestone], [NodeId], [ModifiedDate], [ModifiedUser], [DependsOn], [DueDateBase], [DueDateDiff], [ShortName], [IsCriticalPath]) VALUES (28, N'09b3b524-201c-41ab-8739-8f153222ce55', 5, NULL, N'Obtain final report from company', NULL, 35, N'9eab89c0-225c-4027-9f42-cc35e5656b14', 0, NULL, NULL, 130, 0, NULL, '20160325 15:54:03.243', N'DESKTOP-PJSAQN9\faragol', N'19', 2, 14, NULL, 0)
INSERT INTO [dbo].[Task] ([Id], [ObjectGuid], [ServiceCategoryId], [ServiceId], [Name], [Guidance], [TaskPhaseId], [ResponsibleRoleId], [IsBillable], [HourlyRate], [EstimatedHours], [Sequence], [IsMilestone], [NodeId], [ModifiedDate], [ModifiedUser], [DependsOn], [DueDateBase], [DueDateDiff], [ShortName], [IsCriticalPath]) VALUES (30, N'a71eb876-2f04-4d1f-9684-aec7394d2297', 5, NULL, N'Close the case', NULL, 35, N'9eab89c0-225c-4027-9f42-cc35e5656b14', 0, NULL, NULL, 140, 0, NULL, '20160323 00:00:00.000', N'faragol', N'28,24', 2, 7, NULL, 0)
SET IDENTITY_INSERT [dbo].[Task] OFF
ALTER TABLE [dbo].[Document]
    ADD CONSTRAINT [FK_Document_DocumentTemplate] FOREIGN KEY ([DocumentTemplateId]) REFERENCES [dbo].[DocumentTemplate] ([Id])
ALTER TABLE [dbo].[Address]
    ADD CONSTRAINT [FK_Address_Countries] FOREIGN KEY ([CountryID]) REFERENCES [dbo].[Country] ([Id])
ALTER TABLE [dbo].[Address]
    ADD CONSTRAINT [FK_Address_Provinces] FOREIGN KEY ([ProvinceID]) REFERENCES [dbo].[Province] ([Id])
ALTER TABLE [dbo].[AspNetUserLogins]
    ADD CONSTRAINT [FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE
ALTER TABLE [dbo].[AspNetUserClaims]
    ADD CONSTRAINT [FK_dbo.AspNetUserClaims_dbo.AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE
ALTER TABLE [dbo].[AspNetUserRoles]
    ADD CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[AspNetRoles] ([Id]) ON DELETE CASCADE
ALTER TABLE [dbo].[AspNetUserRoles]
    ADD CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE
COMMIT TRANSACTION
