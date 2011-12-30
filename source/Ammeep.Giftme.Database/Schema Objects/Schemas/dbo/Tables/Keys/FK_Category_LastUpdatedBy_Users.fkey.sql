ALTER TABLE [dbo].[Category]
    ADD CONSTRAINT [FK_Category_LastUpdatedBy_Users] FOREIGN KEY ([LastUpdatedBy]) REFERENCES [dbo].[Users] ([UserId]) ON DELETE NO ACTION ON UPDATE NO ACTION;

