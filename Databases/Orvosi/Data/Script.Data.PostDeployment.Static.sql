 
--This file is auto-generated, any changes made to it will be overwritten when it's regenerated
--Read the readme.md for more information


--########################################################
--########### [dbo].[AddressType]
--########################################################
	--Turn off the output of how many rows affected so output window isn't crowded
	SET NOCOUNT ON
	PRINT 'Updating table [dbo].[AddressType]'

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
		[AddressTypeID],
		[Name]
	INTO #RawData
	FROM (
		SELECT 
			[AddressTypeID],
			[Name]
		FROM [dbo].[AddressType] 
		WHERE 1=2
	) t
	GO

	/* 
	Because the temp table is now created using a SELECT INTO it can be created with IDENTITY fields. 
	Turn on IDENTITY INSERT if they exist and turn it back off after you are done.
	*/
	IF ((SELECT OBJECTPROPERTY( OBJECT_ID(N'[dbo].[AddressType]'), 'TableHasIdentity')) = 1)
		SET IDENTITY_INSERT #RawData ON
	GO

	--This is the data from the file at time of script generation
INSERT INTO #RawData ([AddressTypeID], [Name]) VALUES ('1', 'Main')

	IF ((SELECT OBJECTPROPERTY( OBJECT_ID(N'[dbo].[AddressType]'), 'TableHasIdentity')) = 1)
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
	SELECT s.[AddressTypeID] 
	FROM #RawData s
	LEFT JOIN [dbo].[AddressType] d ON d.[AddressTypeID] = s.[AddressTypeID]
	WHERE d.[Name] = s.[Name]


	SELECT @CounterIdentical = COUNT(*) FROM @IdenticalRecordIDs


	--Update any rows that already exist
	UPDATE [dbo].[AddressType] SET [Name] = s.[Name]
		, ModifiedDate = GETDATE()
	, ModifiedUser = SUSER_NAME()
		OUTPUT Inserted.[AddressTypeID] INTO @IdenticalRecordIDs
	FROM #RawData s
	LEFT JOIN [dbo].[AddressType] d ON s.[AddressTypeID] = d.[AddressTypeID] 
	WHERE s.[AddressTypeID] NOT IN (SELECT id FROM @IdenticalRecordIDs)


	SELECT @CounterUpdated = COUNT(*) - @CounterIdentical FROM @IdenticalRecordIDs


	IF ((SELECT OBJECTPROPERTY( OBJECT_ID(N'[dbo].[AddressType]'), 'TableHasIdentity')) = 1)
		SET IDENTITY_INSERT [dbo].[AddressType] ON

	--Insert any new rows
	INSERT INTO [dbo].[AddressType] ([AddressTypeID], [Name]
	, ModifiedDate, ModifiedUser	)
	OUTPUT Inserted.[AddressTypeID] INTO @IdenticalRecordIDs
	SELECT [AddressTypeID], [Name]
	, GETDATE(), SUSER_NAME()	FROM #RawData
	WHERE [AddressTypeID] NOT IN (SELECT id FROM @IdenticalRecordIDs)

	IF ((SELECT OBJECTPROPERTY( OBJECT_ID(N'[dbo].[AddressType]'), 'TableHasIdentity')) = 1)
		SET IDENTITY_INSERT [dbo].[AddressType] OFF

	SELECT @CounterNew = COUNT(*) - @CounterIdentical - @CounterUpdated FROM @IdenticalRecordIDs

	PRINT 'Records in Project: '+ CONVERT(NVARCHAR(MAX),@CounterInProject)
	PRINT 'Identical Records: '+ CONVERT(NVARCHAR(MAX),@CounterIdentical)
	PRINT 'Updated Records: '+ CONVERT(NVARCHAR(MAX),@CounterUpdated)
	PRINT 'New Records: '+ CONVERT(NVARCHAR(MAX),@CounterNew)

	GO
--########################################################
--########### [dbo].[AspNetRoles]
--########################################################
	--Turn off the output of how many rows affected so output window isn't crowded
	SET NOCOUNT ON
	PRINT 'Updating table [dbo].[AspNetRoles]'

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
		[Name]
	INTO #RawData
	FROM (
		SELECT 
			[Id],
			[Name]
		FROM [dbo].[AspNetRoles] 
		WHERE 1=2
	) t
	GO

	/* 
	Because the temp table is now created using a SELECT INTO it can be created with IDENTITY fields. 
	Turn on IDENTITY INSERT if they exist and turn it back off after you are done.
	*/
	IF ((SELECT OBJECTPROPERTY( OBJECT_ID(N'[dbo].[AspNetRoles]'), 'TableHasIdentity')) = 1)
		SET IDENTITY_INSERT #RawData ON
	GO

	--This is the data from the file at time of script generation
