CREATE TABLE [dbo].[Collaborator] (
    [Id]                 INT              IDENTITY (1, 1) NOT NULL,
    [UserId]             UNIQUEIDENTIFIER NOT NULL,
    [CollaboratorUserId] UNIQUEIDENTIFIER NOT NULL,
    [CreatedDate]        DATETIME         CONSTRAINT [DF_Collaborator_CreatedDate] DEFAULT (getdate()) NOT NULL,
    [CreatedUser]        NVARCHAR (128)   NOT NULL,
    [ModifiedDate]       DATETIME         CONSTRAINT [DF_Collaborator_ModifiedDate] DEFAULT (getdate()) NOT NULL,
    [ModifiedUser]       NVARCHAR (128)   CONSTRAINT [DF_Collaborator_ModifiedUser] DEFAULT (suser_name()) NOT NULL,
    CONSTRAINT [PK_Collaborator] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_AspNetUsers_Collaborator] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_Collaborator_AspNetUsers] FOREIGN KEY ([CollaboratorUserId]) REFERENCES [dbo].[AspNetUsers] ([Id])
);

