CREATE TABLE [dbo].[ServiceRequestTemplateTask] (
    [Id]                       UNIQUEIDENTIFIER NOT NULL,
    [Sequence]                 SMALLINT         NULL,
    [ServiceRequestTemplateId] SMALLINT         NOT NULL,
    [TaskId]                   SMALLINT         NULL,
    [ModifiedDate]             DATETIME         CONSTRAINT [DF_ServiceRequestTemplateTask_ModifiedDate] DEFAULT (getdate()) NOT NULL,
    [ModifiedUser]             NVARCHAR (100)   CONSTRAINT [DF_ServiceRequestTemplateTask_ModifiedUser] DEFAULT (suser_name()) NOT NULL,
    [DueDateType]              NCHAR (10)       NULL,
    [ResponsibleRoleId]        UNIQUEIDENTIFIER NULL,
    CONSTRAINT [PK_ServiceRequestTemplateTask_1] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ServiceRequestTemplateTask_AspNetRoles] FOREIGN KEY ([ResponsibleRoleId]) REFERENCES [dbo].[AspNetRoles] ([Id]),
    CONSTRAINT [FK_ServiceRequestTemplateTask_ServiceRequestTemplate] FOREIGN KEY ([ServiceRequestTemplateId]) REFERENCES [dbo].[ServiceRequestTemplate] ([Id]),
    CONSTRAINT [FK_ServiceRequestTemplateTask_Task] FOREIGN KEY ([TaskId]) REFERENCES [dbo].[Task] ([Id])
);











