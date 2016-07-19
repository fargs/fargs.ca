CREATE TABLE [dbo].[ServiceRequestTemplateTask] (
    [Id]                       SMALLINT       NOT NULL,
    [ParentId]                 SMALLINT       NULL,
    [Sequence]                 SMALLINT       NULL,
    [ServiceRequestTemplateId] SMALLINT       NOT NULL,
    [TaskId]                   SMALLINT       NULL,
    [ModifiedDate]             DATETIME       CONSTRAINT [DF_ServiceRequestTemplateTask_ModifiedDate] DEFAULT (getdate()) NOT NULL,
    [ModifiedUser]             NVARCHAR (100) CONSTRAINT [DF_ServiceRequestTemplateTask_ModifiedUser] DEFAULT (suser_name()) NOT NULL,
    CONSTRAINT [PK_ServiceRequestTemplateTask] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ServiceRequestTemplateTask_ServiceRequestTemplate] FOREIGN KEY ([ServiceRequestTemplateId]) REFERENCES [dbo].[ServiceRequestTemplate] ([Id]),
    CONSTRAINT [FK_ServiceRequestTemplateTask_Task] FOREIGN KEY ([TaskId]) REFERENCES [dbo].[Task] ([Id])
);





