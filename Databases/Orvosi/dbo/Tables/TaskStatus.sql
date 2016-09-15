CREATE TABLE [dbo].[TaskStatus] (
    [Id]                       SMALLINT       NOT NULL,
    [Name]                     NVARCHAR (128) NOT NULL,
    [DependentPrecedence]      TINYINT        NOT NULL,
    [ServiceRequestPrecedence] TINYINT        NOT NULL,
    CONSTRAINT [PK_TaskStatus] PRIMARY KEY CLUSTERED ([Id] ASC)
);

