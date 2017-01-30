CREATE TABLE [dbo].[Feature] (
    [Id]           SMALLINT       NOT NULL,
    [Name]         NVARCHAR (255) NOT NULL,
    [Description]  NVARCHAR (MAX) NULL,
    [IsActive]     BIT            CONSTRAINT [DF_Feature_IsActive] DEFAULT ((1)) NOT NULL,
    [ModifiedBy]   NVARCHAR (100) CONSTRAINT [DF_Feature_ModifiedBy] DEFAULT (suser_name()) NOT NULL,
    [ModifiedDate] DATETIME       CONSTRAINT [DF_Feature_ModifiedDate] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_Feature_1] PRIMARY KEY CLUSTERED ([Id] ASC)
);

