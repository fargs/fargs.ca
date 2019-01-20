CREATE TABLE [dbo].[CompanyServiceHistory] (
    [Id]               UNIQUEIDENTIFIER NOT NULL,
    [ServiceId]        UNIQUEIDENTIFIER NULL,
    [Price]            DECIMAL (18, 2)  NULL,
    [SysStartTime]     DATETIME2 (7)    NOT NULL,
    [SysEndTime]       DATETIME2 (7)    NOT NULL,
    [CompanyId]        UNIQUEIDENTIFIER NOT NULL,
    [Name]             NVARCHAR (250)   NOT NULL,
    [IsTravelRequired] BIT              NOT NULL
);


GO
CREATE CLUSTERED INDEX [ix_CompanyServiceHistory]
    ON [dbo].[CompanyServiceHistory]([SysEndTime] ASC, [SysStartTime] ASC) WITH (DATA_COMPRESSION = PAGE);

