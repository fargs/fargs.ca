CREATE TABLE [dbo].[City] (
    [Id]           SMALLINT       IDENTITY (1, 1) NOT NULL,
    [Name]         NVARCHAR (100) NOT NULL,
    [Code]         NVARCHAR (50)  NOT NULL,
    [ProvinceId]   SMALLINT       NOT NULL,
    [ModifiedDate] SMALLDATETIME  CONSTRAINT [DF_City_ModifiedDate] DEFAULT (getdate()) NOT NULL,
    [ModifiedUser] NVARCHAR (256) CONSTRAINT [DF_City_ModifiedUser] DEFAULT (suser_name()) NOT NULL,
    CONSTRAINT [PK_City] PRIMARY KEY CLUSTERED ([Id] ASC)
);

