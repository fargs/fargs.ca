CREATE TABLE [dbo].[UserClaim] (
    [Id]           INT                                         IDENTITY (1, 1) NOT NULL,
    [SysStartTime] DATETIME2 (0) GENERATED ALWAYS AS ROW START NOT NULL,
    [SysEndTime]   DATETIME2 (0) GENERATED ALWAYS AS ROW END   NOT NULL,
    [UserId]       UNIQUEIDENTIFIER                            NOT NULL,
    [ClaimType]    NVARCHAR (MAX)                              NULL,
    [ClaimValue]   NVARCHAR (MAX)                              NULL,
    CONSTRAINT [PK_UserClaim] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_UserClaim_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([Id]) ON DELETE CASCADE,
    PERIOD FOR SYSTEM_TIME ([SysStartTime], [SysEndTime])
)
WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE=[dbo].[UserClaimHistory], DATA_CONSISTENCY_CHECK=ON));

