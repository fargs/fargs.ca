CREATE TABLE [dbo].[OrganizationHistory] (
    [Id]              UNIQUEIDENTIFIER NOT NULL,
    [SysStartTime]    DATETIME2 (7)    NOT NULL,
    [SysEndTime]      DATETIME2 (7)    NOT NULL,
    [Name]            NVARCHAR (128)   NULL,
    [Code]            NVARCHAR (10)    NULL,
    [ColorCode]       NVARCHAR (10)    NOT NULL,
    [OwnerId]         UNIQUEIDENTIFIER NOT NULL,
    [AdministratorId] UNIQUEIDENTIFIER NULL
);


GO
CREATE CLUSTERED INDEX [ix_OrganizationHistory]
    ON [dbo].[OrganizationHistory]([SysEndTime] ASC, [SysStartTime] ASC) WITH (DATA_COMPRESSION = PAGE);

