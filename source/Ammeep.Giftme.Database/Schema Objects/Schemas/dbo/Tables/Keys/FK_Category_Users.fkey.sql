ALTER TABLE [dbo].[Category]
    ADD CONSTRAINT [FK_Category_Users] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[Account] ([AccountId]) ON DELETE NO ACTION ON UPDATE NO ACTION;



