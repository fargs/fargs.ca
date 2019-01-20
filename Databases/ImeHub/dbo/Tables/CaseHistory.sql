CREATE TABLE [dbo].[CaseHistory] (
    [SysStartTime]      DATETIME2 (7)    NOT NULL,
    [SysEndTime]        DATETIME2 (7)    NOT NULL,
    [Id]                UNIQUEIDENTIFIER NOT NULL,
    [PhysicianId]       UNIQUEIDENTIFIER NOT NULL,
    [CaseNumber]        NVARCHAR (50)    NOT NULL,
    [AlternateKey]      NVARCHAR (128)   NULL,
    [ClaimantName]      NVARCHAR (128)   NULL,
    [StatusId]          TINYINT          NOT NULL,
    [StatusChangedById] UNIQUEIDENTIFIER NULL,
    [StatusChangedDate] DATETIME         NULL,
    [FolderUrl]         NVARCHAR (256)   NULL,
    [MedicolegalTypeId] TINYINT          NULL,
    [ReferralSource]    NVARCHAR (200)   NULL
);


GO
CREATE CLUSTERED INDEX [ix_CaseHistory]
    ON [dbo].[CaseHistory]([SysEndTime] ASC, [SysStartTime] ASC) WITH (DATA_COMPRESSION = PAGE);