INSERT INTO #RawData ([Id], [Name]) VALUES ('5e2cfba4-417a-4685-8e2b-970bd7061cd9', 'Claimant')
INSERT INTO #RawData ([Id], [Name]) VALUES ('7b930663-b091-44ca-924c-d8b11a1ee7ea', 'Company')
INSERT INTO #RawData ([Id], [Name]) VALUES ('7fab67dd-286b-492f-865a-0cb0ce1261ce', 'Super Admin')
INSERT INTO #RawData ([Id], [Name]) VALUES ('8359141f-e423-4e48-8925-4624ba86245a', 'Doctor')
INSERT INTO #RawData ([Id], [Name]) VALUES ('9dd582a0-cf86-4fc0-8894-477266068c12', 'Intake Coordinator')
INSERT INTO #RawData ([Id], [Name]) VALUES ('9eab89c0-225c-4027-9f42-cc35e5656b14', 'Administrator')

	IF ((SELECT OBJECTPROPERTY( OBJECT_ID(N'[dbo].[AspNetRoles]'), 'TableHasIdentity')) = 1)
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
	LEFT JOIN [dbo].[AspNetRoles] d ON d.[Id] = s.[Id]
	WHERE d.[Name] = s.[Name]


	SELECT @CounterIdentical = COUNT(*) FROM @IdenticalRecordIDs


	--Update any rows that already exist
	UPDATE [dbo].[AspNetRoles] SET [Name] = s.[Name]
		, ModifiedDate = GETDATE()
	, ModifiedUser = SUSER_NAME()
		OUTPUT Inserted.[Id] INTO @IdenticalRecordIDs
	FROM #RawData s
	LEFT JOIN [dbo].[AspNetRoles] d ON s.[Id] = d.[Id] 
	WHERE s.[Id] NOT IN (SELECT id FROM @IdenticalRecordIDs)


	SELECT @CounterUpdated = COUNT(*) - @CounterIdentical FROM @IdenticalRecordIDs


	IF ((SELECT OBJECTPROPERTY( OBJECT_ID(N'[dbo].[AspNetRoles]'), 'TableHasIdentity')) = 1)
		SET IDENTITY_INSERT [dbo].[AspNetRoles] ON

	--Insert any new rows
	INSERT INTO [dbo].[AspNetRoles] ([Id], [Name]
	, ModifiedDate, ModifiedUser	)
	OUTPUT Inserted.[Id] INTO @IdenticalRecordIDs
	SELECT [Id], [Name]
	, GETDATE(), SUSER_NAME()	FROM #RawData
	WHERE [Id] NOT IN (SELECT id FROM @IdenticalRecordIDs)

	IF ((SELECT OBJECTPROPERTY( OBJECT_ID(N'[dbo].[AspNetRoles]'), 'TableHasIdentity')) = 1)
		SET IDENTITY_INSERT [dbo].[AspNetRoles] OFF

	SELECT @CounterNew = COUNT(*) - @CounterIdentical - @CounterUpdated FROM @IdenticalRecordIDs

	PRINT 'Records in Project: '+ CONVERT(NVARCHAR(MAX),@CounterInProject)
	PRINT 'Identical Records: '+ CONVERT(NVARCHAR(MAX),@CounterIdentical)
	PRINT 'Updated Records: '+ CONVERT(NVARCHAR(MAX),@CounterUpdated)
	PRINT 'New Records: '+ CONVERT(NVARCHAR(MAX),@CounterNew)

	GO
--########################################################
--########### [dbo].[ConfigurationType]
--########################################################
	--Turn off the output of how many rows affected so output window isn't crowded
	SET NOCOUNT ON
	PRINT 'Updating table [dbo].[ConfigurationType]'

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
		[ShortName]
	INTO #RawData
	FROM (
		SELECT 
			[Id],
			[Name],
			[ShortName]
		FROM [dbo].[ConfigurationType] 
		WHERE 1=2
	) t
	GO

	/* 
	Because the temp table is now created using a SELECT INTO it can be created with IDENTITY fields. 
	Turn on IDENTITY INSERT if they exist and turn it back off after you are done.
	*/
	IF ((SELECT OBJECTPROPERTY( OBJECT_ID(N'[dbo].[ConfigurationType]'), 'TableHasIdentity')) = 1)
		SET IDENTITY_INSERT #RawData ON
	GO

	--This is the data from the file at time of script generation
INSERT INTO #RawData ([Id], [Name], [ShortName]) VALUES ('1', 'Doctor Recruitment Plan', 'DR')
INSERT INTO #RawData ([Id], [Name], [ShortName]) VALUES ('2', 'Assessment Plan', 'DCP')

	IF ((SELECT OBJECTPROPERTY( OBJECT_ID(N'[dbo].[ConfigurationType]'), 'TableHasIdentity')) = 1)
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
	LEFT JOIN [dbo].[ConfigurationType] d ON d.[Id] = s.[Id]
	WHERE d.[Name] = s.[Name] AND d.[ShortName] = s.[ShortName]


	SELECT @CounterIdentical = COUNT(*) FROM @IdenticalRecordIDs


	--Update any rows that already exist
	UPDATE [dbo].[ConfigurationType] SET [Name] = s.[Name], [ShortName] = s.[ShortName]
		, ModifiedDate = GETDATE()
	, ModifiedUser = SUSER_NAME()
		OUTPUT Inserted.[Id] INTO @IdenticalRecordIDs
	FROM #RawData s
	LEFT JOIN [dbo].[ConfigurationType] d ON s.[Id] = d.[Id] 
	WHERE s.[Id] NOT IN (SELECT id FROM @IdenticalRecordIDs)


	SELECT @CounterUpdated = COUNT(*) - @CounterIdentical FROM @IdenticalRecordIDs


	IF ((SELECT OBJECTPROPERTY( OBJECT_ID(N'[dbo].[ConfigurationType]'), 'TableHasIdentity')) = 1)
		SET IDENTITY_INSERT [dbo].[ConfigurationType] ON

	--Insert any new rows
	INSERT INTO [dbo].[ConfigurationType] ([Id], [Name], [ShortName]
	, ModifiedDate, ModifiedUser	)
	OUTPUT Inserted.[Id] INTO @IdenticalRecordIDs
	SELECT [Id], [Name], [ShortName]
	, GETDATE(), SUSER_NAME()	FROM #RawData
	WHERE [Id] NOT IN (SELECT id FROM @IdenticalRecordIDs)

	IF ((SELECT OBJECTPROPERTY( OBJECT_ID(N'[dbo].[ConfigurationType]'), 'TableHasIdentity')) = 1)
		SET IDENTITY_INSERT [dbo].[ConfigurationType] OFF

	SELECT @CounterNew = COUNT(*) - @CounterIdentical - @CounterUpdated FROM @IdenticalRecordIDs

	PRINT 'Records in Project: '+ CONVERT(NVARCHAR(MAX),@CounterInProject)
	PRINT 'Identical Records: '+ CONVERT(NVARCHAR(MAX),@CounterIdentical)
	PRINT 'Updated Records: '+ CONVERT(NVARCHAR(MAX),@CounterUpdated)
	PRINT 'New Records: '+ CONVERT(NVARCHAR(MAX),@CounterNew)

	GO
