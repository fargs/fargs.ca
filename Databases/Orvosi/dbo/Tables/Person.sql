CREATE TABLE [dbo].[Person] (
    [PersonID]     INT              IDENTITY (1, 1) NOT NULL,
    [ObjectGuid]   UNIQUEIDENTIFIER NOT NULL,
    [Title]        NVARCHAR (50)    NULL,
    [FirstName]    NVARCHAR (50)    NOT NULL,
    [LastName]     NVARCHAR (50)    NOT NULL,
    [CountryID]    SMALLINT         NULL,
    [PhoneNo]      NVARCHAR (256)   NULL,
    [FaxNo]        NVARCHAR (256)   NULL,
    [MobileNo]     NVARCHAR (256)   NULL,
    [Email]        NVARCHAR (256)   NULL,
    [AspNetUserID] INT              NULL,
    [ModifiedDate] SMALLDATETIME    CONSTRAINT [DF_Person_ModifiedDate] DEFAULT (getdate()) NOT NULL,
    [ModifiedUser] NVARCHAR (256)   CONSTRAINT [DF_Person_ModifiedUser] DEFAULT (suser_name()) NOT NULL,
    CONSTRAINT [PK_Person] PRIMARY KEY CLUSTERED ([PersonID] ASC)
);



