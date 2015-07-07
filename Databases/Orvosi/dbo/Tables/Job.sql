CREATE TABLE [dbo].[Job] (
    [Id]           INT             IDENTITY (1, 1) NOT NULL,
    [FileId]       SMALLINT        NULL,
    [ServiceId]    INT             NULL,
    [DueDate]      DATE            NULL,
    [StartTime]    TIME (7)        NULL,
    [EndTime]      TIME (7)        NULL,
    [Price]        DECIMAL (18, 2) NULL,
    [ModifiedDate] DATETIME        CONSTRAINT [DF_Assessment_ModifiedDate] DEFAULT (getdate()) NOT NULL,
    [ModifiedUser] NVARCHAR (100)  CONSTRAINT [DF_Assessment_ModifiedUser] DEFAULT (suser_name()) NOT NULL,
    CONSTRAINT [PK_Assessment] PRIMARY KEY CLUSTERED ([Id] ASC)
);

