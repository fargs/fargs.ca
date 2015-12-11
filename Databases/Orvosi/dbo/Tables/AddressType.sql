CREATE TABLE [dbo].[AddressType] (
    [Id]           TINYINT        NOT NULL,
    [Name]         NVARCHAR (50)  NOT NULL,
    [ModifiedDate] DATETIME       CONSTRAINT [DF_AddressType_ModifiedDate] DEFAULT (getdate()) NOT NULL,
    [ModifiedUser] NVARCHAR (256) CONSTRAINT [DF_AddressType_ModifiedUser] DEFAULT (suser_name()) NULL,
    CONSTRAINT [PK_AddressType] PRIMARY KEY CLUSTERED ([Id] ASC)
);





