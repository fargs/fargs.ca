 
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
		[CompanyId],
		[IsTestRecord],
		[RoleLevelId],
		[CompanyName],
		[HourlyRate],
		[Title],
		[LockoutEndDateUtc]
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
			[CompanyId],
			[IsTestRecord],
			[RoleLevelId],
			[CompanyName],
			[HourlyRate],
			[Title],
			[LockoutEndDateUtc]
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
INSERT INTO #RawData ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnabled], [AccessFailedCount], [UserName], [FirstName], [LastName], [CompanyId], [IsTestRecord]) VALUES ('0304e7d6-235e-4f3f-b3a9-cbaf575d1514', 'lesliefarago+1@gmail.com', '1', 'AEJG78HHCMOeWi+cFFvHE15M8eyvgM+WRBFqa8dKIcIV7Hdbq0WwlOKQZuSFWM8NaA==', 'b720d383-6254-4c3a-8c7b-0fe1e70fc659', '0', '0', '1', '0', 'admin@soma.com', 'Soma', 'Administrator', '4', '0')
INSERT INTO #RawData ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnabled], [AccessFailedCount], [UserName], [FirstName], [LastName], [IsTestRecord], [RoleLevelId]) VALUES ('0538a347-fddc-4f03-97b1-a1e816e888ed', 'lfarago@orvosi.ca', '1', 'AB1qZjLOSxKABX/fgDcXS7BDTXxrN6EcTPlXACM6MwBMuGI0ubk7EuTAAyHoVwvZVQ==', '518569f3-f76c-46d1-bfe9-6852c618b9bb', '0', '0', '1', '0', 'lfarago', 'Leslie', 'Farago', '0', '2')
INSERT INTO #RawData ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnabled], [AccessFailedCount], [UserName], [FirstName], [LastName], [CompanyName], [IsTestRecord]) VALUES ('25f67e43-55b4-447e-913a-807b77536c8e', 'lfarago+physician@orvosi.ca', '1', 'ADJY6ainf2JxQHK8API6Lali/4e9iQyienwZBKXVn9BYImKki9v0SkY3Gf0zj5X7Fg==', '8686750b-c415-4173-b2b5-1589fbcb183b', '0', '0', '1', '0', 'hsolo', 'Han', 'Solo', 'Dr. Han Solo MPC', '0')
INSERT INTO #RawData ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnabled], [AccessFailedCount], [UserName], [FirstName], [LastName], [CompanyName], [IsTestRecord], [HourlyRate]) VALUES ('2f133d64-507e-4ec1-b6e0-ad0a06de77bc', 'atarkowski@orvosi.ca', '1', 'AOGMyDiXW5a7OG+SOiyfI/gbUu9BLNECdCSbX+FhDXUsbekDtYALTjaeGrh4I/ntOA==', 'fd741e61-5f33-452e-8ec7-8166eb5446b5', '0', '0', '1', '0', 'atarkowski', 'Alexandra', 'Tarkowski', 'Alexandra Tarkowski Intake Services', '0', '30.00')
INSERT INTO #RawData ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnabled], [AccessFailedCount], [UserName], [FirstName], [LastName], [CompanyName], [IsTestRecord], [HourlyRate]) VALUES ('3b61845f-1968-4795-ae00-f4e3340a7141', 'lgoodson@orvosi.ca', '0', 'ANtErM24Axgt1fgqdUZQ/uIHBxy882LsrN3h+dEGBWpIs286RQ7o4+78l5GScBH9uw==', 'e114b9e7-8eb8-4e38-9d55-4c77e12ddd73', '0', '0', '1', '0', 'lgoodson@orvosi.ca', 'Lorry', 'Goodson', 'Orvosi', '0', '25.00')
INSERT INTO #RawData ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnabled], [AccessFailedCount], [UserName], [FirstName], [LastName], [CompanyName], [IsTestRecord], [HourlyRate]) VALUES ('46b4f52b-f554-4467-a562-ccf687488675', 'tcorbeil@orvosi.ca', '0', 'AJZERFCHgvQYWFniGb/ZKgIRopjnj4ibcZAfODGncLxFMkpI8s2oAwAjDm857KZK0Q==', 'b009f120-ca59-4164-926e-dbad49bd5553', '0', '0', '1', '0', 'tcorbeil@orvosi.ca', 'Tania', 'Corbeil', 'Tania Corbeil Accounting Services', '0', '45.00')
INSERT INTO #RawData ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnabled], [AccessFailedCount], [UserName], [FirstName], [LastName], [CompanyId], [CompanyName], [IsTestRecord]) VALUES ('4953f430-3dad-40e7-a372-d3effcffe3f8', 'admin@seiden.com', '1', 'ADpyuyQemZDSaoVfhplADWugwyxrV9lkHIU96BA8qTSVGDTaSR4REBplu64Ho+bujg==', '01ef1d0d-40ca-4b1d-bfef-4d46224cd936', '0', '0', '1', '0', 'admin@seiden.com', 'Seiden', 'Administrator', '19', 'Seiden', '0')
INSERT INTO #RawData ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnabled], [AccessFailedCount], [UserName], [FirstName], [LastName], [CompanyId], [IsTestRecord]) VALUES ('585e12fa-7536-48ad-b860-78b16d5fd647', 'lesliefarago+2@gmail.com', '1', 'AAYrI73MyC1q0oDUfyYuwjxVFXRfOluJ7VK5kxg7FDAyLH9kwnAc5kLL2BPbZctotw==', 'ff610047-3905-4f19-bfde-a9db3cf62c45', '0', '0', '1', '0', 'admin@ima.com', 'IMA', 'Administrator', '6', '0')
INSERT INTO #RawData ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnabled], [AccessFailedCount], [UserName], [FirstName], [LastName], [CompanyId], [IsTestRecord], [RoleLevelId]) VALUES ('5b92cd8b-a500-428f-88a2-ad14e0a23624', 'lesliefarago@hotmail.com', '1', 'AETh3DsMtkd3no0XgEcojn8MAsTa++5aFazCKlGUc7Vcr45rxPNrVxZej05Q+E6lIw==', 'a6933b73-3429-4718-bc98-6d988ae9928c', '0', '0', '1', '0', 'admin@dime.com', 'DIME', 'Administrator', '2', '0', '4')
INSERT INTO #RawData ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnabled], [AccessFailedCount], [UserName], [FirstName], [LastName], [CompanyId], [CompanyName], [IsTestRecord]) VALUES ('6a254b1a-6801-4224-9979-0d0ef677e983', 'lesliefarago+3@gmail.com', '1', 'AFfmPgeLxhUOKRK0M9KxpASW8AxFGZfh20JBHgwLUfQ3ANFQb9r82xwUQNJeVJOKGA==', '3adaabf6-834f-4c86-86af-0c6c54231a6c', '0', '0', '1', '0', 'admin@scm.com', 'SCM', 'Administrator', '10', 'SCM', '0')
INSERT INTO #RawData ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnabled], [AccessFailedCount], [UserName], [FirstName], [LastName], [CompanyId], [CompanyName], [IsTestRecord]) VALUES ('6a7172db-343b-4926-b156-63b7a59d30ba', 'darla.campbell@seidenhealth.com', '1', 'AK3T2T3X+I7CTB/7/6DMS/RCTTfVhwMpcEZCfqDOkzrK7+K3IGnA9XuX4sj29pu4Pg==', '28c5aca0-2d3c-4021-8a3e-66aaa3df376f', '0', '0', '1', '0', 'darla.campbell@seidenhealth.com', 'Darla', 'Campbell', '19', 'Seiden', '0')
INSERT INTO #RawData ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnabled], [AccessFailedCount], [UserName], [FirstName], [LastName], [CompanyName], [IsTestRecord], [HourlyRate]) VALUES ('7c8f47bd-fcb1-443e-a703-cdeefb3b69bb', 'mabadir@orvosi.ca', '1', 'ADZXBOJTOraKLYJXjsLdOLG2Vwx3JFqhXVJOpla90mbSdmpTRsrj2RKrgPGkgDRnzQ==', '4427304a-3f5c-425b-ac2c-43f9c7779897', '0', '0', '1', '0', 'mabadir@orvosi.ca', 'Michael', 'Abadir', 'Michael Abadir Intake Services', '0', '45.00')
INSERT INTO #RawData ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnabled], [AccessFailedCount], [UserName], [FirstName], [LastName], [CompanyName], [IsTestRecord], [HourlyRate]) VALUES ('8ad861fa-5cc2-4342-89da-b3709b1e59b5', 'dmiller@orvosi.ca', '1', 'AAXIJl1RuX4kn6Q1/D24J2NQ+6zyyS12B2xZg0WagQdIO8MGtWmKtZt+a0Gyzp4fNA==', 'fffed006-06ef-4cf4-8a18-0ac10e420c09', '0', '0', '1', '0', 'dmiller@orvosi.ca', 'Daryl', 'Miller', 'Orvosi', '0', '35.00')
INSERT INTO #RawData ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnabled], [AccessFailedCount], [UserName], [Title], [FirstName], [LastName], [CompanyName], [IsTestRecord]) VALUES ('8dd4e180-6e3a-4968-a00d-eeb6d2cc7f0c', 'sdessouki@orvosi.ca', '1', 'AIXxBTVgRf2E0d30BNWBwsQkbVEmNq1AJhWJrW8cbZIZ6WIKVNieOBwyYV65xag5DQ==', 'c858bf18-eecc-41c1-a89f-7317b7bf1eb5', '0', '0', '1', '0', 'sdessouki', 'Dr.', 'Shariff', 'Dessouki', 'Dr. Shariff Dessouki MPC', '0')
INSERT INTO #RawData ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnabled], [AccessFailedCount], [UserName], [Title], [FirstName], [LastName], [CompanyName], [IsTestRecord]) VALUES ('8e9885d8-a0f7-49f6-9a3e-ff1b4d52f6a9', 'zwaseem@orvosi.ca', '1', 'ANfUw6tc8b3hg3lLg6ablIgl5wIpff+AYu4Vv9jv3HoVh+jDKBfZ9zZaGAQyKgqTUw==', '34dc28fb-595c-4a63-b992-bb3e91a8b12f', '0', '0', '1', '0', 'zwaseem', 'Dr.', 'Zeeshan', 'Waseem', 'Dr. Zeeshan Waseem MPC', '0')
INSERT INTO #RawData ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnabled], [AccessFailedCount], [UserName], [Title], [FirstName], [LastName], [CompanyName], [IsTestRecord]) VALUES ('a34268d3-3b5e-4632-a31b-0eb8c8f73a50', 'jfennell@orvosi.ca', '1', 'AF8iGGUTpWvFb6mG+j13t7EfXxvUbEUO3t6fKDWVToRwXH9z7pCE0UD3HKgrITJ7+g==', '9c4e9060-6de2-4ad4-b3c2-ffaef85ee84e', '0', '0', '1', '0', 'jfennell', 'Dr.', 'Jeremy', 'Fennell', 'Dr. Jeremy Fennel MPC', '0')
INSERT INTO #RawData ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnabled], [AccessFailedCount], [UserName], [FirstName], [LastName], [CompanyId], [CompanyName], [IsTestRecord]) VALUES ('a35b2f60-08cf-484a-925a-21e89d0fb0dd', 'lesliefarago+test1@gmail.com', '1', 'AO0R8gszeY98PYkIqoAIO1JZ+DbbPBVOnZsweTebDRnPgtL+NsoQOuXHbb1b1v54kA==', 'a6d0de6e-7926-406d-ac26-f2ba5c14a83a', '0', '0', '1', '0', 'lesliefarago+test1@gmail.com', 'Test', 'Account 1', '19', 'Test Company', '0')
INSERT INTO #RawData ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnabled], [AccessFailedCount], [UserName], [FirstName], [LastName], [CompanyId], [CompanyName], [IsTestRecord]) VALUES ('affb6a2a-175d-4870-b8ff-c844380e2ecf', 'christine.fong@scmhealthsolutions.ca', '1', 'APhKzzDp7pMASG6rbEGRHSZatX0KcS12U4aatthUqoyHqdJ1rb/JJ1H524Yt1Uz1sw==', '32c4f49f-8581-4742-9df0-4fee837c86db', '0', '0', '1', '0', 'christine.fong@scmhealthsolutions.ca', 'Christine', 'Fong', '10', 'SCM', '0')
INSERT INTO #RawData ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnabled], [AccessFailedCount], [UserName], [FirstName], [LastName], [CompanyId], [CompanyName], [IsTestRecord]) VALUES ('d07c8664-00b5-4852-9537-a3a265c28efb', 'holly.sheldon@seidenhealth.com', '1', 'AAgPlLoFQlKRhj4C9swZyfLEG8q4ldWIFUW+DpiZEPZGAvo8L1RLeS1PNdnkkPrv+w==', 'e04218c5-8005-4565-958e-805ea3ddaac0', '0', '0', '1', '0', 'holly.sheldon@seidenhealth.com', 'Holly', 'Sheldon', '19', 'Seiden', '0')
INSERT INTO #RawData ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnabled], [AccessFailedCount], [UserName], [FirstName], [LastName], [CompanyName], [IsTestRecord], [HourlyRate]) VALUES ('d579d0a4-11ce-46f2-97ec-4c2bfc4dc704', 'dserafini@orvosi.ca', '1', 'AOSBb1qWc3dJIc2us/XJFhSP84dPEQffSP183s/eyM0woJHIOnwo7XtQGVSLAt4+ug==', 'de451c0e-c4fc-49bd-87ac-996c309b3cb9', '0', '0', '1', '0', 'dserafini', 'David', 'Serafini', 'Orvosi', '0', '20.00')
INSERT INTO #RawData ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnabled], [AccessFailedCount], [UserName], [FirstName], [LastName], [CompanyName], [IsTestRecord]) VALUES ('e357dd36-8f80-4883-b509-22d04e7f9e1e', 'lesliefarago+intakedocreview@gmail.com', '1', 'AGcl6Z9PuCP7La79a88PszCx3d8ALuEUW/JD+8Tl5hUA+ULRpgxB3VuqiYfm4I24/g==', '88522830-00c9-456b-8afc-b8398dea1401', '0', '0', '1', '0', 'lesliefarago+intakedocreview@gmail.com', 'Chew', 'Bacca', 'Orvosi', '0')
INSERT INTO #RawData ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName], [FirstName], [LastName], [CompanyName], [IsTestRecord]) VALUES ('ea9fb6ca-ffb7-4348-a332-bd2ee2f2c132', 'afarago@orvosi.ca', '1', 'AE8ncpw14RUkDcrhfvc2oGvRFmiTFx0T6jnV/la1BVcZELJJhGj/yiLAnMNBm59M6w==', '385ad415-8374-406b-bbb7-af2a27653d5c', '0', '0', '2015-12-12T20:04:17.540', '1', '0', 'afarago@orvosi.ca', 'Anna', 'Farago', 'Test Company', '0')
INSERT INTO #RawData ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnabled], [AccessFailedCount], [UserName], [FirstName], [LastName], [CompanyName], [IsTestRecord], [HourlyRate]) VALUES ('f61e8eee-cc97-4fe1-9140-0fc219b05f65', 'mcerulli@orvosi.ca', '0', 'APE3Vmv4VN1w/HCLGypXYUkbP0FEUD51jj49Us/7MLC/G3HBhHDoTrylkV/OgxeK0Q==', '5a756e35-1c85-4e4b-a10d-25a450a60c91', '0', '0', '1', '0', 'mcerulli@orvosi.ca', 'Marc', 'Cerulli', 'Marc Cerulli Intake Services', '0', '40.00')
INSERT INTO #RawData ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnabled], [AccessFailedCount], [UserName], [FirstName], [LastName], [CompanyName], [IsTestRecord]) VALUES ('fe7a2cc3-a949-4c07-a318-6ad7a0729e26', 'lesliefarago+casecoordinator@gmail.com', '1', 'ABPXqT4slp0DkchSX5GBG6VATAZv87teK/Q4xcAAYUdJAZHf2UAixfSgRVp0y9DSJA==', '47bb3816-2eab-435b-8587-a0076a689161', '0', '0', '1', '0', 'lesliefarago+casecoordinator@gmail.com', 'Luke', 'Skywalker', 'Orvosi', '0')

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
	WHERE d.[Email] = s.[Email] AND d.[EmailConfirmed] = s.[EmailConfirmed] AND d.[PasswordHash] = s.[PasswordHash] AND d.[SecurityStamp] = s.[SecurityStamp] AND d.[PhoneNumberConfirmed] = s.[PhoneNumberConfirmed] AND d.[TwoFactorEnabled] = s.[TwoFactorEnabled] AND d.[LockoutEnabled] = s.[LockoutEnabled] AND d.[AccessFailedCount] = s.[AccessFailedCount] AND d.[UserName] = s.[UserName] AND d.[FirstName] = s.[FirstName] AND d.[LastName] = s.[LastName] AND d.[CompanyId] = s.[CompanyId] AND d.[IsTestRecord] = s.[IsTestRecord] AND d.[RoleLevelId] = s.[RoleLevelId] AND d.[CompanyName] = s.[CompanyName] AND d.[HourlyRate] = s.[HourlyRate] AND d.[Title] = s.[Title] AND d.[LockoutEndDateUtc] = s.[LockoutEndDateUtc]


	SELECT @CounterIdentical = COUNT(*) FROM @IdenticalRecordIDs


	--Update any rows that already exist
	UPDATE [dbo].[AspNetUsers] SET [Email] = s.[Email], [EmailConfirmed] = s.[EmailConfirmed], [PasswordHash] = s.[PasswordHash], [SecurityStamp] = s.[SecurityStamp], [PhoneNumberConfirmed] = s.[PhoneNumberConfirmed], [TwoFactorEnabled] = s.[TwoFactorEnabled], [LockoutEnabled] = s.[LockoutEnabled], [AccessFailedCount] = s.[AccessFailedCount], [UserName] = s.[UserName], [FirstName] = s.[FirstName], [LastName] = s.[LastName], [CompanyId] = s.[CompanyId], [IsTestRecord] = s.[IsTestRecord], [RoleLevelId] = s.[RoleLevelId], [CompanyName] = s.[CompanyName], [HourlyRate] = s.[HourlyRate], [Title] = s.[Title], [LockoutEndDateUtc] = s.[LockoutEndDateUtc]
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
	INSERT INTO [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnabled], [AccessFailedCount], [UserName], [FirstName], [LastName], [CompanyId], [IsTestRecord], [RoleLevelId], [CompanyName], [HourlyRate], [Title], [LockoutEndDateUtc]
	, ModifiedDate, ModifiedUser	)
	OUTPUT Inserted.[Id] INTO @IdenticalRecordIDs
	SELECT [Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnabled], [AccessFailedCount], [UserName], [FirstName], [LastName], [CompanyId], [IsTestRecord], [RoleLevelId], [CompanyName], [HourlyRate], [Title], [LockoutEndDateUtc]
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
INSERT INTO #RawData ([UserId], [RoleId]) VALUES ('0304e7d6-235e-4f3f-b3a9-cbaf575d1514', '7b930663-b091-44ca-924c-d8b11a1ee7ea')
INSERT INTO #RawData ([UserId], [RoleId]) VALUES ('0538a347-fddc-4f03-97b1-a1e816e888ed', '7fab67dd-286b-492f-865a-0cb0ce1261ce')
INSERT INTO #RawData ([UserId], [RoleId]) VALUES ('25f67e43-55b4-447e-913a-807b77536c8e', '8359141f-e423-4e48-8925-4624ba86245a')
INSERT INTO #RawData ([UserId], [RoleId]) VALUES ('2f133d64-507e-4ec1-b6e0-ad0a06de77bc', '9dd582a0-cf86-4fc0-8894-477266068c12')
INSERT INTO #RawData ([UserId], [RoleId]) VALUES ('3b61845f-1968-4795-ae00-f4e3340a7141', '9eab89c0-225c-4027-9f42-cc35e5656b14')
INSERT INTO #RawData ([UserId], [RoleId]) VALUES ('46b4f52b-f554-4467-a562-ccf687488675', '5e2cfba4-417a-4685-8e2b-970bd7061cd9')
INSERT INTO #RawData ([UserId], [RoleId]) VALUES ('4953f430-3dad-40e7-a372-d3effcffe3f8', '7b930663-b091-44ca-924c-d8b11a1ee7ea')
INSERT INTO #RawData ([UserId], [RoleId]) VALUES ('585e12fa-7536-48ad-b860-78b16d5fd647', '7b930663-b091-44ca-924c-d8b11a1ee7ea')
INSERT INTO #RawData ([UserId], [RoleId]) VALUES ('5b92cd8b-a500-428f-88a2-ad14e0a23624', '7b930663-b091-44ca-924c-d8b11a1ee7ea')
INSERT INTO #RawData ([UserId], [RoleId]) VALUES ('6a254b1a-6801-4224-9979-0d0ef677e983', '7b930663-b091-44ca-924c-d8b11a1ee7ea')
INSERT INTO #RawData ([UserId], [RoleId]) VALUES ('6a7172db-343b-4926-b156-63b7a59d30ba', '7b930663-b091-44ca-924c-d8b11a1ee7ea')
INSERT INTO #RawData ([UserId], [RoleId]) VALUES ('7c8f47bd-fcb1-443e-a703-cdeefb3b69bb', '9dd582a0-cf86-4fc0-8894-477266068c12')
INSERT INTO #RawData ([UserId], [RoleId]) VALUES ('8ad861fa-5cc2-4342-89da-b3709b1e59b5', '9dd582a0-cf86-4fc0-8894-477266068c12')
INSERT INTO #RawData ([UserId], [RoleId]) VALUES ('8dd4e180-6e3a-4968-a00d-eeb6d2cc7f0c', '8359141f-e423-4e48-8925-4624ba86245a')
INSERT INTO #RawData ([UserId], [RoleId]) VALUES ('8e9885d8-a0f7-49f6-9a3e-ff1b4d52f6a9', '8359141f-e423-4e48-8925-4624ba86245a')
INSERT INTO #RawData ([UserId], [RoleId]) VALUES ('a34268d3-3b5e-4632-a31b-0eb8c8f73a50', '8359141f-e423-4e48-8925-4624ba86245a')
INSERT INTO #RawData ([UserId], [RoleId]) VALUES ('a35b2f60-08cf-484a-925a-21e89d0fb0dd', '7b930663-b091-44ca-924c-d8b11a1ee7ea')
INSERT INTO #RawData ([UserId], [RoleId]) VALUES ('affb6a2a-175d-4870-b8ff-c844380e2ecf', '7b930663-b091-44ca-924c-d8b11a1ee7ea')
INSERT INTO #RawData ([UserId], [RoleId]) VALUES ('d07c8664-00b5-4852-9537-a3a265c28efb', '7b930663-b091-44ca-924c-d8b11a1ee7ea')
INSERT INTO #RawData ([UserId], [RoleId]) VALUES ('d579d0a4-11ce-46f2-97ec-4c2bfc4dc704', '9eab89c0-225c-4027-9f42-cc35e5656b14')
INSERT INTO #RawData ([UserId], [RoleId]) VALUES ('e357dd36-8f80-4883-b509-22d04e7f9e1e', '9dd582a0-cf86-4fc0-8894-477266068c12')
INSERT INTO #RawData ([UserId], [RoleId]) VALUES ('ea9fb6ca-ffb7-4348-a332-bd2ee2f2c132', '7fab67dd-286b-492f-865a-0cb0ce1261ce')
INSERT INTO #RawData ([UserId], [RoleId]) VALUES ('f61e8eee-cc97-4fe1-9140-0fc219b05f65', '9dd582a0-cf86-4fc0-8894-477266068c12')
INSERT INTO #RawData ([UserId], [RoleId]) VALUES ('fe7a2cc3-a949-4c07-a318-6ad7a0729e26', '9eab89c0-225c-4027-9f42-cc35e5656b14')

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
		[ObjectGuid],
		[Name],
		[IsParent],
		[Code],
		[ParentId],
		[LogoCssClass],
		[MasterBookingPageByPhysician],
		[MasterBookingPageByTime],
		[MasterBookingPageTeleconference]
	INTO #RawData
	FROM (
		SELECT 
			[Id],
			[ObjectGuid],
			[Name],
			[IsParent],
			[Code],
			[ParentId],
			[LogoCssClass],
			[MasterBookingPageByPhysician],
			[MasterBookingPageByTime],
			[MasterBookingPageTeleconference]
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
INSERT INTO #RawData ([Id], [ObjectGuid], [Name], [IsParent]) VALUES ('1', 'BB29F8C6-A012-42B9-917B-71750B0C18B9', 'Examworks', '1')
INSERT INTO #RawData ([Id], [ObjectGuid], [Name], [Code], [IsParent], [ParentId], [LogoCssClass], [MasterBookingPageByPhysician], [MasterBookingPageByTime], [MasterBookingPageTeleconference]) VALUES ('2', '02DB607A-4601-42A5-8852-37765BC369D7', 'Direct IME', 'DIME', '0', '1', 'image-logo-dime-md', 'DIMEDoc', 'DIME', 'TELECONFERENCES')
INSERT INTO #RawData ([Id], [ObjectGuid], [Name], [Code], [IsParent], [ParentId]) VALUES ('3', '06101C0E-9B40-4B15-BE8B-AF3F160B384D', 'MAKOS', 'MAKOS', '0', '1')
INSERT INTO #RawData ([Id], [ObjectGuid], [Name], [Code], [IsParent], [ParentId], [LogoCssClass], [MasterBookingPageByPhysician], [MasterBookingPageByTime], [MasterBookingPageTeleconference]) VALUES ('4', 'CDFFD41F-0F1B-4AAA-AB25-09A11F9E2C4D', 'SOMA', 'SOMA', '0', '1', 'image-logo-soma-md', 'SOMA-byPhysician', 'SOMA-byTime', 'TELECONFERENCES')
INSERT INTO #RawData ([Id], [ObjectGuid], [Name], [Code], [IsParent], [ParentId]) VALUES ('5', '95392921-094C-49C9-A20F-4936D2746F38', 'NYRC-ONT', 'NYRC-ONT', '0', '1')
INSERT INTO #RawData ([Id], [ObjectGuid], [Name], [Code], [IsParent], [ParentId], [LogoCssClass], [MasterBookingPageByPhysician], [MasterBookingPageByTime], [MasterBookingPageTeleconference]) VALUES ('6', 'E123586C-1CC6-44D7-9796-026D889995CD', 'IMA', 'IMA', '0', '1', 'image-logo-ima-md', 'IMA-byPhysician', 'IMA-byTime', 'TELECONFERENCES')
INSERT INTO #RawData ([Id], [ObjectGuid], [Name], [Code], [IsParent], [ParentId]) VALUES ('7', 'A5950FC5-878E-4618-84D5-367A6DADC9D9', 'CVS', 'CVS', '0', '1')
INSERT INTO #RawData ([Id], [ObjectGuid], [Name], [Code], [IsParent], [ParentId]) VALUES ('8', '23139222-A144-405A-B81B-22F2D9210B6E', 'NYRC-BC', 'NYRC', '0', '1')
INSERT INTO #RawData ([Id], [ObjectGuid], [Name], [IsParent], [ParentId]) VALUES ('9', '4A0BD5CF-7596-4526-8D98-4944E8C23111', 'National IME', '0', '1')
INSERT INTO #RawData ([Id], [ObjectGuid], [Name], [IsParent], [LogoCssClass], [MasterBookingPageByPhysician], [MasterBookingPageByTime], [MasterBookingPageTeleconference]) VALUES ('10', 'F8C945D1-8641-4087-9DEA-235A02A080B4', 'SCM', '1', 'image-logo-scm-md', 'SCM-byPhysician', 'SCM-byTime', 'TELECONFERENCES')
INSERT INTO #RawData ([Id], [ObjectGuid], [Name], [IsParent], [ParentId]) VALUES ('11', '95D3166F-17FC-46A9-8803-AD9F37C6E439', 'CIRA', '0', '10')
INSERT INTO #RawData ([Id], [ObjectGuid], [Name], [IsParent], [ParentId]) VALUES ('12', '712B5DB2-6A46-43F1-870D-D9453CC69219', 'Sibley', '0', '10')
INSERT INTO #RawData ([Id], [ObjectGuid], [Name], [IsParent], [ParentId]) VALUES ('13', '91EF86B2-8764-4B59-8BCF-2B4D4E816604', 'MDAC', '0', '10')
INSERT INTO #RawData ([Id], [ObjectGuid], [Name], [IsParent], [ParentId]) VALUES ('14', 'BE922264-3E58-4A05-A95F-D725C06976B0', 'TRM', '0', '10')
INSERT INTO #RawData ([Id], [ObjectGuid], [Name], [IsParent]) VALUES ('15', '2D72B086-D104-4A35-94F0-DB4EF597172F', 'AssessMed', '0')
INSERT INTO #RawData ([Id], [ObjectGuid], [Name], [IsParent]) VALUES ('16', '222BE999-C6E7-4272-9A99-82D6B16BE409', 'Benchmark', '0')
INSERT INTO #RawData ([Id], [ObjectGuid], [Name], [IsParent]) VALUES ('17', '689D789E-86D3-4034-B0C9-D03AC03A705A', 'CWC', '0')
INSERT INTO #RawData ([Id], [ObjectGuid], [Name], [Code], [IsParent]) VALUES ('18', 'E755B927-C33C-4E7D-AFF4-EDDB52D1B61F', 'ARSI', 'ARS', '0')
INSERT INTO #RawData ([Id], [ObjectGuid], [Name], [Code], [IsParent], [LogoCssClass], [MasterBookingPageByPhysician], [MasterBookingPageByTime], [MasterBookingPageTeleconference]) VALUES ('19', 'A9CAA53D-6524-4D52-A427-2B6370BE8E98', 'Seiden', 'SEIDEN', '0', 'image-logo-seiden-md', 'SEIDEN-byPhysician', 'SEIDEN-byTime', 'TELECONFERENCES')
INSERT INTO #RawData ([Id], [ObjectGuid], [Name], [IsParent]) VALUES ('20', '55CD0BC5-1307-460E-88FB-9B556A428DF8', 'SIMAC', '0')
INSERT INTO #RawData ([Id], [ObjectGuid], [Name], [Code], [IsParent]) VALUES ('21', '6C7C4B8B-1570-46B0-B857-03707D87093E', 'HVE', 'HVE', '0')
INSERT INTO #RawData ([Id], [ObjectGuid], [Name], [IsParent], [MasterBookingPageByPhysician], [MasterBookingPageByTime], [MasterBookingPageTeleconference]) VALUES ('22', '63714DA5-ADD5-4E0D-84DD-46F71D5486E0', 'Test Company', '0', 'test', 'test', 'test')

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
	WHERE d.[ObjectGuid] = s.[ObjectGuid] AND d.[Name] = s.[Name] AND d.[IsParent] = s.[IsParent] AND d.[Code] = s.[Code] AND d.[ParentId] = s.[ParentId] AND d.[LogoCssClass] = s.[LogoCssClass] AND d.[MasterBookingPageByPhysician] = s.[MasterBookingPageByPhysician] AND d.[MasterBookingPageByTime] = s.[MasterBookingPageByTime] AND d.[MasterBookingPageTeleconference] = s.[MasterBookingPageTeleconference]


	SELECT @CounterIdentical = COUNT(*) FROM @IdenticalRecordIDs


	--Update any rows that already exist
	UPDATE [dbo].[Company] SET [ObjectGuid] = s.[ObjectGuid], [Name] = s.[Name], [IsParent] = s.[IsParent], [Code] = s.[Code], [ParentId] = s.[ParentId], [LogoCssClass] = s.[LogoCssClass], [MasterBookingPageByPhysician] = s.[MasterBookingPageByPhysician], [MasterBookingPageByTime] = s.[MasterBookingPageByTime], [MasterBookingPageTeleconference] = s.[MasterBookingPageTeleconference]
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
	INSERT INTO [dbo].[Company] ([Id], [ObjectGuid], [Name], [IsParent], [Code], [ParentId], [LogoCssClass], [MasterBookingPageByPhysician], [MasterBookingPageByTime], [MasterBookingPageTeleconference]
	, ModifiedDate, ModifiedUser	)
	OUTPUT Inserted.[Id] INTO @IdenticalRecordIDs
	SELECT [Id], [ObjectGuid], [Name], [IsParent], [Code], [ParentId], [LogoCssClass], [MasterBookingPageByPhysician], [MasterBookingPageByTime], [MasterBookingPageTeleconference]
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
		[PhysicianId],
		[CompanyId]
	INTO #RawData
	FROM (
		SELECT 
			[Id],
			[PhysicianId],
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
INSERT INTO #RawData ([Id], [PhysicianId], [CompanyId]) VALUES ('1', 'E62EBEAD-C270-4711-81CB-2CBD6B8031AD', '2')

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
	WHERE d.[PhysicianId] = s.[PhysicianId] AND d.[CompanyId] = s.[CompanyId]


	SELECT @CounterIdentical = COUNT(*) FROM @IdenticalRecordIDs


	--Update any rows that already exist
	UPDATE [dbo].[File] SET [PhysicianId] = s.[PhysicianId], [CompanyId] = s.[CompanyId]
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
	INSERT INTO [dbo].[File] ([Id], [PhysicianId], [CompanyId]
	, ModifiedDate, ModifiedUser	)
	OUTPUT Inserted.[Id] INTO @IdenticalRecordIDs
	SELECT [Id], [PhysicianId], [CompanyId]
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
		[ServiceId],
		[DueDate],
		[StartTime],
		[EndTime],
		[Price],
		[FileId]
	INTO #RawData
	FROM (
		SELECT 
			[Id],
			[ServiceId],
			[DueDate],
			[StartTime],
			[EndTime],
			[Price],
			[FileId]
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
INSERT INTO #RawData ([Id], [ServiceId], [DueDate], [StartTime], [EndTime], [Price], [FileId]) VALUES ('1', '7', '2015-04-29', '11:00:00', '11:30:00', '1000.00', '1')
INSERT INTO #RawData ([Id], [ServiceId], [DueDate], [Price], [FileId]) VALUES ('2', '17', '2015-05-29', '400.00', '1')

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
	WHERE d.[ServiceId] = s.[ServiceId] AND d.[DueDate] = s.[DueDate] AND d.[StartTime] = s.[StartTime] AND d.[EndTime] = s.[EndTime] AND d.[Price] = s.[Price] AND d.[FileId] = s.[FileId]


	SELECT @CounterIdentical = COUNT(*) FROM @IdenticalRecordIDs


	--Update any rows that already exist
	UPDATE [dbo].[Job] SET [ServiceId] = s.[ServiceId], [DueDate] = s.[DueDate], [StartTime] = s.[StartTime], [EndTime] = s.[EndTime], [Price] = s.[Price], [FileId] = s.[FileId]
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
	INSERT INTO [dbo].[Job] ([Id], [ServiceId], [DueDate], [StartTime], [EndTime], [Price], [FileId]
	, ModifiedDate, ModifiedUser	)
	OUTPUT Inserted.[Id] INTO @IdenticalRecordIDs
	SELECT [Id], [ServiceId], [DueDate], [StartTime], [EndTime], [Price], [FileId]
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
		[PhysicianId],
		[ServiceId],
		[CompanyId],
		[LocationId],
		[Price]
	INTO #RawData
	FROM (
		SELECT 
			[Id],
			[PhysicianId],
			[ServiceId],
			[CompanyId],
			[LocationId],
			[Price]
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
INSERT INTO #RawData ([Id], [PhysicianId], [ServiceId], [CompanyId], [LocationId], [Price]) VALUES ('1', '8dd4e180-6e3a-4968-a00d-eeb6d2cc7f0c', '7', '10', '19', '950.00')
INSERT INTO #RawData ([Id], [PhysicianId], [ServiceId], [CompanyId], [LocationId], [Price]) VALUES ('5', '8dd4e180-6e3a-4968-a00d-eeb6d2cc7f0c', '7', '19', '19', '1000.00')
INSERT INTO #RawData ([Id], [PhysicianId], [ServiceId], [CompanyId], [LocationId], [Price]) VALUES ('9', '8dd4e180-6e3a-4968-a00d-eeb6d2cc7f0c', '7', '10', '20', '1400.00')
INSERT INTO #RawData ([Id], [PhysicianId], [ServiceId], [LocationId], [Price]) VALUES ('10', '8dd4e180-6e3a-4968-a00d-eeb6d2cc7f0c', '7', '19', '1200.00')
INSERT INTO #RawData ([Id], [PhysicianId], [ServiceId], [CompanyId], [LocationId], [Price]) VALUES ('11', '8dd4e180-6e3a-4968-a00d-eeb6d2cc7f0c', '7', '10', '22', '950.00')
INSERT INTO #RawData ([Id], [PhysicianId], [ServiceId], [CompanyId], [LocationId], [Price]) VALUES ('12', '8dd4e180-6e3a-4968-a00d-eeb6d2cc7f0c', '20', '10', '19', '1200.00')
INSERT INTO #RawData ([Id], [PhysicianId], [ServiceId], [CompanyId], [LocationId], [Price]) VALUES ('13', '8dd4e180-6e3a-4968-a00d-eeb6d2cc7f0c', '20', '10', '22', '1200.00')
INSERT INTO #RawData ([Id], [PhysicianId], [ServiceId], [CompanyId], [LocationId], [Price]) VALUES ('14', '8dd4e180-6e3a-4968-a00d-eeb6d2cc7f0c', '7', '18', '19', '1200.00')
INSERT INTO #RawData ([Id], [PhysicianId], [ServiceId], [CompanyId], [LocationId], [Price]) VALUES ('15', '8dd4e180-6e3a-4968-a00d-eeb6d2cc7f0c', '7', '18', '22', '1200.00')
INSERT INTO #RawData ([Id], [PhysicianId], [ServiceId], [CompanyId], [LocationId], [Price]) VALUES ('16', '8dd4e180-6e3a-4968-a00d-eeb6d2cc7f0c', '20', '18', '19', '1200.00')
INSERT INTO #RawData ([Id], [PhysicianId], [ServiceId], [CompanyId], [LocationId], [Price]) VALUES ('17', '8dd4e180-6e3a-4968-a00d-eeb6d2cc7f0c', '20', '18', '22', '1200.00')
INSERT INTO #RawData ([Id], [PhysicianId], [ServiceId], [CompanyId], [LocationId], [Price]) VALUES ('18', '8e9885d8-a0f7-49f6-9a3e-ff1b4d52f6a9', '7', '10', '21', '1200.00')
INSERT INTO #RawData ([Id], [PhysicianId], [ServiceId], [CompanyId], [LocationId], [Price]) VALUES ('19', '8e9885d8-a0f7-49f6-9a3e-ff1b4d52f6a9', '8', '10', '21', '1200.00')
INSERT INTO #RawData ([Id], [PhysicianId], [ServiceId], [CompanyId], [LocationId], [Price]) VALUES ('20', '8e9885d8-a0f7-49f6-9a3e-ff1b4d52f6a9', '9', '10', '21', '1200.00')
INSERT INTO #RawData ([Id], [PhysicianId], [ServiceId], [CompanyId], [LocationId], [Price]) VALUES ('21', '8e9885d8-a0f7-49f6-9a3e-ff1b4d52f6a9', '11', '10', '21', '1200.00')
INSERT INTO #RawData ([Id], [PhysicianId], [ServiceId], [CompanyId], [LocationId], [Price]) VALUES ('22', '8e9885d8-a0f7-49f6-9a3e-ff1b4d52f6a9', '12', '10', '21', '1200.00')
INSERT INTO #RawData ([Id], [PhysicianId], [ServiceId], [CompanyId], [LocationId], [Price]) VALUES ('23', '8e9885d8-a0f7-49f6-9a3e-ff1b4d52f6a9', '20', '10', '21', '1200.00')
INSERT INTO #RawData ([Id], [PhysicianId], [ServiceId], [CompanyId], [LocationId], [Price]) VALUES ('24', '25f67e43-55b4-447e-913a-807b77536c8e', '7', '10', '19', '1000.00')

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
	WHERE d.[PhysicianId] = s.[PhysicianId] AND d.[ServiceId] = s.[ServiceId] AND d.[CompanyId] = s.[CompanyId] AND d.[LocationId] = s.[LocationId] AND d.[Price] = s.[Price]


	SELECT @CounterIdentical = COUNT(*) FROM @IdenticalRecordIDs


	--Update any rows that already exist
	UPDATE [dbo].[ServiceCatalogue] SET [PhysicianId] = s.[PhysicianId], [ServiceId] = s.[ServiceId], [CompanyId] = s.[CompanyId], [LocationId] = s.[LocationId], [Price] = s.[Price]
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
	INSERT INTO [dbo].[ServiceCatalogue] ([Id], [PhysicianId], [ServiceId], [CompanyId], [LocationId], [Price]
	, ModifiedDate, ModifiedUser	)
	OUTPUT Inserted.[Id] INTO @IdenticalRecordIDs
	SELECT [Id], [PhysicianId], [ServiceId], [CompanyId], [LocationId], [Price]
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

