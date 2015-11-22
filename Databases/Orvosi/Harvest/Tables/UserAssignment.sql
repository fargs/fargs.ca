CREATE TABLE [Harvest].[UserAssignment] (
    [Id]               BIGINT          IDENTITY (1, 1) NOT NULL,
    [CreatedAt]        DATETIME        NOT NULL,
    [UpdatedAt]        DATETIME        NOT NULL,
    [UserId]           BIGINT          NOT NULL,
    [ProjectId]        BIGINT          NOT NULL,
    [IsProjectManager] BIT             NOT NULL,
    [Deactivated]      BIT             NOT NULL,
    [HourlyRate]       DECIMAL (18, 2) NULL,
    [Budget]           DECIMAL (18, 2) NULL,
    [Estimate]         DECIMAL (18, 2) NULL,
    CONSTRAINT [PK_Harvest.UserAssignment] PRIMARY KEY CLUSTERED ([Id] ASC)
);

