CREATE TABLE [dbo].[Feedback] (
    [FeedbackId]      INT        IDENTITY (1, 1) NOT NULL,
    [Name]            NVARCHAR (100) NOT NULL,
	[InterpretationComments]            NVARCHAR (4000) NOT NULL,
	[WasItClear]            BIT NOT NULL,	
	[NotClearComments]            NVARCHAR (4000) NOT NULL,
	[WantsToSeeReservedGifts]            BIT NOT NULL,	
	[SeeReservedGiftsComments]            NVARCHAR (4000) NOT NULL,
	[AgreesWithPricingModels]            BIT NOT NULL,	
	[PricingModelComments]            NVARCHAR (4000) NOT NULL,
	[FinalComments]            NVARCHAR (4000) NOT NULL,
    [CreatedBy]       INT        NOT NULL,
    [CreateDate]      DATETIME   NOT NULL,
    [LastUpdatedBy]   INT        NOT NULL,
    [LastUpdatedDate] DATETIME   NOT NULL
);
