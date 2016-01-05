CREATE TABLE [dbo].[AvailableSlot] (
    [Id]             SMALLINT       IDENTITY (1, 1) NOT NULL,
    [AvailableDayId] SMALLINT       NOT NULL,
    [StartTime]      TIME (7)       NOT NULL,
    [EndTime]        TIME (7)       NULL,
    [Duration]       SMALLINT       NULL,
    [ModifiedDate]   DATETIME       CONSTRAINT [DF_AvailableSlot_ModifiedDate] DEFAULT (getdate()) NOT NULL,
    [ModifiedUser]   NVARCHAR (100) CONSTRAINT [DF_AvailableSlot_ModifiedUser] DEFAULT (suser_name()) NOT NULL,
    CONSTRAINT [PK_AvailableSlot] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_AvailableDay_AvailableSlot] FOREIGN KEY ([AvailableDayId]) REFERENCES [dbo].[AvailableDay] ([Id]) ON DELETE CASCADE
);



