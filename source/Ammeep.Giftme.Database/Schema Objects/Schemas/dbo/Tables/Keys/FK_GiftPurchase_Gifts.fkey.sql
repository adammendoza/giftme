ALTER TABLE [dbo].[GiftPurchase]
    ADD CONSTRAINT [FK_GiftPurchase_Gifts] FOREIGN KEY ([GiftId]) REFERENCES [dbo].[Gifts] ([GiftId]) ON DELETE NO ACTION ON UPDATE NO ACTION;

