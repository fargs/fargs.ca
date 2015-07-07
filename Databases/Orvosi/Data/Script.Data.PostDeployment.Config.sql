 
--This file is auto-generated, any changes made to it will be overwritten when it's regenerated
--Read the readme.md for more information


--########################################################
--########### [dbo].[AspNetUsers]
--########################################################
	--Turn off the output of how many rows affected so output window isn't crowded
	SET NOCOUNT ON
	PRINT 'Updating table [dbo].[AspNetUsers]'

	IF OBJECT_ID('tempdb..#RawData') IS NOT NULL DROP TABLE #RawData
	GO

	/*
	Create a temporary table to load the source data
	This uses a select into from the destination table to create the temp table for the source.
	This resolves an issue where server colations are different between environments.
	This ensure the collation of the destination table is used for the temp table.
	WHERE 1=2 is used to return 0 records.
	*/
	SELECT 
		[Id],
		[Email],
		[EmailConfirmed],
		[PasswordHash],
		[SecurityStamp],
		[PhoneNumberConfirmed],
		[TwoFactorEnabled],
		[LockoutEnabled],
		[AccessFailedCount],
		[UserName],
		[FirstName],
		[LastName],
		[RoleLevelId],
		[LockoutEndDateUtc],
		[Title]
	INTO #RawData
	FROM (
		SELECT 
			[Id],
			[Email],
			[EmailConfirmed],
			[PasswordHash],
			[SecurityStamp],
			[PhoneNumberConfirmed],
			[TwoFactorEnabled],
			[LockoutEnabled],
			[AccessFailedCount],
			[UserName],
			[FirstName],
			[LastName],
			[RoleLevelId],
			[LockoutEndDateUtc],
			[Title]
		FROM [dbo].[AspNetUsers] 
		WHERE 1=2
	) t
	GO

	/* 
	Because the temp table is now created using a SELECT INTO it can be created with IDENTITY fields. 
	Turn on IDENTITY INSERT if they exist and turn it back off after you are done.
	*/
	IF ((SELECT OBJECTPROPERTY( OBJECT_ID(N'[dbo].[AspNetUsers]'), 'TableHasIdentity')) = 1)
		SET IDENTITY_INSERT #RawData ON
	GO

	--This is the data from the file at time of script generation
INSERT INTO #RawData ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnabled], [AccessFailedCount], [UserName], [FirstName], [LastName], [RoleLevelId]) VALUES ('0538a347-fddc-4f03-97b1-a1e816e888ed', 'lesliefarago@gmail.com', '1', 'AIiSrF9UikA+4G8u7KdOXRcSXVxvHWsAbaK5FwhNyybTBrUhgYAYaxSwc13TU9SEHw==', 'bb1f8b74-e9a4-4c24-b4f9-f6f13ed13ff6', '0', '0', '1', '0', 'lesliefarago@gmail.com', 'Leslie', 'Farago', '2')
INSERT INTO #RawData ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName], [FirstName], [LastName], [RoleLevelId]) VALUES ('491841d0-dd83-482a-b710-476397e03e96', 'stenace@gmail.com', '0', 'AJkpSRahjMyojeOme0jZXNyTQisjk5qCK9ofRofq5Dm3iPSZLKQQYFTH8cGLLyJNKw==', '1a8e8284-3e9b-4bc6-8b80-a6e5f90168e0', '0', '0', '2015-06-22T00:00:00', '1', '0', 'stenace@gmail.com', 'Sabrina', 'Tenace', '3')
INSERT INTO #RawData ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnabled], [AccessFailedCount], [UserName], [Title], [FirstName], [RoleLevelId]) VALUES ('5b92cd8b-a500-428f-88a2-ad14e0a23624', 'lesliefarago@hotmail.com', '1', 'AKHk74KgvKs/AB9cku8C96KBfQoJECetXKl3ZLwoonNgVzQhfj0ff4k3a3q3Eb4Lpw==', 'd74768cc-c403-4a72-8ea3-8d4d375a526d', '0', '0', '1', '0', 'lesliefarago@hotmail.com', 'Mr.', 'Les', '4')
INSERT INTO #RawData ([Id], [Email], [EmailConfirmed], [SecurityStamp], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnabled], [AccessFailedCount], [UserName], [Title], [FirstName], [LastName], [RoleLevelId]) VALUES ('e62ebead-c270-4711-81cb-2cbd6b8031ad', 'lfarago@orvosi.ca', '0', '3134f817-b80c-4dbe-809a-8665efd614aa', '0', '0', '1', '0', 'lfarago@orvosi.ca', 'Dr.', 'Sabrina', 'Farago', '5')

	IF ((SELECT OBJECTPROPERTY( OBJECT_ID(N'[dbo].[AspNetUsers]'), 'TableHasIdentity')) = 1)
		SET IDENTITY_INSERT #RawData OFF
	GO

	PRINT CONVERT(NVARCHAR(MAX),@@ROWCOUNT) + ' rows affected'
	GO
	
	
	DECLARE @CounterInProject INT,
			@CounterIdentical INT,
			@CounterUpdated INT,
			@CounterNew INT

	SELECT @CounterInProject = COUNT(*) FROM #RawData

	--This table keeps track of which rows in the source have already been checked/added or updated
	DECLARE @IdenticalRecordIDs TABLE(ID NVARCHAR(128))
	
	--Ignore any rows that are equal
	INSERT INTO @IdenticalRecordIDs
	SELECT s.[Id] 
	FROM #RawData s
	LEFT JOIN [dbo].[AspNetUsers] d ON d.[Id] = s.[Id]
	WHERE d.[Email] = s.[Email] AND d.[EmailConfirmed] = s.[EmailConfirmed] AND d.[PasswordHash] = s.[PasswordHash] AND d.[SecurityStamp] = s.[SecurityStamp] AND d.[PhoneNumberConfirmed] = s.[PhoneNumberConfirmed] AND d.[TwoFactorEnabled] = s.[TwoFactorEnabled] AND d.[LockoutEnabled] = s.[LockoutEnabled] AND d.[AccessFailedCount] = s.[AccessFailedCount] AND d.[UserName] = s.[UserName] AND d.[FirstName] = s.[FirstName] AND d.[LastName] = s.[LastName] AND d.[RoleLevelId] = s.[RoleLevelId] AND d.[LockoutEndDateUtc] = s.[LockoutEndDateUtc] AND d.[Title] = s.[Title]


	SELECT @CounterIdentical = COUNT(*) FROM @IdenticalRecordIDs


	--Update any rows that already exist
	UPDATE [dbo].[AspNetUsers] SET [Email] = s.[Email], [EmailConfirmed] = s.[EmailConfirmed], [PasswordHash] = s.[PasswordHash], [SecurityStamp] = s.[SecurityStamp], [PhoneNumberConfirmed] = s.[PhoneNumberConfirmed], [TwoFactorEnabled] = s.[TwoFactorEnabled], [LockoutEnabled] = s.[LockoutEnabled], [AccessFailedCount] = s.[AccessFailedCount], [UserName] = s.[UserName], [FirstName] = s.[FirstName], [LastName] = s.[LastName], [RoleLevelId] = s.[RoleLevelId], [LockoutEndDateUtc] = s.[LockoutEndDateUtc], [Title] = s.[Title]
		, ModifiedDate = GETDATE()
	, ModifiedUser = SUSER_NAME()
		OUTPUT Inserted.[Id] INTO @IdenticalRecordIDs
	FROM #RawData s
	LEFT JOIN [dbo].[AspNetUsers] d ON s.[Id] = d.[Id] 
	WHERE s.[Id] NOT IN (SELECT id FROM @IdenticalRecordIDs)


	SELECT @CounterUpdated = COUNT(*) - @CounterIdentical FROM @IdenticalRecordIDs


	IF ((SELECT OBJECTPROPERTY( OBJECT_ID(N'[dbo].[AspNetUsers]'), 'TableHasIdentity')) = 1)
		SET IDENTITY_INSERT [dbo].[AspNetUsers] ON

	--Insert any new rows
	INSERT INTO [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnabled], [AccessFailedCount], [UserName], [FirstName], [LastName], [RoleLevelId], [LockoutEndDateUtc], [Title]
	, ModifiedDate, ModifiedUser	)
	OUTPUT Inserted.[Id] INTO @IdenticalRecordIDs
	SELECT [Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnabled], [AccessFailedCount], [UserName], [FirstName], [LastName], [RoleLevelId], [LockoutEndDateUtc], [Title]
	, GETDATE(), SUSER_NAME()	FROM #RawData
	WHERE [Id] NOT IN (SELECT id FROM @IdenticalRecordIDs)

	IF ((SELECT OBJECTPROPERTY( OBJECT_ID(N'[dbo].[AspNetUsers]'), 'TableHasIdentity')) = 1)
		SET IDENTITY_INSERT [dbo].[AspNetUsers] OFF

	SELECT @CounterNew = COUNT(*) - @CounterIdentical - @CounterUpdated FROM @IdenticalRecordIDs

	PRINT 'Records in Project: '+ CONVERT(NVARCHAR(MAX),@CounterInProject)
	PRINT 'Identical Records: '+ CONVERT(NVARCHAR(MAX),@CounterIdentical)
	PRINT 'Updated Records: '+ CONVERT(NVARCHAR(MAX),@CounterUpdated)
	PRINT 'New Records: '+ CONVERT(NVARCHAR(MAX),@CounterNew)

	GO
