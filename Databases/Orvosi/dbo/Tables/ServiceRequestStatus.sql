CREATE TABLE [dbo].[ServiceRequestStatus] (
    [Id]        SMALLINT       NOT NULL,
    [Name]      NVARCHAR (128) NOT NULL,
    [Code]      NVARCHAR (10)  NULL,
    [ColorCode] NVARCHAR (10)  NULL,
    CONSTRAINT [PK_ServiceRequestStatus] PRIMARY KEY CLUSTERED ([Id] ASC)
);

