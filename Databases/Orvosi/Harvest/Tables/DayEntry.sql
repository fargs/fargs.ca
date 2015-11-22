CREATE TABLE [Harvest].[DayEntry] (
    [Id]               BIGINT          IDENTITY (1, 1) NOT NULL,
    [UpdatedAt]        DATETIME        NOT NULL,
    [CreatedAt]        DATETIME        NOT NULL,
    [Notes]            NVARCHAR (MAX)  NULL,
    [SpentAt]          DATETIME        NOT NULL,
    [Hours]            DECIMAL (18, 2) NOT NULL,
    [UserId]           BIGINT          NOT NULL,
    [ProjectId]        BIGINT          NOT NULL,
    [TaskId]           BIGINT          NOT NULL,
    [AdjustmentRecord] BIT             NOT NULL,
    [IsBilled]         BIT             NOT NULL,
    [IsClosed]         BIT             NOT NULL,
    [TimerStartedAt]   DATETIME        NULL,
    [StartedAt]        DATETIME        NULL,
    [EndedAt]          DATETIME        NULL,
    [Task]             NVARCHAR (MAX)  NULL,
    [Project]          NVARCHAR (MAX)  NULL,
    CONSTRAINT [PK_Harvest.DayEntry] PRIMARY KEY CLUSTERED ([Id] ASC)
);

