CREATE TABLE [dbo].[TaskPhase] (
    [Id]       TINYINT      NOT NULL,
    [Name]     VARCHAR (50) NOT NULL,
    [Sequence] TINYINT      NOT NULL,
    CONSTRAINT [PK_TaskPhase] PRIMARY KEY CLUSTERED ([Id] ASC)
);