--########################################################
--########### [dbo].[EntityType]
--########################################################
	--Turn off the output of how many rows affected so output window isn't crowded
	SET NOCOUNT ON
	PRINT 'Updating table [dbo].[EntityType]'

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
		[Name]
	INTO #RawData
	FROM (
		SELECT 
			[Id],
			[Name]
		FROM [dbo].[EntityType] 
		WHERE 1=2
	) t
	GO

	/* 
	Because the temp table is now created using a SELECT INTO it can be created with IDENTITY fields. 
	Turn on IDENTITY INSERT if they exist and turn it back off after you are done.
	*/
	IF ((SELECT OBJECTPROPERTY( OBJECT_ID(N'[dbo].[EntityType]'), 'TableHasIdentity')) = 1)
		SET IDENTITY_INSERT #RawData ON
	GO

	--This is the data from the file at time of script generation
INSERT INTO #RawData ([Id], [Name]) VALUES ('1', 'Orvosi')

	IF ((SELECT OBJECTPROPERTY( OBJECT_ID(N'[dbo].[EntityType]'), 'TableHasIdentity')) = 1)
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
	LEFT JOIN [dbo].[EntityType] d ON d.[Id] = s.[Id]
	WHERE d.[Name] = s.[Name]


	SELECT @CounterIdentical = COUNT(*) FROM @IdenticalRecordIDs


	--Update any rows that already exist
	UPDATE [dbo].[EntityType] SET [Name] = s.[Name]
		, ModifiedDate = GETDATE()
	, ModifiedUser = SUSER_NAME()
		OUTPUT Inserted.[Id] INTO @IdenticalRecordIDs
	FROM #RawData s
	LEFT JOIN [dbo].[EntityType] d ON s.[Id] = d.[Id] 
	WHERE s.[Id] NOT IN (SELECT id FROM @IdenticalRecordIDs)


	SELECT @CounterUpdated = COUNT(*) - @CounterIdentical FROM @IdenticalRecordIDs


	IF ((SELECT OBJECTPROPERTY( OBJECT_ID(N'[dbo].[EntityType]'), 'TableHasIdentity')) = 1)
		SET IDENTITY_INSERT [dbo].[EntityType] ON

	--Insert any new rows
	INSERT INTO [dbo].[EntityType] ([Id], [Name]
	, ModifiedDate, ModifiedUser	)
	OUTPUT Inserted.[Id] INTO @IdenticalRecordIDs
	SELECT [Id], [Name]
	, GETDATE(), SUSER_NAME()	FROM #RawData
	WHERE [Id] NOT IN (SELECT id FROM @IdenticalRecordIDs)

	IF ((SELECT OBJECTPROPERTY( OBJECT_ID(N'[dbo].[EntityType]'), 'TableHasIdentity')) = 1)
		SET IDENTITY_INSERT [dbo].[EntityType] OFF

	SELECT @CounterNew = COUNT(*) - @CounterIdentical - @CounterUpdated FROM @IdenticalRecordIDs

	PRINT 'Records in Project: '+ CONVERT(NVARCHAR(MAX),@CounterInProject)
	PRINT 'Identical Records: '+ CONVERT(NVARCHAR(MAX),@CounterIdentical)
	PRINT 'Updated Records: '+ CONVERT(NVARCHAR(MAX),@CounterUpdated)
	PRINT 'New Records: '+ CONVERT(NVARCHAR(MAX),@CounterNew)

	GO
--########################################################
--########### [dbo].[Status]
--########################################################
	--Turn off the output of how many rows affected so output window isn't crowded
	SET NOCOUNT ON
	PRINT 'Updating table [dbo].[Status]'

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
		[Name]
	INTO #RawData
	FROM (
		SELECT 
			[Id],
			[Name]
		FROM [dbo].[Status] 
		WHERE 1=2
	) t
	GO

	/* 
	Because the temp table is now created using a SELECT INTO it can be created with IDENTITY fields. 
	Turn on IDENTITY INSERT if they exist and turn it back off after you are done.
	*/
	IF ((SELECT OBJECTPROPERTY( OBJECT_ID(N'[dbo].[Status]'), 'TableHasIdentity')) = 1)
		SET IDENTITY_INSERT #RawData ON
	GO

	--This is the data from the file at time of script generation
