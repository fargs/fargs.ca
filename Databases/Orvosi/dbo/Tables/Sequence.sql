CREATE TABLE [dbo].[Sequence] (
    [Id]           INT            IDENTITY (1, 1) NOT NULL,
    [Sequence]     INT            NULL,
    [ModifiedDate] DATETIME       CONSTRAINT [DF_Sequence_ModifiedDate] DEFAULT (getdate()) NOT NULL,
    [ModifiedUser] NVARCHAR (256) CONSTRAINT [DF_Sequence_ModifiedUser] DEFAULT (suser_name()) NULL
);



