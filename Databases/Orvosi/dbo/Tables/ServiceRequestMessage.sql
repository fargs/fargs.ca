CREATE TABLE [dbo].[ServiceRequestMessage] (
    [Id]               UNIQUEIDENTIFIER NOT NULL,
    [Message]          NVARCHAR (256)   NOT NULL,
    [PostedDate]       DATETIME         NOT NULL,
    [UserId]           UNIQUEIDENTIFIER NOT NULL,
    [ServiceRequestId] INT              NOT NULL,
    [ModifiedDate]     DATETIME         CONSTRAINT [DF_ServiceRequestMessage_ModifiedDate] DEFAULT (getdate()) NOT NULL,
    [ModifiedUser]     NVARCHAR (100)   CONSTRAINT [DF_ServiceRequestMessage_ModifiedUser] DEFAULT (suser_name()) NOT NULL,
    CONSTRAINT [PK_ServiceRequestMessage] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ServiceRequestMessage_AspNetUsers] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]),
    CONSTRAINT [FK_ServiceRequestMessage_ServiceRequest] FOREIGN KEY ([ServiceRequestId]) REFERENCES [dbo].[ServiceRequest] ([Id])
);

