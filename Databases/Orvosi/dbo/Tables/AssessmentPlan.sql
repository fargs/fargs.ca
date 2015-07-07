CREATE TABLE [dbo].[AssessmentPlan] (
    [Id]            SMALLINT       NOT NULL,
    [EntityTypeID]  TINYINT        NOT NULL,
    [EntityID]      BIGINT         NOT NULL,
    [DatePart]      NVARCHAR (10)  NULL,
    [Sequence]      SMALLINT       NULL,
    [ExpectedCount] TINYINT        NULL,
    [ModifiedDate]  DATETIME       CONSTRAINT [DF_AssessmentPlan_ModifiedDate] DEFAULT (getdate()) NOT NULL,
    [ModifiedUser]  NVARCHAR (100) NOT NULL,
    CONSTRAINT [PK_AssessmentPlan] PRIMARY KEY CLUSTERED ([Id] ASC)
);



