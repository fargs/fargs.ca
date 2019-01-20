CREATE TABLE [dbo].[MedicolegalType] (
    [Id]        TINYINT        NOT NULL,
    [Name]      NVARCHAR (128) NOT NULL,
    [Code]      NVARCHAR (10)  NOT NULL,
    [ColorCode] NCHAR (10)     NOT NULL,
    CONSTRAINT [PK_MedicolegalType] PRIMARY KEY CLUSTERED ([Id] ASC)
);

