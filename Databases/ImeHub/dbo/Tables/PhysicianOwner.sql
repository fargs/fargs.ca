CREATE TABLE [dbo].[PhysicianOwner] (
    [PhysicianId]                 UNIQUEIDENTIFIER                            NOT NULL,
    [SysStartTime]                DATETIME2 (7) GENERATED ALWAYS AS ROW START NOT NULL,
    [SysEndTime]                  DATETIME2 (7) GENERATED ALWAYS AS ROW END   NOT NULL,
    [Email]                       NVARCHAR (128)                              NOT NULL,
    [UserId]                      UNIQUEIDENTIFIER                            NULL,
    [AcceptanceStatusId]          TINYINT                                     NOT NULL,
    [AcceptanceStatusChangedDate] DATETIME                                    NOT NULL,
    [Title]                       NVARCHAR (10)                               NULL,
    [FirstName]                   NVARCHAR (128)                              NULL,
    [LastName]                    NVARCHAR (128)                              NULL,
    CONSTRAINT [PK_PhysicianOwner] PRIMARY KEY CLUSTERED ([PhysicianId] ASC),
    CONSTRAINT [FK_PhysicianOwner_Physician] FOREIGN KEY ([PhysicianId]) REFERENCES [dbo].[Physician] ([Id]),
    CONSTRAINT [FK_PhysicianOwner_PhysicianOwnerAcceptanceStatus] FOREIGN KEY ([AcceptanceStatusId]) REFERENCES [dbo].[PhysicianOwnerAcceptanceStatus] ([Id]),
    CONSTRAINT [FK_PhysicianOwner_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([Id]),
    PERIOD FOR SYSTEM_TIME ([SysStartTime], [SysEndTime])
)
WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE=[dbo].[PhysicianOwnerHistory], DATA_CONSISTENCY_CHECK=ON));



