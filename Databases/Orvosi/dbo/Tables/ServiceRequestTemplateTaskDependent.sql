CREATE TABLE [dbo].[ServiceRequestTemplateTaskDependent] (
    [ParentId] UNIQUEIDENTIFIER NOT NULL,
    [ChildId]  UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_ServiceRequestTemplateTaskDependent] PRIMARY KEY CLUSTERED ([ParentId] ASC, [ChildId] ASC),
    CONSTRAINT [FK_ServiceRequestTemplateTaskDependent_Dependent] FOREIGN KEY ([ChildId]) REFERENCES [dbo].[ServiceRequestTemplateTask] ([Id]),
    CONSTRAINT [FK_ServiceRequestTemplateTaskDependent_ServiceRequestTemplateTask] FOREIGN KEY ([ParentId]) REFERENCES [dbo].[ServiceRequestTemplateTask] ([Id])
);

