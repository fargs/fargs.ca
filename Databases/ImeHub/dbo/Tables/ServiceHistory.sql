CREATE TABLE [dbo].[ServiceHistory] (
    [Id]               UNIQUEIDENTIFIER NOT NULL,
    [SysStartTime]     DATETIME2 (0)    NOT NULL,
    [SysEndTime]       DATETIME2 (0)    NOT NULL,
    [CompanyId]        UNIQUEIDENTIFIER NOT NULL,
    [Name]             NVARCHAR (128)   NOT NULL,
    [Description]      NVARCHAR (2000)  NULL,
    [Code]             NVARCHAR (10)    NULL,
    [ColorCode]        VARCHAR (10)     NULL,
    [Price]            DECIMAL (18, 2)  NOT NULL,
    [IsTravelRequired] BIT              NOT NULL
);

