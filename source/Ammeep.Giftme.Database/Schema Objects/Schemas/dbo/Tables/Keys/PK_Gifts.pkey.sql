﻿ALTER TABLE [dbo].[Gifts]
    ADD CONSTRAINT [PK_Gifts] PRIMARY KEY CLUSTERED ([GiftId] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);

