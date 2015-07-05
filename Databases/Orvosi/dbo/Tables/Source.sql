CREATE TABLE [dbo].[Source] (
    [SourceID]     TINYINT        NOT NULL,
    [Name]         NVARCHAR (50)  NOT NULL,
    [ModifiedDate] DATETIME       CONSTRAINT [DF_Source_ModifiedDate] DEFAULT (getdate()) NOT NULL,
    [ModifiedUser] NVARCHAR (256) CONSTRAINT [DF_Source_ModifiedUser] DEFAULT (suser_name()) NULL,
    CONSTRAINT [PK_Source] PRIMARY KEY CLUSTERED ([SourceID] ASC)
);

