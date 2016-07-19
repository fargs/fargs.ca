CREATE TABLE [dbo].[AspNetUserClaims] (
    [Id]           INT              IDENTITY (1, 1) NOT NULL,
    [UserId]       UNIQUEIDENTIFIER NOT NULL,
    [ClaimType]    NVARCHAR (MAX)   NULL,
    [ClaimValue]   NVARCHAR (MAX)   NULL,
    [ModifiedDate] DATETIME         CONSTRAINT [DF_AspNetUserClaims_ModifiedDate] DEFAULT (getdate()) NOT NULL,
    [ModifiedUser] NVARCHAR (256)   CONSTRAINT [DF_AspNetUserClaims_ModifiedUser] DEFAULT (suser_name()) NULL,
    CONSTRAINT [PK_dbo.AspNetUserClaims] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.AspNetUserClaims_dbo.AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE
);








GO
CREATE NONCLUSTERED INDEX [IX_UserId]
    ON [dbo].[AspNetUserClaims]([UserId] ASC);

