CREATE TABLE [dbo].[UserInbox] (
    [Id]                      INT              NOT NULL,
    [UserId]                  UNIQUEIDENTIFIER NOT NULL,
    [ServiceRequestMessageId] UNIQUEIDENTIFIER NULL,
    [IsRead]                  BIT              CONSTRAINT [DF__UserInbox__IsRea__5DB5E0CB] DEFAULT ((0)) NOT NULL,
    [ModifiedDate]            DATETIME         CONSTRAINT [DF_UserInbox_ModifiedDate] DEFAULT (getdate()) NOT NULL,
    [ModifiedUser]            NVARCHAR (100)   CONSTRAINT [DF_UserInbox_ModifiedUser] DEFAULT (suser_name()) NOT NULL,
    CONSTRAINT [PK_UserInbox] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_UserInbox_AspNetUsers] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]),
    CONSTRAINT [FK_UserInbox_ServiceRequestMessage] FOREIGN KEY ([ServiceRequestMessageId]) REFERENCES [dbo].[ServiceRequestMessage] ([Id])
);

