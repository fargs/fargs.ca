CREATE TABLE [dbo].[ConfigurationType] (
    [Id]           SMALLINT       NOT NULL,
    [Name]         NVARCHAR (128) NOT NULL,
    [ShortName]    NVARCHAR (10)  NOT NULL,
    [Description]  NVARCHAR (MAX) NULL,
    [ModifiedDate] DATETIME       CONSTRAINT [DF_ConfigurationType_ModifiedDate] DEFAULT (getdate()) NOT NULL,
    [ModifiedUser] NVARCHAR (256) NOT NULL,
    CONSTRAINT [PK_ConfigurationType] PRIMARY KEY CLUSTERED ([Id] ASC)
);



