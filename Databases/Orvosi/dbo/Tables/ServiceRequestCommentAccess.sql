CREATE TABLE [dbo].[ServiceRequestCommentAccess] (
    [Id]                      UNIQUEIDENTIFIER NOT NULL,
    [ServiceRequestCommentId] UNIQUEIDENTIFIER NOT NULL,
    [UserId]                  UNIQUEIDENTIFIER NOT NULL,
    [CreatedDate]             DATETIME         NOT NULL,
    [CreatedUser]             NVARCHAR (100)   NOT NULL,
    [ModifiedDate]            DATETIME         NOT NULL,
    [ModifiedUser]            NVARCHAR (100)   NOT NULL,
    CONSTRAINT [PK_ServiceRequestCommentAccess] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ServiceRequestCommentAccess_AspNetUsers] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_ServiceRequestCommentAccess_ServiceRequestComment] FOREIGN KEY ([ServiceRequestCommentId]) REFERENCES [dbo].[ServiceRequestComment] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE
);

