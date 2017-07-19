CREATE TABLE [dbo].[Physician] (
    [Id]                       UNIQUEIDENTIFIER NOT NULL,
    [Designations]             NVARCHAR (128)   NULL,
    [SpecialtyId]              TINYINT          NULL,
    [OtherSpecialties]         NVARCHAR (2000)  NULL,
    [Pediatrics]               BIT              NULL,
    [Adolescents]              BIT              NULL,
    [Adults]                   BIT              NULL,
    [Geriatrics]               BIT              NULL,
    [PrimaryAddressId]         SMALLINT         NULL,
    [ModifiedDate]             DATETIME         CONSTRAINT [DF_Physician_ModifiedDate] DEFAULT (getdate()) NOT NULL,
    [ModifiedUser]             NVARCHAR (100)   CONSTRAINT [DF_Physician_ModifiedUser] DEFAULT (suser_name()) NOT NULL,
    [BoxCaseTemplateFolderId]  NVARCHAR (128)   NULL,
    [BoxAddOnTemplateFolderId] NVARCHAR (128)   NULL,
    [BoxInvoicesFolderId]      NVARCHAR (128)   NULL,
    CONSTRAINT [PK_Physician] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Physician_AspNetUsers] FOREIGN KEY ([Id]) REFERENCES [dbo].[AspNetUsers] ([Id]),
    CONSTRAINT [FK_Physician_PhysicianSpeciality] FOREIGN KEY ([SpecialtyId]) REFERENCES [dbo].[PhysicianSpeciality] ([Id])
);











