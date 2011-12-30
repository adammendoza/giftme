CREATE TABLE [dbo].[Category] (
    [CategoryId]      INT        IDENTITY (1, 1) NOT NULL,
    [Name]            NCHAR (20) NOT NULL,
    [CreatedBy]       INT        NOT NULL,
    [CreateDate]      DATETIME   NOT NULL,
    [LastUpdatedBy]   INT        NOT NULL,
    [LastUpdatedDate] DATETIME   NOT NULL
);

