CREATE TABLE [dbo].[Physician_ServiceRequestTemplate] (
    [PhysicianId]              UNIQUEIDENTIFIER NOT NULL,
    [ServiceRequestTemplateId] SMALLINT         NOT NULL,
    [ServiceCategoryId]        SMALLINT         NULL,
    [ServiceId]                SMALLINT         NULL,
    [ModifiedDate]             DATETIME         NOT NULL,
    [ModifiedUser]             NVARCHAR (100)   NOT NULL,
    CONSTRAINT [PK_Physician_ServiceRequestTemplate] PRIMARY KEY CLUSTERED ([PhysicianId] ASC, [ServiceRequestTemplateId] ASC),
    CONSTRAINT [FK_Physician_ServiceRequestTemplate_ServiceRequestTemplate] FOREIGN KEY ([ServiceRequestTemplateId]) REFERENCES [dbo].[ServiceRequestTemplate] ([Id])
);

