 
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
		[Id],
		[Name]
	INTO #RawData
	FROM (
		SELECT 
			[Id],
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
INSERT INTO #RawData ([Id], [Name]) VALUES ('1', 'Company Assessment Office')
INSERT INTO #RawData ([Id], [Name]) VALUES ('2', 'Physician Clinic')
INSERT INTO #RawData ([Id], [Name]) VALUES ('3', 'Primary Office')
INSERT INTO #RawData ([Id], [Name]) VALUES ('4', 'Billing Address')

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
	SELECT s.[Id] 
	FROM #RawData s
	LEFT JOIN [dbo].[AddressType] d ON d.[Id] = s.[Id]
	WHERE d.[Name] = s.[Name]


	SELECT @CounterIdentical = COUNT(*) FROM @IdenticalRecordIDs


	--Update any rows that already exist
	UPDATE [dbo].[AddressType] SET [Name] = s.[Name]
		, ModifiedDate = GETDATE()
	, ModifiedUser = SUSER_NAME()
		OUTPUT Inserted.[Id] INTO @IdenticalRecordIDs
	FROM #RawData s
	LEFT JOIN [dbo].[AddressType] d ON s.[Id] = d.[Id] 
	WHERE s.[Id] NOT IN (SELECT id FROM @IdenticalRecordIDs)


	SELECT @CounterUpdated = COUNT(*) - @CounterIdentical FROM @IdenticalRecordIDs


	IF ((SELECT OBJECTPROPERTY( OBJECT_ID(N'[dbo].[AddressType]'), 'TableHasIdentity')) = 1)
		SET IDENTITY_INSERT [dbo].[AddressType] ON

	--Insert any new rows
	INSERT INTO [dbo].[AddressType] ([Id], [Name]
	, ModifiedDate, ModifiedUser	)
	OUTPUT Inserted.[Id] INTO @IdenticalRecordIDs
	SELECT [Id], [Name]
	, GETDATE(), SUSER_NAME()	FROM #RawData
	WHERE [Id] NOT IN (SELECT id FROM @IdenticalRecordIDs)

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
		[Name],
		[RoleCategoryId]
	INTO #RawData
	FROM (
		SELECT 
			[Id],
			[Name],
			[RoleCategoryId]
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
INSERT INTO #RawData ([Id], [Name], [RoleCategoryId]) VALUES ('22B5C8AC-2C96-4A74-8057-976914031A7E', 'Document Reviewer', '3')
INSERT INTO #RawData ([Id], [Name], [RoleCategoryId]) VALUES ('5e2cfba4-417a-4685-8e2b-970bd7061cd9', 'Accountant', '3')
INSERT INTO #RawData ([Id], [Name], [RoleCategoryId]) VALUES ('7b930663-b091-44ca-924c-d8b11a1ee7ea', 'Company Booking Agent', '2')
INSERT INTO #RawData ([Id], [Name], [RoleCategoryId]) VALUES ('7fab67dd-286b-492f-865a-0cb0ce1261ce', 'Super Admin', '4')
INSERT INTO #RawData ([Id], [Name], [RoleCategoryId]) VALUES ('8359141f-e423-4e48-8925-4624ba86245a', 'Physician', '1')
INSERT INTO #RawData ([Id], [Name], [RoleCategoryId]) VALUES ('9dd582a0-cf86-4fc0-8894-477266068c12', 'Intake Assistant', '3')
INSERT INTO #RawData ([Id], [Name], [RoleCategoryId]) VALUES ('9eab89c0-225c-4027-9f42-cc35e5656b14', 'Case Coordinator', '4')

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
	WHERE d.[Name] = s.[Name] AND d.[RoleCategoryId] = s.[RoleCategoryId]


	SELECT @CounterIdentical = COUNT(*) FROM @IdenticalRecordIDs


	--Update any rows that already exist
	UPDATE [dbo].[AspNetRoles] SET [Name] = s.[Name], [RoleCategoryId] = s.[RoleCategoryId]
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
	INSERT INTO [dbo].[AspNetRoles] ([Id], [Name], [RoleCategoryId]
	, ModifiedDate, ModifiedUser	)
	OUTPUT Inserted.[Id] INTO @IdenticalRecordIDs
	SELECT [Id], [Name], [RoleCategoryId]
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
INSERT INTO #RawData ([Id], [Name]) VALUES ('2', 'Company')
INSERT INTO #RawData ([Id], [Name]) VALUES ('3', 'Physician')

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
--########### [dbo].[Country]
--########################################################
	--Turn off the output of how many rows affected so output window isn't crowded
	SET NOCOUNT ON
	PRINT 'Updating table [dbo].[Country]'

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
		[ISO3DigitCountry],
		[ISO2CountryCode],
		[ISO3CountryCode]
	INTO #RawData
	FROM (
		SELECT 
			[Id],
			[Name],
			[ISO3DigitCountry],
			[ISO2CountryCode],
			[ISO3CountryCode]
		FROM [dbo].[Country] 
		WHERE 1=2
	) t
	GO

	/* 
	Because the temp table is now created using a SELECT INTO it can be created with IDENTITY fields. 
	Turn on IDENTITY INSERT if they exist and turn it back off after you are done.
	*/
	IF ((SELECT OBJECTPROPERTY( OBJECT_ID(N'[dbo].[Country]'), 'TableHasIdentity')) = 1)
		SET IDENTITY_INSERT #RawData ON
	GO

	--This is the data from the file at time of script generation
INSERT INTO #RawData ([Id], [Name], [ISO3DigitCountry], [ISO2CountryCode], [ISO3CountryCode]) VALUES ('124', 'Canada', '124', 'CA', 'CAN')

	IF ((SELECT OBJECTPROPERTY( OBJECT_ID(N'[dbo].[Country]'), 'TableHasIdentity')) = 1)
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
	LEFT JOIN [dbo].[Country] d ON d.[Id] = s.[Id]
	WHERE d.[Name] = s.[Name] AND d.[ISO3DigitCountry] = s.[ISO3DigitCountry] AND d.[ISO2CountryCode] = s.[ISO2CountryCode] AND d.[ISO3CountryCode] = s.[ISO3CountryCode]


	SELECT @CounterIdentical = COUNT(*) FROM @IdenticalRecordIDs


	--Update any rows that already exist
	UPDATE [dbo].[Country] SET [Name] = s.[Name], [ISO3DigitCountry] = s.[ISO3DigitCountry], [ISO2CountryCode] = s.[ISO2CountryCode], [ISO3CountryCode] = s.[ISO3CountryCode]
		, ModifiedDate = GETDATE()
	, ModifiedUser = SUSER_NAME()
		OUTPUT Inserted.[Id] INTO @IdenticalRecordIDs
	FROM #RawData s
	LEFT JOIN [dbo].[Country] d ON s.[Id] = d.[Id] 
	WHERE s.[Id] NOT IN (SELECT id FROM @IdenticalRecordIDs)


	SELECT @CounterUpdated = COUNT(*) - @CounterIdentical FROM @IdenticalRecordIDs


	IF ((SELECT OBJECTPROPERTY( OBJECT_ID(N'[dbo].[Country]'), 'TableHasIdentity')) = 1)
		SET IDENTITY_INSERT [dbo].[Country] ON

	--Insert any new rows
	INSERT INTO [dbo].[Country] ([Id], [Name], [ISO3DigitCountry], [ISO2CountryCode], [ISO3CountryCode]
	, ModifiedDate, ModifiedUser	)
	OUTPUT Inserted.[Id] INTO @IdenticalRecordIDs
	SELECT [Id], [Name], [ISO3DigitCountry], [ISO2CountryCode], [ISO3CountryCode]
	, GETDATE(), SUSER_NAME()	FROM #RawData
	WHERE [Id] NOT IN (SELECT id FROM @IdenticalRecordIDs)

	IF ((SELECT OBJECTPROPERTY( OBJECT_ID(N'[dbo].[Country]'), 'TableHasIdentity')) = 1)
		SET IDENTITY_INSERT [dbo].[Country] OFF

	SELECT @CounterNew = COUNT(*) - @CounterIdentical - @CounterUpdated FROM @IdenticalRecordIDs

	PRINT 'Records in Project: '+ CONVERT(NVARCHAR(MAX),@CounterInProject)
	PRINT 'Identical Records: '+ CONVERT(NVARCHAR(MAX),@CounterIdentical)
	PRINT 'Updated Records: '+ CONVERT(NVARCHAR(MAX),@CounterUpdated)
	PRINT 'New Records: '+ CONVERT(NVARCHAR(MAX),@CounterNew)

	GO
--########################################################
--########### [dbo].[Province]
--########################################################
	--Turn off the output of how many rows affected so output window isn't crowded
	SET NOCOUNT ON
	PRINT 'Updating table [dbo].[Province]'

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
		[CountryID],
		[ProvinceName],
		[ProvinceCode]
	INTO #RawData
	FROM (
		SELECT 
			[Id],
			[CountryID],
			[ProvinceName],
			[ProvinceCode]
		FROM [dbo].[Province] 
		WHERE 1=2
	) t
	GO

	/* 
	Because the temp table is now created using a SELECT INTO it can be created with IDENTITY fields. 
	Turn on IDENTITY INSERT if they exist and turn it back off after you are done.
	*/
	IF ((SELECT OBJECTPROPERTY( OBJECT_ID(N'[dbo].[Province]'), 'TableHasIdentity')) = 1)
		SET IDENTITY_INSERT #RawData ON
	GO

	--This is the data from the file at time of script generation
INSERT INTO #RawData ([Id], [CountryID], [ProvinceName], [ProvinceCode]) VALUES ('1', '124', 'Alberta', 'CA-AB')
INSERT INTO #RawData ([Id], [CountryID], [ProvinceName], [ProvinceCode]) VALUES ('2', '124', 'British Columbia', 'CA-BC')
INSERT INTO #RawData ([Id], [CountryID], [ProvinceName], [ProvinceCode]) VALUES ('3', '124', 'Manitoba', 'CA-MB')
INSERT INTO #RawData ([Id], [CountryID], [ProvinceName], [ProvinceCode]) VALUES ('4', '124', 'New Brunswick', 'CA-NB')
INSERT INTO #RawData ([Id], [CountryID], [ProvinceName], [ProvinceCode]) VALUES ('5', '124', 'Newfoundland and Labrador', 'CA-NL')
INSERT INTO #RawData ([Id], [CountryID], [ProvinceName], [ProvinceCode]) VALUES ('6', '124', 'Northwest Territories', 'CA-NT')
INSERT INTO #RawData ([Id], [CountryID], [ProvinceName], [ProvinceCode]) VALUES ('7', '124', 'Nova Scotia', 'CA-NS')
INSERT INTO #RawData ([Id], [CountryID], [ProvinceName], [ProvinceCode]) VALUES ('8', '124', 'Nunavut', 'CA-NU')
INSERT INTO #RawData ([Id], [CountryID], [ProvinceName], [ProvinceCode]) VALUES ('9', '124', 'Ontario', 'CA-ON')
INSERT INTO #RawData ([Id], [CountryID], [ProvinceName], [ProvinceCode]) VALUES ('10', '124', 'Prince Edward Island', 'CA-PE')
INSERT INTO #RawData ([Id], [CountryID], [ProvinceName], [ProvinceCode]) VALUES ('11', '124', 'Québec', 'CA-QC')
INSERT INTO #RawData ([Id], [CountryID], [ProvinceName], [ProvinceCode]) VALUES ('12', '124', 'Saskatchewan', 'CA-SK')
INSERT INTO #RawData ([Id], [CountryID], [ProvinceName], [ProvinceCode]) VALUES ('13', '124', 'Yukon', 'CA-YT')

	IF ((SELECT OBJECTPROPERTY( OBJECT_ID(N'[dbo].[Province]'), 'TableHasIdentity')) = 1)
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
	LEFT JOIN [dbo].[Province] d ON d.[Id] = s.[Id]
	WHERE d.[CountryID] = s.[CountryID] AND d.[ProvinceName] = s.[ProvinceName] AND d.[ProvinceCode] = s.[ProvinceCode]


	SELECT @CounterIdentical = COUNT(*) FROM @IdenticalRecordIDs


	--Update any rows that already exist
	UPDATE [dbo].[Province] SET [CountryID] = s.[CountryID], [ProvinceName] = s.[ProvinceName], [ProvinceCode] = s.[ProvinceCode]
		, ModifiedDate = GETDATE()
	, ModifiedUser = SUSER_NAME()
		OUTPUT Inserted.[Id] INTO @IdenticalRecordIDs
	FROM #RawData s
	LEFT JOIN [dbo].[Province] d ON s.[Id] = d.[Id] 
	WHERE s.[Id] NOT IN (SELECT id FROM @IdenticalRecordIDs)


	SELECT @CounterUpdated = COUNT(*) - @CounterIdentical FROM @IdenticalRecordIDs


	IF ((SELECT OBJECTPROPERTY( OBJECT_ID(N'[dbo].[Province]'), 'TableHasIdentity')) = 1)
		SET IDENTITY_INSERT [dbo].[Province] ON

	--Insert any new rows
	INSERT INTO [dbo].[Province] ([Id], [CountryID], [ProvinceName], [ProvinceCode]
	, ModifiedDate, ModifiedUser	)
	OUTPUT Inserted.[Id] INTO @IdenticalRecordIDs
	SELECT [Id], [CountryID], [ProvinceName], [ProvinceCode]
	, GETDATE(), SUSER_NAME()	FROM #RawData
	WHERE [Id] NOT IN (SELECT id FROM @IdenticalRecordIDs)

	IF ((SELECT OBJECTPROPERTY( OBJECT_ID(N'[dbo].[Province]'), 'TableHasIdentity')) = 1)
		SET IDENTITY_INSERT [dbo].[Province] OFF

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
INSERT INTO #RawData ([Id], [Name]) VALUES ('2', 'Physician')

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
		[ObjectGuid],
		[Name],
		[Code],
		[Price],
		[ServiceCategoryId],
		[ServicePortfolioId]
	INTO #RawData
	FROM (
		SELECT 
			[Id],
			[ObjectGuid],
			[Name],
			[Code],
			[Price],
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
INSERT INTO #RawData ([Id], [ObjectGuid], [Name], [Code], [Price], [ServiceCategoryId], [ServicePortfolioId]) VALUES ('7', '1E3F1212-CC9F-42D8-AE1D-6DCDEEFA826F', 'Accident Benefit', 'AB', '900.00', '5', '2')
INSERT INTO #RawData ([Id], [ObjectGuid], [Name], [Code], [Price], [ServiceCategoryId], [ServicePortfolioId]) VALUES ('8', '066296CE-2A6F-46A2-A2FB-F9B8A009E17A', 'Long Term Disability', 'LTD', '1000.00', '5', '2')
INSERT INTO #RawData ([Id], [ObjectGuid], [Name], [Code], [Price], [ServiceCategoryId], [ServicePortfolioId]) VALUES ('9', '38D9D51F-5067-4A36-A4DC-CBDE8E8D4602', 'Catastrophic Assessment', 'CAT', '1200.00', '5', '2')
INSERT INTO #RawData ([Id], [ObjectGuid], [Name], [Code], [Price], [ServiceCategoryId], [ServicePortfolioId]) VALUES ('11', '64D1C80D-5990-43F8-8FA1-4FFE1F7B8344', 'Plaintiff Medical', 'PM', '2000.00', '5', '2')
INSERT INTO #RawData ([Id], [ObjectGuid], [Name], [Code], [Price], [ServiceCategoryId], [ServicePortfolioId]) VALUES ('12', '9CEC9840-5748-456D-9389-236AABDD6AEE', 'Defense Medical', 'DM', '3000.00', '5', '2')
INSERT INTO #RawData ([Id], [ObjectGuid], [Name], [Code], [Price], [ServiceCategoryId], [ServicePortfolioId]) VALUES ('13', '7710DAAE-A0D6-42B5-962A-CF87C01D59FA', 'Teleconference', 'TELE', '100.00', '6', '2')
INSERT INTO #RawData ([Id], [ObjectGuid], [Name], [Code], [Price], [ServiceCategoryId], [ServicePortfolioId]) VALUES ('14', '09046119-ABBE-4CCB-93E5-CD7E48B112B5', 'Intact File', 'INTC', '100.00', '7', '2')
INSERT INTO #RawData ([Id], [ObjectGuid], [Name], [Code], [Price], [ServiceCategoryId], [ServicePortfolioId]) VALUES ('15', 'D00D66FB-F54D-4C1F-8D0F-F5B03216AD13', 'TD Bodily Injury File', 'TDBI', '100.00', '7', '2')
INSERT INTO #RawData ([Id], [ObjectGuid], [Name], [Code], [Price], [ServiceCategoryId], [ServicePortfolioId]) VALUES ('16', '3BB85306-5B85-496E-A72D-541C95228DBA', 'Paper Review', 'PR', '200.00', '7', '2')
INSERT INTO #RawData ([Id], [ObjectGuid], [Name], [Code], [Price], [ServiceCategoryId], [ServicePortfolioId]) VALUES ('17', '2BFDDE91-C5EC-416B-8AF7-F4A7E0B66726', 'Addendum', 'ADD', '200.00', '7', '2')
INSERT INTO #RawData ([Id], [ObjectGuid], [Name], [Code], [Price], [ServiceCategoryId], [ServicePortfolioId]) VALUES ('18', '3C23A6E4-765C-4B61-B6F5-FFC9EAFCAE21', 'Intake Assistance', 'I', '200.00', '2', '1')
INSERT INTO #RawData ([Id], [ObjectGuid], [Name], [Code], [Price], [ServiceCategoryId], [ServicePortfolioId]) VALUES ('19', '8EC089F5-546D-4386-8AD4-490FBAB823BB', 'Case Coordination', 'CC', '100.00', '1', '1')
INSERT INTO #RawData ([Id], [ObjectGuid], [Name], [Code], [Price], [ServiceCategoryId], [ServicePortfolioId]) VALUES ('20', '8A4FCF4B-A5B3-4990-B867-9CD20C5C4064', 'Accident Benefit - Post 104', 'Post-104', '1200.00', '5', '2')

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
	WHERE d.[ObjectGuid] = s.[ObjectGuid] AND d.[Name] = s.[Name] AND d.[Code] = s.[Code] AND d.[Price] = s.[Price] AND d.[ServiceCategoryId] = s.[ServiceCategoryId] AND d.[ServicePortfolioId] = s.[ServicePortfolioId]


	SELECT @CounterIdentical = COUNT(*) FROM @IdenticalRecordIDs


	--Update any rows that already exist
	UPDATE [dbo].[Service] SET [ObjectGuid] = s.[ObjectGuid], [Name] = s.[Name], [Code] = s.[Code], [Price] = s.[Price], [ServiceCategoryId] = s.[ServiceCategoryId], [ServicePortfolioId] = s.[ServicePortfolioId]
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
	INSERT INTO [dbo].[Service] ([Id], [ObjectGuid], [Name], [Code], [Price], [ServiceCategoryId], [ServicePortfolioId]
	, ModifiedDate, ModifiedUser	)
	OUTPUT Inserted.[Id] INTO @IdenticalRecordIDs
	SELECT [Id], [ObjectGuid], [Name], [Code], [Price], [ServiceCategoryId], [ServicePortfolioId]
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
		[Sequence],
		[EstimatedHours],
		[HourlyRate]
	INTO #RawData
	FROM (
		SELECT 
			[Id],
			[ServiceId],
			[TaskId],
			[Sequence],
			[EstimatedHours],
			[HourlyRate]
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
INSERT INTO #RawData ([Id], [ServiceId], [TaskId], [Sequence], [EstimatedHours], [HourlyRate]) VALUES ('1', '7', '1', '1', '1.00', '25.00')
INSERT INTO #RawData ([Id], [ServiceId], [TaskId], [Sequence], [EstimatedHours], [HourlyRate]) VALUES ('2', '7', '11', '2', '1.00', '30.00')
INSERT INTO #RawData ([Id], [ServiceId], [TaskId], [Sequence], [EstimatedHours], [HourlyRate]) VALUES ('3', '7', '12', '3', '1.00', '35.00')
INSERT INTO #RawData ([Id], [ServiceId], [TaskId], [Sequence], [EstimatedHours], [HourlyRate]) VALUES ('5', '7', '8', '5', '1.00', '35.00')
INSERT INTO #RawData ([Id], [ServiceId], [TaskId], [Sequence]) VALUES ('6', '7', '9', '6')
INSERT INTO #RawData ([Id], [ServiceId], [TaskId], [Sequence], [EstimatedHours], [HourlyRate]) VALUES ('7', '20', '1', '1', '1.00', '25.00')
INSERT INTO #RawData ([Id], [ServiceId], [TaskId], [Sequence], [EstimatedHours], [HourlyRate]) VALUES ('8', '20', '11', '2', '1.00', '30.00')
INSERT INTO #RawData ([Id], [ServiceId], [TaskId], [Sequence], [EstimatedHours], [HourlyRate]) VALUES ('9', '20', '12', '3', '1.00', '35.00')
INSERT INTO #RawData ([Id], [ServiceId], [TaskId], [Sequence], [EstimatedHours], [HourlyRate]) VALUES ('10', '20', '8', '5', '1.00', '35.00')
INSERT INTO #RawData ([Id], [ServiceId], [TaskId], [Sequence]) VALUES ('11', '20', '9', '6')

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
	WHERE d.[ServiceId] = s.[ServiceId] AND d.[TaskId] = s.[TaskId] AND d.[Sequence] = s.[Sequence] AND d.[EstimatedHours] = s.[EstimatedHours] AND d.[HourlyRate] = s.[HourlyRate]


	SELECT @CounterIdentical = COUNT(*) FROM @IdenticalRecordIDs


	--Update any rows that already exist
	UPDATE [dbo].[ServiceTask] SET [ServiceId] = s.[ServiceId], [TaskId] = s.[TaskId], [Sequence] = s.[Sequence], [EstimatedHours] = s.[EstimatedHours], [HourlyRate] = s.[HourlyRate]
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
	INSERT INTO [dbo].[ServiceTask] ([Id], [ServiceId], [TaskId], [Sequence], [EstimatedHours], [HourlyRate]
	, ModifiedDate, ModifiedUser	)
	OUTPUT Inserted.[Id] INTO @IdenticalRecordIDs
	SELECT [Id], [ServiceId], [TaskId], [Sequence], [EstimatedHours], [HourlyRate]
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
--########################################################
--########### [dbo].[Lookup]
--########################################################
	--Turn off the output of how many rows affected so output window isn't crowded
	SET NOCOUNT ON
	PRINT 'Updating table [dbo].[Lookup]'

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
		FROM [dbo].[Lookup] 
		WHERE 1=2
	) t
	GO

	/* 
	Because the temp table is now created using a SELECT INTO it can be created with IDENTITY fields. 
	Turn on IDENTITY INSERT if they exist and turn it back off after you are done.
	*/
	IF ((SELECT OBJECTPROPERTY( OBJECT_ID(N'[dbo].[Lookup]'), 'TableHasIdentity')) = 1)
		SET IDENTITY_INSERT #RawData ON
	GO

	--This is the data from the file at time of script generation
INSERT INTO #RawData ([Id], [Name]) VALUES ('1', 'RelationshipStatus')
INSERT INTO #RawData ([Id], [Name]) VALUES ('2', 'ServiceRequestStatus')
INSERT INTO #RawData ([Id], [Name]) VALUES ('3', 'TaskStatus')
INSERT INTO #RawData ([Id], [Name]) VALUES ('4', 'Locations')
INSERT INTO #RawData ([Id], [Name]) VALUES ('5', 'PhysicianLocationStatus')
INSERT INTO #RawData ([Id], [Name]) VALUES ('6', 'Specialties')
INSERT INTO #RawData ([Id], [Name]) VALUES ('7', 'ServiceStatus')
INSERT INTO #RawData ([Id], [Name]) VALUES ('8', 'TaskPhase')
INSERT INTO #RawData ([Id], [Name]) VALUES ('9', 'Timeline')

	IF ((SELECT OBJECTPROPERTY( OBJECT_ID(N'[dbo].[Lookup]'), 'TableHasIdentity')) = 1)
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
	LEFT JOIN [dbo].[Lookup] d ON d.[Id] = s.[Id]
	WHERE d.[Name] = s.[Name]


	SELECT @CounterIdentical = COUNT(*) FROM @IdenticalRecordIDs


	--Update any rows that already exist
	UPDATE [dbo].[Lookup] SET [Name] = s.[Name]
		, ModifiedDate = GETDATE()
	, ModifiedUser = SUSER_NAME()
		OUTPUT Inserted.[Id] INTO @IdenticalRecordIDs
	FROM #RawData s
	LEFT JOIN [dbo].[Lookup] d ON s.[Id] = d.[Id] 
	WHERE s.[Id] NOT IN (SELECT id FROM @IdenticalRecordIDs)


	SELECT @CounterUpdated = COUNT(*) - @CounterIdentical FROM @IdenticalRecordIDs


	IF ((SELECT OBJECTPROPERTY( OBJECT_ID(N'[dbo].[Lookup]'), 'TableHasIdentity')) = 1)
		SET IDENTITY_INSERT [dbo].[Lookup] ON

	--Insert any new rows
	INSERT INTO [dbo].[Lookup] ([Id], [Name]
	, ModifiedDate, ModifiedUser	)
	OUTPUT Inserted.[Id] INTO @IdenticalRecordIDs
	SELECT [Id], [Name]
	, GETDATE(), SUSER_NAME()	FROM #RawData
	WHERE [Id] NOT IN (SELECT id FROM @IdenticalRecordIDs)

	IF ((SELECT OBJECTPROPERTY( OBJECT_ID(N'[dbo].[Lookup]'), 'TableHasIdentity')) = 1)
		SET IDENTITY_INSERT [dbo].[Lookup] OFF

	SELECT @CounterNew = COUNT(*) - @CounterIdentical - @CounterUpdated FROM @IdenticalRecordIDs

	PRINT 'Records in Project: '+ CONVERT(NVARCHAR(MAX),@CounterInProject)
	PRINT 'Identical Records: '+ CONVERT(NVARCHAR(MAX),@CounterIdentical)
	PRINT 'Updated Records: '+ CONVERT(NVARCHAR(MAX),@CounterUpdated)
	PRINT 'New Records: '+ CONVERT(NVARCHAR(MAX),@CounterNew)

	GO
--########################################################
--########### [dbo].[LookupItem]
--########################################################
	--Turn off the output of how many rows affected so output window isn't crowded
	SET NOCOUNT ON
	PRINT 'Updating table [dbo].[LookupItem]'

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
		[LookupId],
		[Text],
		[Value]
	INTO #RawData
	FROM (
		SELECT 
			[Id],
			[LookupId],
			[Text],
			[Value]
		FROM [dbo].[LookupItem] 
		WHERE 1=2
	) t
	GO

	/* 
	Because the temp table is now created using a SELECT INTO it can be created with IDENTITY fields. 
	Turn on IDENTITY INSERT if they exist and turn it back off after you are done.
	*/
	IF ((SELECT OBJECTPROPERTY( OBJECT_ID(N'[dbo].[LookupItem]'), 'TableHasIdentity')) = 1)
		SET IDENTITY_INSERT #RawData ON
	GO

	--This is the data from the file at time of script generation
INSERT INTO #RawData ([Id], [LookupId], [Text], [Value]) VALUES ('1', '1', 'Interested', '1')
INSERT INTO #RawData ([Id], [LookupId], [Text], [Value]) VALUES ('2', '1', 'Not interested', '2')
INSERT INTO #RawData ([Id], [LookupId], [Text], [Value]) VALUES ('4', '1', 'In Progress', '3')
INSERT INTO #RawData ([Id], [LookupId], [Text], [Value]) VALUES ('5', '1', 'Active', '4')
INSERT INTO #RawData ([Id], [LookupId], [Text], [Value]) VALUES ('8', '1', 'Assessor Package Submitted', '5')
INSERT INTO #RawData ([Id], [LookupId], [Text], [Value]) VALUES ('9', '1', 'Ready to Work', '6')
INSERT INTO #RawData ([Id], [LookupId], [Text], [Value]) VALUES ('10', '2', 'Open', '1')
INSERT INTO #RawData ([Id], [LookupId], [Text], [Value]) VALUES ('11', '2', 'Closed', '2')
INSERT INTO #RawData ([Id], [LookupId], [Text], [Value]) VALUES ('14', '3', 'ToDo', '1')
INSERT INTO #RawData ([Id], [LookupId], [Text], [Value]) VALUES ('15', '3', 'In Progress', '2')
INSERT INTO #RawData ([Id], [LookupId], [Text], [Value]) VALUES ('16', '3', 'Done', '3')
INSERT INTO #RawData ([Id], [LookupId], [Text], [Value]) VALUES ('18', '4', 'Oakville', '1')
INSERT INTO #RawData ([Id], [LookupId], [Text], [Value]) VALUES ('19', '4', 'GTA', '2')
INSERT INTO #RawData ([Id], [LookupId], [Text], [Value]) VALUES ('20', '4', 'London', '3')
INSERT INTO #RawData ([Id], [LookupId], [Text], [Value]) VALUES ('21', '4', 'Ottawa', '4')
INSERT INTO #RawData ([Id], [LookupId], [Text], [Value]) VALUES ('22', '4', 'Hamilton/Niagara', '5')
INSERT INTO #RawData ([Id], [LookupId], [Text], [Value]) VALUES ('23', '4', 'Vancouver', '6')
INSERT INTO #RawData ([Id], [LookupId], [Text], [Value]) VALUES ('24', '5', 'Primary Location', '1')
INSERT INTO #RawData ([Id], [LookupId], [Text], [Value]) VALUES ('25', '5', 'Willing To Travel', '2')
INSERT INTO #RawData ([Id], [LookupId], [Text], [Value]) VALUES ('26', '5', 'Unwilling To Travel', '3')
INSERT INTO #RawData ([Id], [LookupId], [Text], [Value]) VALUES ('27', '6', 'Physiatry', '1')
INSERT INTO #RawData ([Id], [LookupId], [Text], [Value]) VALUES ('28', '6', 'Orthopedics', '2')
INSERT INTO #RawData ([Id], [LookupId], [Text], [Value]) VALUES ('29', '6', 'Neurology', '3')
INSERT INTO #RawData ([Id], [LookupId], [Text], [Value]) VALUES ('30', '7', 'No Show', '1')
INSERT INTO #RawData ([Id], [LookupId], [Text], [Value]) VALUES ('31', '7', 'Cancellation', '2')
INSERT INTO #RawData ([Id], [LookupId], [Text], [Value]) VALUES ('32', '7', 'Late Cancellation', '3')
INSERT INTO #RawData ([Id], [LookupId], [Text], [Value]) VALUES ('33', '8', 'Preparation', '1')
INSERT INTO #RawData ([Id], [LookupId], [Text], [Value]) VALUES ('34', '8', 'Assessment', '2')
INSERT INTO #RawData ([Id], [LookupId], [Text], [Value]) VALUES ('35', '8', 'Reporting', '3')
INSERT INTO #RawData ([Id], [LookupId], [Text], [Value]) VALUES ('36', '7', 'Examination Complete', '4')
INSERT INTO #RawData ([Id], [LookupId], [Text]) VALUES ('37', '9', 'Past')
INSERT INTO #RawData ([Id], [LookupId], [Text]) VALUES ('38', '9', 'Present')
INSERT INTO #RawData ([Id], [LookupId], [Text]) VALUES ('39', '9', 'Future')
INSERT INTO #RawData ([Id], [LookupId], [Text]) VALUES ('40', '3', 'Obsolete')

	IF ((SELECT OBJECTPROPERTY( OBJECT_ID(N'[dbo].[LookupItem]'), 'TableHasIdentity')) = 1)
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
	LEFT JOIN [dbo].[LookupItem] d ON d.[Id] = s.[Id]
	WHERE d.[LookupId] = s.[LookupId] AND d.[Text] = s.[Text] AND d.[Value] = s.[Value]


	SELECT @CounterIdentical = COUNT(*) FROM @IdenticalRecordIDs


	--Update any rows that already exist
	UPDATE [dbo].[LookupItem] SET [LookupId] = s.[LookupId], [Text] = s.[Text], [Value] = s.[Value]
		, ModifiedDate = GETDATE()
	, ModifiedUser = SUSER_NAME()
		OUTPUT Inserted.[Id] INTO @IdenticalRecordIDs
	FROM #RawData s
	LEFT JOIN [dbo].[LookupItem] d ON s.[Id] = d.[Id] 
	WHERE s.[Id] NOT IN (SELECT id FROM @IdenticalRecordIDs)


	SELECT @CounterUpdated = COUNT(*) - @CounterIdentical FROM @IdenticalRecordIDs


	IF ((SELECT OBJECTPROPERTY( OBJECT_ID(N'[dbo].[LookupItem]'), 'TableHasIdentity')) = 1)
		SET IDENTITY_INSERT [dbo].[LookupItem] ON

	--Insert any new rows
	INSERT INTO [dbo].[LookupItem] ([Id], [LookupId], [Text], [Value]
	, ModifiedDate, ModifiedUser	)
	OUTPUT Inserted.[Id] INTO @IdenticalRecordIDs
	SELECT [Id], [LookupId], [Text], [Value]
	, GETDATE(), SUSER_NAME()	FROM #RawData
	WHERE [Id] NOT IN (SELECT id FROM @IdenticalRecordIDs)

	IF ((SELECT OBJECTPROPERTY( OBJECT_ID(N'[dbo].[LookupItem]'), 'TableHasIdentity')) = 1)
		SET IDENTITY_INSERT [dbo].[LookupItem] OFF

	SELECT @CounterNew = COUNT(*) - @CounterIdentical - @CounterUpdated FROM @IdenticalRecordIDs

	PRINT 'Records in Project: '+ CONVERT(NVARCHAR(MAX),@CounterInProject)
	PRINT 'Identical Records: '+ CONVERT(NVARCHAR(MAX),@CounterIdentical)
	PRINT 'Updated Records: '+ CONVERT(NVARCHAR(MAX),@CounterUpdated)
	PRINT 'New Records: '+ CONVERT(NVARCHAR(MAX),@CounterNew)

	GO
--########################################################
--########### [dbo].[ServiceRequestTemplate]
--########################################################
	--Turn off the output of how many rows affected so output window isn't crowded
	SET NOCOUNT ON
	PRINT 'Updating table [dbo].[ServiceRequestTemplate]'

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
		[ServiceCategoryId]
	INTO #RawData
	FROM (
		SELECT 
			[Id],
			[ServiceCategoryId]
		FROM [dbo].[ServiceRequestTemplate] 
		WHERE 1=2
	) t
	GO

	/* 
	Because the temp table is now created using a SELECT INTO it can be created with IDENTITY fields. 
	Turn on IDENTITY INSERT if they exist and turn it back off after you are done.
	*/
	IF ((SELECT OBJECTPROPERTY( OBJECT_ID(N'[dbo].[ServiceRequestTemplate]'), 'TableHasIdentity')) = 1)
		SET IDENTITY_INSERT #RawData ON
	GO

	--This is the data from the file at time of script generation
INSERT INTO #RawData ([Id], [ServiceCategoryId]) VALUES ('1', '5')

	IF ((SELECT OBJECTPROPERTY( OBJECT_ID(N'[dbo].[ServiceRequestTemplate]'), 'TableHasIdentity')) = 1)
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
	LEFT JOIN [dbo].[ServiceRequestTemplate] d ON d.[Id] = s.[Id]
	WHERE d.[ServiceCategoryId] = s.[ServiceCategoryId]


	SELECT @CounterIdentical = COUNT(*) FROM @IdenticalRecordIDs


	--Update any rows that already exist
	UPDATE [dbo].[ServiceRequestTemplate] SET [ServiceCategoryId] = s.[ServiceCategoryId]
		, ModifiedDate = GETDATE()
	, ModifiedUser = SUSER_NAME()
		OUTPUT Inserted.[Id] INTO @IdenticalRecordIDs
	FROM #RawData s
	LEFT JOIN [dbo].[ServiceRequestTemplate] d ON s.[Id] = d.[Id] 
	WHERE s.[Id] NOT IN (SELECT id FROM @IdenticalRecordIDs)


	SELECT @CounterUpdated = COUNT(*) - @CounterIdentical FROM @IdenticalRecordIDs


	IF ((SELECT OBJECTPROPERTY( OBJECT_ID(N'[dbo].[ServiceRequestTemplate]'), 'TableHasIdentity')) = 1)
		SET IDENTITY_INSERT [dbo].[ServiceRequestTemplate] ON

	--Insert any new rows
	INSERT INTO [dbo].[ServiceRequestTemplate] ([Id], [ServiceCategoryId]
	, ModifiedDate, ModifiedUser	)
	OUTPUT Inserted.[Id] INTO @IdenticalRecordIDs
	SELECT [Id], [ServiceCategoryId]
	, GETDATE(), SUSER_NAME()	FROM #RawData
	WHERE [Id] NOT IN (SELECT id FROM @IdenticalRecordIDs)

	IF ((SELECT OBJECTPROPERTY( OBJECT_ID(N'[dbo].[ServiceRequestTemplate]'), 'TableHasIdentity')) = 1)
		SET IDENTITY_INSERT [dbo].[ServiceRequestTemplate] OFF

	SELECT @CounterNew = COUNT(*) - @CounterIdentical - @CounterUpdated FROM @IdenticalRecordIDs

	PRINT 'Records in Project: '+ CONVERT(NVARCHAR(MAX),@CounterInProject)
	PRINT 'Identical Records: '+ CONVERT(NVARCHAR(MAX),@CounterIdentical)
	PRINT 'Updated Records: '+ CONVERT(NVARCHAR(MAX),@CounterUpdated)
	PRINT 'New Records: '+ CONVERT(NVARCHAR(MAX),@CounterNew)

	GO
--########################################################
--########### [dbo].[ServiceRequestTemplateTask]
--########################################################
	--Turn off the output of how many rows affected so output window isn't crowded
	SET NOCOUNT ON
	PRINT 'Updating table [dbo].[ServiceRequestTemplateTask]'

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
		[ServiceRequestTemplateId],
		[TaskPhaseId],
		[TaskName],
		[Guidance],
		[Sequence],
		[IsBillable],
		[ResponsibleRoleId]
	INTO #RawData
	FROM (
		SELECT 
			[Id],
			[ServiceRequestTemplateId],
			[TaskPhaseId],
			[TaskName],
			[Guidance],
			[Sequence],
			[IsBillable],
			[ResponsibleRoleId]
		FROM [dbo].[ServiceRequestTemplateTask] 
		WHERE 1=2
	) t
	GO

	/* 
	Because the temp table is now created using a SELECT INTO it can be created with IDENTITY fields. 
	Turn on IDENTITY INSERT if they exist and turn it back off after you are done.
	*/
	IF ((SELECT OBJECTPROPERTY( OBJECT_ID(N'[dbo].[ServiceRequestTemplateTask]'), 'TableHasIdentity')) = 1)
		SET IDENTITY_INSERT #RawData ON
	GO

	--This is the data from the file at time of script generation
INSERT INTO #RawData ([Id], [ServiceRequestTemplateId], [TaskPhaseId], [TaskName], [Guidance], [Sequence], [IsBillable], [ResponsibleRoleId]) VALUES ('1', '1', '33', 'Create case folder and calendar event', 'Create the calendar event with the calendar title value, copy the location address into the location of the event, add the Case URL and the Case Folder name to the event description.', '1', '0', '9eab89c0-225c-4027-9f42-cc35e5656b14')
INSERT INTO #RawData ([Id], [ServiceRequestTemplateId], [TaskPhaseId], [TaskName], [Guidance], [Sequence], [IsBillable], [ResponsibleRoleId]) VALUES ('2', '1', '33', 'Assign resources', 'Assign resources in the web app, invite on the calendar event, grant access to the case folder.', '2', '0', '9eab89c0-225c-4027-9f42-cc35e5656b14')
INSERT INTO #RawData ([Id], [ServiceRequestTemplateId], [TaskPhaseId], [TaskName], [Guidance], [Sequence], [IsBillable], [ResponsibleRoleId]) VALUES ('3', '1', '33', 'Save med brief to case folder', 'Download from company portal, secure docs, or email and save to the MedBrief folder in the case folder.', '3', '0', '9eab89c0-225c-4027-9f42-cc35e5656b14')
INSERT INTO #RawData ([Id], [ServiceRequestTemplateId], [TaskPhaseId], [TaskName], [Sequence], [IsBillable], [ResponsibleRoleId]) VALUES ('4', '1', '34', 'Write the document review', '4', '1', '22B5C8AC-2C96-4A74-8057-976914031A7E')
INSERT INTO #RawData ([Id], [ServiceRequestTemplateId], [TaskPhaseId], [TaskName], [Sequence], [IsBillable], [ResponsibleRoleId]) VALUES ('5', '1', '34', 'Conduct the intake interview', '5', '1', '9dd582a0-cf86-4fc0-8894-477266068c12')
INSERT INTO #RawData ([Id], [ServiceRequestTemplateId], [TaskPhaseId], [TaskName], [Sequence], [IsBillable], [ResponsibleRoleId]) VALUES ('6', '1', '34', 'Complete physician sections', '6', '0', '8359141f-e423-4e48-8925-4624ba86245a')
INSERT INTO #RawData ([Id], [ServiceRequestTemplateId], [TaskPhaseId], [TaskName], [Guidance], [Sequence], [IsBillable], [ResponsibleRoleId]) VALUES ('7', '1', '34', 'Draft the report', 'This notifies the physician that you have completed the report to the best of your ability and it iws ready for their review and approval.', '7', '1', '9dd582a0-cf86-4fc0-8894-477266068c12')
INSERT INTO #RawData ([Id], [ServiceRequestTemplateId], [TaskPhaseId], [TaskName], [Guidance], [Sequence], [IsBillable], [ResponsibleRoleId]) VALUES ('9', '1', '34', 'Approve the report for submission', 'Marking this task as complete will submit the report and invoice to the company.', '8', '0', '8359141f-e423-4e48-8925-4624ba86245a')
INSERT INTO #RawData ([Id], [ServiceRequestTemplateId], [TaskPhaseId], [TaskName], [Sequence], [IsBillable], [ResponsibleRoleId]) VALUES ('10', '1', '34', 'Submit the report', '9', '0', '9eab89c0-225c-4027-9f42-cc35e5656b14')
INSERT INTO #RawData ([Id], [ServiceRequestTemplateId], [TaskPhaseId], [TaskName], [Sequence], [IsBillable], [ResponsibleRoleId]) VALUES ('11', '1', '35', 'Notify physician of QA comments', '10', '0', '9eab89c0-225c-4027-9f42-cc35e5656b14')
INSERT INTO #RawData ([Id], [ServiceRequestTemplateId], [TaskPhaseId], [TaskName], [Sequence], [IsBillable], [ResponsibleRoleId]) VALUES ('12', '1', '35', 'Respond to QA comments', '11', '0', '8359141f-e423-4e48-8925-4624ba86245a')
INSERT INTO #RawData ([Id], [ServiceRequestTemplateId], [TaskPhaseId], [TaskName], [Sequence], [IsBillable], [ResponsibleRoleId]) VALUES ('13', '1', '35', 'Obtain finalized report from company', '12', '0', '9eab89c0-225c-4027-9f42-cc35e5656b14')

	IF ((SELECT OBJECTPROPERTY( OBJECT_ID(N'[dbo].[ServiceRequestTemplateTask]'), 'TableHasIdentity')) = 1)
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
	LEFT JOIN [dbo].[ServiceRequestTemplateTask] d ON d.[Id] = s.[Id]
	WHERE d.[ServiceRequestTemplateId] = s.[ServiceRequestTemplateId] AND d.[TaskPhaseId] = s.[TaskPhaseId] AND d.[TaskName] = s.[TaskName] AND d.[Guidance] = s.[Guidance] AND d.[Sequence] = s.[Sequence] AND d.[IsBillable] = s.[IsBillable] AND d.[ResponsibleRoleId] = s.[ResponsibleRoleId]


	SELECT @CounterIdentical = COUNT(*) FROM @IdenticalRecordIDs


	--Update any rows that already exist
	UPDATE [dbo].[ServiceRequestTemplateTask] SET [ServiceRequestTemplateId] = s.[ServiceRequestTemplateId], [TaskPhaseId] = s.[TaskPhaseId], [TaskName] = s.[TaskName], [Guidance] = s.[Guidance], [Sequence] = s.[Sequence], [IsBillable] = s.[IsBillable], [ResponsibleRoleId] = s.[ResponsibleRoleId]
		, ModifiedDate = GETDATE()
	, ModifiedUser = SUSER_NAME()
		OUTPUT Inserted.[Id] INTO @IdenticalRecordIDs
	FROM #RawData s
	LEFT JOIN [dbo].[ServiceRequestTemplateTask] d ON s.[Id] = d.[Id] 
	WHERE s.[Id] NOT IN (SELECT id FROM @IdenticalRecordIDs)


	SELECT @CounterUpdated = COUNT(*) - @CounterIdentical FROM @IdenticalRecordIDs


	IF ((SELECT OBJECTPROPERTY( OBJECT_ID(N'[dbo].[ServiceRequestTemplateTask]'), 'TableHasIdentity')) = 1)
		SET IDENTITY_INSERT [dbo].[ServiceRequestTemplateTask] ON

	--Insert any new rows
	INSERT INTO [dbo].[ServiceRequestTemplateTask] ([Id], [ServiceRequestTemplateId], [TaskPhaseId], [TaskName], [Guidance], [Sequence], [IsBillable], [ResponsibleRoleId]
	, ModifiedDate, ModifiedUser	)
	OUTPUT Inserted.[Id] INTO @IdenticalRecordIDs
	SELECT [Id], [ServiceRequestTemplateId], [TaskPhaseId], [TaskName], [Guidance], [Sequence], [IsBillable], [ResponsibleRoleId]
	, GETDATE(), SUSER_NAME()	FROM #RawData
	WHERE [Id] NOT IN (SELECT id FROM @IdenticalRecordIDs)

	IF ((SELECT OBJECTPROPERTY( OBJECT_ID(N'[dbo].[ServiceRequestTemplateTask]'), 'TableHasIdentity')) = 1)
		SET IDENTITY_INSERT [dbo].[ServiceRequestTemplateTask] OFF

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
		[ObjectGuid],
		[Name],
		[ResponsibleRoleId],
		[IsBillable],
		[Sequence],
		[IsMilestone],
		[ServiceCategoryId],
		[Guidance],
		[TaskPhaseId],
		[HourlyRate],
		[EstimatedHours]
	INTO #RawData
	FROM (
		SELECT 
			[Id],
			[ObjectGuid],
			[Name],
			[ResponsibleRoleId],
			[IsBillable],
			[Sequence],
			[IsMilestone],
			[ServiceCategoryId],
			[Guidance],
			[TaskPhaseId],
			[HourlyRate],
			[EstimatedHours]
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
INSERT INTO #RawData ([Id], [ObjectGuid], [Name], [ResponsibleRoleId], [IsBillable], [Sequence], [IsMilestone]) VALUES ('1', '483BFBB6-EF26-4F9E-9D3F-6F88D2CB3E2C', 'Case Preparation', '9eab89c0-225c-4027-9f42-cc35e5656b14', '1', '1', '1')
INSERT INTO #RawData ([Id], [ObjectGuid], [ServiceCategoryId], [Name], [Guidance], [TaskPhaseId], [ResponsibleRoleId], [IsBillable], [HourlyRate], [EstimatedHours], [Sequence], [IsMilestone]) VALUES ('8', '6312316D-3813-4EAB-9C8C-9C10528C92E1', '5', 'Draft the report', 'This notifies the physician that the report is ready for their review and approval.', '34', '9dd582a0-cf86-4fc0-8894-477266068c12', '1', '35.00', '2.00', '70', '1')
INSERT INTO #RawData ([Id], [ObjectGuid], [ServiceCategoryId], [Name], [Guidance], [TaskPhaseId], [ResponsibleRoleId], [IsBillable], [Sequence], [IsMilestone]) VALUES ('9', '34892EF1-225A-4226-A15B-0394086EF270', '5', 'Approve the report for submission', 'Marking this task as complete will submit the report and invoice to the company.', '34', '8359141f-e423-4e48-8925-4624ba86245a', '0', '80', '1')
INSERT INTO #RawData ([Id], [ObjectGuid], [ServiceCategoryId], [Name], [TaskPhaseId], [ResponsibleRoleId], [IsBillable], [HourlyRate], [EstimatedHours], [Sequence], [IsMilestone]) VALUES ('11', 'F44ACEDB-78C4-4034-8E28-6F4CB95E6DDA', '5', 'Write the document review', '34', '22B5C8AC-2C96-4A74-8057-976914031A7E', '1', '30.00', '2.00', '40', '0')
INSERT INTO #RawData ([Id], [ObjectGuid], [ServiceCategoryId], [Name], [TaskPhaseId], [ResponsibleRoleId], [IsBillable], [HourlyRate], [EstimatedHours], [Sequence], [IsMilestone]) VALUES ('12', '1F789573-44A3-4A6F-B2CC-28A29E4A939A', '5', 'Conduct the intake interview', '34', '9dd582a0-cf86-4fc0-8894-477266068c12', '1', '35.00', '1.00', '50', '0')
INSERT INTO #RawData ([Id], [ObjectGuid], [ServiceCategoryId], [Name], [TaskPhaseId], [ResponsibleRoleId], [IsBillable], [Sequence], [IsMilestone]) VALUES ('15', 'AA9CCE73-0924-4D85-AAC3-5C09D151F9A9', '5', 'Complete physician sections', '34', '8359141f-e423-4e48-8925-4624ba86245a', '0', '60', '1')
INSERT INTO #RawData ([Id], [ObjectGuid], [ServiceCategoryId], [Name], [Guidance], [TaskPhaseId], [ResponsibleRoleId], [IsBillable], [HourlyRate], [EstimatedHours], [Sequence], [IsMilestone]) VALUES ('16', 'D5A87506-051E-4EDB-A205-2771A5A5D299', '5', 'Create case folder and calendar event', 'Create the calendar event with the calendar title value, copy the location address into the location of the event, add the Case URL and the Case Folder name to the event description.', '33', '9eab89c0-225c-4027-9f42-cc35e5656b14', '0', '25.00', '0.25', '10', '0')
INSERT INTO #RawData ([Id], [ObjectGuid], [ServiceCategoryId], [Name], [Guidance], [TaskPhaseId], [ResponsibleRoleId], [IsBillable], [HourlyRate], [EstimatedHours], [Sequence], [IsMilestone]) VALUES ('17', 'B38E7C22-DC20-4803-9BE0-6E24D35CCE69', '5', 'Assign resources', 'Assign resources in the web app, invite on the calendar event, grant access to the case folder.', '33', '9eab89c0-225c-4027-9f42-cc35e5656b14', '0', '25.00', '0.25', '20', '0')
INSERT INTO #RawData ([Id], [ObjectGuid], [ServiceCategoryId], [Name], [Guidance], [TaskPhaseId], [ResponsibleRoleId], [IsBillable], [HourlyRate], [EstimatedHours], [Sequence], [IsMilestone]) VALUES ('18', '877636D6-9807-4E8F-9F74-B01844AA0167', '5', 'Save med brief to case folder', 'Download from company portal, secure docs, or email and save to the MedBrief folder in the case folder.', '33', '9eab89c0-225c-4027-9f42-cc35e5656b14', '0', '25.00', '0.25', '30', '0')
INSERT INTO #RawData ([Id], [ObjectGuid], [ServiceCategoryId], [Name], [TaskPhaseId], [ResponsibleRoleId], [IsBillable], [Sequence], [IsMilestone]) VALUES ('19', 'DC5FA10B-0191-40E3-AAEA-E4BAC4E2DE29', '5', 'Submit the report', '34', '9eab89c0-225c-4027-9f42-cc35e5656b14', '0', '90', '0')
INSERT INTO #RawData ([Id], [ObjectGuid], [ServiceCategoryId], [Name], [TaskPhaseId], [ResponsibleRoleId], [IsBillable], [Sequence], [IsMilestone]) VALUES ('20', 'EABCB486-89DF-47A5-9A55-827AD1C46D65', '5', 'Waiting for QA confirmation', '35', '9eab89c0-225c-4027-9f42-cc35e5656b14', '0', '110', '0')
INSERT INTO #RawData ([Id], [ObjectGuid], [ServiceCategoryId], [Name], [TaskPhaseId], [ResponsibleRoleId], [IsBillable], [Sequence], [IsMilestone]) VALUES ('21', 'FFDF2F2F-14CB-4F1A-A80C-680F178ABF1D', '5', 'Respond to QA comments', '35', '8359141f-e423-4e48-8925-4624ba86245a', '0', '120', '0')
INSERT INTO #RawData ([Id], [ObjectGuid], [ServiceCategoryId], [Name], [TaskPhaseId], [ResponsibleRoleId], [IsBillable], [Sequence], [IsMilestone]) VALUES ('24', '5CEF61A4-E695-4921-BCB6-B245381B0F61', '5', 'Submit the invoice', '34', '9eab89c0-225c-4027-9f42-cc35e5656b14', '0', '100', '0')

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
	WHERE d.[ObjectGuid] = s.[ObjectGuid] AND d.[Name] = s.[Name] AND d.[ResponsibleRoleId] = s.[ResponsibleRoleId] AND d.[IsBillable] = s.[IsBillable] AND d.[Sequence] = s.[Sequence] AND d.[IsMilestone] = s.[IsMilestone] AND d.[ServiceCategoryId] = s.[ServiceCategoryId] AND d.[Guidance] = s.[Guidance] AND d.[TaskPhaseId] = s.[TaskPhaseId] AND d.[HourlyRate] = s.[HourlyRate] AND d.[EstimatedHours] = s.[EstimatedHours]


	SELECT @CounterIdentical = COUNT(*) FROM @IdenticalRecordIDs


	--Update any rows that already exist
	UPDATE [dbo].[Task] SET [ObjectGuid] = s.[ObjectGuid], [Name] = s.[Name], [ResponsibleRoleId] = s.[ResponsibleRoleId], [IsBillable] = s.[IsBillable], [Sequence] = s.[Sequence], [IsMilestone] = s.[IsMilestone], [ServiceCategoryId] = s.[ServiceCategoryId], [Guidance] = s.[Guidance], [TaskPhaseId] = s.[TaskPhaseId], [HourlyRate] = s.[HourlyRate], [EstimatedHours] = s.[EstimatedHours]
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
	INSERT INTO [dbo].[Task] ([Id], [ObjectGuid], [Name], [ResponsibleRoleId], [IsBillable], [Sequence], [IsMilestone], [ServiceCategoryId], [Guidance], [TaskPhaseId], [HourlyRate], [EstimatedHours]
	, ModifiedDate, ModifiedUser	)
	OUTPUT Inserted.[Id] INTO @IdenticalRecordIDs
	SELECT [Id], [ObjectGuid], [Name], [ResponsibleRoleId], [IsBillable], [Sequence], [IsMilestone], [ServiceCategoryId], [Guidance], [TaskPhaseId], [HourlyRate], [EstimatedHours]
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

