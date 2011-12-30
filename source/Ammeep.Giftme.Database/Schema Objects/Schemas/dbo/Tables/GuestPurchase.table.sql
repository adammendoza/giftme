CREATE TABLE [dbo].[GuestPurchase] (
    [GuestPurchaseId] INT IDENTITY (1, 1) NOT NULL,
    [GuestId]         INT NOT NULL,
    [GiftId]          INT NOT NULL,
    [Quantity]        INT NOT NULL
);

