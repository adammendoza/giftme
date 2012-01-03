ALTER TABLE [dbo].[Category]
    ADD CONSTRAINT [FK_Category_Users1] FOREIGN KEY ([LastUpdatedBy]) REFERENCES [dbo].[Account] ([AccountId]) ON DELETE NO ACTION ON UPDATE NO ACTION;