INSERT INTO #RawData ([Id], [Name]) VALUES ('1', 'Not Started')
INSERT INTO #RawData ([Id], [Name]) VALUES ('2', 'In Progress')
INSERT INTO #RawData ([Id], [Name]) VALUES ('3', 'Completed')

	IF ((SELECT OBJECTPROPERTY( OBJECT_ID(N'[dbo].[Status]'), 'TableHasIdentity')) = 1)
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
	LEFT JOIN [dbo].[Status] d ON d.[Id] = s.[Id]
	WHERE d.[Name] = s.[Name]


	SELECT @CounterIdentical = COUNT(*) FROM @IdenticalRecordIDs


	--Update any rows that already exist
	UPDATE [dbo].[Status] SET [Name] = s.[Name]
		, ModifiedDate = GETDATE()
	, ModifiedUser = SUSER_NAME()
		OUTPUT Inserted.[Id] INTO @IdenticalRecordIDs
	FROM #RawData s
	LEFT JOIN [dbo].[Status] d ON s.[Id] = d.[Id] 
	WHERE s.[Id] NOT IN (SELECT id FROM @IdenticalRecordIDs)


	SELECT @CounterUpdated = COUNT(*) - @CounterIdentical FROM @IdenticalRecordIDs


	IF ((SELECT OBJECTPROPERTY( OBJECT_ID(N'[dbo].[Status]'), 'TableHasIdentity')) = 1)
		SET IDENTITY_INSERT [dbo].[Status] ON

	--Insert any new rows
	INSERT INTO [dbo].[Status] ([Id], [Name]
	, ModifiedDate, ModifiedUser	)
	OUTPUT Inserted.[Id] INTO @IdenticalRecordIDs
	SELECT [Id], [Name]
	, GETDATE(), SUSER_NAME()	FROM #RawData
	WHERE [Id] NOT IN (SELECT id FROM @IdenticalRecordIDs)

	IF ((SELECT OBJECTPROPERTY( OBJECT_ID(N'[dbo].[Status]'), 'TableHasIdentity')) = 1)
		SET IDENTITY_INSERT [dbo].[Status] OFF

	SELECT @CounterNew = COUNT(*) - @CounterIdentical - @CounterUpdated FROM @IdenticalRecordIDs

	PRINT 'Records in Project: '+ CONVERT(NVARCHAR(MAX),@CounterInProject)
	PRINT 'Identical Records: '+ CONVERT(NVARCHAR(MAX),@CounterIdentical)
	PRINT 'Updated Records: '+ CONVERT(NVARCHAR(MAX),@CounterUpdated)
	PRINT 'New Records: '+ CONVERT(NVARCHAR(MAX),@CounterNew)

	GO
--########################################################
--########### [dbo].[BillableHourCategory]
--########################################################
	--Turn off the output of how many rows affected so output window isn't crowded
	SET NOCOUNT ON
	PRINT 'Updating table [dbo].[BillableHourCategory]'

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
		[Hours],
		[Code]
	INTO #RawData
	FROM (
		SELECT 
			[Id],
			[Hours],
			[Code]
		FROM [dbo].[BillableHourCategory] 
		WHERE 1=2
	) t
	GO

	/* 
	Because the temp table is now created using a SELECT INTO it can be created with IDENTITY fields. 
	Turn on IDENTITY INSERT if they exist and turn it back off after you are done.
	*/
	IF ((SELECT OBJECTPROPERTY( OBJECT_ID(N'[dbo].[BillableHourCategory]'), 'TableHasIdentity')) = 1)
		SET IDENTITY_INSERT #RawData ON
	GO

	--This is the data from the file at time of script generation
INSERT INTO #RawData ([Id], [Hours], [Code]) VALUES ('1', '0.00', 'X')
INSERT INTO #RawData ([Id], [Hours], [Code]) VALUES ('2', '0.50', 'A')
INSERT INTO #RawData ([Id], [Hours], [Code]) VALUES ('3', '1.00', 'B')
INSERT INTO #RawData ([Id], [Hours], [Code]) VALUES ('4', '1.50', 'C')
INSERT INTO #RawData ([Id], [Hours], [Code]) VALUES ('5', '2.00', 'D')
INSERT INTO #RawData ([Id], [Hours], [Code]) VALUES ('6', '2.50', 'E')
INSERT INTO #RawData ([Id], [Hours], [Code]) VALUES ('7', '3.00', 'F')
INSERT INTO #RawData ([Id], [Hours], [Code]) VALUES ('8', '3.50', 'G')
INSERT INTO #RawData ([Id], [Hours], [Code]) VALUES ('9', '4.00', 'H')
INSERT INTO #RawData ([Id], [Hours], [Code]) VALUES ('10', '4.50', 'I')
INSERT INTO #RawData ([Id], [Hours], [Code]) VALUES ('11', '5.00', 'J')

	IF ((SELECT OBJECTPROPERTY( OBJECT_ID(N'[dbo].[BillableHourCategory]'), 'TableHasIdentity')) = 1)
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
	LEFT JOIN [dbo].[BillableHourCategory] d ON d.[Id] = s.[Id]
	WHERE d.[Hours] = s.[Hours] AND d.[Code] = s.[Code]


	SELECT @CounterIdentical = COUNT(*) FROM @IdenticalRecordIDs


	--Update any rows that already exist
	UPDATE [dbo].[BillableHourCategory] SET [Hours] = s.[Hours], [Code] = s.[Code]
		, ModifiedDate = GETDATE()
	, ModifiedUser = SUSER_NAME()
		OUTPUT Inserted.[Id] INTO @IdenticalRecordIDs
	FROM #RawData s
	LEFT JOIN [dbo].[BillableHourCategory] d ON s.[Id] = d.[Id] 
	WHERE s.[Id] NOT IN (SELECT id FROM @IdenticalRecordIDs)


	SELECT @CounterUpdated = COUNT(*) - @CounterIdentical FROM @IdenticalRecordIDs


	IF ((SELECT OBJECTPROPERTY( OBJECT_ID(N'[dbo].[BillableHourCategory]'), 'TableHasIdentity')) = 1)
		SET IDENTITY_INSERT [dbo].[BillableHourCategory] ON

	--Insert any new rows
	INSERT INTO [dbo].[BillableHourCategory] ([Id], [Hours], [Code]
	, ModifiedDate, ModifiedUser	)
	OUTPUT Inserted.[Id] INTO @IdenticalRecordIDs
	SELECT [Id], [Hours], [Code]
	, GETDATE(), SUSER_NAME()	FROM #RawData
	WHERE [Id] NOT IN (SELECT id FROM @IdenticalRecordIDs)

	IF ((SELECT OBJECTPROPERTY( OBJECT_ID(N'[dbo].[BillableHourCategory]'), 'TableHasIdentity')) = 1)
		SET IDENTITY_INSERT [dbo].[BillableHourCategory] OFF

	SELECT @CounterNew = COUNT(*) - @CounterIdentical - @CounterUpdated FROM @IdenticalRecordIDs

	PRINT 'Records in Project: '+ CONVERT(NVARCHAR(MAX),@CounterInProject)
	PRINT 'Identical Records: '+ CONVERT(NVARCHAR(MAX),@CounterIdentical)
	PRINT 'Updated Records: '+ CONVERT(NVARCHAR(MAX),@CounterUpdated)
	PRINT 'New Records: '+ CONVERT(NVARCHAR(MAX),@CounterNew)

	GO
