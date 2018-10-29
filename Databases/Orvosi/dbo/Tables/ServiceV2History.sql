CREATE TABLE [dbo].[ServiceV2History] (
    [Id]                 UNIQUEIDENTIFIER NOT NULL,
    [Name]               NVARCHAR (128)   NOT NULL,
    [Description]        NVARCHAR (MAX)   NULL,
    [Code]               NVARCHAR (10)    NULL,
    [Price]              DECIMAL (18, 2)  NOT NULL,
    [IsLocationRequired] BIT              NOT NULL,
    [ColorCode]          VARCHAR (10)     NULL,
    [PhysicianId]        UNIQUEIDENTIFIER NULL,
    [SysStartTime]       DATETIME2 (0)    NOT NULL,
    [SysEndTime]         DATETIME2 (0)    NOT NULL,
    [IsTravelRequired]   BIT              NOT NULL
);

