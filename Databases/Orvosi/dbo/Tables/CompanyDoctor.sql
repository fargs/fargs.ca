CREATE TABLE [dbo].[CompanyDoctor] (
    [Id]               INT             IDENTITY (1, 1) NOT NULL,
    [CompanyId]        SMALLINT        NOT NULL,
    [DoctorId]         NVARCHAR (128)  NOT NULL,
    [AssessmentTypeId] TINYINT         NULL,
    [Price]            DECIMAL (18, 2) NULL,
    [ModifiedDate]     DATETIME        CONSTRAINT [DF_CompanyDoctor_ModifiedDate] DEFAULT (getdate()) NOT NULL,
    [ModifiedUser]     NVARCHAR (256)  CONSTRAINT [DF_CompanyDoctor_ModifiedUser] DEFAULT (suser_name()) NOT NULL,
    CONSTRAINT [PK_CompanyDoctor] PRIMARY KEY CLUSTERED ([Id] ASC)
);



