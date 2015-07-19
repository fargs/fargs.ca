﻿CREATE TABLE [dbo].[AspNetUsers] (
    [Id]                   NVARCHAR (128) NOT NULL,
    [Email]                NVARCHAR (256) NULL,
    [EmailConfirmed]       BIT            NOT NULL,
    [PasswordHash]         NVARCHAR (MAX) NULL,
    [SecurityStamp]        NVARCHAR (MAX) NULL,
    [PhoneNumber]          NVARCHAR (MAX) NULL,
    [PhoneNumberConfirmed] BIT            NOT NULL,
    [TwoFactorEnabled]     BIT            NOT NULL,
    [LockoutEndDateUtc]    DATETIME       NULL,
    [LockoutEnabled]       BIT            NOT NULL,
    [AccessFailedCount]    INT            NOT NULL,
    [UserName]             NVARCHAR (256) NOT NULL,
    [Title]                NVARCHAR (50)  NULL,
    [FirstName]            NVARCHAR (128) NULL,
    [LastName]             NVARCHAR (128) NULL,
    [RoleLevelId]          SMALLINT       NULL,
    [EmployeeId]           NVARCHAR (50)  NULL,
    [ModifiedDate]         DATETIME       CONSTRAINT [DF_AspNetUsers_ModifiedDate] DEFAULT (getdate()) NULL,
    [ModifiedUser]         NVARCHAR (256) CONSTRAINT [DF_AspNetUsers_ModifiedUser] DEFAULT (suser_name()) NULL,
    CONSTRAINT [PK_dbo.AspNetUsers] PRIMARY KEY CLUSTERED ([Id] ASC)
);










GO
CREATE UNIQUE NONCLUSTERED INDEX [UserNameIndex]
    ON [dbo].[AspNetUsers]([UserName] ASC);

