ALTER TABLE [dbo].[Category]
    ADD CONSTRAINT [FK_Category_Users1] FOREIGN KEY ([LastUpdatedBy]) REFERENCES [dbo].[Users] ([UserId]) ON DELETE NO ACTION ON UPDATE NO ACTION;

