CREATE TABLE [dbo].[PhysicianHistory] (
    [Id]                       UNIQUEIDENTIFIER NOT NULL,
    [Designations]             NVARCHAR (128)   NULL,
    [SpecialtyId]              TINYINT          NULL,
    [OtherSpecialties]         NVARCHAR (2000)  NULL,
    [Pediatrics]               BIT              NULL,
    [Adolescents]              BIT              NULL,
    [Adults]                   BIT              NULL,
    [Geriatrics]               BIT              NULL,
    [PrimaryAddressId]         SMALLINT         NULL,
    [ModifiedDate]             DATETIME         NOT NULL,
    [ModifiedUser]             NVARCHAR (100)   NOT NULL,
    [BoxCaseTemplateFolderId]  NVARCHAR (128)   NULL,
    [BoxAddOnTemplateFolderId] NVARCHAR (128)   NULL,
    [BoxInvoicesFolderId]      NVARCHAR (128)   NULL,
    [SysStartTime]             DATETIME2 (0)    NOT NULL,
    [SysEndTime]               DATETIME2 (0)    NOT NULL
);


GO
CREATE CLUSTERED INDEX [ix_PhysicianHistory]
    ON [dbo].[PhysicianHistory]([SysEndTime] ASC, [SysStartTime] ASC) WITH (DATA_COMPRESSION = PAGE);

