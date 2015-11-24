CREATE TABLE [dbo].[SpecialRequest] (
    [Id]               SMALLINT         IDENTITY (1, 1) NOT NULL,
    [ObjectGuid]       UNIQUEIDENTIFIER CONSTRAINT [DF_SpecialRequest_ObjectGuid] DEFAULT (newid()) NULL,
    [PhysicianId]      NVARCHAR (128)   NULL,
    [ServiceId]        INT              NULL,
    [Timeframe]        NVARCHAR (128)   NULL,
    [AdditionalNotes]  NVARCHAR (2000)  NULL,
    [ModifiedDate]     DATETIME         CONSTRAINT [DF_SpecialRequest_ModifiedDate] DEFAULT (getdate()) NOT NULL,
    [ModifiedUserName] NVARCHAR (128)   CONSTRAINT [DF_SpecialRequest_ModifiedUser] DEFAULT (suser_name()) NOT NULL,
    [ModifiedUserId]   NVARCHAR (128)   CONSTRAINT [DF_SpecialRequest_ModifiedUserId] DEFAULT (suser_name()) NOT NULL,
    CONSTRAINT [PK_SpecialRequest] PRIMARY KEY CLUSTERED ([Id] ASC)
);

