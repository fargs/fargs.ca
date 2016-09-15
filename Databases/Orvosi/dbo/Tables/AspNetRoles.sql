CREATE TABLE [dbo].[AspNetRoles] (
    [Id]             UNIQUEIDENTIFIER NOT NULL,
    [Name]           NVARCHAR (256)   NOT NULL,
    [RoleCategoryId] TINYINT          NULL,
    [ModifiedDate]   DATETIME         CONSTRAINT [DF_AspNetRoles_ModifiedDate] DEFAULT (getdate()) NOT NULL,
    [ModifiedUser]   NVARCHAR (256)   CONSTRAINT [DF_AspNetRoles_ModifiedUser] DEFAULT (suser_name()) NULL,
    CONSTRAINT [PK_dbo.AspNetRoles] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_AspNetRoles_RoleCategory] FOREIGN KEY ([RoleCategoryId]) REFERENCES [dbo].[RoleCategory] ([Id])
);












GO
CREATE UNIQUE NONCLUSTERED INDEX [RoleNameIndex]
    ON [dbo].[AspNetRoles]([Name] ASC);

