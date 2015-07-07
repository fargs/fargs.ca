CREATE TABLE [dbo].[ServiceCategory] (
    [Id]           SMALLINT       IDENTITY (1, 1) NOT NULL,
    [Name]         NVARCHAR (128) NOT NULL,
    [Description]  NVARCHAR (MAX) NULL,
    [ModifiedDate] DATETIME       CONSTRAINT [DF_ServiceCategory_ModifiedDate] DEFAULT (getdate()) NOT NULL,
    [ModifiedUser] NVARCHAR (100) CONSTRAINT [DF_ServiceCategory_ModifiedUser] DEFAULT (suser_name()) NOT NULL,
    CONSTRAINT [PK_ServiceCategory] PRIMARY KEY CLUSTERED ([Id] ASC)
);

