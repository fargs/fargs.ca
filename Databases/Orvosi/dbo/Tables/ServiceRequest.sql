CREATE TABLE [dbo].[ServiceRequest] (
    [Id]                                 INT              IDENTITY (1, 1) NOT NULL,
    [ObjectGuid]                         UNIQUEIDENTIFIER CONSTRAINT [DF__ServiceRe__Objec__7BB05806] DEFAULT (newid()) NOT NULL,
    [CompanyReferenceId]                 NVARCHAR (128)   NULL,
    [ClaimantName]                       NVARCHAR (128)   NULL,
    [ServiceCatalogueId]                 SMALLINT         NULL,
    [HarvestProjectId]                   BIGINT           NULL,
    [Title]                              NVARCHAR (256)   NULL,
    [Body]                               NVARCHAR (MAX)   NULL,
    [AddressId]                          INT              NULL,
    [RequestedDate]                      DATETIME         NULL,
    [RequestedBy]                        UNIQUEIDENTIFIER NULL,
    [CancelledDate]                      DATETIME         NULL,
    [AvailableSlotId]                    SMALLINT         NULL,
    [AppointmentDate]                    DATE             NULL,
    [StartTime]                          TIME (7)         NULL,
    [EndTime]                            TIME (7)         NULL,
    [DueDate]                            DATE             NULL,
    [CaseCoordinatorId]                  UNIQUEIDENTIFIER NULL,
    [IntakeAssistantId]                  UNIQUEIDENTIFIER NULL,
    [DocumentReviewerId]                 UNIQUEIDENTIFIER NULL,
    [Price]                              DECIMAL (18, 2)  NULL,
    [Notes]                              NVARCHAR (2000)  NULL,
    [InvoiceItemId]                      INT              NULL,
    [DocumentFolderLink]                 NVARCHAR (2000)  NULL,
    [CompanyId]                          SMALLINT         NULL,
    [IsNoShow]                           BIT              CONSTRAINT [DF_ServiceRequest_IsNoShow] DEFAULT ((0)) NOT NULL,
    [IsLateCancellation]                 BIT              CONSTRAINT [DF_ServiceRequest_IsLateCancellation] DEFAULT ((0)) NOT NULL,
    [ModifiedDate]                       DATETIME         CONSTRAINT [DF_ServiceRequest_ModifiedDate] DEFAULT (getdate()) NOT NULL,
    [ModifiedUser]                       NVARCHAR (100)   CONSTRAINT [DF_ServiceRequest_ModifiedUser] DEFAULT (suser_name()) NOT NULL,
    [NoShowRate]                         DECIMAL (18, 2)  NULL,
    [LateCancellationRate]               DECIMAL (18, 2)  NULL,
    [PhysicianId]                        UNIQUEIDENTIFIER NOT NULL,
    [ServiceId]                          SMALLINT         NULL,
    [LocationId]                         INT              NULL,
    [ServiceCataloguePrice]              DECIMAL (18, 2)  NULL,
    [BoxCaseFolderId]                    NVARCHAR (128)   NULL,
    [IntakeAssistantBoxCollaborationId]  NVARCHAR (50)    NULL,
    [DocumentReviewerBoxCollaborationId] NVARCHAR (50)    NULL,
    CONSTRAINT [PK_ServiceRequest] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ServiceRequest_Address] FOREIGN KEY ([AddressId]) REFERENCES [dbo].[Address] ([Id]),
    CONSTRAINT [FK_ServiceRequest_AvailableSlot] FOREIGN KEY ([AvailableSlotId]) REFERENCES [dbo].[AvailableSlot] ([Id]),
    CONSTRAINT [FK_ServiceRequest_CaseCoordinator] FOREIGN KEY ([CaseCoordinatorId]) REFERENCES [dbo].[AspNetUsers] ([Id]),
    CONSTRAINT [FK_ServiceRequest_Company] FOREIGN KEY ([CompanyId]) REFERENCES [dbo].[Company] ([Id]),
    CONSTRAINT [FK_ServiceRequest_DocumentReviewer] FOREIGN KEY ([DocumentReviewerId]) REFERENCES [dbo].[AspNetUsers] ([Id]),
    CONSTRAINT [FK_ServiceRequest_IntakeAssistant] FOREIGN KEY ([IntakeAssistantId]) REFERENCES [dbo].[AspNetUsers] ([Id]),
    CONSTRAINT [FK_ServiceRequest_Physician] FOREIGN KEY ([PhysicianId]) REFERENCES [dbo].[Physician] ([Id]),
    CONSTRAINT [FK_ServiceRequest_Service] FOREIGN KEY ([ServiceId]) REFERENCES [dbo].[Service] ([Id])
);





















