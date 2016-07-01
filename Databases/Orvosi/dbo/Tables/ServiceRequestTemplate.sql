CREATE TABLE [dbo].[ServiceRequestTemplate] (
    [Id]                 SMALLINT       IDENTITY (1, 1) NOT NULL,
    [ServiceCatelogueId] SMALLINT       NULL,
    [PhysicianId]        NVARCHAR (128) NULL,
    [ServiceCategoryId]  SMALLINT       NULL,
    [ServiceId]          SMALLINT       NULL,
    [CompanyId]          SMALLINT       NULL,
    [LocationId]         SMALLINT       NULL,
    [ModifiedDate]       DATETIME       CONSTRAINT [DF_ServiceRequestTemplate_ModifiedDate] DEFAULT (getdate()) NOT NULL,
    [ModifiedUser]       NVARCHAR (100) CONSTRAINT [DF_ServiceRequestTemplate_ModifiedUser] DEFAULT (suser_name()) NOT NULL,
    CONSTRAINT [PK_ServiceRequestTemplate] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [CK_ServiceRequestTemplate] CHECK ([ServiceId] IS NULL AND [ServiceCategoryId] IS NOT NULL OR ([ServiceId] IS NOT NULL OR [ServiceCategoryId] IS NULL)),
    CONSTRAINT [FK_ServiceRequestTemplate_Service] FOREIGN KEY ([ServiceId]) REFERENCES [dbo].[Service] ([Id]),
    CONSTRAINT [FK_ServiceRequestTemplate_ServiceCategory] FOREIGN KEY ([ServiceCategoryId]) REFERENCES [dbo].[ServiceCategory] ([Id])
);



