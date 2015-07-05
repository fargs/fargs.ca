CREATE TABLE [dbo].[Configuration] (
    [Id]                  SMALLINT       IDENTITY (1, 1) NOT NULL,
    [EntityTypeID]        TINYINT        NOT NULL,
    [EntityID]            NVARCHAR (128) NOT NULL,
    [ConfigurationTypeID] TINYINT        NULL,
    [ConfigValue]         SQL_VARIANT    NULL,
    [DatePart]            NVARCHAR (10)  NULL,
    [Sequence]            SMALLINT       NULL,
    [AssessmentTypeID]    INT            NULL,
    [ModifiedDate]        DATETIME       CONSTRAINT [DF_Configuration_ModifiedDate] DEFAULT (getdate()) NOT NULL,
    [ModifiedUser]        NVARCHAR (100) NOT NULL,
    CONSTRAINT [PK_Configuration] PRIMARY KEY CLUSTERED ([Id] ASC)
);



