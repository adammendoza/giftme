﻿ALTER TABLE [dbo].[Gifts]
    ADD CONSTRAINT [FK_Gifts_LastUpdatedBy_Users] FOREIGN KEY ([LastUpdatedBy]) REFERENCES [dbo].[Account] ([AccountId]) ON DELETE NO ACTION ON UPDATE NO ACTION;



