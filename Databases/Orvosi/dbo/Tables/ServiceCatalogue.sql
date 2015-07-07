CREATE TABLE [dbo].[ServiceCatalogue] (
    [Id]           SMALLINT        IDENTITY (1, 1) NOT NULL,
    [DoctorId]     NVARCHAR (128)  NULL,
    [ServiceId]    SMALLINT        NULL,
    [CompanyId]    SMALLINT        NULL,
    [Rate]         DECIMAL (18, 2) NULL,
    [ModifiedDate] DATETIME        CONSTRAINT [DF_ServiceCatalogue_ModifiedDate] DEFAULT (getdate()) NOT NULL,
    [ModifiedUser] NVARCHAR (100)  CONSTRAINT [DF_ServiceCatalogue_ModifiedUser] DEFAULT (suser_name()) NOT NULL,
    CONSTRAINT [PK_ServiceCatalogue] PRIMARY KEY CLUSTERED ([Id] ASC)
);

