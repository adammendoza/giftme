ALTER TABLE [dbo].[GiftPurchase]
    ADD CONSTRAINT [FK_GiftPurchase_Guest] FOREIGN KEY ([GuestId]) REFERENCES [dbo].[Guest] ([GuestId]) ON DELETE NO ACTION ON UPDATE NO ACTION;

