CREATE TABLE [dbo].[ErrorLevel] (
    [ErrorLevelID] TINYINT        NOT NULL,
    [Name]         NVARCHAR (50)  NOT NULL,
    [ModifiedDate] DATETIME       CONSTRAINT [DF_ErrorLevel_ModifiedDate] DEFAULT (getdate()) NOT NULL,
    [ModifiedUser] NVARCHAR (256) CONSTRAINT [DF_ErrorLevel_ModifiedUser] DEFAULT (suser_name()) NULL,
    CONSTRAINT [PK_ErrorLevel] PRIMARY KEY CLUSTERED ([ErrorLevelID] ASC)
);

