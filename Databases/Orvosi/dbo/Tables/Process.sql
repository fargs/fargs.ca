CREATE TABLE [dbo].[Process] (
    [Id]                SMALLINT            IDENTITY (1, 1) NOT NULL,
    [Name]              NVARCHAR (128)      NOT NULL,
    [AccountableRoleId] NVARCHAR (128)      NULL,
    [Sequence]          [sys].[hierarchyid] NULL,
    [ModifiedDate]      DATETIME            CONSTRAINT [DF_Process_ModifiedDate] DEFAULT (getdate()) NOT NULL,
    [ModifiedUser]      NVARCHAR (100)      CONSTRAINT [DF_Process_ModifiedUser] DEFAULT (suser_name()) NOT NULL,
    CONSTRAINT [PK_Process] PRIMARY KEY CLUSTERED ([Id] ASC)
);



