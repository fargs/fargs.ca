CREATE TABLE [dbo].[PhysicianService] (
    [Id]           SMALLINT         IDENTITY (1, 1) NOT NULL,
    [PhysicianId]  UNIQUEIDENTIFIER NOT NULL,
    [ServiceId]    SMALLINT         NOT NULL,
    [CreatedDate]  DATETIME         CONSTRAINT [DF_PhysicianService_CreatedDate] DEFAULT (getdate()) NOT NULL,
    [CreatedUser]  NVARCHAR (100)   NOT NULL,
    [ModifiedDate] DATETIME         CONSTRAINT [DF_PhysicianService_ModifiedDate] DEFAULT (getdate()) NOT NULL,
    [ModifiedUser] NVARCHAR (100)   NOT NULL,
    CONSTRAINT [PK_PhysicianService] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_PhysicianService_Physician] FOREIGN KEY ([PhysicianId]) REFERENCES [dbo].[Physician] ([Id]),
    CONSTRAINT [FK_PhysicianService_Service] FOREIGN KEY ([ServiceId]) REFERENCES [dbo].[Service] ([Id])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_PhysicianService]
    ON [dbo].[PhysicianService]([PhysicianId] ASC, [ServiceId] ASC);

