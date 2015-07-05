CREATE TABLE [dbo].[Company] (
    [Id]           INT            IDENTITY (1, 1) NOT NULL,
    [Name]         NVARCHAR (128) NULL,
    [ParentId]     INT            NULL,
    [ModifiedDate] DATETIME       CONSTRAINT [DF_Company_ModifiedDate] DEFAULT (getdate()) NOT NULL,
    [ModifiedUser] NVARCHAR (256) CONSTRAINT [DF_Company_ModifiedUser] DEFAULT (suser_name()) NOT NULL,
    CONSTRAINT [PK_Company] PRIMARY KEY CLUSTERED ([Id] ASC)
);



