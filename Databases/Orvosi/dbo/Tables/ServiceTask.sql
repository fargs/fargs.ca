CREATE TABLE [dbo].[ServiceTask] (
    [Id]              SMALLINT        IDENTITY (1, 1) NOT NULL,
    [ServiceId]       INT             NOT NULL,
    [TaskId]          SMALLINT        NOT NULL,
    [IsBillable]      BIT             CONSTRAINT [DF_ServiceTask_IsBillable] DEFAULT ((0)) NULL,
    [BillableHoursId] DECIMAL (18, 2) NULL,
    [Sequence]        SMALLINT        NULL,
    [ModifiedDate]    DATETIME        CONSTRAINT [DF_ServiceTask_ModifiedDate] DEFAULT (getdate()) NOT NULL,
    [ModifiedUser]    NVARCHAR (100)  CONSTRAINT [DF_ServiceTask_ModifiedUser] DEFAULT (suser_name()) NOT NULL,
    CONSTRAINT [PK_ServiceTask] PRIMARY KEY CLUSTERED ([Id] ASC)
);

