﻿CREATE TABLE [dbo].[PhysicianInsurance] (
    [Id]           SMALLINT       IDENTITY (1, 1) NOT NULL,
    [PhysicianId]  NVARCHAR (128) NOT NULL,
    [Insurer]      NVARCHAR (256) NULL,
    [PolicyNumber] NVARCHAR (128) NULL,
    [ExpiryDate]   DATE           NULL,
    [DocumentId]   SMALLINT       NULL,
    [ModifiedDate] DATETIME       CONSTRAINT [DF_PhysicianInsurance_ModifiedDate] DEFAULT (getdate()) NOT NULL,
    [ModifiedUser] NVARCHAR (100) CONSTRAINT [DF_PhysicianInsurance_ModifiedUser] DEFAULT (suser_name()) NOT NULL,
    CONSTRAINT [PK_PhysicianInsurance] PRIMARY KEY CLUSTERED ([Id] ASC)
);
