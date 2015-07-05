CREATE TABLE [dbo].[AssessmentType] (
    [Id]                                SMALLINT        IDENTITY (1, 1) NOT NULL,
    [Name]                              NVARCHAR (128)  NOT NULL,
    [EstimatedHoursForDocumentReview]   DECIMAL (18, 2) NULL,
    [EstimatedHoursForIntakeAssistance] DECIMAL (18, 2) NULL,
    [EstimatedHoursForReportAssistance] DECIMAL (18, 2) NULL,
    [ModifiedDate]                      DATETIME        CONSTRAINT [DF_AssessmentType_ModifiedDate] DEFAULT (getdate()) NOT NULL,
    [ModifiedUser]                      NVARCHAR (100)  CONSTRAINT [DF_AssessmentType_ModifiedUser] DEFAULT (suser_name()) NOT NULL,
    CONSTRAINT [PK_AssessmentType] PRIMARY KEY CLUSTERED ([Id] ASC)
);



