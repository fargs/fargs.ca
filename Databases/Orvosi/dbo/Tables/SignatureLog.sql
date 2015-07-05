CREATE TABLE [dbo].[SignatureLog] (
    [SignatureLogID] BIGINT           IDENTITY (1, 1) NOT NULL,
    [ObjectGuid]     UNIQUEIDENTIFIER NOT NULL,
    [SignedDate]     DATETIME         NOT NULL,
    [AspNetUserID]   INT              NOT NULL,
    [Reason]         NVARCHAR (MAX)   NULL,
    [CreatedDate]    DATETIME         NOT NULL,
    [CreatedUser]    NVARCHAR (255)   NOT NULL,
    [ModifiedDate]   DATETIME         NOT NULL,
    [ModifiedUser]   NVARCHAR (255)   NOT NULL,
    CONSTRAINT [PK_SignatureLog] PRIMARY KEY CLUSTERED ([SignatureLogID] ASC)
);

