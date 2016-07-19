CREATE TABLE [dbo].[PhysicianLocation] (
    [Id]            SMALLINT         IDENTITY (1, 1) NOT NULL,
    [PhysicianId]   UNIQUEIDENTIFIER NOT NULL,
    [LocationId]    SMALLINT         NOT NULL,
    [StatusId]      TINYINT          NULL,
    [Price]         DECIMAL (18, 2)  NULL,
    [PriceIncrease] DECIMAL (18, 2)  NULL,
    [ModifiedDate]  DATETIME         CONSTRAINT [DF_PhysicianLocation_ModifiedDate] DEFAULT (getdate()) NOT NULL,
    [ModifiedUser]  NVARCHAR (100)   CONSTRAINT [DF_PhysicianLocation_ModifiedUser] DEFAULT (suser_name()) NOT NULL,
    CONSTRAINT [PK_PhysicianLocation] PRIMARY KEY CLUSTERED ([Id] ASC)
);