--########################################################
--########### [dbo].[AspNetUserRoles]
--########################################################
	--Turn off the output of how many rows affected so output window isn't crowded
	SET NOCOUNT ON
	PRINT 'Updating table [dbo].[AspNetUserRoles]'

	IF OBJECT_ID('tempdb..#RawData') IS NOT NULL DROP TABLE #RawData
	GO

	/*
	Create a temporary table to load the source data
	This uses a select into from the destination table to create the temp table for the source.
	This resolves an issue where server colations are different between environments.
	This ensure the collation of the destination table is used for the temp table.
	WHERE 1=2 is used to return 0 records.
	*/
	SELECT 
		[UserId],
		[RoleId]
	INTO #RawData
	FROM (
		SELECT 
			[UserId],
			[RoleId]
		FROM [dbo].[AspNetUserRoles] 
		WHERE 1=2
	) t
	GO

	/* 
	Because the temp table is now created using a SELECT INTO it can be created with IDENTITY fields. 
	Turn on IDENTITY INSERT if they exist and turn it back off after you are done.
	*/
	IF ((SELECT OBJECTPROPERTY( OBJECT_ID(N'[dbo].[AspNetUserRoles]'), 'TableHasIdentity')) = 1)
		SET IDENTITY_INSERT #RawData ON
	GO

	--This is the data from the file at time of script generation
INSERT INTO #RawData ([UserId], [RoleId]) VALUES ('0538a347-fddc-4f03-97b1-a1e816e888ed', '7fab67dd-286b-492f-865a-0cb0ce1261ce')
INSERT INTO #RawData ([UserId], [RoleId]) VALUES ('491841d0-dd83-482a-b710-476397e03e96', '9eab89c0-225c-4027-9f42-cc35e5656b14')
INSERT INTO #RawData ([UserId], [RoleId]) VALUES ('5b92cd8b-a500-428f-88a2-ad14e0a23624', '9dd582a0-cf86-4fc0-8894-477266068c12')
INSERT INTO #RawData ([UserId], [RoleId]) VALUES ('e62ebead-c270-4711-81cb-2cbd6b8031ad', '8359141f-e423-4e48-8925-4624ba86245a')

	IF ((SELECT OBJECTPROPERTY( OBJECT_ID(N'[dbo].[AspNetUserRoles]'), 'TableHasIdentity')) = 1)
		SET IDENTITY_INSERT #RawData OFF
	GO

	PRINT CONVERT(NVARCHAR(MAX),@@ROWCOUNT) + ' rows affected'
	GO
	
	
	DECLARE @CounterInProject INT,
			@CounterIdentical INT,
			@CounterUpdated INT,
			@CounterNew INT

	SELECT @CounterInProject = COUNT(*) FROM #RawData

	--This table keeps track of which rows in the source have already been checked/added or updated
	DECLARE @IdenticalRecordIDs TABLE(ID NVARCHAR(128))
	
	--Ignore any rows that are equal
	INSERT INTO @IdenticalRecordIDs
	SELECT s.[UserId] 
	FROM #RawData s
	LEFT JOIN [dbo].[AspNetUserRoles] d ON d.[UserId] = s.[UserId]
	WHERE d.[RoleId] = s.[RoleId]


	SELECT @CounterIdentical = COUNT(*) FROM @IdenticalRecordIDs


	--Update any rows that already exist
	UPDATE [dbo].[AspNetUserRoles] SET [RoleId] = s.[RoleId]
		, ModifiedDate = GETDATE()
	, ModifiedUser = SUSER_NAME()
		OUTPUT Inserted.[UserId] INTO @IdenticalRecordIDs
	FROM #RawData s
	LEFT JOIN [dbo].[AspNetUserRoles] d ON s.[UserId] = d.[UserId] 
	WHERE s.[UserId] NOT IN (SELECT id FROM @IdenticalRecordIDs)


	SELECT @CounterUpdated = COUNT(*) - @CounterIdentical FROM @IdenticalRecordIDs


	IF ((SELECT OBJECTPROPERTY( OBJECT_ID(N'[dbo].[AspNetUserRoles]'), 'TableHasIdentity')) = 1)
		SET IDENTITY_INSERT [dbo].[AspNetUserRoles] ON

	--Insert any new rows
	INSERT INTO [dbo].[AspNetUserRoles] ([UserId], [RoleId]
	, ModifiedDate, ModifiedUser	)
	OUTPUT Inserted.[UserId] INTO @IdenticalRecordIDs
	SELECT [UserId], [RoleId]
	, GETDATE(), SUSER_NAME()	FROM #RawData
	WHERE [UserId] NOT IN (SELECT id FROM @IdenticalRecordIDs)

	IF ((SELECT OBJECTPROPERTY( OBJECT_ID(N'[dbo].[AspNetUserRoles]'), 'TableHasIdentity')) = 1)
		SET IDENTITY_INSERT [dbo].[AspNetUserRoles] OFF

	SELECT @CounterNew = COUNT(*) - @CounterIdentical - @CounterUpdated FROM @IdenticalRecordIDs

	PRINT 'Records in Project: '+ CONVERT(NVARCHAR(MAX),@CounterInProject)
	PRINT 'Identical Records: '+ CONVERT(NVARCHAR(MAX),@CounterIdentical)
	PRINT 'Updated Records: '+ CONVERT(NVARCHAR(MAX),@CounterUpdated)
	PRINT 'New Records: '+ CONVERT(NVARCHAR(MAX),@CounterNew)

	GO
--########################################################
--########### [dbo].[AspNetUserLogins]
--########################################################
	--Turn off the output of how many rows affected so output window isn't crowded
	SET NOCOUNT ON
	PRINT 'Updating table [dbo].[AspNetUserLogins]'

	IF OBJECT_ID('tempdb..#RawData') IS NOT NULL DROP TABLE #RawData
	GO

	/*
	Create a temporary table to load the source data
	This uses a select into from the destination table to create the temp table for the source.
	This resolves an issue where server colations are different between environments.
	This ensure the collation of the destination table is used for the temp table.
	WHERE 1=2 is used to return 0 records.
	*/
	SELECT 
		[LoginProvider],
		[ProviderKey],
		[UserId]
	INTO #RawData
	FROM (
		SELECT 
			[LoginProvider],
			[ProviderKey],
			[UserId]
		FROM [dbo].[AspNetUserLogins] 
		WHERE 1=2
	) t
	GO

	/* 
	Because the temp table is now created using a SELECT INTO it can be created with IDENTITY fields. 
	Turn on IDENTITY INSERT if they exist and turn it back off after you are done.
	*/
	IF ((SELECT OBJECTPROPERTY( OBJECT_ID(N'[dbo].[AspNetUserLogins]'), 'TableHasIdentity')) = 1)
		SET IDENTITY_INSERT #RawData ON
	GO

	--This is the data from the file at time of script generation
