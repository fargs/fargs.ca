CREATE TABLE [dbo].[Assessment] (
    [Id]                  INT              IDENTITY (1, 1) NOT NULL,
    [AssessmentTypeId]    SMALLINT         NULL,
    [AssessmentDate]      DATE             NULL,
    [StartTime]           TIME (7)         NULL,
    [EndTime]             TIME (7)         NULL,
    [DoctorId]            UNIQUEIDENTIFIER NULL,
    [IntakeCoordinatorId] UNIQUEIDENTIFIER NULL,
    [AdministratorId]     UNIQUEIDENTIFIER NULL,
    [CompanyId]           SMALLINT         NULL,
    [ModifiedDate]        DATETIME         CONSTRAINT [DF_Assessment_ModifiedDate] DEFAULT (getdate()) NOT NULL,
    [ModifiedUser]        NVARCHAR (100)   CONSTRAINT [DF_Assessment_ModifiedUser] DEFAULT (suser_name()) NOT NULL,
    CONSTRAINT [PK_Assessment] PRIMARY KEY CLUSTERED ([Id] ASC)
);





