CREATE TABLE [dbo].[AvailableDayResource] (
    [Id]             UNIQUEIDENTIFIER NOT NULL,
    [AvailableDayId] SMALLINT         NOT NULL,
    [UserId]         UNIQUEIDENTIFIER NOT NULL,
    [CreatedDate]    DATETIME         NOT NULL,
    [CreatedUser]    NVARCHAR (100)   NOT NULL,
    [ModifiedDate]   DATETIME         NOT NULL,
    [ModifiedUser]   NVARCHAR (100)   NOT NULL,
    CONSTRAINT [PK_AvailableDayResource] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_AvailableDayResource_AspNetUsers] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]),
    CONSTRAINT [FK_AvailableDayResource_AvailableDay] FOREIGN KEY ([AvailableDayId]) REFERENCES [dbo].[AvailableDay] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE
);



