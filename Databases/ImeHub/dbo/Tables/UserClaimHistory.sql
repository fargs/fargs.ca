CREATE TABLE [dbo].[UserClaimHistory] (
    [Id]           INT              NOT NULL,
    [SysStartTime] DATETIME2 (0)    NOT NULL,
    [SysEndTime]   DATETIME2 (0)    NOT NULL,
    [UserId]       UNIQUEIDENTIFIER NOT NULL,
    [ClaimType]    NVARCHAR (MAX)   NULL,
    [ClaimValue]   NVARCHAR (MAX)   NULL
);


GO
CREATE CLUSTERED INDEX [ix_UserClaimHistory]
    ON [dbo].[UserClaimHistory]([SysEndTime] ASC, [SysStartTime] ASC) WITH (DATA_COMPRESSION = PAGE);

