ALTER TABLE [dbo].[Gifts]
    ADD CONSTRAINT [FK_Gifts_Category] FOREIGN KEY ([Category]) REFERENCES [dbo].[Category] ([CategoryId]) ON DELETE NO ACTION ON UPDATE NO ACTION;

