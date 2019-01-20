CREATE TABLE [dbo].[CancellationStatus] (
    [Id]        TINYINT        NOT NULL,
    [Name]      NVARCHAR (128) NOT NULL,
    [Code]      NVARCHAR (10)  NULL,
    [ColorCode] NVARCHAR (10)  NULL,
    CONSTRAINT [PK_CancellationStatus] PRIMARY KEY CLUSTERED ([Id] ASC)
);

