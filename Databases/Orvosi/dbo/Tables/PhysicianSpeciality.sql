CREATE TABLE [dbo].[PhysicianSpeciality] (
    [Id]           TINYINT        NOT NULL,
    [Name]         NVARCHAR (128) NOT NULL,
    [Description]  NVARCHAR (MAX) NULL,
    [ModifiedDate] DATETIME       CONSTRAINT [DF_PhysicianSpeciality_ModifiedDate] DEFAULT (getdate()) NOT NULL,
    [ModifiedUser] NVARCHAR (100) CONSTRAINT [DF_PhysicianSpeciality_ModifiedUser] DEFAULT (suser_name()) NOT NULL,
    CONSTRAINT [PK_PhysicianSpeciality] PRIMARY KEY CLUSTERED ([Id] ASC)
);

