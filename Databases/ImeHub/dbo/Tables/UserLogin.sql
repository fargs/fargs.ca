CREATE TABLE [dbo].[UserLogin] (
    [SysStartTime]  DATETIME2 (0) GENERATED ALWAYS AS ROW START NOT NULL,
    [SysEndTime]    DATETIME2 (0) GENERATED ALWAYS AS ROW END   NOT NULL,
    [LoginProvider] NVARCHAR (128)                              NOT NULL,
    [ProviderKey]   NVARCHAR (128)                              NOT NULL,
    [UserId]        UNIQUEIDENTIFIER                            NOT NULL,
    CONSTRAINT [PK_UserLogin] PRIMARY KEY CLUSTERED ([LoginProvider] ASC, [ProviderKey] ASC, [UserId] ASC),
    CONSTRAINT [FK_UserLogin_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([Id]) ON DELETE CASCADE,
    PERIOD FOR SYSTEM_TIME ([SysStartTime], [SysEndTime])
)
WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE=[dbo].[UserLoginHistory], DATA_CONSISTENCY_CHECK=ON));

