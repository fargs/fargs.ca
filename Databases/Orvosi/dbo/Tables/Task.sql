CREATE TABLE [dbo].[Task] (
    [Id]                SMALLINT        IDENTITY (1, 1) NOT NULL,
    [Name]              NVARCHAR (128)  NOT NULL,
    [ResponsibleRoleId] NVARCHAR (128)  NULL,
    [IsBillable]        BIT             CONSTRAINT [DF_Task_IsBillable] DEFAULT ((0)) NULL,
    [BillableHoursId]   DECIMAL (18, 2) NULL,
    [Sequence]          SMALLINT        NULL,
    [IsMilestone]       BIT             CONSTRAINT [DF_Task_IsMilestone] DEFAULT ((0)) NULL,
    [ModifiedDate]      DATETIME        CONSTRAINT [DF_Task_ModifiedDate] DEFAULT (getdate()) NOT NULL,
    [ModifiedUser]      NVARCHAR (100)  CONSTRAINT [DF_Task_ModifiedUser] DEFAULT (suser_name()) NOT NULL,
    CONSTRAINT [PK_Task] PRIMARY KEY CLUSTERED ([Id] ASC)
);





