/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/
print 'Inserting Test Data'
SET IDENTITY_INSERT [AccountType] ON
INSERT [AccountType] ([AccountTypeId], [Name], [RequiresPassword]) VALUES (0, N'Guest', 0)
INSERT [AccountType] ([AccountTypeId], [Name], [RequiresPassword]) VALUES (1, N'Host', 1)
INSERT [AccountType] ([AccountTypeId], [Name], [RequiresPassword]) VALUES (2, N'Admin', 1)
SET IDENTITY_INSERT [AccountType] OFF
SET IDENTITY_INSERT [Account] ON
INSERT [Account] ([AccountId], [AccountType], [UserName], [Email], [Name], [PasswordHash], [PasswordSalt]) VALUES (1, 1, N'Joanne', N'une_cute_tomato@hotmail.com', N'Joanne', N'b330b6b4b13c004506fe9c107390a0a79952b599b6d3b22fd9b92698badf3d4c', N'c684877cac5b20699e84fd1ded1d894e659d102f65b7501073ea929f8206ac74')
INSERT [Account] ([AccountId], [AccountType], [UserName], [Email], [Name], [PasswordHash], [PasswordSalt]) VALUES (2, 2, N'God', N'a.palamountain@gmail.com', N'Amy Palamountain', N'388c6a665a79223f9f748349bd1036ac77db219747896c658ee026dce57c00ed', N'b66d7c0379018d8e0a8c3da1eca563d7e6fc162e7baa121fde94718e808be23e')
INSERT [Account] ([AccountId], [AccountType], [UserName], [Email], [Name], [PasswordHash], [PasswordSalt]) VALUES (3, 2, N'System', N'giftme@giftme.com', N'System Account', N'1F6RZ43FEGLJ6OMJ2Z39UTGQSRFCVVUFZ728HNB16LDTLHIAZ17A9NI7ZGY8PRKIL6KZLE1EN129GM4B84CTOK6COU80O3CF0U7FXJOL0SM7K8RDBNZKERT108BR50QJPM8LEPEPGQTVXYRF3VP04R0916FP8XGZ22IX0', N'EUJ6QMZQ8FGOCOGNM2UZ3RB1QSNGWFMZ8NR40V9Y5STFV9DUVPUSC0FNJHFYZIUSTHGOXBMRSVUBXW6HFSL7KTM773LJFBYNY0LDSOXJ23ZH64PNAYJ6WM8')
SET IDENTITY_INSERT [Account] OFF

SET IDENTITY_INSERT [Category] ON
INSERT [Category] ([CategoryId], [Name], [CreatedBy], [CreateDate], [LastUpdatedBy], [LastUpdatedDate]) VALUES (0, N'All', 3, CAST(0x00005732017D693C AS DateTime), 3, CAST(0x00005AB7015B9FAC AS DateTime))
INSERT [Category] ([CategoryId], [Name], [CreatedBy], [CreateDate], [LastUpdatedBy], [LastUpdatedDate]) VALUES (1, N'Experience', 3, CAST(0x00005732017D693C AS DateTime), 3, CAST(0x00005AB7015B9FAC AS DateTime))
INSERT [Category] ([CategoryId], [Name], [CreatedBy], [CreateDate], [LastUpdatedBy], [LastUpdatedDate]) VALUES (2, N'Kitchen & Dining', 3, CAST(0x000076010133B519 AS DateTime), 3, CAST(0x0000559600E46CD2 AS DateTime))
INSERT [Category] ([CategoryId], [Name], [CreatedBy], [CreateDate], [LastUpdatedBy], [LastUpdatedDate]) VALUES (3, N'Living Room', 3, CAST(0x000093C90171F16E AS DateTime), 3, CAST(0x0000648D001F33C8 AS DateTime))
INSERT [Category] ([CategoryId], [Name], [CreatedBy], [CreateDate], [LastUpdatedBy], [LastUpdatedDate]) VALUES (4, N'Outdoor & Sports', 3, CAST(0x00006FC4012B6B86 AS DateTime), 3, CAST(0x0000914300F2953B AS DateTime))
INSERT [Category] ([CategoryId], [Name], [CreatedBy], [CreateDate], [LastUpdatedBy], [LastUpdatedDate]) VALUES (5, N'Travel', 3, CAST(0x00008E0100563DB4 AS DateTime), 3, CAST(0x00008664005189C4 AS DateTime))
INSERT [Category] ([CategoryId], [Name], [CreatedBy], [CreateDate], [LastUpdatedBy], [LastUpdatedDate]) VALUES (6, N'Other', 3, CAST(0x00007FDF0147F8B8 AS DateTime), 3, CAST(0x00006C15013C4C64 AS DateTime))
SET IDENTITY_INSERT [Category] OFF
