CREATE TABLE [dbo].[Gifts] (
    [GiftId]               INT            IDENTITY (1, 1) NOT NULL,
    [Name]                 NVARCHAR (50)  NOT NULL,
    [Description]          NVARCHAR (300) NOT NULL,
    [ImageLocation]        NVARCHAR (200) NOT NULL,
    [Website]              NVARCHAR (200) NOT NULL,
    [SuggestedStores]      NVARCHAR (50)  NOT NULL,
    [SpecificItemRequried] BIT            NOT NULL,
    [QuantityRequired]     INT            NOT NULL,
    [QuantityRemaining]    INT            NOT NULL,
    [Category]             INT            NOT NULL,
    [RetailPrice]          DECIMAL (18)   NOT NULL,
    [IsActive]             BIT            NOT NULL,
    [CreatedBy]            INT            NOT NULL,
    [CreatedDate]          DATETIME       NOT NULL,
    [LastUpdatedBy]        INT            NOT NULL,
    [LastUpdatedDate]      DATETIME       NOT NULL
);

