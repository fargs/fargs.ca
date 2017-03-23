CREATE TABLE [dbo].[ServiceRequestResource] (
    [Id]                 UNIQUEIDENTIFIER NOT NULL,
    [ServiceRequestId]   INT              NOT NULL,
    [UserId]             UNIQUEIDENTIFIER NOT NULL,
    [RoleId]             UNIQUEIDENTIFIER NULL,
    [BoxCollaborationId] NVARCHAR (50)    NULL,
    [CreatedDate]        DATETIME         NOT NULL,
    [CreatedUser]        NVARCHAR (100)   NOT NULL,
    [ModifiedDate]       DATETIME         NOT NULL,
    [ModifiedUser]       NVARCHAR (100)   NOT NULL,
    CONSTRAINT [PK_ServiceRequestResource] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ServiceRequestResource_AspNetRoles] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[AspNetRoles] ([Id]),
    CONSTRAINT [FK_ServiceRequestResource_AspNetUsers] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]),
    CONSTRAINT [FK_ServiceRequestResource_ServiceRequest] FOREIGN KEY ([ServiceRequestId]) REFERENCES [dbo].[ServiceRequest] ([Id])
);