--########################################################
--########### [dbo].[CancellationType]
--########################################################
	--Turn off the output of how many rows affected so output window isn't crowded
	SET NOCOUNT ON
	PRINT 'Updating table [dbo].[CancellationType]'

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
		[Description],
		[Code]
	INTO #RawData
	FROM (
		SELECT 
			[Id],
			[Name],
			[Description],
			[Code]
		FROM [dbo].[CancellationType] 
		WHERE 1=2
	) t
	GO

	/* 
	Because the temp table is now created using a SELECT INTO it can be created with IDENTITY fields. 
	Turn on IDENTITY INSERT if they exist and turn it back off after you are done.
	*/
	IF ((SELECT OBJECTPROPERTY( OBJECT_ID(N'[dbo].[CancellationType]'), 'TableHasIdentity')) = 1)
		SET IDENTITY_INSERT #RawData ON
	GO

	--This is the data from the file at time of script generation
INSERT INTO #RawData ([Id], [Name], [Description], [Code]) VALUES ('1', 'Late Cancellation', 'Doc review still gets paid but Intake does not.', 'LC        ')
INSERT INTO #RawData ([Id], [Name], [Description], [Code]) VALUES ('2', 'No Show', 'Doc review still gets paid, and intake gets 0.5 hours ', 'NS        ')
INSERT INTO #RawData ([Id], [Name], [Description], [Code]) VALUES ('3', 'Cancellation', 'Neither Doc Review or Intake get paid.  Doc Review may appeal this in certain cases.', 'C         ')

	IF ((SELECT OBJECTPROPERTY( OBJECT_ID(N'[dbo].[CancellationType]'), 'TableHasIdentity')) = 1)
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
	LEFT JOIN [dbo].[CancellationType] d ON d.[Id] = s.[Id]
	WHERE d.[Name] = s.[Name] AND d.[Description] = s.[Description] AND d.[Code] = s.[Code]


	SELECT @CounterIdentical = COUNT(*) FROM @IdenticalRecordIDs


	--Update any rows that already exist
	UPDATE [dbo].[CancellationType] SET [Name] = s.[Name], [Description] = s.[Description], [Code] = s.[Code]
		, ModifiedDate = GETDATE()
	, ModifiedUser = SUSER_NAME()
		OUTPUT Inserted.[Id] INTO @IdenticalRecordIDs
	FROM #RawData s
	LEFT JOIN [dbo].[CancellationType] d ON s.[Id] = d.[Id] 
	WHERE s.[Id] NOT IN (SELECT id FROM @IdenticalRecordIDs)


	SELECT @CounterUpdated = COUNT(*) - @CounterIdentical FROM @IdenticalRecordIDs


	IF ((SELECT OBJECTPROPERTY( OBJECT_ID(N'[dbo].[CancellationType]'), 'TableHasIdentity')) = 1)
		SET IDENTITY_INSERT [dbo].[CancellationType] ON

	--Insert any new rows
	INSERT INTO [dbo].[CancellationType] ([Id], [Name], [Description], [Code]
	, ModifiedDate, ModifiedUser	)
	OUTPUT Inserted.[Id] INTO @IdenticalRecordIDs
	SELECT [Id], [Name], [Description], [Code]
	, GETDATE(), SUSER_NAME()	FROM #RawData
	WHERE [Id] NOT IN (SELECT id FROM @IdenticalRecordIDs)

	IF ((SELECT OBJECTPROPERTY( OBJECT_ID(N'[dbo].[CancellationType]'), 'TableHasIdentity')) = 1)
		SET IDENTITY_INSERT [dbo].[CancellationType] OFF

	SELECT @CounterNew = COUNT(*) - @CounterIdentical - @CounterUpdated FROM @IdenticalRecordIDs

	PRINT 'Records in Project: '+ CONVERT(NVARCHAR(MAX),@CounterInProject)
	PRINT 'Identical Records: '+ CONVERT(NVARCHAR(MAX),@CounterIdentical)
	PRINT 'Updated Records: '+ CONVERT(NVARCHAR(MAX),@CounterUpdated)
	PRINT 'New Records: '+ CONVERT(NVARCHAR(MAX),@CounterNew)

	GO
--########################################################
--########### [dbo].[ServicePortfolio]
--########################################################
	--Turn off the output of how many rows affected so output window isn't crowded
	SET NOCOUNT ON
	PRINT 'Updating table [dbo].[ServicePortfolio]'

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
		[Name]
	INTO #RawData
	FROM (
		SELECT 
			[Id],
			[Name]
		FROM [dbo].[ServicePortfolio] 
		WHERE 1=2
	) t
	GO

	/* 
	Because the temp table is now created using a SELECT INTO it can be created with IDENTITY fields. 
	Turn on IDENTITY INSERT if they exist and turn it back off after you are done.
	*/
	IF ((SELECT OBJECTPROPERTY( OBJECT_ID(N'[dbo].[ServicePortfolio]'), 'TableHasIdentity')) = 1)
		SET IDENTITY_INSERT #RawData ON
	GO

	--This is the data from the file at time of script generation
