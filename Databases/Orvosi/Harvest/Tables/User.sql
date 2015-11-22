﻿CREATE TABLE [Harvest].[User] (
    [Id]                           BIGINT          IDENTITY (1, 1) NOT NULL,
    [CreatedAt]                    DATETIME        NOT NULL,
    [UpdatedAt]                    DATETIME        NOT NULL,
    [Email]                        NVARCHAR (MAX)  NULL,
    [Telephone]                    NVARCHAR (MAX)  NULL,
    [FirstName]                    NVARCHAR (MAX)  NULL,
    [LastName]                     NVARCHAR (MAX)  NULL,
    [AvatarUrl]                    NVARCHAR (MAX)  NULL,
    [IdentityUrl]                  NVARCHAR (MAX)  NULL,
    [OpensocialIdentifier]         NVARCHAR (MAX)  NULL,
    [Department]                   NVARCHAR (MAX)  NULL,
    [IsAdmin]                      BIT             NOT NULL,
    [IsContractor]                 BIT             NOT NULL,
    [IsActive]                     BIT             NOT NULL,
    [HasAccessToAllFutureProjects] BIT             NOT NULL,
    [WantsNewsletter]              BIT             NOT NULL,
    [WantsWeeklyDigest]            BIT             NOT NULL,
    [WeeklyDigestSentOn]           DATETIME        NULL,
    [DefaultHourlyRate]            DECIMAL (18, 2) NULL,
    [CostRate]                     DECIMAL (18, 2) NULL,
    [Timezone]                     NVARCHAR (MAX)  NULL,
    CONSTRAINT [PK_Harvest.User] PRIMARY KEY CLUSTERED ([Id] ASC)
);

