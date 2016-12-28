CREATE TABLE [dbo].[AvailableDayResources] (
    [Id]             SMALLINT         IDENTITY (1, 1) NOT NULL,
    [AvailableDayId] SMALLINT         NOT NULL,
    [UserId]         UNIQUEIDENTIFIER NOT NULL,
    [ResourceRoleId] UNIQUEIDENTIFIER NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_AvailableDayResources_AvailableDay] FOREIGN KEY ([AvailableDayId]) REFERENCES [dbo].[AvailableDay] ([Id])
);

