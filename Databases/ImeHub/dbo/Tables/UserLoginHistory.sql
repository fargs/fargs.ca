CREATE TABLE [dbo].[UserLoginHistory] (
    [SysStartTime]  DATETIME2 (0)    NOT NULL,
    [SysEndTime]    DATETIME2 (0)    NOT NULL,
    [LoginProvider] NVARCHAR (128)   NOT NULL,
    [ProviderKey]   NVARCHAR (128)   NOT NULL,
    [UserId]        UNIQUEIDENTIFIER NOT NULL
);


GO
CREATE CLUSTERED INDEX [ix_UserLoginHistory]
    ON [dbo].[UserLoginHistory]([SysEndTime] ASC, [SysStartTime] ASC) WITH (DATA_COMPRESSION = PAGE);

