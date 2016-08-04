CREATE TABLE [dbo].[ServiceRequestTask] (
    [Id]                    INT              IDENTITY (1, 1) NOT NULL,
    [ObjectGuid]            UNIQUEIDENTIFIER CONSTRAINT [DF_ServiceRequestTask_ObjectGuid] DEFAULT (newid()) NOT NULL,
    [ServiceRequestId]      INT              NOT NULL,
    [TaskId]                SMALLINT         NULL,
    [TaskName]              NVARCHAR (128)   NOT NULL,
    [ResponsibleRoleId]     UNIQUEIDENTIFIER NULL,
    [ResponsibleRoleName]   NVARCHAR (128)   NULL,
    [Sequence]              SMALLINT         NULL,
    [AssignedTo]            UNIQUEIDENTIFIER NULL,
    [IsBillable]            BIT              CONSTRAINT [DF_ServiceRequestTask_IsBillable] DEFAULT ((0)) NOT NULL,
    [HourlyRate]            DECIMAL (18, 2)  NULL,
    [EstimatedHours]        DECIMAL (18, 2)  NULL,
    [ActualHours]           DECIMAL (18, 2)  NULL,
    [CompletedDate]         DATETIME         NULL,
    [Notes]                 NVARCHAR (2000)  NULL,
    [InvoiceItemId]         SMALLINT         NULL,
    [ModifiedDate]          DATETIME         CONSTRAINT [DF_ServiceRequestTask_ModifiedDate] DEFAULT (getdate()) NOT NULL,
    [ModifiedUser]          NVARCHAR (100)   CONSTRAINT [DF_ServiceRequestTask_ModifiedUser] DEFAULT (suser_name()) NOT NULL,
    [TaskPhaseId]           TINYINT          NULL,
    [TaskPhaseName]         NVARCHAR (128)   NULL,
    [Guidance]              NVARCHAR (1000)  NULL,
    [IsObsolete]            BIT              CONSTRAINT [DF_ServiceRequestTask_IsObsolete] DEFAULT ((0)) NOT NULL,
    [DependsOn]             NVARCHAR (50)    NULL,
    [DueDateBase]           TINYINT          NULL,
    [DueDateDiff]           SMALLINT         NULL,
    [ShortName]             NVARCHAR (50)    NULL,
    [IsCriticalPath]        BIT              CONSTRAINT [DF_ServiceRequestTask_IsCriticalPath] DEFAULT ((0)) NOT NULL,
    [IsDependentOnExamDate] AS               (CONVERT([bit],case when [DependsOn]='133' then (1) else (0) end)),
    CONSTRAINT [PK_ServiceRequestTask] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ServiceRequestTask_AspNetUsers] FOREIGN KEY ([AssignedTo]) REFERENCES [dbo].[AspNetUsers] ([Id]),
    CONSTRAINT [FK_ServiceRequestTask_ServiceRequest] FOREIGN KEY ([ServiceRequestId]) REFERENCES [dbo].[ServiceRequest] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_ServiceRequestTask_Task] FOREIGN KEY ([TaskId]) REFERENCES [dbo].[Task] ([Id])
);






GO
CREATE NONCLUSTERED INDEX [IX_ServiceRequest_IsComplete]
    ON [dbo].[ServiceRequestTask]([CompletedDate] ASC, [IsObsolete] ASC, [ServiceRequestId] ASC)
    INCLUDE([AssignedTo]);

