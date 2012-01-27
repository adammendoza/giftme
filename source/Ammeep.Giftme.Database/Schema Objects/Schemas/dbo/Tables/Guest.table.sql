CREATE TABLE [dbo].[Guest] (
    [GuestId] INT            IDENTITY (1, 1) NOT NULL,
    [Name]    NVARCHAR (100) NOT NULL,
    [Email]   NVARCHAR (200) NOT NULL,
	[GiftPurchaseId] INT NOT NULL,
	[CreatedDate]          DATETIME       NOT NULL,
);

