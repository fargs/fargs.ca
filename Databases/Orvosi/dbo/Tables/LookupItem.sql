CREATE TABLE [dbo].[LookupItem] (
    [Id]           SMALLINT       NOT NULL,
    [LookupId]     SMALLINT       NOT NULL,
    [Text]         NVARCHAR (128) NOT NULL,
    [Value]        SMALLINT       NULL,
    [ModifiedDate] DATETIME       CONSTRAINT [DF_LookupItem_ModifiedDate] DEFAULT (getdate()) NOT NULL,
    [ModifiedUser] NVARCHAR (100) CONSTRAINT [DF_LookupItem_ModifiedUser] DEFAULT (suser_name()) NOT NULL,
    [ShortText]    NVARCHAR (50)  NULL,
    CONSTRAINT [PK_LookupItem] PRIMARY KEY CLUSTERED ([Id] ASC)
);







