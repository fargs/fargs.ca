CREATE TABLE [dbo].[AspNetUserLogins] (
    [LoginProvider] NVARCHAR (128)   NOT NULL,
    [ProviderKey]   NVARCHAR (128)   NOT NULL,
    [UserId]        UNIQUEIDENTIFIER NOT NULL,
    [ModifiedDate]  DATETIME         CONSTRAINT [DF_AspNetUserLogins_ModifiedDate] DEFAULT (getdate()) NOT NULL,
    [ModifiedUser]  NVARCHAR (256)   CONSTRAINT [DF_AspNetUserLogins_ModifiedUser] DEFAULT (suser_name()) NULL,
    CONSTRAINT [PK_dbo.AspNetUserLogins] PRIMARY KEY CLUSTERED ([LoginProvider] ASC, [ProviderKey] ASC, [UserId] ASC),
    CONSTRAINT [FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE
);








GO
CREATE NONCLUSTERED INDEX [IX_UserId]
    ON [dbo].[AspNetUserLogins]([UserId] ASC);

