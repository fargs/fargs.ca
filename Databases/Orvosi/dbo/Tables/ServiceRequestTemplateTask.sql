CREATE TABLE [dbo].[ServiceRequestTemplateTask] (
    [Id]                                UNIQUEIDENTIFIER NOT NULL,
    [Sequence]                          SMALLINT         NULL,
    [ServiceRequestTemplateId]          SMALLINT         NOT NULL,
    [TaskId]                            SMALLINT         NULL,
    [ModifiedDate]                      DATETIME         CONSTRAINT [DF_ServiceRequestTemplateTask_ModifiedDate] DEFAULT (getdate()) NOT NULL,
    [ModifiedUser]                      NVARCHAR (100)   CONSTRAINT [DF_ServiceRequestTemplateTask_ModifiedUser] DEFAULT (suser_name()) NOT NULL,
    [DueDateType]                       NCHAR (10)       NULL,
    [ResponsibleRoleId]                 UNIQUEIDENTIFIER NULL,
    [IsBaselineDate]                    BIT              CONSTRAINT [DF_ServiceRequestTemplateTask_IsBaselineDate] DEFAULT ((0)) NOT NULL,
    [DueDateDurationFromBaseline]       SMALLINT         NULL,
    [EffectiveDateDurationFromBaseline] SMALLINT         NULL,
    [IsCriticalPath]                    BIT              CONSTRAINT [DF_ServiceRequestTemplateTask_IsCriticalPath] DEFAULT ((0)) NOT NULL,
    [IsBillable]                        BIT              CONSTRAINT [DF_ServiceRequestTemplateTask_IsBillable] DEFAULT ((0)) NOT NULL,
    [IsDeleted]                         BIT              CONSTRAINT [DF_ServiceRequestTemplateTask_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_ServiceRequestTemplateTask_1] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ServiceRequestTemplateTask_AspNetRoles] FOREIGN KEY ([ResponsibleRoleId]) REFERENCES [dbo].[AspNetRoles] ([Id]),
    CONSTRAINT [FK_ServiceRequestTemplateTask_ServiceRequestTemplate] FOREIGN KEY ([ServiceRequestTemplateId]) REFERENCES [dbo].[ServiceRequestTemplate] ([Id]),
    CONSTRAINT [FK_ServiceRequestTemplateTask_Task] FOREIGN KEY ([TaskId]) REFERENCES [dbo].[Task] ([Id])
);

















