CREATE TABLE [dbo].[AvailableDay] (
    [Id]           SMALLINT         IDENTITY (1, 1) NOT NULL,
    [PhysicianId]  UNIQUEIDENTIFIER NOT NULL,
    [Day]          DATE             NOT NULL,
    [IsPrebook]    BIT              CONSTRAINT [DF_AvailableDay_IsPrebook] DEFAULT ((0)) NOT NULL,
    [CompanyId]    SMALLINT         NULL,
    [LocationId]   INT              NULL,
    [ModifiedDate] DATETIME         CONSTRAINT [DF_AvailableDay_ModifiedDate] DEFAULT (getdate()) NOT NULL,
    [ModifiedUser] NVARCHAR (100)   CONSTRAINT [DF_AvailableDay_ModifiedUser] DEFAULT (suser_name()) NOT NULL,
    CONSTRAINT [PK_AvailableDay] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_AvailableDay_AvailableDay] FOREIGN KEY ([LocationId]) REFERENCES [dbo].[Address] ([Id]),
    CONSTRAINT [FK_AvailableDay_Company] FOREIGN KEY ([CompanyId]) REFERENCES [dbo].[Company] ([Id])
);







