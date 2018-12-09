﻿CREATE TABLE [dbo].[UserHistory] (
    [Id]                      UNIQUEIDENTIFIER NOT NULL,
    [SysStartTime]            DATETIME2 (0)    NOT NULL,
    [SysEndTime]              DATETIME2 (0)    NOT NULL,
    [Email]                   NVARCHAR (256)   NULL,
    [EmailConfirmed]          BIT              NOT NULL,
    [PasswordHash]            NVARCHAR (MAX)   NULL,
    [SecurityStamp]           NVARCHAR (MAX)   NULL,
    [PhoneNumber]             NVARCHAR (128)   NULL,
    [PhoneNumberConfirmed]    BIT              NOT NULL,
    [TwoFactorEnabled]        BIT              NOT NULL,
    [LockoutEndDateUtc]       DATETIME         NULL,
    [LockoutEnabled]          BIT              NOT NULL,
    [AccessFailedCount]       INT              NOT NULL,
    [UserName]                NVARCHAR (256)   NOT NULL,
    [Title]                   NVARCHAR (50)    NULL,
    [FirstName]               NVARCHAR (128)   NULL,
    [LastName]                NVARCHAR (128)   NULL,
    [LastActivationDate]      DATETIME         NULL,
    [LogoCssClass]            NVARCHAR (50)    NULL,
    [ColorCode]               NVARCHAR (50)    NULL,
    [Notes]                   NVARCHAR (MAX)   NULL,
    [IsTestRecord]            BIT              NOT NULL,
    [IsAppTester]             BIT              NOT NULL,
    [PhysicianId]             UNIQUEIDENTIFIER NULL,
    [RoleId]                  UNIQUEIDENTIFIER NOT NULL,
    [EmailProviderCredential] NVARCHAR (MAX)   NULL,
    [EmailProvider]           NVARCHAR (128)   NULL
);



