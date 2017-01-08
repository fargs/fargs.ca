CREATE TABLE [dbo].[ServiceRequestTemplate] (
    [Id]           SMALLINT       IDENTITY (1, 1) NOT NULL,
    [Name]         NVARCHAR (128) NOT NULL,
    [ModifiedDate] DATETIME       CONSTRAINT [DF_ServiceRequestTemplate_ModifiedDate] DEFAULT (getdate()) NOT NULL,
    [ModifiedUser] NVARCHAR (100) CONSTRAINT [DF_ServiceRequestTemplate_ModifiedUser] DEFAULT (suser_name()) NOT NULL,
    [IsDefault]    BIT            CONSTRAINT [DF_ServiceRequestTemplate_IsDefault] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_ServiceRequestTemplate] PRIMARY KEY CLUSTERED ([Id] ASC)
);







