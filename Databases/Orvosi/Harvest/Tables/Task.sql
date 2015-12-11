CREATE TABLE [Harvest].[Task] (
    [Id]                BIGINT          IDENTITY (1, 1) NOT NULL,
    [CreatedAt]         DATETIME        NOT NULL,
    [UpdatedAt]         DATETIME        NOT NULL,
    [Name]              NVARCHAR (MAX)  NULL,
    [BillableByDefault] BIT             NOT NULL,
    [IsDefault]         BIT             NOT NULL,
    [DefaultHourlyRate] DECIMAL (18, 2) NULL,
    [Deactivated]       BIT             NOT NULL,
    [Billable]          BIT             NULL,
    [Project_Id]        BIGINT          NULL,
    CONSTRAINT [PK_Harvest.Task] PRIMARY KEY CLUSTERED ([Id] ASC)
);




GO
CREATE NONCLUSTERED INDEX [IX_Project_Id]
    ON [Harvest].[Task]([Project_Id] ASC);

