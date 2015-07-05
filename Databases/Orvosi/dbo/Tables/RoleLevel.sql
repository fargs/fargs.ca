CREATE TABLE [dbo].[RoleLevel] (
    [Id]                     INT             IDENTITY (1, 1) NOT NULL,
    [AspNetRoleId]           NVARCHAR (128)  NOT NULL,
    [Level]                  TINYINT         NOT NULL,
    [HourlyRate]             DECIMAL (18, 2) NULL,
    [SalesLeadCommission]    DECIMAL (18, 2) NULL,
    [SalesSupportCommission] DECIMAL (18, 2) NULL,
    [WeeklyPay]              AS              ([dbo].[CalculateWeeklyPay]([HourlyRate])),
    [BiWeeklyPay]            AS              ([dbo].[CalculateBiWeeklyPay]([HourlyRate])),
    [YearlyPay]              AS              ([dbo].[CalculateYearlyPay]([HourlyRate])),
    [ModifiedDate]           DATETIME        CONSTRAINT [DF_RoleLevel_ModifiedDate] DEFAULT (getdate()) NOT NULL,
    [ModifiedUser]           NVARCHAR (128)  CONSTRAINT [DF_RoleLevel_ModifiedUser] DEFAULT (suser_name()) NOT NULL,
    CONSTRAINT [PK_RoleLevel] PRIMARY KEY CLUSTERED ([Id] ASC)
);