INSERT INTO #RawData ([LoginProvider], [ProviderKey], [UserId]) VALUES ('Google', '100551808610732833937', '5b92cd8b-a500-428f-88a2-ad14e0a23624')
INSERT INTO #RawData ([LoginProvider], [ProviderKey], [UserId]) VALUES ('Google', '114524886465863552780', 'e62ebead-c270-4711-81cb-2cbd6b8031ad')

	IF ((SELECT OBJECTPROPERTY( OBJECT_ID(N'[dbo].[AspNetUserLogins]'), 'TableHasIdentity')) = 1)
		SET IDENTITY_INSERT #RawData OFF
	GO

	PRINT CONVERT(NVARCHAR(MAX),@@ROWCOUNT) + ' rows affected'
	GO
	
	
	DECLARE @CounterInProject INT,
			@CounterIdentical INT,
			@CounterUpdated INT,
			@CounterNew INT

	SELECT @CounterInProject = COUNT(*) FROM #RawData

	--This table keeps track of which rows in the source have already been checked/added or updated
	DECLARE @IdenticalRecordIDs TABLE(ID NVARCHAR(128))
	
	--Ignore any rows that are equal
	INSERT INTO @IdenticalRecordIDs
	SELECT s.[LoginProvider] 
	FROM #RawData s
	LEFT JOIN [dbo].[AspNetUserLogins] d ON d.[LoginProvider] = s.[LoginProvider]
	WHERE d.[ProviderKey] = s.[ProviderKey] AND d.[UserId] = s.[UserId]


	SELECT @CounterIdentical = COUNT(*) FROM @IdenticalRecordIDs


	--Update any rows that already exist
	UPDATE [dbo].[AspNetUserLogins] SET [ProviderKey] = s.[ProviderKey], [UserId] = s.[UserId]
		, ModifiedDate = GETDATE()
	, ModifiedUser = SUSER_NAME()
		OUTPUT Inserted.[LoginProvider] INTO @IdenticalRecordIDs
	FROM #RawData s
	LEFT JOIN [dbo].[AspNetUserLogins] d ON s.[LoginProvider] = d.[LoginProvider] 
	WHERE s.[LoginProvider] NOT IN (SELECT id FROM @IdenticalRecordIDs)


	SELECT @CounterUpdated = COUNT(*) - @CounterIdentical FROM @IdenticalRecordIDs


	IF ((SELECT OBJECTPROPERTY( OBJECT_ID(N'[dbo].[AspNetUserLogins]'), 'TableHasIdentity')) = 1)
		SET IDENTITY_INSERT [dbo].[AspNetUserLogins] ON

	--Insert any new rows
	INSERT INTO [dbo].[AspNetUserLogins] ([LoginProvider], [ProviderKey], [UserId]
	, ModifiedDate, ModifiedUser	)
	OUTPUT Inserted.[LoginProvider] INTO @IdenticalRecordIDs
	SELECT [LoginProvider], [ProviderKey], [UserId]
	, GETDATE(), SUSER_NAME()	FROM #RawData
	WHERE [LoginProvider] NOT IN (SELECT id FROM @IdenticalRecordIDs)

	IF ((SELECT OBJECTPROPERTY( OBJECT_ID(N'[dbo].[AspNetUserLogins]'), 'TableHasIdentity')) = 1)
		SET IDENTITY_INSERT [dbo].[AspNetUserLogins] OFF

	SELECT @CounterNew = COUNT(*) - @CounterIdentical - @CounterUpdated FROM @IdenticalRecordIDs

	PRINT 'Records in Project: '+ CONVERT(NVARCHAR(MAX),@CounterInProject)
	PRINT 'Identical Records: '+ CONVERT(NVARCHAR(MAX),@CounterIdentical)
	PRINT 'Updated Records: '+ CONVERT(NVARCHAR(MAX),@CounterUpdated)
	PRINT 'New Records: '+ CONVERT(NVARCHAR(MAX),@CounterNew)

	GO
--########################################################
--########### [dbo].[Company]
--########################################################
	--Turn off the output of how many rows affected so output window isn't crowded
	SET NOCOUNT ON
	PRINT 'Updating table [dbo].[Company]'

	IF OBJECT_ID('tempdb..#RawData') IS NOT NULL DROP TABLE #RawData
	GO

	/*
	Create a temporary table to load the source data
	This uses a select into from the destination table to create the temp table for the source.
	This resolves an issue where server colations are different between environments.
	This ensure the collation of the destination table is used for the temp table.
	WHERE 1=2 is used to return 0 records.
	*/
	SELECT 
		[Id],
		[Name],
		[ParentId]
	INTO #RawData
	FROM (
		SELECT 
			[Id],
			[Name],
			[ParentId]
		FROM [dbo].[Company] 
		WHERE 1=2
	) t
	GO

	/* 
	Because the temp table is now created using a SELECT INTO it can be created with IDENTITY fields. 
	Turn on IDENTITY INSERT if they exist and turn it back off after you are done.
	*/
	IF ((SELECT OBJECTPROPERTY( OBJECT_ID(N'[dbo].[Company]'), 'TableHasIdentity')) = 1)
		SET IDENTITY_INSERT #RawData ON
	GO

	--This is the data from the file at time of script generation
INSERT INTO #RawData ([Id], [Name]) VALUES ('1', 'Examworks')
INSERT INTO #RawData ([Id], [Name], [ParentId]) VALUES ('2', 'Direct IME', '1')
INSERT INTO #RawData ([Id], [Name], [ParentId]) VALUES ('3', 'MAKOS', '1')
INSERT INTO #RawData ([Id], [Name], [ParentId]) VALUES ('4', 'SOMA', '1')
INSERT INTO #RawData ([Id], [Name], [ParentId]) VALUES ('5', 'NYRC-ONT', '1')
INSERT INTO #RawData ([Id], [Name], [ParentId]) VALUES ('6', 'IMA', '1')
INSERT INTO #RawData ([Id], [Name], [ParentId]) VALUES ('7', 'CVS', '1')
INSERT INTO #RawData ([Id], [Name], [ParentId]) VALUES ('8', 'NYRC-BC', '1')
INSERT INTO #RawData ([Id], [Name], [ParentId]) VALUES ('9', 'National IME', '1')
INSERT INTO #RawData ([Id], [Name]) VALUES ('10', 'SCM')
INSERT INTO #RawData ([Id], [Name], [ParentId]) VALUES ('11', 'CIRA', '10')
INSERT INTO #RawData ([Id], [Name], [ParentId]) VALUES ('12', 'Sibley', '10')
INSERT INTO #RawData ([Id], [Name], [ParentId]) VALUES ('13', 'MDAC', '10')
INSERT INTO #RawData ([Id], [Name], [ParentId]) VALUES ('14', 'TRM', '10')
INSERT INTO #RawData ([Id], [Name]) VALUES ('15', 'AssessMed')
INSERT INTO #RawData ([Id], [Name]) VALUES ('16', 'Benchmark')
INSERT INTO #RawData ([Id], [Name]) VALUES ('17', 'CWC')
INSERT INTO #RawData ([Id], [Name]) VALUES ('18', 'ARSI')
INSERT INTO #RawData ([Id], [Name]) VALUES ('19', 'Seiden')
INSERT INTO #RawData ([Id], [Name]) VALUES ('20', 'SIMAC')

	IF ((SELECT OBJECTPROPERTY( OBJECT_ID(N'[dbo].[Company]'), 'TableHasIdentity')) = 1)
		SET IDENTITY_INSERT #RawData OFF
	GO

	PRINT CONVERT(NVARCHAR(MAX),@@ROWCOUNT) + ' rows affected'
	GO
	
	
	DECLARE @CounterInProject INT,
			@CounterIdentical INT,
			@CounterUpdated INT,
			@CounterNew INT

	SELECT @CounterInProject = COUNT(*) FROM #RawData

	--This table keeps track of which rows in the source have already been checked/added or updated
	DECLARE @IdenticalRecordIDs TABLE(ID NVARCHAR(128))
	
	--Ignore any rows that are equal
	INSERT INTO @IdenticalRecordIDs
	SELECT s.[Id] 
	FROM #RawData s
	LEFT JOIN [dbo].[Company] d ON d.[Id] = s.[Id]
	WHERE d.[Name] = s.[Name] AND d.[ParentId] = s.[ParentId]


	SELECT @CounterIdentical = COUNT(*) FROM @IdenticalRecordIDs


	--Update any rows that already exist
	UPDATE [dbo].[Company] SET [Name] = s.[Name], [ParentId] = s.[ParentId]
		, ModifiedDate = GETDATE()
	, ModifiedUser = SUSER_NAME()
		OUTPUT Inserted.[Id] INTO @IdenticalRecordIDs
	FROM #RawData s
	LEFT JOIN [dbo].[Company] d ON s.[Id] = d.[Id] 
	WHERE s.[Id] NOT IN (SELECT id FROM @IdenticalRecordIDs)


	SELECT @CounterUpdated = COUNT(*) - @CounterIdentical FROM @IdenticalRecordIDs


	IF ((SELECT OBJECTPROPERTY( OBJECT_ID(N'[dbo].[Company]'), 'TableHasIdentity')) = 1)
		SET IDENTITY_INSERT [dbo].[Company] ON

	--Insert any new rows
	INSERT INTO [dbo].[Company] ([Id], [Name], [ParentId]
	, ModifiedDate, ModifiedUser	)
	OUTPUT Inserted.[Id] INTO @IdenticalRecordIDs
	SELECT [Id], [Name], [ParentId]
	, GETDATE(), SUSER_NAME()	FROM #RawData
	WHERE [Id] NOT IN (SELECT id FROM @IdenticalRecordIDs)

	IF ((SELECT OBJECTPROPERTY( OBJECT_ID(N'[dbo].[Company]'), 'TableHasIdentity')) = 1)
		SET IDENTITY_INSERT [dbo].[Company] OFF

	SELECT @CounterNew = COUNT(*) - @CounterIdentical - @CounterUpdated FROM @IdenticalRecordIDs

	PRINT 'Records in Project: '+ CONVERT(NVARCHAR(MAX),@CounterInProject)
	PRINT 'Identical Records: '+ CONVERT(NVARCHAR(MAX),@CounterIdentical)
	PRINT 'Updated Records: '+ CONVERT(NVARCHAR(MAX),@CounterUpdated)
	PRINT 'New Records: '+ CONVERT(NVARCHAR(MAX),@CounterNew)

	GO
