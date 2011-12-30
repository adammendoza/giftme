ALTER TABLE [dbo].[Gifts]
    ADD CONSTRAINT [FK_Gifts_CreatedBy_Users] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[Users] ([UserId]) ON DELETE NO ACTION ON UPDATE NO ACTION;

