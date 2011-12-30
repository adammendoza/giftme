ALTER TABLE [dbo].[Category]
    ADD CONSTRAINT [FK_Category_CreatedBy_Users] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[Users] ([UserId]) ON DELETE NO ACTION ON UPDATE NO ACTION;

