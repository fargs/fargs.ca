CREATE TABLE [Harvest].[TaskAssignment] (
    [Id]          BIGINT          IDENTITY (1, 1) NOT NULL,
    [CreatedAt]   DATETIME        NOT NULL,
    [UpdatedAt]   DATETIME        NOT NULL,
    [TaskId]      BIGINT          NOT NULL,
    [ProjectId]   BIGINT          NOT NULL,
    [Billable]    BIT             NOT NULL,
    [Deactivated] BIT             NOT NULL,
    [HourlyRate]  DECIMAL (18, 2) NOT NULL,
    [Budget]      DECIMAL (18, 2) NULL,
    [Estimate]    DECIMAL (18, 2) NULL,
    CONSTRAINT [PK_Harvest.TaskAssignment] PRIMARY KEY CLUSTERED ([Id] ASC)
);