--########################################################
--########### [dbo].[Configuration]
--########################################################
	--Turn off the output of how many rows affected so output window isn't crowded
	SET NOCOUNT ON
	PRINT 'Updating table [dbo].[Configuration]'

	IF OBJECT_ID('tempdb..#RawData') IS NOT NULL DROP TABLE #RawData
	GO

	/*
	Create a temporary table to load the source data
	This uses a select into from the destination table to create the temp table for the source.
	This resolves an issue where server colations are different between environments.
	This ensure the collation of the destination table is used for the temp table.
	WHERE 1=2 is used to return 0 records.
	*/
	SELECT 
		[Id],
		[EntityTypeID],
		[EntityID],
		[ConfigurationTypeID],
		[ConfigValue],
		[DatePart],
		[Sequence]
	INTO #RawData
	FROM (
		SELECT 
			[Id],
			[EntityTypeID],
			[EntityID],
			[ConfigurationTypeID],
			[ConfigValue],
			[DatePart],
			[Sequence]
		FROM [dbo].[Configuration] 
		WHERE 1=2
	) t
	GO

	/* 
	Because the temp table is now created using a SELECT INTO it can be created with IDENTITY fields. 
	Turn on IDENTITY INSERT if they exist and turn it back off after you are done.
	*/
	IF ((SELECT OBJECTPROPERTY( OBJECT_ID(N'[dbo].[Configuration]'), 'TableHasIdentity')) = 1)
		SET IDENTITY_INSERT #RawData ON
	GO

	--This is the data from the file at time of script generation
INSERT INTO #RawData ([Id], [EntityTypeID], [EntityID], [ConfigurationTypeID], [ConfigValue], [DatePart]) VALUES ('1', '1', '1', '1', '2', 'm')
INSERT INTO #RawData ([Id], [EntityTypeID], [EntityID], [ConfigurationTypeID], [ConfigValue], [DatePart]) VALUES ('2', '1', '1', '2', '10', 'm')
INSERT INTO #RawData ([Id], [EntityTypeID], [EntityID], [ConfigurationTypeID], [ConfigValue], [DatePart], [Sequence]) VALUES ('3', '1', '1', '1', '0', 'm', '2')
INSERT INTO #RawData ([Id], [EntityTypeID], [EntityID], [ConfigurationTypeID], [ConfigValue], [DatePart], [Sequence]) VALUES ('4', '1', '1', '1', '0', 'm', '3')
INSERT INTO #RawData ([Id], [EntityTypeID], [EntityID], [ConfigurationTypeID], [ConfigValue], [DatePart], [Sequence]) VALUES ('5', '1', '1', '1', '0', 'm', '4')

	IF ((SELECT OBJECTPROPERTY( OBJECT_ID(N'[dbo].[Configuration]'), 'TableHasIdentity')) = 1)
		SET IDENTITY_INSERT #RawData OFF
	GO

	PRINT CONVERT(NVARCHAR(MAX),@@ROWCOUNT) + ' rows affected'
	GO
	
	
	DECLARE @CounterInProject INT,
			@CounterIdentical INT,
			@CounterUpdated INT,
			@CounterNew INT

	SELECT @CounterInProject = COUNT(*) FROM #RawData

	--This table keeps track of which rows in the source have already been checked/added or updated
	DECLARE @IdenticalRecordIDs TABLE(ID NVARCHAR(128))
	
	--Ignore any rows that are equal
	INSERT INTO @IdenticalRecordIDs
	SELECT s.[Id] 
	FROM #RawData s
	LEFT JOIN [dbo].[Configuration] d ON d.[Id] = s.[Id]
	WHERE d.[EntityTypeID] = s.[EntityTypeID] AND d.[EntityID] = s.[EntityID] AND d.[ConfigurationTypeID] = s.[ConfigurationTypeID] AND d.[ConfigValue] = s.[ConfigValue] AND d.[DatePart] = s.[DatePart] AND d.[Sequence] = s.[Sequence]


	SELECT @CounterIdentical = COUNT(*) FROM @IdenticalRecordIDs


	--Update any rows that already exist
	UPDATE [dbo].[Configuration] SET [EntityTypeID] = s.[EntityTypeID], [EntityID] = s.[EntityID], [ConfigurationTypeID] = s.[ConfigurationTypeID], [ConfigValue] = s.[ConfigValue], [DatePart] = s.[DatePart], [Sequence] = s.[Sequence]
		, ModifiedDate = GETDATE()
	, ModifiedUser = SUSER_NAME()
		OUTPUT Inserted.[Id] INTO @IdenticalRecordIDs
	FROM #RawData s
	LEFT JOIN [dbo].[Configuration] d ON s.[Id] = d.[Id] 
	WHERE s.[Id] NOT IN (SELECT id FROM @IdenticalRecordIDs)


	SELECT @CounterUpdated = COUNT(*) - @CounterIdentical FROM @IdenticalRecordIDs


	IF ((SELECT OBJECTPROPERTY( OBJECT_ID(N'[dbo].[Configuration]'), 'TableHasIdentity')) = 1)
		SET IDENTITY_INSERT [dbo].[Configuration] ON

	--Insert any new rows
	INSERT INTO [dbo].[Configuration] ([Id], [EntityTypeID], [EntityID], [ConfigurationTypeID], [ConfigValue], [DatePart], [Sequence]
	, ModifiedDate, ModifiedUser	)
	OUTPUT Inserted.[Id] INTO @IdenticalRecordIDs
	SELECT [Id], [EntityTypeID], [EntityID], [ConfigurationTypeID], [ConfigValue], [DatePart], [Sequence]
	, GETDATE(), SUSER_NAME()	FROM #RawData
	WHERE [Id] NOT IN (SELECT id FROM @IdenticalRecordIDs)

	IF ((SELECT OBJECTPROPERTY( OBJECT_ID(N'[dbo].[Configuration]'), 'TableHasIdentity')) = 1)
		SET IDENTITY_INSERT [dbo].[Configuration] OFF

	SELECT @CounterNew = COUNT(*) - @CounterIdentical - @CounterUpdated FROM @IdenticalRecordIDs

	PRINT 'Records in Project: '+ CONVERT(NVARCHAR(MAX),@CounterInProject)
	PRINT 'Identical Records: '+ CONVERT(NVARCHAR(MAX),@CounterIdentical)
	PRINT 'Updated Records: '+ CONVERT(NVARCHAR(MAX),@CounterUpdated)
	PRINT 'New Records: '+ CONVERT(NVARCHAR(MAX),@CounterNew)

	GO
