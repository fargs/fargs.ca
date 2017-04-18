CREATE TABLE [dbo].[ServiceRequestTask] (
    [Id]                           INT              IDENTITY (1, 1) NOT NULL,
    [ObjectGuid]                   UNIQUEIDENTIFIER CONSTRAINT [DF_ServiceRequestTask_ObjectGuid] DEFAULT (newid()) NOT NULL,
    [ServiceRequestId]             INT              NOT NULL,
    [TaskId]                       SMALLINT         NULL,
    [TaskName]                     NVARCHAR (128)   NOT NULL,
    [ResponsibleRoleId]            UNIQUEIDENTIFIER NULL,
    [ResponsibleRoleName]          NVARCHAR (128)   NULL,
    [Sequence]                     SMALLINT         NULL,
    [AssignedTo]                   UNIQUEIDENTIFIER NULL,
    [IsBillable]                   BIT              CONSTRAINT [DF_ServiceRequestTask_IsBillable] DEFAULT ((0)) NOT NULL,
    [HourlyRate]                   DECIMAL (18, 2)  NULL,
    [EstimatedHours]               DECIMAL (18, 2)  NULL,
    [ActualHours]                  DECIMAL (18, 2)  NULL,
    [CompletedDate]                DATETIME         NULL,
    [Notes]                        NVARCHAR (2000)  NULL,
    [InvoiceItemId]                SMALLINT         NULL,
    [ModifiedDate]                 DATETIME         CONSTRAINT [DF_ServiceRequestTask_ModifiedDate] DEFAULT (getdate()) NOT NULL,
    [ModifiedUser]                 NVARCHAR (100)   CONSTRAINT [DF_ServiceRequestTask_ModifiedUser] DEFAULT (suser_name()) NOT NULL,
    [TaskPhaseId]                  TINYINT          NULL,
    [TaskPhaseName]                NVARCHAR (128)   NULL,
    [Guidance]                     NVARCHAR (1000)  NULL,
    [IsObsolete]                   BIT              CONSTRAINT [DF_ServiceRequestTask_IsObsolete] DEFAULT ((0)) NOT NULL,
    [DependsOn]                    NVARCHAR (50)    NULL,
    [DueDateBase]                  TINYINT          NULL,
    [DueDateDiff]                  SMALLINT         NULL,
    [ShortName]                    NVARCHAR (50)    NULL,
    [IsCriticalPath]               BIT              CONSTRAINT [DF_ServiceRequestTask_IsCriticalPath] DEFAULT ((0)) NOT NULL,
    [IsDependentOnExamDate]        AS               (CONVERT([bit],case when [DependsOn]='133' then (1) else (0) end)),
    [Workload]                     TINYINT          NULL,
    [ServiceRequestTemplateTaskId] UNIQUEIDENTIFIER NULL,
    [TaskType]                     VARCHAR (20)     NULL,
    [CompletedBy]                  UNIQUEIDENTIFIER NULL,
    [DueDate]                      DATETIME         NULL,
    [TaskStatusId]                 SMALLINT         CONSTRAINT [DF_ServiceRequestTask_TaskStatusId] DEFAULT ((2)) NOT NULL,
    [TaskStatusChangedBy]          UNIQUEIDENTIFIER NULL,
    [TaskStatusChangedDate]        DATETIME         NULL,
    [CreatedDate]                  DATETIME         NULL,
    [CreatedUser]                  NVARCHAR (100)   NULL,
    [EffectiveDate]                DATETIME         NULL,
    [EffectiveDateBase]            TINYINT          NULL,
    [EffectiveDateDiff]            SMALLINT         NULL,
    CONSTRAINT [PK_ServiceRequestTask] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ServiceRequestTask_AspNetUsers] FOREIGN KEY ([AssignedTo]) REFERENCES [dbo].[AspNetUsers] ([Id]),
    CONSTRAINT [FK_ServiceRequestTask_AspNetUsers_TaskStatusChangedBy] FOREIGN KEY ([TaskStatusChangedBy]) REFERENCES [dbo].[AspNetUsers] ([Id]),
    CONSTRAINT [FK_ServiceRequestTask_AspNetUsers1] FOREIGN KEY ([CompletedBy]) REFERENCES [dbo].[AspNetUsers] ([Id]),
    CONSTRAINT [FK_ServiceRequestTask_ServiceRequest] FOREIGN KEY ([ServiceRequestId]) REFERENCES [dbo].[ServiceRequest] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_ServiceRequestTask_ServiceRequestTemplateTask] FOREIGN KEY ([ServiceRequestTemplateTaskId]) REFERENCES [dbo].[ServiceRequestTemplateTask] ([Id]),
    CONSTRAINT [FK_ServiceRequestTask_Task] FOREIGN KEY ([TaskId]) REFERENCES [dbo].[Task] ([Id]),
    CONSTRAINT [FK_ServiceRequestTask_TaskStatus] FOREIGN KEY ([TaskStatusId]) REFERENCES [dbo].[TaskStatus] ([Id])
);


