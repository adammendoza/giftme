ALTER TABLE [dbo].[GiftPurchase]
    ADD CONSTRAINT [FK_GiftPurchase_Account] FOREIGN KEY ([AccountId]) REFERENCES [dbo].[Account] ([AccountId]) ON DELETE NO ACTION ON UPDATE NO ACTION;

