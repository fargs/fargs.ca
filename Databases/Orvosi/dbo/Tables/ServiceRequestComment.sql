CREATE TABLE [dbo].[ServiceRequestComment] (
    [Id]               UNIQUEIDENTIFIER NOT NULL,
    [Comment]          NVARCHAR (MAX)   NOT NULL,
    [PostedDate]       DATETIME         NOT NULL,
    [UserId]           UNIQUEIDENTIFIER NOT NULL,
    [ServiceRequestId] INT              NOT NULL,
    [CommentTypeId]    TINYINT          NOT NULL,
    [IsPrivate]        BIT              CONSTRAINT [DF_ServiceRequestComment_IsPrivate] DEFAULT ((0)) NOT NULL,
    [CreatedDate]      DATETIME         NOT NULL,
    [CreatedUser]      NVARCHAR (100)   NOT NULL,
    [ModifiedDate]     DATETIME         NOT NULL,
    [ModifiedUser]     NVARCHAR (100)   NOT NULL,
    CONSTRAINT [PK_ServiceRequestComment] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ServiceRequestComment_AspNetUsers] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]),
    CONSTRAINT [FK_ServiceRequestComment_CommentType] FOREIGN KEY ([CommentTypeId]) REFERENCES [dbo].[CommentType] ([Id]),
    CONSTRAINT [FK_ServiceRequestComment_ServiceRequest] FOREIGN KEY ([ServiceRequestId]) REFERENCES [dbo].[ServiceRequest] ([Id])
);