--########################################################
--########### [dbo].[Task]
--########################################################
	--Turn off the output of how many rows affected so output window isn't crowded
	SET NOCOUNT ON
	PRINT 'Updating table [dbo].[Task]'

	IF OBJECT_ID('tempdb..#RawData') IS NOT NULL DROP TABLE #RawData
	GO

	/*
	Create a temporary table to load the source data
	This uses a select into from the destination table to create the temp table for the source.
	This resolves an issue where server colations are different between environments.
	This ensure the collation of the destination table is used for the temp table.
	WHERE 1=2 is used to return 0 records.
	*/
	SELECT 
		[Id],
		[Name],
		[ResponsibleRoleId],
		[IsBillable],
		[Sequence],
		[IsMilestone]
	INTO #RawData
	FROM (
		SELECT 
			[Id],
			[Name],
			[ResponsibleRoleId],
			[IsBillable],
			[Sequence],
			[IsMilestone]
		FROM [dbo].[Task] 
		WHERE 1=2
	) t
	GO

	/* 
	Because the temp table is now created using a SELECT INTO it can be created with IDENTITY fields. 
	Turn on IDENTITY INSERT if they exist and turn it back off after you are done.
	*/
	IF ((SELECT OBJECTPROPERTY( OBJECT_ID(N'[dbo].[Task]'), 'TableHasIdentity')) = 1)
		SET IDENTITY_INSERT #RawData ON
	GO

	--This is the data from the file at time of script generation
INSERT INTO #RawData ([Id], [Name], [ResponsibleRoleId], [IsBillable], [Sequence], [IsMilestone]) VALUES ('1', 'Assessment booked', '7b930663-b091-44ca-924c-d8b11a1ee7ea', '0', '1', '1')
INSERT INTO #RawData ([Id], [Name], [ResponsibleRoleId], [IsBillable], [Sequence], [IsMilestone]) VALUES ('2', 'Intake coordinator assigned', '9eab89c0-225c-4027-9f42-cc35e5656b14', '0', '2', '1')
INSERT INTO #RawData ([Id], [Name], [ResponsibleRoleId], [IsBillable], [Sequence], [IsMilestone]) VALUES ('3', 'Med briefs received', '7b930663-b091-44ca-924c-d8b11a1ee7ea', '0', '3', '1')
INSERT INTO #RawData ([Id], [Name], [ResponsibleRoleId], [IsBillable], [Sequence], [IsMilestone]) VALUES ('4', 'Med briefs reviewed by Intake Coordinator', '9dd582a0-cf86-4fc0-8894-477266068c12', '0', '5', '1')
INSERT INTO #RawData ([Id], [Name], [ResponsibleRoleId], [IsBillable], [Sequence], [IsMilestone]) VALUES ('5', 'Med briefs reviewed by Doctor', '8359141f-e423-4e48-8925-4624ba86245a', '0', '6', '1')
INSERT INTO #RawData ([Id], [Name], [ResponsibleRoleId], [IsBillable], [Sequence], [IsMilestone]) VALUES ('6', 'Consent form received', '9eab89c0-225c-4027-9f42-cc35e5656b14', '0', '7', '1')
INSERT INTO #RawData ([Id], [Name], [ResponsibleRoleId], [IsBillable], [Sequence], [IsMilestone]) VALUES ('7', 'Reminder sent to claimant', '9eab89c0-225c-4027-9f42-cc35e5656b14', '0', '8', '1')
INSERT INTO #RawData ([Id], [Name], [ResponsibleRoleId], [IsBillable], [Sequence], [IsMilestone]) VALUES ('8', 'Report drafted and sent for review', '9dd582a0-cf86-4fc0-8894-477266068c12', '0', '10', '1')
INSERT INTO #RawData ([Id], [Name], [ResponsibleRoleId], [IsBillable], [Sequence], [IsMilestone]) VALUES ('9', 'Report submitted', '8359141f-e423-4e48-8925-4624ba86245a', '0', '11', '1')
INSERT INTO #RawData ([Id], [Name], [ResponsibleRoleId], [IsBillable], [Sequence], [IsMilestone]) VALUES ('10', 'Final report confirmation received', '9eab89c0-225c-4027-9f42-cc35e5656b14', '0', '12', '1')
INSERT INTO #RawData ([Id], [Name], [ResponsibleRoleId], [IsBillable], [Sequence], [IsMilestone]) VALUES ('11', 'Doc Review', '9dd582a0-cf86-4fc0-8894-477266068c12', '1', '4', '0')
INSERT INTO #RawData ([Id], [Name], [ResponsibleRoleId], [IsBillable], [Sequence], [IsMilestone]) VALUES ('12', 'Intake', '9dd582a0-cf86-4fc0-8894-477266068c12', '1', '9', '0')
INSERT INTO #RawData ([Id], [Name], [ResponsibleRoleId], [IsBillable], [Sequence], [IsMilestone]) VALUES ('13', 'Addendum', '9dd582a0-cf86-4fc0-8894-477266068c12', '1', '1', '0')
INSERT INTO #RawData ([Id], [Name], [ResponsibleRoleId], [IsBillable], [Sequence], [IsMilestone]) VALUES ('14', 'Paper Review', '9dd582a0-cf86-4fc0-8894-477266068c12', '1', '1', '0')

	IF ((SELECT OBJECTPROPERTY( OBJECT_ID(N'[dbo].[Task]'), 'TableHasIdentity')) = 1)
		SET IDENTITY_INSERT #RawData OFF
	GO

	PRINT CONVERT(NVARCHAR(MAX),@@ROWCOUNT) + ' rows affected'
	GO
	
	
	DECLARE @CounterInProject INT,
			@CounterIdentical INT,
			@CounterUpdated INT,
			@CounterNew INT

	SELECT @CounterInProject = COUNT(*) FROM #RawData

	--This table keeps track of which rows in the source have already been checked/added or updated
	DECLARE @IdenticalRecordIDs TABLE(ID NVARCHAR(128))
	
	--Ignore any rows that are equal
	INSERT INTO @IdenticalRecordIDs
	SELECT s.[Id] 
	FROM #RawData s
	LEFT JOIN [dbo].[Task] d ON d.[Id] = s.[Id]
	WHERE d.[Name] = s.[Name] AND d.[ResponsibleRoleId] = s.[ResponsibleRoleId] AND d.[IsBillable] = s.[IsBillable] AND d.[Sequence] = s.[Sequence] AND d.[IsMilestone] = s.[IsMilestone]


	SELECT @CounterIdentical = COUNT(*) FROM @IdenticalRecordIDs


	--Update any rows that already exist
	UPDATE [dbo].[Task] SET [Name] = s.[Name], [ResponsibleRoleId] = s.[ResponsibleRoleId], [IsBillable] = s.[IsBillable], [Sequence] = s.[Sequence], [IsMilestone] = s.[IsMilestone]
		, ModifiedDate = GETDATE()
	, ModifiedUser = SUSER_NAME()
		OUTPUT Inserted.[Id] INTO @IdenticalRecordIDs
	FROM #RawData s
	LEFT JOIN [dbo].[Task] d ON s.[Id] = d.[Id] 
	WHERE s.[Id] NOT IN (SELECT id FROM @IdenticalRecordIDs)


	SELECT @CounterUpdated = COUNT(*) - @CounterIdentical FROM @IdenticalRecordIDs


	IF ((SELECT OBJECTPROPERTY( OBJECT_ID(N'[dbo].[Task]'), 'TableHasIdentity')) = 1)
		SET IDENTITY_INSERT [dbo].[Task] ON

	--Insert any new rows
	INSERT INTO [dbo].[Task] ([Id], [Name], [ResponsibleRoleId], [IsBillable], [Sequence], [IsMilestone]
	, ModifiedDate, ModifiedUser	)
	OUTPUT Inserted.[Id] INTO @IdenticalRecordIDs
	SELECT [Id], [Name], [ResponsibleRoleId], [IsBillable], [Sequence], [IsMilestone]
	, GETDATE(), SUSER_NAME()	FROM #RawData
	WHERE [Id] NOT IN (SELECT id FROM @IdenticalRecordIDs)

	IF ((SELECT OBJECTPROPERTY( OBJECT_ID(N'[dbo].[Task]'), 'TableHasIdentity')) = 1)
		SET IDENTITY_INSERT [dbo].[Task] OFF

	SELECT @CounterNew = COUNT(*) - @CounterIdentical - @CounterUpdated FROM @IdenticalRecordIDs

	PRINT 'Records in Project: '+ CONVERT(NVARCHAR(MAX),@CounterInProject)
	PRINT 'Identical Records: '+ CONVERT(NVARCHAR(MAX),@CounterIdentical)
	PRINT 'Updated Records: '+ CONVERT(NVARCHAR(MAX),@CounterUpdated)
	PRINT 'New Records: '+ CONVERT(NVARCHAR(MAX),@CounterNew)

	GO
