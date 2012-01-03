CREATE TABLE [dbo].[GiftPurchase] (
    [GiftPurchaseId] INT      IDENTITY (1, 1) NOT NULL,
    [AccountId]      INT      NOT NULL,
    [GiftId]         INT      NOT NULL,
    [Quantity]       INT      NOT NULL,
    [Confirmed]      BIT      NOT NULL,
    [CreatedOn]      DATETIME NOT NULL,
    [ConfimedOn]     DATETIME NOT NULL
);

