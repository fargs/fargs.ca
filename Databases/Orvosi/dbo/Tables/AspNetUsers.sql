CREATE TABLE [dbo].[AspNetUsers] (
    [Id]                   UNIQUEIDENTIFIER NOT NULL,
    [Email]                NVARCHAR (256)   NULL,
    [EmailConfirmed]       BIT              NOT NULL,
    [PasswordHash]         NVARCHAR (MAX)   NULL,
    [SecurityStamp]        NVARCHAR (MAX)   NULL,
    [PhoneNumber]          NVARCHAR (MAX)   NULL,
    [PhoneNumberConfirmed] BIT              NOT NULL,
    [TwoFactorEnabled]     BIT              NOT NULL,
    [LockoutEndDateUtc]    DATETIME         NULL,
    [LockoutEnabled]       BIT              NOT NULL,
    [AccessFailedCount]    INT              NOT NULL,
    [UserName]             NVARCHAR (256)   NOT NULL,
    [Title]                NVARCHAR (50)    NULL,
    [FirstName]            NVARCHAR (128)   NULL,
    [LastName]             NVARCHAR (128)   NULL,
    [EmployeeId]           NVARCHAR (50)    NULL,
    [CompanyId]            SMALLINT         NULL,
    [CompanyName]          NVARCHAR (200)   NULL,
    [ModifiedDate]         DATETIME         CONSTRAINT [DF_AspNetUsers_ModifiedDate] DEFAULT (getdate()) NOT NULL,
    [ModifiedUser]         NVARCHAR (256)   CONSTRAINT [DF_AspNetUsers_ModifiedUser] DEFAULT (suser_name()) NOT NULL,
    [LastActivationDate]   DATETIME         NULL,
    [IsTestRecord]         BIT              CONSTRAINT [DF_AspNetUsers_IsTestRecord] DEFAULT ((0)) NOT NULL,
    [RoleLevelId]          TINYINT          NULL,
    [HourlyRate]           DECIMAL (18, 2)  NULL,
    [LogoCssClass]         NVARCHAR (50)    NULL,
    [ColorCode]            NVARCHAR (50)    NULL,
    [BoxFolderId]          NVARCHAR (128)   NULL,
    [BoxUserId]            NVARCHAR (50)    NULL,
    [BoxAccessToken]       NVARCHAR (128)   NULL,
    [BoxRefreshToken]      NVARCHAR (128)   NULL,
    [HstNumber]            NVARCHAR (50)    NULL,
    [Notes]                NVARCHAR (MAX)   NULL,
    [IsAppTester]          BIT              CONSTRAINT [DF_AspNetUsers_IsAppTester] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_dbo.AspNetUsers] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_AspNetUsers_Company] FOREIGN KEY ([CompanyId]) REFERENCES [dbo].[Company] ([Id])
);






























GO
CREATE UNIQUE NONCLUSTERED INDEX [UserNameIndex]
    ON [dbo].[AspNetUsers]([UserName] ASC);

