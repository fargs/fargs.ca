CREATE TABLE [dbo].[CancellationType] (
    [Id]           SMALLINT       IDENTITY (1, 1) NOT NULL,
    [Name]         NVARCHAR (128) NOT NULL,
    [Description]  NVARCHAR (MAX) NULL,
    [Code]         NCHAR (10)     NULL,
    [ModifiedDate] DATETIME       CONSTRAINT [DF_CancellationType_ModifiedDate] DEFAULT (getdate()) NOT NULL,
    [ModifiedUser] NVARCHAR (100) CONSTRAINT [DF_CancellationType_ModifiedUser] DEFAULT (suser_name()) NOT NULL,
    CONSTRAINT [PK_CancellationType] PRIMARY KEY CLUSTERED ([Id] ASC)
);

