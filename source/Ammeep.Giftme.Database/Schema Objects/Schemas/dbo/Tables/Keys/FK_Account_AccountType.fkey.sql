ALTER TABLE [dbo].[Account]
    ADD CONSTRAINT [FK_Account_AccountType] FOREIGN KEY ([AccountType]) REFERENCES [dbo].[AccountType] ([AccountTypeId]) ON DELETE NO ACTION ON UPDATE NO ACTION;

