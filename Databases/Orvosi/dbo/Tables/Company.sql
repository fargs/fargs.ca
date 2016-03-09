CREATE TABLE [dbo].[Company] (
    [Id]           SMALLINT         IDENTITY (1, 1) NOT NULL,
    [ObjectGuid]   UNIQUEIDENTIFIER CONSTRAINT [DF_Company_ObjectGuid] DEFAULT (newid()) NULL,
    [Name]         NVARCHAR (128)   NULL,
    [Code]         NVARCHAR (50)    NULL,
    [IsParent]     BIT              CONSTRAINT [DF_Company_IsParent] DEFAULT ((0)) NOT NULL,
    [ParentId]     INT              NULL,
    [LogoCssClass] NVARCHAR (50)    NULL,
    [BillingEmail] NVARCHAR (128)   NULL,
    [ReportsEmail] NVARCHAR (128)   NULL,
    [Phone]        NVARCHAR (50)    NULL,
    [ModifiedDate] DATETIME         CONSTRAINT [DF_Company_ModifiedDate] DEFAULT (getdate()) NOT NULL,
    [ModifiedUser] NVARCHAR (256)   CONSTRAINT [DF_Company_ModifiedUser] DEFAULT (suser_name()) NOT NULL,
    CONSTRAINT [PK_Company] PRIMARY KEY CLUSTERED ([Id] ASC)
);