--########################################################
--########### [dbo].[File]
--########################################################
	--Turn off the output of how many rows affected so output window isn't crowded
	SET NOCOUNT ON
	PRINT 'Updating table [dbo].[File]'

	IF OBJECT_ID('tempdb..#RawData') IS NOT NULL DROP TABLE #RawData
	GO

	/*
	Create a temporary table to load the source data
	This uses a select into from the destination table to create the temp table for the source.
	This resolves an issue where server colations are different between environments.
	This ensure the collation of the destination table is used for the temp table.
	WHERE 1=2 is used to return 0 records.
	*/
	SELECT 
		[Id],
		[DoctorId],
		[CompanyId]
	INTO #RawData
	FROM (
		SELECT 
			[Id],
			[DoctorId],
			[CompanyId]
		FROM [dbo].[File] 
		WHERE 1=2
	) t
	GO

	/* 
	Because the temp table is now created using a SELECT INTO it can be created with IDENTITY fields. 
	Turn on IDENTITY INSERT if they exist and turn it back off after you are done.
	*/
	IF ((SELECT OBJECTPROPERTY( OBJECT_ID(N'[dbo].[File]'), 'TableHasIdentity')) = 1)
		SET IDENTITY_INSERT #RawData ON
	GO

	--This is the data from the file at time of script generation
INSERT INTO #RawData ([Id], [DoctorId], [CompanyId]) VALUES ('1', 'E62EBEAD-C270-4711-81CB-2CBD6B8031AD', '2')

	IF ((SELECT OBJECTPROPERTY( OBJECT_ID(N'[dbo].[File]'), 'TableHasIdentity')) = 1)
		SET IDENTITY_INSERT #RawData OFF
	GO

	PRINT CONVERT(NVARCHAR(MAX),@@ROWCOUNT) + ' rows affected'
	GO
	
	
	DECLARE @CounterInProject INT,
			@CounterIdentical INT,
			@CounterUpdated INT,
			@CounterNew INT

	SELECT @CounterInProject = COUNT(*) FROM #RawData

	--This table keeps track of which rows in the source have already been checked/added or updated
	DECLARE @IdenticalRecordIDs TABLE(ID NVARCHAR(128))
	
	--Ignore any rows that are equal
	INSERT INTO @IdenticalRecordIDs
	SELECT s.[Id] 
	FROM #RawData s
	LEFT JOIN [dbo].[File] d ON d.[Id] = s.[Id]
	WHERE d.[DoctorId] = s.[DoctorId] AND d.[CompanyId] = s.[CompanyId]


	SELECT @CounterIdentical = COUNT(*) FROM @IdenticalRecordIDs


	--Update any rows that already exist
	UPDATE [dbo].[File] SET [DoctorId] = s.[DoctorId], [CompanyId] = s.[CompanyId]
		, ModifiedDate = GETDATE()
	, ModifiedUser = SUSER_NAME()
		OUTPUT Inserted.[Id] INTO @IdenticalRecordIDs
	FROM #RawData s
	LEFT JOIN [dbo].[File] d ON s.[Id] = d.[Id] 
	WHERE s.[Id] NOT IN (SELECT id FROM @IdenticalRecordIDs)


	SELECT @CounterUpdated = COUNT(*) - @CounterIdentical FROM @IdenticalRecordIDs


	IF ((SELECT OBJECTPROPERTY( OBJECT_ID(N'[dbo].[File]'), 'TableHasIdentity')) = 1)
		SET IDENTITY_INSERT [dbo].[File] ON

	--Insert any new rows
	INSERT INTO [dbo].[File] ([Id], [DoctorId], [CompanyId]
	, ModifiedDate, ModifiedUser	)
	OUTPUT Inserted.[Id] INTO @IdenticalRecordIDs
	SELECT [Id], [DoctorId], [CompanyId]
	, GETDATE(), SUSER_NAME()	FROM #RawData
	WHERE [Id] NOT IN (SELECT id FROM @IdenticalRecordIDs)

	IF ((SELECT OBJECTPROPERTY( OBJECT_ID(N'[dbo].[File]'), 'TableHasIdentity')) = 1)
		SET IDENTITY_INSERT [dbo].[File] OFF

	SELECT @CounterNew = COUNT(*) - @CounterIdentical - @CounterUpdated FROM @IdenticalRecordIDs

	PRINT 'Records in Project: '+ CONVERT(NVARCHAR(MAX),@CounterInProject)
	PRINT 'Identical Records: '+ CONVERT(NVARCHAR(MAX),@CounterIdentical)
	PRINT 'Updated Records: '+ CONVERT(NVARCHAR(MAX),@CounterUpdated)
	PRINT 'New Records: '+ CONVERT(NVARCHAR(MAX),@CounterNew)

	GO
--########################################################
--########### [dbo].[Job]
--########################################################
	--Turn off the output of how many rows affected so output window isn't crowded
	SET NOCOUNT ON
	PRINT 'Updating table [dbo].[Job]'

	IF OBJECT_ID('tempdb..#RawData') IS NOT NULL DROP TABLE #RawData
	GO

	/*
	Create a temporary table to load the source data
	This uses a select into from the destination table to create the temp table for the source.
	This resolves an issue where server colations are different between environments.
	This ensure the collation of the destination table is used for the temp table.
	WHERE 1=2 is used to return 0 records.
	*/
	SELECT 
		[Id],
		[FileId],
		[ServiceId],
		[DueDate],
		[StartTime],
		[EndTime],
		[Price]
	INTO #RawData
	FROM (
		SELECT 
			[Id],
			[FileId],
			[ServiceId],
			[DueDate],
			[StartTime],
			[EndTime],
			[Price]
		FROM [dbo].[Job] 
		WHERE 1=2
	) t
	GO

	/* 
	Because the temp table is now created using a SELECT INTO it can be created with IDENTITY fields. 
	Turn on IDENTITY INSERT if they exist and turn it back off after you are done.
	*/
	IF ((SELECT OBJECTPROPERTY( OBJECT_ID(N'[dbo].[Job]'), 'TableHasIdentity')) = 1)
		SET IDENTITY_INSERT #RawData ON
	GO

	--This is the data from the file at time of script generation
