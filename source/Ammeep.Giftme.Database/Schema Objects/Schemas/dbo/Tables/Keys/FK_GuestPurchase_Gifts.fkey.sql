ALTER TABLE [dbo].[GuestPurchase]
    ADD CONSTRAINT [FK_GuestPurchase_Gifts] FOREIGN KEY ([GiftId]) REFERENCES [dbo].[Gifts] ([GiftId]) ON DELETE NO ACTION ON UPDATE NO ACTION;

