CREATE TABLE [dbo].[AvailableDay] (
    [Id]           SMALLINT       IDENTITY (1, 1) NOT NULL,
    [PhysicianId]  NVARCHAR (128) NOT NULL,
    [Day]          DATE           NOT NULL,
    [CompanyId]    SMALLINT       NULL,
    [LocationId]   SMALLINT       NULL,
    [ModifiedDate] DATETIME       CONSTRAINT [DF_AvailableDay_ModifiedDate] DEFAULT (getdate()) NOT NULL,
    [ModifiedUser] NVARCHAR (100) CONSTRAINT [DF_AvailableDay_ModifiedUser] DEFAULT (suser_name()) NOT NULL,
    CONSTRAINT [PK_AvailableDay] PRIMARY KEY CLUSTERED ([Id] ASC)
);