INSERT INTO #RawData ([Id], [Name]) VALUES ('1', 'Orvosi')
INSERT INTO #RawData ([Id], [Name]) VALUES ('2', 'Physiatrist')

	IF ((SELECT OBJECTPROPERTY( OBJECT_ID(N'[dbo].[ServicePortfolio]'), 'TableHasIdentity')) = 1)
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
	LEFT JOIN [dbo].[ServicePortfolio] d ON d.[Id] = s.[Id]
	WHERE d.[Name] = s.[Name]


	SELECT @CounterIdentical = COUNT(*) FROM @IdenticalRecordIDs


	--Update any rows that already exist
	UPDATE [dbo].[ServicePortfolio] SET [Name] = s.[Name]
		, ModifiedDate = GETDATE()
	, ModifiedUser = SUSER_NAME()
		OUTPUT Inserted.[Id] INTO @IdenticalRecordIDs
	FROM #RawData s
	LEFT JOIN [dbo].[ServicePortfolio] d ON s.[Id] = d.[Id] 
	WHERE s.[Id] NOT IN (SELECT id FROM @IdenticalRecordIDs)


	SELECT @CounterUpdated = COUNT(*) - @CounterIdentical FROM @IdenticalRecordIDs


	IF ((SELECT OBJECTPROPERTY( OBJECT_ID(N'[dbo].[ServicePortfolio]'), 'TableHasIdentity')) = 1)
		SET IDENTITY_INSERT [dbo].[ServicePortfolio] ON

	--Insert any new rows
	INSERT INTO [dbo].[ServicePortfolio] ([Id], [Name]
	, ModifiedDate, ModifiedUser	)
	OUTPUT Inserted.[Id] INTO @IdenticalRecordIDs
	SELECT [Id], [Name]
	, GETDATE(), SUSER_NAME()	FROM #RawData
	WHERE [Id] NOT IN (SELECT id FROM @IdenticalRecordIDs)

	IF ((SELECT OBJECTPROPERTY( OBJECT_ID(N'[dbo].[ServicePortfolio]'), 'TableHasIdentity')) = 1)
		SET IDENTITY_INSERT [dbo].[ServicePortfolio] OFF

	SELECT @CounterNew = COUNT(*) - @CounterIdentical - @CounterUpdated FROM @IdenticalRecordIDs

	PRINT 'Records in Project: '+ CONVERT(NVARCHAR(MAX),@CounterInProject)
	PRINT 'Identical Records: '+ CONVERT(NVARCHAR(MAX),@CounterIdentical)
	PRINT 'Updated Records: '+ CONVERT(NVARCHAR(MAX),@CounterUpdated)
	PRINT 'New Records: '+ CONVERT(NVARCHAR(MAX),@CounterNew)

	GO
--########################################################
--########### [dbo].[ServiceCategory]
--########################################################
	--Turn off the output of how many rows affected so output window isn't crowded
	SET NOCOUNT ON
	PRINT 'Updating table [dbo].[ServiceCategory]'

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
		[Name]
	INTO #RawData
	FROM (
		SELECT 
			[Id],
			[Name]
		FROM [dbo].[ServiceCategory] 
		WHERE 1=2
	) t
	GO

	/* 
	Because the temp table is now created using a SELECT INTO it can be created with IDENTITY fields. 
	Turn on IDENTITY INSERT if they exist and turn it back off after you are done.
	*/
	IF ((SELECT OBJECTPROPERTY( OBJECT_ID(N'[dbo].[ServiceCategory]'), 'TableHasIdentity')) = 1)
		SET IDENTITY_INSERT #RawData ON
	GO

	--This is the data from the file at time of script generation
INSERT INTO #RawData ([Id], [Name]) VALUES ('1', 'Administration')
INSERT INTO #RawData ([Id], [Name]) VALUES ('2', 'Intake Coordination')
INSERT INTO #RawData ([Id], [Name]) VALUES ('3', 'Invoicing')
INSERT INTO #RawData ([Id], [Name]) VALUES ('4', 'Medical Consultation')
INSERT INTO #RawData ([Id], [Name]) VALUES ('5', 'Independent Medical Exam')
INSERT INTO #RawData ([Id], [Name]) VALUES ('6', 'Medical Consultation')
INSERT INTO #RawData ([Id], [Name]) VALUES ('7', 'Add-on')

	IF ((SELECT OBJECTPROPERTY( OBJECT_ID(N'[dbo].[ServiceCategory]'), 'TableHasIdentity')) = 1)
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
	LEFT JOIN [dbo].[ServiceCategory] d ON d.[Id] = s.[Id]
	WHERE d.[Name] = s.[Name]


	SELECT @CounterIdentical = COUNT(*) FROM @IdenticalRecordIDs


	--Update any rows that already exist
	UPDATE [dbo].[ServiceCategory] SET [Name] = s.[Name]
		, ModifiedDate = GETDATE()
	, ModifiedUser = SUSER_NAME()
		OUTPUT Inserted.[Id] INTO @IdenticalRecordIDs
	FROM #RawData s
	LEFT JOIN [dbo].[ServiceCategory] d ON s.[Id] = d.[Id] 
	WHERE s.[Id] NOT IN (SELECT id FROM @IdenticalRecordIDs)


	SELECT @CounterUpdated = COUNT(*) - @CounterIdentical FROM @IdenticalRecordIDs


	IF ((SELECT OBJECTPROPERTY( OBJECT_ID(N'[dbo].[ServiceCategory]'), 'TableHasIdentity')) = 1)
		SET IDENTITY_INSERT [dbo].[ServiceCategory] ON

	--Insert any new rows
	INSERT INTO [dbo].[ServiceCategory] ([Id], [Name]
	, ModifiedDate, ModifiedUser	)
	OUTPUT Inserted.[Id] INTO @IdenticalRecordIDs
	SELECT [Id], [Name]
	, GETDATE(), SUSER_NAME()	FROM #RawData
	WHERE [Id] NOT IN (SELECT id FROM @IdenticalRecordIDs)

	IF ((SELECT OBJECTPROPERTY( OBJECT_ID(N'[dbo].[ServiceCategory]'), 'TableHasIdentity')) = 1)
		SET IDENTITY_INSERT [dbo].[ServiceCategory] OFF

	SELECT @CounterNew = COUNT(*) - @CounterIdentical - @CounterUpdated FROM @IdenticalRecordIDs

	PRINT 'Records in Project: '+ CONVERT(NVARCHAR(MAX),@CounterInProject)
	PRINT 'Identical Records: '+ CONVERT(NVARCHAR(MAX),@CounterIdentical)
	PRINT 'Updated Records: '+ CONVERT(NVARCHAR(MAX),@CounterUpdated)
	PRINT 'New Records: '+ CONVERT(NVARCHAR(MAX),@CounterNew)

	GO
