CREATE TABLE [dbo].[ServiceRequest] (
    [Id]                 INT              IDENTITY (1, 1) NOT NULL,
    [ObjectGuid]         UNIQUEIDENTIFIER CONSTRAINT [DF__ServiceRe__Objec__7BB05806] DEFAULT (newid()) NOT NULL,
    [CompanyReferenceId] NVARCHAR (128)   NULL,
    [ServiceCatalogueId] SMALLINT         NULL,
    [HarvestProjectId]   BIGINT           NULL,
    [Title]              NVARCHAR (256)   NULL,
    [Body]               NVARCHAR (MAX)   NULL,
    [AddressId]          INT              NULL,
    [RequestedDate]      DATETIME         NOT NULL,
    [RequestedBy]        UNIQUEIDENTIFIER NOT NULL,
    [CancelledDate]      DATETIME         NULL,
    [AssignedTo]         UNIQUEIDENTIFIER NULL,
    [StatusId]           TINYINT          CONSTRAINT [DF_ServiceRequest_StatusId] DEFAULT ((1)) NULL,
    [DueDate]            DATE             NULL,
    [StartTime]          TIME (7)         NULL,
    [EndTime]            TIME (7)         NULL,
    [Price]              DECIMAL (18, 2)  NULL,
    [ModifiedDate]       DATETIME         CONSTRAINT [DF_ServiceRequest_ModifiedDate] DEFAULT (getdate()) NOT NULL,
    [ModifiedUser]       NVARCHAR (100)   CONSTRAINT [DF_ServiceRequest_ModifiedUser] DEFAULT (suser_name()) NOT NULL,
    CONSTRAINT [PK_ServiceRequest] PRIMARY KEY CLUSTERED ([Id] ASC)
);