INSERT INTO #RawData ([Id], [FileId], [ServiceId], [DueDate], [StartTime], [EndTime], [Price]) VALUES ('1', '1', '7', '2015-04-29', '11:00:00', '11:30:00', '1000.00')
INSERT INTO #RawData ([Id], [FileId], [ServiceId], [DueDate], [Price]) VALUES ('2', '1', '17', '2015-05-29', '400.00')

	IF ((SELECT OBJECTPROPERTY( OBJECT_ID(N'[dbo].[Job]'), 'TableHasIdentity')) = 1)
		SET IDENTITY_INSERT #RawData OFF
	GO

	PRINT CONVERT(NVARCHAR(MAX),@@ROWCOUNT) + ' rows affected'
	GO
	
	
	DECLARE @CounterInProject INT,
			@CounterIdentical INT,
			@CounterUpdated INT,
			@CounterNew INT

	SELECT @CounterInProject = COUNT(*) FROM #RawData

	--This table keeps track of which rows in the source have already been checked/added or updated
	DECLARE @IdenticalRecordIDs TABLE(ID NVARCHAR(128))
	
	--Ignore any rows that are equal
	INSERT INTO @IdenticalRecordIDs
	SELECT s.[Id] 
	FROM #RawData s
	LEFT JOIN [dbo].[Job] d ON d.[Id] = s.[Id]
	WHERE d.[FileId] = s.[FileId] AND d.[ServiceId] = s.[ServiceId] AND d.[DueDate] = s.[DueDate] AND d.[StartTime] = s.[StartTime] AND d.[EndTime] = s.[EndTime] AND d.[Price] = s.[Price]


	SELECT @CounterIdentical = COUNT(*) FROM @IdenticalRecordIDs


	--Update any rows that already exist
	UPDATE [dbo].[Job] SET [FileId] = s.[FileId], [ServiceId] = s.[ServiceId], [DueDate] = s.[DueDate], [StartTime] = s.[StartTime], [EndTime] = s.[EndTime], [Price] = s.[Price]
		, ModifiedDate = GETDATE()
	, ModifiedUser = SUSER_NAME()
		OUTPUT Inserted.[Id] INTO @IdenticalRecordIDs
	FROM #RawData s
	LEFT JOIN [dbo].[Job] d ON s.[Id] = d.[Id] 
	WHERE s.[Id] NOT IN (SELECT id FROM @IdenticalRecordIDs)


	SELECT @CounterUpdated = COUNT(*) - @CounterIdentical FROM @IdenticalRecordIDs


	IF ((SELECT OBJECTPROPERTY( OBJECT_ID(N'[dbo].[Job]'), 'TableHasIdentity')) = 1)
		SET IDENTITY_INSERT [dbo].[Job] ON

	--Insert any new rows
	INSERT INTO [dbo].[Job] ([Id], [FileId], [ServiceId], [DueDate], [StartTime], [EndTime], [Price]
	, ModifiedDate, ModifiedUser	)
	OUTPUT Inserted.[Id] INTO @IdenticalRecordIDs
	SELECT [Id], [FileId], [ServiceId], [DueDate], [StartTime], [EndTime], [Price]
	, GETDATE(), SUSER_NAME()	FROM #RawData
	WHERE [Id] NOT IN (SELECT id FROM @IdenticalRecordIDs)

	IF ((SELECT OBJECTPROPERTY( OBJECT_ID(N'[dbo].[Job]'), 'TableHasIdentity')) = 1)
		SET IDENTITY_INSERT [dbo].[Job] OFF

	SELECT @CounterNew = COUNT(*) - @CounterIdentical - @CounterUpdated FROM @IdenticalRecordIDs

	PRINT 'Records in Project: '+ CONVERT(NVARCHAR(MAX),@CounterInProject)
	PRINT 'Identical Records: '+ CONVERT(NVARCHAR(MAX),@CounterIdentical)
	PRINT 'Updated Records: '+ CONVERT(NVARCHAR(MAX),@CounterUpdated)
	PRINT 'New Records: '+ CONVERT(NVARCHAR(MAX),@CounterNew)

	GO
--########################################################
--########### [dbo].[JobTask]
--########################################################
	--Turn off the output of how many rows affected so output window isn't crowded
	SET NOCOUNT ON
	PRINT 'Updating table [dbo].[JobTask]'

	IF OBJECT_ID('tempdb..#RawData') IS NOT NULL DROP TABLE #RawData
	GO

	/*
	Create a temporary table to load the source data
	This uses a select into from the destination table to create the temp table for the source.
	This resolves an issue where server colations are different between environments.
	This ensure the collation of the destination table is used for the temp table.
	WHERE 1=2 is used to return 0 records.
	*/
	SELECT 
		[Id],
		[TaskId],
		[JobId],
		[DurationFromDueDateInDays],
		[ResponsibleRoleId],
		[EmployeeId],
		[RoleLevelId],
		[IsBillable],
		[IsMilestone],
		[BillableHourCategoryId],
		[StatusId]
	INTO #RawData
	FROM (
		SELECT 
			[Id],
			[TaskId],
			[JobId],
			[DurationFromDueDateInDays],
			[ResponsibleRoleId],
			[EmployeeId],
			[RoleLevelId],
			[IsBillable],
			[IsMilestone],
			[BillableHourCategoryId],
			[StatusId]
		FROM [dbo].[JobTask] 
		WHERE 1=2
	) t
	GO

	/* 
	Because the temp table is now created using a SELECT INTO it can be created with IDENTITY fields. 
	Turn on IDENTITY INSERT if they exist and turn it back off after you are done.
	*/
	IF ((SELECT OBJECTPROPERTY( OBJECT_ID(N'[dbo].[JobTask]'), 'TableHasIdentity')) = 1)
		SET IDENTITY_INSERT #RawData ON
	GO

	--This is the data from the file at time of script generation
INSERT INTO #RawData ([Id], [TaskId], [JobId], [DurationFromDueDateInDays], [ResponsibleRoleId], [EmployeeId], [RoleLevelId], [IsBillable], [IsMilestone]) VALUES ('1', '3', '1', '-2', '7b930663-b091-44ca-924c-d8b11a1ee7ea', '491841d0-dd83-482a-b710-476397e03e96', '3', '0', '1')
INSERT INTO #RawData ([Id], [TaskId], [JobId], [DurationFromDueDateInDays], [ResponsibleRoleId], [EmployeeId], [RoleLevelId], [IsBillable], [BillableHourCategoryId], [IsMilestone], [StatusId]) VALUES ('2', '11', '1', '-2', '9dd582a0-cf86-4fc0-8894-477266068c12', '5b92cd8b-a500-428f-88a2-ad14e0a23624', '5', '1', '6', '0', '3')
INSERT INTO #RawData ([Id], [TaskId], [JobId], [DurationFromDueDateInDays], [ResponsibleRoleId], [EmployeeId], [IsBillable], [IsMilestone]) VALUES ('3', '9', '1', '0', '8359141f-e423-4e48-8925-4624ba86245a', 'e62ebead-c270-4711-81cb-2cbd6b8031ad', '0', '1')

	IF ((SELECT OBJECTPROPERTY( OBJECT_ID(N'[dbo].[JobTask]'), 'TableHasIdentity')) = 1)
		SET IDENTITY_INSERT #RawData OFF
	GO

	PRINT CONVERT(NVARCHAR(MAX),@@ROWCOUNT) + ' rows affected'
	GO
	
	
	DECLARE @CounterInProject INT,
			@CounterIdentical INT,
			@CounterUpdated INT,
			@CounterNew INT

	SELECT @CounterInProject = COUNT(*) FROM #RawData

	--This table keeps track of which rows in the source have already been checked/added or updated
	DECLARE @IdenticalRecordIDs TABLE(ID NVARCHAR(128))
	
	--Ignore any rows that are equal
	INSERT INTO @IdenticalRecordIDs
	SELECT s.[Id] 
	FROM #RawData s
	LEFT JOIN [dbo].[JobTask] d ON d.[Id] = s.[Id]
	WHERE d.[TaskId] = s.[TaskId] AND d.[JobId] = s.[JobId] AND d.[DurationFromDueDateInDays] = s.[DurationFromDueDateInDays] AND d.[ResponsibleRoleId] = s.[ResponsibleRoleId] AND d.[EmployeeId] = s.[EmployeeId] AND d.[RoleLevelId] = s.[RoleLevelId] AND d.[IsBillable] = s.[IsBillable] AND d.[IsMilestone] = s.[IsMilestone] AND d.[BillableHourCategoryId] = s.[BillableHourCategoryId] AND d.[StatusId] = s.[StatusId]


	SELECT @CounterIdentical = COUNT(*) FROM @IdenticalRecordIDs


	--Update any rows that already exist
	UPDATE [dbo].[JobTask] SET [TaskId] = s.[TaskId], [JobId] = s.[JobId], [DurationFromDueDateInDays] = s.[DurationFromDueDateInDays], [ResponsibleRoleId] = s.[ResponsibleRoleId], [EmployeeId] = s.[EmployeeId], [RoleLevelId] = s.[RoleLevelId], [IsBillable] = s.[IsBillable], [IsMilestone] = s.[IsMilestone], [BillableHourCategoryId] = s.[BillableHourCategoryId], [StatusId] = s.[StatusId]
		, ModifiedDate = GETDATE()
	, ModifiedUser = SUSER_NAME()
		OUTPUT Inserted.[Id] INTO @IdenticalRecordIDs
	FROM #RawData s
	LEFT JOIN [dbo].[JobTask] d ON s.[Id] = d.[Id] 
	WHERE s.[Id] NOT IN (SELECT id FROM @IdenticalRecordIDs)


	SELECT @CounterUpdated = COUNT(*) - @CounterIdentical FROM @IdenticalRecordIDs


	IF ((SELECT OBJECTPROPERTY( OBJECT_ID(N'[dbo].[JobTask]'), 'TableHasIdentity')) = 1)
		SET IDENTITY_INSERT [dbo].[JobTask] ON

	--Insert any new rows
	INSERT INTO [dbo].[JobTask] ([Id], [TaskId], [JobId], [DurationFromDueDateInDays], [ResponsibleRoleId], [EmployeeId], [RoleLevelId], [IsBillable], [IsMilestone], [BillableHourCategoryId], [StatusId]
	, ModifiedDate, ModifiedUser	)
	OUTPUT Inserted.[Id] INTO @IdenticalRecordIDs
	SELECT [Id], [TaskId], [JobId], [DurationFromDueDateInDays], [ResponsibleRoleId], [EmployeeId], [RoleLevelId], [IsBillable], [IsMilestone], [BillableHourCategoryId], [StatusId]
	, GETDATE(), SUSER_NAME()	FROM #RawData
	WHERE [Id] NOT IN (SELECT id FROM @IdenticalRecordIDs)

	IF ((SELECT OBJECTPROPERTY( OBJECT_ID(N'[dbo].[JobTask]'), 'TableHasIdentity')) = 1)
		SET IDENTITY_INSERT [dbo].[JobTask] OFF

	SELECT @CounterNew = COUNT(*) - @CounterIdentical - @CounterUpdated FROM @IdenticalRecordIDs

	PRINT 'Records in Project: '+ CONVERT(NVARCHAR(MAX),@CounterInProject)
	PRINT 'Identical Records: '+ CONVERT(NVARCHAR(MAX),@CounterIdentical)
	PRINT 'Updated Records: '+ CONVERT(NVARCHAR(MAX),@CounterUpdated)
	PRINT 'New Records: '+ CONVERT(NVARCHAR(MAX),@CounterNew)

	GO
