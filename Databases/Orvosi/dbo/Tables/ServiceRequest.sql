CREATE TABLE [dbo].[ServiceRequest] (
    [Id]                 INT              IDENTITY (1, 1) NOT NULL,
    [ObjectGuid]         UNIQUEIDENTIFIER CONSTRAINT [DF__ServiceRe__Objec__7BB05806] DEFAULT (newid()) NOT NULL,
    [CompanyReferenceId] NVARCHAR (128)   NULL,
    [ClaimantName]       NVARCHAR (128)   NULL,
    [ServiceCatalogueId] SMALLINT         NULL,
    [HarvestProjectId]   BIGINT           NULL,
    [Title]              NVARCHAR (256)   NULL,
    [Body]               NVARCHAR (MAX)   NULL,
    [AddressId]          INT              NULL,
    [RequestedDate]      DATETIME         NULL,
    [RequestedBy]        UNIQUEIDENTIFIER NULL,
    [CancelledDate]      DATETIME         NULL,
    [StatusId]           TINYINT          CONSTRAINT [DF_ServiceRequest_StatusId] DEFAULT ((10)) NULL,
    [AvailableSlotId]    SMALLINT         NULL,
    [AppointmentDate]    DATE             NULL,
    [StartTime]          TIME (7)         NULL,
    [EndTime]            TIME (7)         NULL,
    [DueDate]            DATE             NULL,
    [CaseCoordinatorId]  UNIQUEIDENTIFIER NULL,
    [IntakeAssistantId]  UNIQUEIDENTIFIER NULL,
    [DocumentReviewerId] UNIQUEIDENTIFIER NULL,
    [Price]              DECIMAL (18, 2)  NULL,
    [InvoiceId]          SMALLINT         NULL,
    [DocumentFolderLink] NVARCHAR (2000)  NULL,
    [CompanyId]          SMALLINT         NULL,
    [ModifiedDate]       DATETIME         CONSTRAINT [DF_ServiceRequest_ModifiedDate] DEFAULT (getdate()) NOT NULL,
    [ModifiedUser]       NVARCHAR (100)   CONSTRAINT [DF_ServiceRequest_ModifiedUser] DEFAULT (suser_name()) NOT NULL,
    CONSTRAINT [PK_ServiceRequest] PRIMARY KEY CLUSTERED ([Id] ASC)
);







