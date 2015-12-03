CREATE TABLE [dbo].[LookupItem] (
    [Id]           SMALLINT       IDENTITY (1, 1) NOT NULL,
    [LookupId]     SMALLINT       NOT NULL,
    [Text]         NVARCHAR (128) NOT NULL,
    [Value]        SMALLINT       NOT NULL,
    [ModifiedDate] DATETIME       CONSTRAINT [DF_LookupItem_ModifiedDate] DEFAULT (getdate()) NOT NULL,
    [ModifiedUser] NVARCHAR (100) CONSTRAINT [DF_LookupItem_ModifiedUser] DEFAULT (suser_name()) NOT NULL,
    CONSTRAINT [PK_LookupItem] PRIMARY KEY CLUSTERED ([Id] ASC)
);

