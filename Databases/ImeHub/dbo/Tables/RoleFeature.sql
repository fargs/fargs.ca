CREATE TABLE [dbo].[RoleFeature] (
    [Id]           UNIQUEIDENTIFIER                            NOT NULL,
    [SysStartTime] DATETIME2 (0) GENERATED ALWAYS AS ROW START NOT NULL,
    [SysEndTime]   DATETIME2 (0) GENERATED ALWAYS AS ROW END   NOT NULL,
    [RoleId]       UNIQUEIDENTIFIER                            NOT NULL,
    [FeatureId]    UNIQUEIDENTIFIER                            NOT NULL,
    [IsActive]     BIT                                         CONSTRAINT [DF_RoleFeature_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_RoleFeature] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_RoleFeature_Feature] FOREIGN KEY ([FeatureId]) REFERENCES [dbo].[Feature] ([Id]),
    CONSTRAINT [FK_RoleFeature_Role] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[Role] ([Id]),
    PERIOD FOR SYSTEM_TIME ([SysStartTime], [SysEndTime])
)
WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE=[dbo].[RoleFeatureHistory], DATA_CONSISTENCY_CHECK=ON));

