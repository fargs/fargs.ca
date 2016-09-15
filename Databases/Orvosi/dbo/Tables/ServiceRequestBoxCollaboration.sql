CREATE TABLE [dbo].[ServiceRequestBoxCollaboration] (
    [Id]                 INT              IDENTITY (1, 1) NOT NULL,
    [UserId]             UNIQUEIDENTIFIER NOT NULL,
    [ServiceRequestId]   INT              NOT NULL,
    [BoxCollaborationId] NVARCHAR (50)    NULL,
    [ModifiedDate]       DATETIME         CONSTRAINT [DF_ServiceRequestBoxCollaboration_ModifiedDate] DEFAULT (getdate()) NOT NULL,
    [ModifiedUser]       NVARCHAR (100)   CONSTRAINT [DF_ServiceRequestBoxCollaboration_ModifiedUser] DEFAULT (suser_name()) NOT NULL,
    CONSTRAINT [PK_ServiceRequestBoxCollaboration] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ServiceRequestBoxCollaboration_AspNetUsers] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]),
    CONSTRAINT [FK_ServiceRequestBoxCollaboration_ServiceRequest] FOREIGN KEY ([ServiceRequestId]) REFERENCES [dbo].[ServiceRequest] ([Id])
);







