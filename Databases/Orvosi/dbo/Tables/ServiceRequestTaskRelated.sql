CREATE TABLE [dbo].[ServiceRequestTaskRelated] (
    [Id]                   INT              IDENTITY (1, 1) NOT NULL,
    [ServiceRequestTaskId] UNIQUEIDENTIFIER NULL,
    [RelatedTaskId]        UNIQUEIDENTIFIER NULL,
    [Relationship]         NVARCHAR (50)    NOT NULL,
    CONSTRAINT [PK_ServiceRequestTaskRelated] PRIMARY KEY CLUSTERED ([Id] ASC)
);

