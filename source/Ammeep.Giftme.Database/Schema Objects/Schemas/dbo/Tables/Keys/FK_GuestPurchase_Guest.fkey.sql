ALTER TABLE [dbo].[GuestPurchase]
    ADD CONSTRAINT [FK_GuestPurchase_Guest] FOREIGN KEY ([GuestId]) REFERENCES [dbo].[Guest] ([GuestId]) ON DELETE NO ACTION ON UPDATE NO ACTION;

