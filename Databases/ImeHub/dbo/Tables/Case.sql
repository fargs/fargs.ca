CREATE TABLE [dbo].[Case] (
    [SysStartTime]      DATETIME2 (7) GENERATED ALWAYS AS ROW START NOT NULL,
    [SysEndTime]        DATETIME2 (7) GENERATED ALWAYS AS ROW END   NOT NULL,
    [Id]                UNIQUEIDENTIFIER                            NOT NULL,
    [PhysicianId]       UNIQUEIDENTIFIER                            NOT NULL,
    [CaseNumber]        NVARCHAR (50)                               NOT NULL,
    [AlternateKey]      NVARCHAR (128)                              NULL,
    [ClaimantName]      NVARCHAR (128)                              NULL,
    [StatusId]          TINYINT                                     NOT NULL,
    [StatusChangedById] UNIQUEIDENTIFIER                            NULL,
    [StatusChangedDate] DATETIME                                    NULL,
    [FolderUrl]         NVARCHAR (256)                              NULL,
    [MedicolegalTypeId] TINYINT                                     NULL,
    [ReferralSource]    NVARCHAR (200)                              NULL,
    CONSTRAINT [PK_Case] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Case_CaseStatus] FOREIGN KEY ([StatusId]) REFERENCES [dbo].[CaseStatus] ([Id]),
    CONSTRAINT [FK_Case_MedicolegalType] FOREIGN KEY ([MedicolegalTypeId]) REFERENCES [dbo].[MedicolegalType] ([Id]),
    CONSTRAINT [FK_Case_Physician] FOREIGN KEY ([PhysicianId]) REFERENCES [dbo].[Physician] ([Id]),
    PERIOD FOR SYSTEM_TIME ([SysStartTime], [SysEndTime])
)
WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE=[dbo].[CaseHistory], DATA_CONSISTENCY_CHECK=ON));