--########################################################
--########### [dbo].[Service]
--########################################################
	--Turn off the output of how many rows affected so output window isn't crowded
	SET NOCOUNT ON
	PRINT 'Updating table [dbo].[Service]'

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
		[Code],
		[IsAddOn],
		[ServiceCategoryId],
		[ServicePortfolioId]
	INTO #RawData
	FROM (
		SELECT 
			[Id],
			[Name],
			[Code],
			[IsAddOn],
			[ServiceCategoryId],
			[ServicePortfolioId]
		FROM [dbo].[Service] 
		WHERE 1=2
	) t
	GO

	/* 
	Because the temp table is now created using a SELECT INTO it can be created with IDENTITY fields. 
	Turn on IDENTITY INSERT if they exist and turn it back off after you are done.
	*/
	IF ((SELECT OBJECTPROPERTY( OBJECT_ID(N'[dbo].[Service]'), 'TableHasIdentity')) = 1)
		SET IDENTITY_INSERT #RawData ON
	GO

	--This is the data from the file at time of script generation
INSERT INTO #RawData ([Id], [Name], [Code], [IsAddOn], [ServiceCategoryId], [ServicePortfolioId]) VALUES ('7', 'Accident Benefit Physiatry Assessment', 'AB        ', '0', '5', '2')
INSERT INTO #RawData ([Id], [Name], [Code], [IsAddOn], [ServiceCategoryId], [ServicePortfolioId]) VALUES ('8', 'Long Term Disability Physiatry Assessment', 'LTD       ', '0', '5', '2')
INSERT INTO #RawData ([Id], [Name], [Code], [IsAddOn], [ServiceCategoryId], [ServicePortfolioId]) VALUES ('9', 'Catastrophic Physiatry Assessment', 'CAT       ', '0', '5', '2')
INSERT INTO #RawData ([Id], [Name], [Code], [IsAddOn], [ServiceCategoryId], [ServicePortfolioId]) VALUES ('10', 'Defence Medical Physiatry Assessment', 'DM        ', '0', '5', '2')
INSERT INTO #RawData ([Id], [Name], [Code], [IsAddOn], [ServiceCategoryId], [ServicePortfolioId]) VALUES ('11', 'Plaintiff Medical Physiatry Assessment', 'PM        ', '0', '5', '2')
INSERT INTO #RawData ([Id], [Name], [Code], [IsAddOn], [ServiceCategoryId], [ServicePortfolioId]) VALUES ('12', 'Medlegal Physiatry Assessment (uncategorized)', 'ML        ', '0', '5', '2')
INSERT INTO #RawData ([Id], [Name], [Code], [IsAddOn], [ServiceCategoryId], [ServicePortfolioId]) VALUES ('13', 'Teleconference', 'TELE      ', '0', '6', '2')
INSERT INTO #RawData ([Id], [Name], [Code], [IsAddOn], [ServiceCategoryId], [ServicePortfolioId]) VALUES ('14', 'Intact File', 'INTC      ', '1', '7', '2')
INSERT INTO #RawData ([Id], [Name], [Code], [IsAddOn], [ServiceCategoryId], [ServicePortfolioId]) VALUES ('15', 'TD Bodily Injury File', 'TDBI      ', '1', '7', '2')
INSERT INTO #RawData ([Id], [Name], [Code], [IsAddOn], [ServiceCategoryId], [ServicePortfolioId]) VALUES ('16', 'Paper Review', 'PR        ', '1', '7', '2')
INSERT INTO #RawData ([Id], [Name], [Code], [IsAddOn], [ServiceCategoryId], [ServicePortfolioId]) VALUES ('17', 'Addendum', 'ADD       ', '1', '7', '2')

	IF ((SELECT OBJECTPROPERTY( OBJECT_ID(N'[dbo].[Service]'), 'TableHasIdentity')) = 1)
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
	LEFT JOIN [dbo].[Service] d ON d.[Id] = s.[Id]
	WHERE d.[Name] = s.[Name] AND d.[Code] = s.[Code] AND d.[IsAddOn] = s.[IsAddOn] AND d.[ServiceCategoryId] = s.[ServiceCategoryId] AND d.[ServicePortfolioId] = s.[ServicePortfolioId]


	SELECT @CounterIdentical = COUNT(*) FROM @IdenticalRecordIDs


	--Update any rows that already exist
	UPDATE [dbo].[Service] SET [Name] = s.[Name], [Code] = s.[Code], [IsAddOn] = s.[IsAddOn], [ServiceCategoryId] = s.[ServiceCategoryId], [ServicePortfolioId] = s.[ServicePortfolioId]
		, ModifiedDate = GETDATE()
	, ModifiedUser = SUSER_NAME()
		OUTPUT Inserted.[Id] INTO @IdenticalRecordIDs
	FROM #RawData s
	LEFT JOIN [dbo].[Service] d ON s.[Id] = d.[Id] 
	WHERE s.[Id] NOT IN (SELECT id FROM @IdenticalRecordIDs)


	SELECT @CounterUpdated = COUNT(*) - @CounterIdentical FROM @IdenticalRecordIDs


	IF ((SELECT OBJECTPROPERTY( OBJECT_ID(N'[dbo].[Service]'), 'TableHasIdentity')) = 1)
		SET IDENTITY_INSERT [dbo].[Service] ON

	--Insert any new rows
	INSERT INTO [dbo].[Service] ([Id], [Name], [Code], [IsAddOn], [ServiceCategoryId], [ServicePortfolioId]
	, ModifiedDate, ModifiedUser	)
	OUTPUT Inserted.[Id] INTO @IdenticalRecordIDs
	SELECT [Id], [Name], [Code], [IsAddOn], [ServiceCategoryId], [ServicePortfolioId]
	, GETDATE(), SUSER_NAME()	FROM #RawData
	WHERE [Id] NOT IN (SELECT id FROM @IdenticalRecordIDs)

	IF ((SELECT OBJECTPROPERTY( OBJECT_ID(N'[dbo].[Service]'), 'TableHasIdentity')) = 1)
		SET IDENTITY_INSERT [dbo].[Service] OFF

	SELECT @CounterNew = COUNT(*) - @CounterIdentical - @CounterUpdated FROM @IdenticalRecordIDs

	PRINT 'Records in Project: '+ CONVERT(NVARCHAR(MAX),@CounterInProject)
	PRINT 'Identical Records: '+ CONVERT(NVARCHAR(MAX),@CounterIdentical)
	PRINT 'Updated Records: '+ CONVERT(NVARCHAR(MAX),@CounterUpdated)
	PRINT 'New Records: '+ CONVERT(NVARCHAR(MAX),@CounterNew)

	GO
