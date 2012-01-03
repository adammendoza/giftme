SET IDENTITY_INSERT [AccountType] ON
INSERT [AccountType] ([AccountTypeId], [Name], [RequiresPassword]) VALUES (0, N'Guest', 0)
INSERT [AccountType] ([AccountTypeId], [Name], [RequiresPassword]) VALUES (1, N'Host', 1)
INSERT [AccountType] ([AccountTypeId], [Name], [RequiresPassword]) VALUES (2, N'Admin', 1)
SET IDENTITY_INSERT [AccountType] OFF