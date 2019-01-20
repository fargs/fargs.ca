CREATE TABLE [dbo].[ServiceRequestHistory] (
    [SysStartTime]                  DATETIME2 (7)    NOT NULL,
    [SysEndTime]                    DATETIME2 (7)    NOT NULL,
    [Id]                            UNIQUEIDENTIFIER NOT NULL,
    [PhysicianId]                   UNIQUEIDENTIFIER NOT NULL,
    [CaseNumber]                    NVARCHAR (50)    NOT NULL,
    [AlternateKey]                  NVARCHAR (128)   NULL,
    [ClaimantName]                  NVARCHAR (128)   NULL,
    [Title]                         NVARCHAR (256)   NULL,
    [RequestedDate]                 DATETIME         NULL,
    [RequestedBy]                   UNIQUEIDENTIFIER NULL,
    [StatusId]                      TINYINT          NOT NULL,
    [StatusChangedById]             UNIQUEIDENTIFIER NULL,
    [StatusChangedDate]             DATETIME         NULL,
    [ServiceId]                     UNIQUEIDENTIFIER NOT NULL,
    [FolderUrl]                     NVARCHAR (256)   NULL,
    [DueDate]                       DATE             NULL,
    [AvailableSlotId]               UNIQUEIDENTIFIER NULL,
    [AppointmentDate]               DATE             NULL,
    [StartTime]                     TIME (7)         NULL,
    [EndTime]                       TIME (7)         NULL,
    [AddressId]                     UNIQUEIDENTIFIER NULL,
    [CancellationStatusId]          TINYINT          NOT NULL,
    [CancellationStatusChangedDate] DATETIME         NULL,
    [CancellationStatusChangedById] UNIQUEIDENTIFIER NULL,
    [HasErrors]                     BIT              NOT NULL,
    [HasWarnings]                   BIT              NOT NULL,
    [MedicolegalTypeId]             TINYINT          NULL,
    [ReferralSource]                NVARCHAR (200)   NULL
);


GO
CREATE CLUSTERED INDEX [ix_ServiceRequestHistory]
    ON [dbo].[ServiceRequestHistory]([SysEndTime] ASC, [SysStartTime] ASC) WITH (DATA_COMPRESSION = PAGE);