GO
ALTER TABLE [dbo].[ServiceRequestTask] NOCHECK CONSTRAINT [FK_ServiceRequestTask_AspNetUsers_TaskStatusChangedBy];


GO
ALTER TABLE [dbo].[ServiceRequestTask] NOCHECK CONSTRAINT [FK_ServiceRequestTask_AspNetUsers1];


GO
ALTER TABLE [dbo].[ServiceRequestTask] NOCHECK CONSTRAINT [FK_ServiceRequestTask_Task];




GO
ALTER TABLE [dbo].[ServiceRequestTask] NOCHECK CONSTRAINT [FK_ServiceRequestTask_AspNetUsers_TaskStatusChangedBy];


GO
ALTER TABLE [dbo].[ServiceRequestTask] NOCHECK CONSTRAINT [FK_ServiceRequestTask_AspNetUsers1];


GO
ALTER TABLE [dbo].[ServiceRequestTask] NOCHECK CONSTRAINT [FK_ServiceRequestTask_Task];




GO
ALTER TABLE [dbo].[ServiceRequestTask] NOCHECK CONSTRAINT [FK_ServiceRequestTask_AspNetUsers_TaskStatusChangedBy];


GO
ALTER TABLE [dbo].[ServiceRequestTask] NOCHECK CONSTRAINT [FK_ServiceRequestTask_AspNetUsers1];


GO
ALTER TABLE [dbo].[ServiceRequestTask] NOCHECK CONSTRAINT [FK_ServiceRequestTask_Task];




GO
ALTER TABLE [dbo].[ServiceRequestTask] NOCHECK CONSTRAINT [FK_ServiceRequestTask_AspNetUsers_TaskStatusChangedBy];


GO
ALTER TABLE [dbo].[ServiceRequestTask] NOCHECK CONSTRAINT [FK_ServiceRequestTask_AspNetUsers1];


GO
ALTER TABLE [dbo].[ServiceRequestTask] NOCHECK CONSTRAINT [FK_ServiceRequestTask_Task];




GO
ALTER TABLE [dbo].[ServiceRequestTask] NOCHECK CONSTRAINT [FK_ServiceRequestTask_AspNetUsers_TaskStatusChangedBy];


GO
ALTER TABLE [dbo].[ServiceRequestTask] NOCHECK CONSTRAINT [FK_ServiceRequestTask_AspNetUsers1];


GO
ALTER TABLE [dbo].[ServiceRequestTask] NOCHECK CONSTRAINT [FK_ServiceRequestTask_Task];




GO
ALTER TABLE [dbo].[ServiceRequestTask] NOCHECK CONSTRAINT [FK_ServiceRequestTask_AspNetUsers_TaskStatusChangedBy];


GO
ALTER TABLE [dbo].[ServiceRequestTask] NOCHECK CONSTRAINT [FK_ServiceRequestTask_Task];




GO
ALTER TABLE [dbo].[ServiceRequestTask] NOCHECK CONSTRAINT [FK_ServiceRequestTask_AspNetUsers_TaskStatusChangedBy];


GO
ALTER TABLE [dbo].[ServiceRequestTask] NOCHECK CONSTRAINT [FK_ServiceRequestTask_Task];




GO
ALTER TABLE [dbo].[ServiceRequestTask] NOCHECK CONSTRAINT [FK_ServiceRequestTask_Task];




GO
ALTER TABLE [dbo].[ServiceRequestTask] NOCHECK CONSTRAINT [FK_ServiceRequestTask_Task];




GO
ALTER TABLE [dbo].[ServiceRequestTask] NOCHECK CONSTRAINT [FK_ServiceRequestTask_Task];




GO
ALTER TABLE [dbo].[ServiceRequestTask] NOCHECK CONSTRAINT [FK_ServiceRequestTask_Task];








GO
CREATE NONCLUSTERED INDEX [IX_ServiceRequest_IsComplete]
    ON [dbo].[ServiceRequestTask]([CompletedDate] ASC, [IsObsolete] ASC, [ServiceRequestId] ASC)
    INCLUDE([AssignedTo]);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_ServiceRequestTask_Guid]
    ON [dbo].[ServiceRequestTask]([ObjectGuid] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ServiceRequestTask_AssignedTo_ServiceRequestId]
    ON [dbo].[ServiceRequestTask]([AssignedTo] ASC, [ServiceRequestId] ASC);


GO


