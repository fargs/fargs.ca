CREATE TABLE [dbo].[ServiceRequestTemplateTaskRelated] (
    [Id]                           INT              IDENTITY (1, 1) NOT NULL,
    [ServiceRequestTemplateTaskId] UNIQUEIDENTIFIER NULL,
    [RelatedTaskId]                UNIQUEIDENTIFIER NULL,
    [Relationship]                 NVARCHAR (50)    NOT NULL,
    CONSTRAINT [PK_ServiceRequestTemplateTaskRelated] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ServiceRequestTemplateTaskRelated_ServiceRequestTemplateTask] FOREIGN KEY ([ServiceRequestTemplateTaskId]) REFERENCES [dbo].[ServiceRequestTemplateTask] ([Id]),
    CONSTRAINT [FK_ServiceRequestTemplateTaskRelated_ServiceRequestTemplateTask1] FOREIGN KEY ([RelatedTaskId]) REFERENCES [dbo].[ServiceRequestTemplateTask] ([Id])
);

