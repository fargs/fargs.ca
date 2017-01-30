CREATE TABLE [dbo].[AspNetRolesFeature] (
    [Id]            BIGINT           IDENTITY (1, 1) NOT NULL,
    [AspNetRolesId] UNIQUEIDENTIFIER NOT NULL,
    [FeatureId]     SMALLINT         NOT NULL,
    [IsActive]      BIT              CONSTRAINT [DF_AspNetRolesFeature_IsActive] DEFAULT ((1)) NOT NULL,
    [ModifiedBy]    NVARCHAR (100)   CONSTRAINT [DF_AspNetRolesFeature_ModifiedBy] DEFAULT (suser_name()) NOT NULL,
    [ModifiedDate]  DATETIME         CONSTRAINT [DF_AspNetRolesFeature_ModifiedDate] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_AspNetRolesFeature] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_AspNetRolesFeature_AspNetRoles] FOREIGN KEY ([AspNetRolesId]) REFERENCES [dbo].[AspNetRoles] ([Id]),
    CONSTRAINT [FK_AspNetRolesFeature_Feature] FOREIGN KEY ([FeatureId]) REFERENCES [dbo].[Feature] ([Id])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_AspNetRolesFeature]
    ON [dbo].[AspNetRolesFeature]([Id] ASC);

