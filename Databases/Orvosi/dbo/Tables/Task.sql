CREATE TABLE [dbo].[Task] (
    [Id]                SMALLINT            IDENTITY (1, 1) NOT NULL,
    [ObjectGuid]        UNIQUEIDENTIFIER    CONSTRAINT [DF_Task_ObjectGuid] DEFAULT (newid()) NOT NULL,
    [ServiceCategoryId] SMALLINT            NULL,
    [ServiceId]         SMALLINT            NULL,
    [Name]              NVARCHAR (128)      NOT NULL,
    [Guidance]          NVARCHAR (1000)     NULL,
    [TaskPhaseId]       TINYINT             NULL,
    [ResponsibleRoleId] UNIQUEIDENTIFIER    NULL,
    [IsBillable]        BIT                 CONSTRAINT [DF_Task_IsBillable] DEFAULT ((0)) NULL,
    [HourlyRate]        DECIMAL (18, 2)     NULL,
    [EstimatedHours]    DECIMAL (18, 2)     NULL,
    [Sequence]          SMALLINT            NULL,
    [IsMilestone]       BIT                 CONSTRAINT [DF_Task_IsMilestone] DEFAULT ((0)) NULL,
    [NodeId]            [sys].[hierarchyid] NULL,
    [ModifiedDate]      DATETIME            CONSTRAINT [DF_Task_ModifiedDate] DEFAULT (getdate()) NOT NULL,
    [ModifiedUser]      NVARCHAR (100)      CONSTRAINT [DF_Task_ModifiedUser] DEFAULT (suser_name()) NOT NULL,
    [DependsOn]         NVARCHAR (50)       NULL,
    [DueDateBase]       TINYINT             NULL,
    [DueDateDiff]       SMALLINT            NULL,
    [ShortName]         NVARCHAR (50)       NULL,
    [IsCriticalPath]    BIT                 CONSTRAINT [DF_Task_IsCriticalPath] DEFAULT ((0)) NOT NULL,
    [TaskType]          VARCHAR (50)        NULL,
    [Workload]          TINYINT             NULL,
    CONSTRAINT [PK_Task] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Task_AspNetRoles] FOREIGN KEY ([ResponsibleRoleId]) REFERENCES [dbo].[AspNetRoles] ([Id]),
    CONSTRAINT [FK_Task_TaskPhase] FOREIGN KEY ([TaskPhaseId]) REFERENCES [dbo].[TaskPhase] ([Id])
);



















