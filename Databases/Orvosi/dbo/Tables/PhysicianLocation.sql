CREATE TABLE [dbo].[PhysicianLocation] (
    [Id]           SMALLINT       IDENTITY (1, 1) NOT NULL,
    [PhysicianId]  NVARCHAR (128) NOT NULL,
    [LocationId]   SMALLINT       NULL,
    [IsPrimary]    BIT            CONSTRAINT [DF_PhysicianLocation_IsPrimary] DEFAULT ((0)) NOT NULL,
    [Preference]   TINYINT        NULL,
    [ModifiedDate] DATETIME       CONSTRAINT [DF_PhysicianLocation_ModifiedDate] DEFAULT (getdate()) NOT NULL,
    [ModifiedUser] NVARCHAR (100) CONSTRAINT [DF_PhysicianLocation_ModifiedUser] DEFAULT (suser_name()) NOT NULL,
    CONSTRAINT [PK_PhysicianLocation] PRIMARY KEY CLUSTERED ([Id] ASC)
);

