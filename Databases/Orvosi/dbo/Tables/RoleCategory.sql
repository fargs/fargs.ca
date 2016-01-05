CREATE TABLE [dbo].[RoleCategory] (
    [Id]           SMALLINT       NOT NULL,
    [Name]         NVARCHAR (128) NOT NULL,
    [ModifiedDate] DATETIME       CONSTRAINT [DF_RoleCategory_ModifiedDate] DEFAULT (getdate()) NOT NULL,
    [ModifiedUser] NVARCHAR (100) CONSTRAINT [DF_RoleCategory_ModifiedUser] DEFAULT (suser_name()) NOT NULL,
    CONSTRAINT [PK_RoleCategory] PRIMARY KEY CLUSTERED ([Id] ASC)
);



