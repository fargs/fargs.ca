CREATE TABLE [dbo].[Province] (
    [ProvinceID]     SMALLINT       IDENTITY (1, 1) NOT NULL,
    [CountryID]      SMALLINT       NOT NULL,
    [ProvinceName]   NVARCHAR (100) NULL,
    [ProvinceCode]   NVARCHAR (50)  NOT NULL,
    [ProvinceTypeID] TINYINT        NULL,
    [ModifiedDate]   DATETIME       CONSTRAINT [DF_Province_ModifiedDate] DEFAULT (getdate()) NOT NULL,
    [ModifiedUser]   NVARCHAR (256) CONSTRAINT [DF_Province_ModifiedUser] DEFAULT (suser_name()) NULL,
    CONSTRAINT [PK_Province] PRIMARY KEY CLUSTERED ([ProvinceID] ASC),
    CONSTRAINT [IX_Province_ProvinceCode] UNIQUE NONCLUSTERED ([ProvinceCode] ASC, [CountryID] ASC)
);



