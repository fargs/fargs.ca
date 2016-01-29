﻿CREATE TABLE [dbo].[ServiceRequestTemplateTask] (
    [Id]                       SMALLINT        IDENTITY (1, 1) NOT NULL,
    [ServiceRequestTemplateId] SMALLINT        NOT NULL,
    [TaskPhaseId]              TINYINT         NULL,
    [TaskName]                 NVARCHAR (128)  NULL,
    [Guidance]                 NVARCHAR (1000) NULL,
    [Sequence]                 SMALLINT        NULL,
    [IsBillable]               BIT             CONSTRAINT [DF_ServiceRequestTemplateTask_IsBillable] DEFAULT ((0)) NOT NULL,
    [EstimatedHours]           DECIMAL (18, 2) NULL,
    [HourlyRate]               DECIMAL (18, 2) NULL,
    [ResponsibleRoleId]        NVARCHAR (128)  NULL,
    [ModifiedDate]             DATETIME        CONSTRAINT [DF_ServiceRequestTemplateTask_ModifiedDate] DEFAULT (getdate()) NOT NULL,
    [ModifiedUser]             NVARCHAR (100)  CONSTRAINT [DF_ServiceRequestTemplateTask_ModifiedUser] DEFAULT (suser_name()) NOT NULL,
    CONSTRAINT [PK_ServiceRequestTemplateTask] PRIMARY KEY CLUSTERED ([Id] ASC)
);


