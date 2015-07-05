CREATE TABLE [dbo].[Timeframe] (
    [PK_Date]       DATE          NOT NULL,
    [Sequence]      SMALLINT      NULL,
    [Year]          SMALLINT      NULL,
    [Month]         NVARCHAR (50) NULL,
    [Month_Of_Year] SMALLINT      NULL,
    [Day_Of_Month]  SMALLINT      NULL,
    [Week_Of_Year]  SMALLINT      NULL,
    CONSTRAINT [PK_Timeframe] PRIMARY KEY CLUSTERED ([PK_Date] ASC)
);

