CREATE TABLE [dbo].[AvailableDay] (
    [Id]           UNIQUEIDENTIFIER                            NOT NULL,
    [SysStartTime] DATETIME2 (7) GENERATED ALWAYS AS ROW START NOT NULL,
    [SysEndTime]   DATETIME2 (7) GENERATED ALWAYS AS ROW END   NOT NULL,
    [PhysicianId]  UNIQUEIDENTIFIER                            NOT NULL,
    [Day]          DATE                                        NOT NULL,
    [CompanyId]    UNIQUEIDENTIFIER                            NULL,
    [AddressId]    UNIQUEIDENTIFIER                            NULL,
    [CityId]       UNIQUEIDENTIFIER                            NULL,
    CONSTRAINT [PK_AvailableDay] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_AvailableDay_Address] FOREIGN KEY ([AddressId]) REFERENCES [dbo].[Address] ([Id]),
    CONSTRAINT [FK_AvailableDay_City] FOREIGN KEY ([CityId]) REFERENCES [dbo].[City] ([Id]),
    CONSTRAINT [FK_AvailableDay_Company] FOREIGN KEY ([CompanyId]) REFERENCES [dbo].[Company] ([Id]),
    CONSTRAINT [FK_AvailableDay_Physician] FOREIGN KEY ([PhysicianId]) REFERENCES [dbo].[Physician] ([Id]),
    PERIOD FOR SYSTEM_TIME ([SysStartTime], [SysEndTime])
)
WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE=[dbo].[AvailableDayHistory], DATA_CONSISTENCY_CHECK=ON));