--########################################################
--########### [dbo].[ServiceTask]
--########################################################
	--Turn off the output of how many rows affected so output window isn't crowded
	SET NOCOUNT ON
	PRINT 'Updating table [dbo].[ServiceTask]'

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
		[ServiceId],
		[TaskId],
		[IsBillable],
		[Sequence]
	INTO #RawData
	FROM (
		SELECT 
			[Id],
			[ServiceId],
			[TaskId],
			[IsBillable],
			[Sequence]
		FROM [dbo].[ServiceTask] 
		WHERE 1=2
	) t
	GO

	/* 
	Because the temp table is now created using a SELECT INTO it can be created with IDENTITY fields. 
	Turn on IDENTITY INSERT if they exist and turn it back off after you are done.
	*/
	IF ((SELECT OBJECTPROPERTY( OBJECT_ID(N'[dbo].[ServiceTask]'), 'TableHasIdentity')) = 1)
		SET IDENTITY_INSERT #RawData ON
	GO

	--This is the data from the file at time of script generation
INSERT INTO #RawData ([Id], [ServiceId], [TaskId], [IsBillable], [Sequence]) VALUES ('1', '7', '3', '0', '1')
INSERT INTO #RawData ([Id], [ServiceId], [TaskId], [IsBillable], [Sequence]) VALUES ('2', '7', '11', '0', '2')
INSERT INTO #RawData ([Id], [ServiceId], [TaskId], [IsBillable], [Sequence]) VALUES ('3', '7', '9', '0', '3')

	IF ((SELECT OBJECTPROPERTY( OBJECT_ID(N'[dbo].[ServiceTask]'), 'TableHasIdentity')) = 1)
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
	LEFT JOIN [dbo].[ServiceTask] d ON d.[Id] = s.[Id]
	WHERE d.[ServiceId] = s.[ServiceId] AND d.[TaskId] = s.[TaskId] AND d.[IsBillable] = s.[IsBillable] AND d.[Sequence] = s.[Sequence]


	SELECT @CounterIdentical = COUNT(*) FROM @IdenticalRecordIDs


	--Update any rows that already exist
	UPDATE [dbo].[ServiceTask] SET [ServiceId] = s.[ServiceId], [TaskId] = s.[TaskId], [IsBillable] = s.[IsBillable], [Sequence] = s.[Sequence]
		, ModifiedDate = GETDATE()
	, ModifiedUser = SUSER_NAME()
		OUTPUT Inserted.[Id] INTO @IdenticalRecordIDs
	FROM #RawData s
	LEFT JOIN [dbo].[ServiceTask] d ON s.[Id] = d.[Id] 
	WHERE s.[Id] NOT IN (SELECT id FROM @IdenticalRecordIDs)


	SELECT @CounterUpdated = COUNT(*) - @CounterIdentical FROM @IdenticalRecordIDs


	IF ((SELECT OBJECTPROPERTY( OBJECT_ID(N'[dbo].[ServiceTask]'), 'TableHasIdentity')) = 1)
		SET IDENTITY_INSERT [dbo].[ServiceTask] ON

	--Insert any new rows
	INSERT INTO [dbo].[ServiceTask] ([Id], [ServiceId], [TaskId], [IsBillable], [Sequence]
	, ModifiedDate, ModifiedUser	)
	OUTPUT Inserted.[Id] INTO @IdenticalRecordIDs
	SELECT [Id], [ServiceId], [TaskId], [IsBillable], [Sequence]
	, GETDATE(), SUSER_NAME()	FROM #RawData
	WHERE [Id] NOT IN (SELECT id FROM @IdenticalRecordIDs)

	IF ((SELECT OBJECTPROPERTY( OBJECT_ID(N'[dbo].[ServiceTask]'), 'TableHasIdentity')) = 1)
		SET IDENTITY_INSERT [dbo].[ServiceTask] OFF

	SELECT @CounterNew = COUNT(*) - @CounterIdentical - @CounterUpdated FROM @IdenticalRecordIDs

	PRINT 'Records in Project: '+ CONVERT(NVARCHAR(MAX),@CounterInProject)
	PRINT 'Identical Records: '+ CONVERT(NVARCHAR(MAX),@CounterIdentical)
	PRINT 'Updated Records: '+ CONVERT(NVARCHAR(MAX),@CounterUpdated)
	PRINT 'New Records: '+ CONVERT(NVARCHAR(MAX),@CounterNew)

	GO

