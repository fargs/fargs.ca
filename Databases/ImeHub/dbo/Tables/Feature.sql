CREATE TABLE [dbo].[Feature] (
    [Id]           UNIQUEIDENTIFIER                            NOT NULL,
    [SysStartTime] DATETIME2 (0) GENERATED ALWAYS AS ROW START NOT NULL,
    [SysEndTime]   DATETIME2 (0) GENERATED ALWAYS AS ROW END   NOT NULL,
    [Name]         NVARCHAR (255)                              NOT NULL,
    [ParentID]     UNIQUEIDENTIFIER                            NULL,
    [IsActive]     BIT                                         CONSTRAINT [DF_Feature_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_Feature] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Feature_Parent] FOREIGN KEY ([ParentID]) REFERENCES [dbo].[Feature] ([Id]),
    PERIOD FOR SYSTEM_TIME ([SysStartTime], [SysEndTime])
)
WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE=[dbo].[FeatureHistory], DATA_CONSISTENCY_CHECK=ON));

