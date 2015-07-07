CREATE TABLE [dbo].[JobTask] (
    [Id]                        INT            IDENTITY (1, 1) NOT NULL,
    [TaskId]                    SMALLINT       NULL,
    [JobId]                     INT            NULL,
    [DurationFromDueDateInDays] SMALLINT       NULL,
    [DueDate]                   DATE           NULL,
    [StartTime]                 TIME (7)       NULL,
    [EndTime]                   TIME (7)       NULL,
    [ResponsibleRoleId]         NVARCHAR (128) NULL,
    [EmployeeId]                NVARCHAR (128) NULL,
    [RoleLevelId]               INT            NULL,
    [IsBillable]                BIT            NULL,
    [BillableHourCategoryId]    SMALLINT       NULL,
    [IsMilestone]               BIT            NULL,
    [StatusId]                  TINYINT        NULL,
    [ModifiedDate]              DATETIME       CONSTRAINT [DF_JobTask_ModifiedDate] DEFAULT (getdate()) NOT NULL,
    [ModifiedUser]              NVARCHAR (100) CONSTRAINT [DF_JobTask_ModifiedUser] DEFAULT (suser_name()) NOT NULL,
    CONSTRAINT [PK_JobTask] PRIMARY KEY CLUSTERED ([Id] ASC)
);

