CREATE TABLE [dbo].[Job] (
    [Id]                INT             IDENTITY (1, 1) NOT NULL,
    [ExternalProjectId] NVARCHAR (128)  NULL,
    [Name]              NVARCHAR (256)  NULL,
    [Code]              NVARCHAR (256)  NULL,
    [PhysicianId]       INT             NULL,
    [CompanyId]         SMALLINT        NULL,
    [ServiceId]         INT             NULL,
    [DueDate]           DATE            NULL,
    [StartTime]         TIME (7)        NULL,
    [EndTime]           TIME (7)        NULL,
    [Price]             DECIMAL (18, 2) NULL,
    [FileId]            SMALLINT        NULL,
    [ModifiedDate]      DATETIME        CONSTRAINT [DF_Job_ModifiedDate] DEFAULT (getdate()) NOT NULL,
    [ModifiedUser]      NVARCHAR (100)  CONSTRAINT [DF_Job_ModifiedUser] DEFAULT (suser_name()) NOT NULL,
    CONSTRAINT [PK_Job] PRIMARY KEY CLUSTERED ([Id] ASC)
);



