CREATE TABLE [dbo].[TeamRole] (
    [SysStartTime] DATETIME2 (0) GENERATED ALWAYS AS ROW START NOT NULL,
    [SysEndTime]   DATETIME2 (0) GENERATED ALWAYS AS ROW END   NOT NULL,
    [Id]           UNIQUEIDENTIFIER                            NOT NULL,
    [Name]         NVARCHAR (256)                              NOT NULL,
    [Code]         NVARCHAR (10)                               NULL,
    [ColorCode]    NVARCHAR (50)                               NULL,
    [PhysicianId]  UNIQUEIDENTIFIER                            NOT NULL,
    CONSTRAINT [PK_TeamRole] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_TeamRole_Physician] FOREIGN KEY ([PhysicianId]) REFERENCES [dbo].[Physician] ([Id]),
    PERIOD FOR SYSTEM_TIME ([SysStartTime], [SysEndTime])
)
WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE=[dbo].[TeamRoleHistory], DATA_CONSISTENCY_CHECK=ON));