--########################################################
--########### [dbo].[ServiceCatalogue]
--########################################################
	--Turn off the output of how many rows affected so output window isn't crowded
	SET NOCOUNT ON
	PRINT 'Updating table [dbo].[ServiceCatalogue]'

	IF OBJECT_ID('tempdb..#RawData') IS NOT NULL DROP TABLE #RawData
	GO

	/*
	Create a temporary table to load the source data
	This uses a select into from the destination table to create the temp table for the source.
	This resolves an issue where server colations are different between environments.
	This ensure the collation of the destination table is used for the temp table.
	WHERE 1=2 is used to return 0 records.
	*/
	SELECT 
		[Id],
		[DoctorId],
		[ServiceId],
		[CompanyId],
		[Rate]
	INTO #RawData
	FROM (
		SELECT 
			[Id],
			[DoctorId],
			[ServiceId],
			[CompanyId],
			[Rate]
		FROM [dbo].[ServiceCatalogue] 
		WHERE 1=2
	) t
	GO

	/* 
	Because the temp table is now created using a SELECT INTO it can be created with IDENTITY fields. 
	Turn on IDENTITY INSERT if they exist and turn it back off after you are done.
	*/
	IF ((SELECT OBJECTPROPERTY( OBJECT_ID(N'[dbo].[ServiceCatalogue]'), 'TableHasIdentity')) = 1)
		SET IDENTITY_INSERT #RawData ON
	GO

	--This is the data from the file at time of script generation
INSERT INTO #RawData ([Id], [DoctorId], [ServiceId], [CompanyId], [Rate]) VALUES ('1', 'e62ebead-c270-4711-81cb-2cbd6b8031ad', '7', '2', '1000.00')

	IF ((SELECT OBJECTPROPERTY( OBJECT_ID(N'[dbo].[ServiceCatalogue]'), 'TableHasIdentity')) = 1)
		SET IDENTITY_INSERT #RawData OFF
	GO

	PRINT CONVERT(NVARCHAR(MAX),@@ROWCOUNT) + ' rows affected'
	GO
	
	
	DECLARE @CounterInProject INT,
			@CounterIdentical INT,
			@CounterUpdated INT,
			@CounterNew INT

	SELECT @CounterInProject = COUNT(*) FROM #RawData

	--This table keeps track of which rows in the source have already been checked/added or updated
	DECLARE @IdenticalRecordIDs TABLE(ID NVARCHAR(128))
	
	--Ignore any rows that are equal
	INSERT INTO @IdenticalRecordIDs
	SELECT s.[Id] 
	FROM #RawData s
	LEFT JOIN [dbo].[ServiceCatalogue] d ON d.[Id] = s.[Id]
	WHERE d.[DoctorId] = s.[DoctorId] AND d.[ServiceId] = s.[ServiceId] AND d.[CompanyId] = s.[CompanyId] AND d.[Rate] = s.[Rate]


	SELECT @CounterIdentical = COUNT(*) FROM @IdenticalRecordIDs


	--Update any rows that already exist
	UPDATE [dbo].[ServiceCatalogue] SET [DoctorId] = s.[DoctorId], [ServiceId] = s.[ServiceId], [CompanyId] = s.[CompanyId], [Rate] = s.[Rate]
		, ModifiedDate = GETDATE()
	, ModifiedUser = SUSER_NAME()
		OUTPUT Inserted.[Id] INTO @IdenticalRecordIDs
	FROM #RawData s
	LEFT JOIN [dbo].[ServiceCatalogue] d ON s.[Id] = d.[Id] 
	WHERE s.[Id] NOT IN (SELECT id FROM @IdenticalRecordIDs)


	SELECT @CounterUpdated = COUNT(*) - @CounterIdentical FROM @IdenticalRecordIDs


	IF ((SELECT OBJECTPROPERTY( OBJECT_ID(N'[dbo].[ServiceCatalogue]'), 'TableHasIdentity')) = 1)
		SET IDENTITY_INSERT [dbo].[ServiceCatalogue] ON

	--Insert any new rows
	INSERT INTO [dbo].[ServiceCatalogue] ([Id], [DoctorId], [ServiceId], [CompanyId], [Rate]
	, ModifiedDate, ModifiedUser	)
	OUTPUT Inserted.[Id] INTO @IdenticalRecordIDs
	SELECT [Id], [DoctorId], [ServiceId], [CompanyId], [Rate]
	, GETDATE(), SUSER_NAME()	FROM #RawData
	WHERE [Id] NOT IN (SELECT id FROM @IdenticalRecordIDs)

	IF ((SELECT OBJECTPROPERTY( OBJECT_ID(N'[dbo].[ServiceCatalogue]'), 'TableHasIdentity')) = 1)
		SET IDENTITY_INSERT [dbo].[ServiceCatalogue] OFF

	SELECT @CounterNew = COUNT(*) - @CounterIdentical - @CounterUpdated FROM @IdenticalRecordIDs

	PRINT 'Records in Project: '+ CONVERT(NVARCHAR(MAX),@CounterInProject)
	PRINT 'Identical Records: '+ CONVERT(NVARCHAR(MAX),@CounterIdentical)
	PRINT 'Updated Records: '+ CONVERT(NVARCHAR(MAX),@CounterUpdated)
	PRINT 'New Records: '+ CONVERT(NVARCHAR(MAX),@CounterNew)

	GO

