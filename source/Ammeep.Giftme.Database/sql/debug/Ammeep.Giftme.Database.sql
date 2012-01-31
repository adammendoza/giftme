/*
Deployment script for Giftme
*/

GO
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, CONCAT_NULL_YIELDS_NULL, QUOTED_IDENTIFIER ON;

SET NUMERIC_ROUNDABORT OFF;


GO
:setvar DatabaseName "Giftme"
:setvar DefaultDataPath "c:\Program Files\Microsoft SQL Server\MSSQL10.SQLEXPRESS\MSSQL\DATA\"
:setvar DefaultLogPath "c:\Program Files\Microsoft SQL Server\MSSQL10.SQLEXPRESS\MSSQL\DATA\"

GO
:on error exit
GO
USE [master]
GO
IF (DB_ID(N'$(DatabaseName)') IS NOT NULL
    AND DATABASEPROPERTYEX(N'$(DatabaseName)','Status') <> N'ONLINE')
BEGIN
    RAISERROR(N'The state of the target database, %s, is not set to ONLINE. To deploy to this database, its state must be set to ONLINE.', 16, 127,N'$(DatabaseName)') WITH NOWAIT
    RETURN
END

GO
IF (DB_ID(N'$(DatabaseName)') IS NOT NULL) 
BEGIN
    ALTER DATABASE [$(DatabaseName)]
    SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE [$(DatabaseName)];
END

GO
PRINT N'Creating $(DatabaseName)...'
GO
CREATE DATABASE [$(DatabaseName)]
    ON 
    PRIMARY(NAME = [Giftme], FILENAME = N'$(DefaultDataPath)Giftme.mdf')
    LOG ON (NAME = [Giftme_log], FILENAME = N'$(DefaultLogPath)Giftme_log.ldf') COLLATE Latin1_General_CI_AS
GO
EXECUTE sp_dbcmptlevel [$(DatabaseName)], 100;


GO
IF EXISTS (SELECT 1
           FROM   [master].[dbo].[sysdatabases]
           WHERE  [name] = N'$(DatabaseName)')
    BEGIN
        ALTER DATABASE [$(DatabaseName)]
            SET ANSI_NULLS ON,
                ANSI_PADDING ON,
                ANSI_WARNINGS ON,
                ARITHABORT ON,
                CONCAT_NULL_YIELDS_NULL ON,
                NUMERIC_ROUNDABORT OFF,
                QUOTED_IDENTIFIER ON,
                ANSI_NULL_DEFAULT ON,
                CURSOR_DEFAULT LOCAL,
                RECOVERY FULL,
                CURSOR_CLOSE_ON_COMMIT OFF,
                AUTO_CREATE_STATISTICS ON,
                AUTO_SHRINK OFF,
                AUTO_UPDATE_STATISTICS ON,
                RECURSIVE_TRIGGERS OFF 
            WITH ROLLBACK IMMEDIATE;
        ALTER DATABASE [$(DatabaseName)]
            SET AUTO_CLOSE OFF 
            WITH ROLLBACK IMMEDIATE;
    END


GO
IF EXISTS (SELECT 1
           FROM   [master].[dbo].[sysdatabases]
           WHERE  [name] = N'$(DatabaseName)')
    BEGIN
        ALTER DATABASE [$(DatabaseName)]
            SET ALLOW_SNAPSHOT_ISOLATION OFF;
    END


GO
IF EXISTS (SELECT 1
           FROM   [master].[dbo].[sysdatabases]
           WHERE  [name] = N'$(DatabaseName)')
    BEGIN
        ALTER DATABASE [$(DatabaseName)]
            SET READ_COMMITTED_SNAPSHOT OFF;
    END


GO
IF EXISTS (SELECT 1
           FROM   [master].[dbo].[sysdatabases]
           WHERE  [name] = N'$(DatabaseName)')
    BEGIN
        ALTER DATABASE [$(DatabaseName)]
            SET AUTO_UPDATE_STATISTICS_ASYNC OFF,
                PAGE_VERIFY NONE,
                DATE_CORRELATION_OPTIMIZATION OFF,
                DISABLE_BROKER,
                PARAMETERIZATION SIMPLE,
                SUPPLEMENTAL_LOGGING OFF 
            WITH ROLLBACK IMMEDIATE;
    END


GO
IF IS_SRVROLEMEMBER(N'sysadmin') = 1
    BEGIN
        IF EXISTS (SELECT 1
                   FROM   [master].[dbo].[sysdatabases]
                   WHERE  [name] = N'$(DatabaseName)')
            BEGIN
                EXECUTE sp_executesql N'ALTER DATABASE [$(DatabaseName)]
    SET TRUSTWORTHY OFF,
        DB_CHAINING OFF 
    WITH ROLLBACK IMMEDIATE';
            END
    END
ELSE
    BEGIN
        PRINT N'The database settings cannot be modified. You must be a SysAdmin to apply these settings.';
    END


GO
IF IS_SRVROLEMEMBER(N'sysadmin') = 1
    BEGIN
        IF EXISTS (SELECT 1
                   FROM   [master].[dbo].[sysdatabases]
                   WHERE  [name] = N'$(DatabaseName)')
            BEGIN
                EXECUTE sp_executesql N'ALTER DATABASE [$(DatabaseName)]
    SET HONOR_BROKER_PRIORITY OFF 
    WITH ROLLBACK IMMEDIATE';
            END
    END
ELSE
    BEGIN
        PRINT N'The database settings cannot be modified. You must be a SysAdmin to apply these settings.';
    END


GO
USE [$(DatabaseName)]
GO
IF fulltextserviceproperty(N'IsFulltextInstalled') = 1
    EXECUTE sp_fulltext_database 'enable';


GO
/*
 Pre-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be executed before the build script.	
 Use SQLCMD syntax to include a file in the pre-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the pre-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

GO
PRINT N'Creating [dbo].[Account]...';


GO
CREATE TABLE [dbo].[Account] (
    [AccountId]    INT            IDENTITY (1, 1) NOT NULL,
    [AccountType]  INT            NULL,
    [UserName]     NVARCHAR (50)  NOT NULL,
    [Email]        NVARCHAR (50)  NOT NULL,
    [Name]         NVARCHAR (50)  NOT NULL,
    [PasswordHash] NVARCHAR (256) NOT NULL,
    [PasswordSalt] NVARCHAR (256) NOT NULL
);


GO
PRINT N'Creating PK_Users...';


GO
ALTER TABLE [dbo].[Account]
    ADD CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED ([AccountId] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);


GO
PRINT N'Creating [dbo].[AccountType]...';


GO
CREATE TABLE [dbo].[AccountType] (
    [AccountTypeId]    INT          IDENTITY (1, 1) NOT NULL,
    [Name]             VARCHAR (50) NOT NULL,
    [RequiresPassword] BIT          NOT NULL
);


GO
PRINT N'Creating PK_AccountType...';


GO
ALTER TABLE [dbo].[AccountType]
    ADD CONSTRAINT [PK_AccountType] PRIMARY KEY CLUSTERED ([AccountTypeId] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);


GO
PRINT N'Creating [dbo].[Category]...';


GO
CREATE TABLE [dbo].[Category] (
    [CategoryId]      INT        IDENTITY (1, 1) NOT NULL,
    [Name]            NCHAR (20) NOT NULL,
    [CreatedBy]       INT        NOT NULL,
    [CreateDate]      DATETIME   NOT NULL,
    [LastUpdatedBy]   INT        NOT NULL,
    [LastUpdatedDate] DATETIME   NOT NULL
);


GO
PRINT N'Creating PK_Category...';


GO
ALTER TABLE [dbo].[Category]
    ADD CONSTRAINT [PK_Category] PRIMARY KEY CLUSTERED ([CategoryId] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);


GO
PRINT N'Creating [dbo].[GiftPurchase]...';


GO
CREATE TABLE [dbo].[GiftPurchase] (
    [GiftPurchaseId] INT              IDENTITY (1, 1) NOT NULL,
    [GuestId]        INT              NOT NULL,
    [GiftId]         INT              NOT NULL,
    [Quantity]       INT              NOT NULL,
    [ConfirmationId] UNIQUEIDENTIFIER NOT NULL,
    [Confirmed]      BIT              NOT NULL,
    [CreatedOn]      DATETIME         NOT NULL,
    [ConfimedOn]     DATETIME         NULL
);


GO
PRINT N'Creating PK_GiftPurchase...';


GO
ALTER TABLE [dbo].[GiftPurchase]
    ADD CONSTRAINT [PK_GiftPurchase] PRIMARY KEY CLUSTERED ([GiftPurchaseId] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);


GO
PRINT N'Creating [dbo].[Gifts]...';


GO
CREATE TABLE [dbo].[Gifts] (
    [GiftId]               INT            IDENTITY (1, 1) NOT NULL,
    [Name]                 NVARCHAR (50)  NOT NULL,
    [Description]          NVARCHAR (300) NOT NULL,
    [ImageLocation]        NVARCHAR (200) NOT NULL,
    [Website]              NVARCHAR (200) NOT NULL,
    [SuggestedStores]      NVARCHAR (50)  NOT NULL,
    [SpecificItemRequried] BIT            NOT NULL,
    [QuantityRequired]     INT            NOT NULL,
    [QuantityRemaining]    INT            NOT NULL,
    [Category]             INT            NOT NULL,
    [RetailPrice]          DECIMAL (18)   NOT NULL,
    [IsActive]             BIT            NOT NULL,
    [CreatedBy]            INT            NOT NULL,
    [CreatedDate]          DATETIME       NOT NULL,
    [LastUpdatedBy]        INT            NOT NULL,
    [LastUpdatedDate]      DATETIME       NOT NULL
);


GO
PRINT N'Creating PK_Gifts...';


GO
ALTER TABLE [dbo].[Gifts]
    ADD CONSTRAINT [PK_Gifts] PRIMARY KEY CLUSTERED ([GiftId] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);


GO
PRINT N'Creating [dbo].[Guest]...';


GO
CREATE TABLE [dbo].[Guest] (
    [GuestId]        INT            IDENTITY (1, 1) NOT NULL,
    [Name]           NVARCHAR (100) NOT NULL,
    [Email]          NVARCHAR (200) NOT NULL,
    [GiftPurchaseId] INT            NOT NULL,
    [CreatedDate]    DATETIME       NOT NULL
);


GO
PRINT N'Creating PK_Guest...';


GO
ALTER TABLE [dbo].[Guest]
    ADD CONSTRAINT [PK_Guest] PRIMARY KEY CLUSTERED ([GuestId] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);


GO
PRINT N'Creating FK_Account_AccountType...';


GO
ALTER TABLE [dbo].[Account] WITH NOCHECK
    ADD CONSTRAINT [FK_Account_AccountType] FOREIGN KEY ([AccountType]) REFERENCES [dbo].[AccountType] ([AccountTypeId]) ON DELETE NO ACTION ON UPDATE NO ACTION;


GO
PRINT N'Creating FK_Category_CreatedBy_Users...';


GO
ALTER TABLE [dbo].[Category] WITH NOCHECK
    ADD CONSTRAINT [FK_Category_CreatedBy_Users] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[Account] ([AccountId]) ON DELETE NO ACTION ON UPDATE NO ACTION;


GO
PRINT N'Creating FK_Category_LastUpdatedBy_Users...';


GO
ALTER TABLE [dbo].[Category] WITH NOCHECK
    ADD CONSTRAINT [FK_Category_LastUpdatedBy_Users] FOREIGN KEY ([LastUpdatedBy]) REFERENCES [dbo].[Account] ([AccountId]) ON DELETE NO ACTION ON UPDATE NO ACTION;


GO
PRINT N'Creating FK_Category_Users...';


GO
ALTER TABLE [dbo].[Category] WITH NOCHECK
    ADD CONSTRAINT [FK_Category_Users] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[Account] ([AccountId]) ON DELETE NO ACTION ON UPDATE NO ACTION;


GO
PRINT N'Creating FK_Category_Users1...';


GO
ALTER TABLE [dbo].[Category] WITH NOCHECK
    ADD CONSTRAINT [FK_Category_Users1] FOREIGN KEY ([LastUpdatedBy]) REFERENCES [dbo].[Account] ([AccountId]) ON DELETE NO ACTION ON UPDATE NO ACTION;


GO
PRINT N'Creating FK_GiftPurchase_Gifts...';


GO
ALTER TABLE [dbo].[GiftPurchase] WITH NOCHECK
    ADD CONSTRAINT [FK_GiftPurchase_Gifts] FOREIGN KEY ([GiftId]) REFERENCES [dbo].[Gifts] ([GiftId]) ON DELETE NO ACTION ON UPDATE NO ACTION;


GO
PRINT N'Creating FK_GiftPurchase_Guest...';


GO
ALTER TABLE [dbo].[GiftPurchase] WITH NOCHECK
    ADD CONSTRAINT [FK_GiftPurchase_Guest] FOREIGN KEY ([GuestId]) REFERENCES [dbo].[Guest] ([GuestId]) ON DELETE NO ACTION ON UPDATE NO ACTION;


GO
PRINT N'Creating FK_Gifts_Category...';


GO
ALTER TABLE [dbo].[Gifts] WITH NOCHECK
    ADD CONSTRAINT [FK_Gifts_Category] FOREIGN KEY ([Category]) REFERENCES [dbo].[Category] ([CategoryId]) ON DELETE NO ACTION ON UPDATE NO ACTION;


GO
PRINT N'Creating FK_Gifts_CreatedBy_Users...';


GO
ALTER TABLE [dbo].[Gifts] WITH NOCHECK
    ADD CONSTRAINT [FK_Gifts_CreatedBy_Users] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[Account] ([AccountId]) ON DELETE NO ACTION ON UPDATE NO ACTION;


GO
PRINT N'Creating FK_Gifts_LastUpdatedBy_Users...';


GO
ALTER TABLE [dbo].[Gifts] WITH NOCHECK
    ADD CONSTRAINT [FK_Gifts_LastUpdatedBy_Users] FOREIGN KEY ([LastUpdatedBy]) REFERENCES [dbo].[Account] ([AccountId]) ON DELETE NO ACTION ON UPDATE NO ACTION;


GO
-- Refactoring step to update target server with deployed transaction logs
CREATE TABLE  [dbo].[__RefactorLog] (OperationKey UNIQUEIDENTIFIER NOT NULL PRIMARY KEY)
GO
sp_addextendedproperty N'microsoft_database_tools_support', N'refactoring log', N'schema', N'dbo', N'table', N'__RefactorLog'
GO

GO
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
INSERT [Account] ([AccountId], [AccountType], [UserName], [Email], [Name], [PasswordHash], [PasswordSalt]) VALUES (1, 2, N'Regina19', N'Test@Test.org', N'Jennifer Bright', N'A9MGX5YL4ZD22H5Y3T9H2US6Q8SX31RB4DZ6S7SC3ZUJ71JMVZZ9I79GHFPPC0H372YOIZ6W0WD3Z8ZPP5142ISX4JOCH3Z2HRC42U4WT28LT6G8F0Z7ZD0O91KYATFWXAL8WUMU05AZA73Y2FWG3R34QNV1A5LZNBOIK7CLE5', N'KYUWSLGPHP5RL6RDPUMMB04AD3E7WE1P5Q0')
INSERT [Account] ([AccountId], [AccountType], [UserName], [Email], [Name], [PasswordHash], [PasswordSalt]) VALUES (2, 1, N'Jolene84', N'Test@Test.net', N'Edwin Brady', N'T21E2XLVUWYT56HI8ZKJUJBBYIKEV3B2YK91O8T3P9R2PIIC2P3LTLCARPGJD70RLWVKWFT06HUKR8MG08B6ZP8C8ZAZSAQH9ZHTNYFNV3FCRRIJ30J4WTRVNIBX5U7WQDBSQTS1TNAWAWLHGRWSQLLF8M54MHI1VJLDFQS97HB8JVKQXUHDYF54SVJ4NOFB590RA71PYMK9ACW', N'GDD4DJY22DJQ03FZPEJKTJJP68T3OFMU5RWCHI8R1270JO6TZ1ZQ7G6XUNX4QVEMRPK862I65RIRCI6I203MFVNPLZITL306NK8U3YMWJTKFXEKBHE2QRR7V1VM7KOK2QB3R9BF3D72MA8R7F6C70AFWDDS8BEBV8IOLAT15ZYCKOKLT3ESECD1GW7EWI6SYV7ESK6XL5671XV7TRZ5WMXIUG7O2F4MS6DOH6S9WH6866ZJ10K4J1VCXPJBCHU60')
INSERT [Account] ([AccountId], [AccountType], [UserName], [Email], [Name], [PasswordHash], [PasswordSalt]) VALUES (3, 0, N'Shane700', N'Test@Test.net', N'Alisha Flores', N'1F6RZ43FEGLJ6OMJ2Z39UTGQSRFCVVUFZ728HNB16LDTLHIAZ17A9NI7ZGY8PRKIL6KZLE1EN129GM4B84CTOK6COU80O3CF0U7FXJOL0SM7K8RDBNZKERT108BR50QJPM8LEPEPGQTVXYRF3VP04R0916FP8XGZ22IX0', N'EUJ6QMZQ8FGOCOGNM2UZ3RB1QSNGWFMZ8NR40V9Y5STFV9DUVPUSC0FNJHFYZIUSTHGOXBMRSVUBXW6HFSL7KTM773LJFBYNY0LDSOXJ23ZH64PNAYJ6WM8')
SET IDENTITY_INSERT [Account] OFF
SET IDENTITY_INSERT [Category] ON
INSERT [Category] ([CategoryId], [Name], [CreatedBy], [CreateDate], [LastUpdatedBy], [LastUpdatedDate]) VALUES (0, N'All', 3, CAST(0x00005732017D693C AS DateTime), 1, CAST(0x00005AB7015B9FAC AS DateTime))
INSERT [Category] ([CategoryId], [Name], [CreatedBy], [CreateDate], [LastUpdatedBy], [LastUpdatedDate]) VALUES (1, N'Bedroom', 3, CAST(0x00005732017D693C AS DateTime), 1, CAST(0x00005AB7015B9FAC AS DateTime))
INSERT [Category] ([CategoryId], [Name], [CreatedBy], [CreateDate], [LastUpdatedBy], [LastUpdatedDate]) VALUES (2, N'Living Room', 3, CAST(0x000076010133B519 AS DateTime), 1, CAST(0x0000559600E46CD2 AS DateTime))
INSERT [Category] ([CategoryId], [Name], [CreatedBy], [CreateDate], [LastUpdatedBy], [LastUpdatedDate]) VALUES (3, N'Kitchen', 3, CAST(0x000093C90171F16E AS DateTime), 1, CAST(0x0000648D001F33C8 AS DateTime))
INSERT [Category] ([CategoryId], [Name], [CreatedBy], [CreateDate], [LastUpdatedBy], [LastUpdatedDate]) VALUES (4, N'Outdoor', 2, CAST(0x00006FC4012B6B86 AS DateTime), 2, CAST(0x0000914300F2953B AS DateTime))
INSERT [Category] ([CategoryId], [Name], [CreatedBy], [CreateDate], [LastUpdatedBy], [LastUpdatedDate]) VALUES (5, N'Laundry', 1, CAST(0x00008E0100563DB4 AS DateTime), 1, CAST(0x00008664005189C4 AS DateTime))
INSERT [Category] ([CategoryId], [Name], [CreatedBy], [CreateDate], [LastUpdatedBy], [LastUpdatedDate]) VALUES (6, N'Dining', 2, CAST(0x00007FDF0147F8B8 AS DateTime), 2, CAST(0x00006C15013C4C64 AS DateTime))
INSERT [Category] ([CategoryId], [Name], [CreatedBy], [CreateDate], [LastUpdatedBy], [LastUpdatedDate]) VALUES (7, N'Travel', 3, CAST(0x00007219006D9596 AS DateTime), 1, CAST(0x000069F200C875AF AS DateTime))
INSERT [Category] ([CategoryId], [Name], [CreatedBy], [CreateDate], [LastUpdatedBy], [LastUpdatedDate]) VALUES (8, N'Food', 2, CAST(0x00008E0A00D963FE AS DateTime), 2, CAST(0x00008E4E00568E5D AS DateTime))
INSERT [Category] ([CategoryId], [Name], [CreatedBy], [CreateDate], [LastUpdatedBy], [LastUpdatedDate]) VALUES (9, N'Experience', 3, CAST(0x000052DC0031A502 AS DateTime), 1, CAST(0x00006646007EA8A8 AS DateTime))
INSERT [Category] ([CategoryId], [Name], [CreatedBy], [CreateDate], [LastUpdatedBy], [LastUpdatedDate]) VALUES (10, N'Bow-chicka-bowow', 1, CAST(0x0000892B007F1739 AS DateTime), 1, CAST(0x0000749200D0CD22 AS DateTime))
SET IDENTITY_INSERT [Category] OFF
SET IDENTITY_INSERT [Gifts] ON
INSERT [Gifts] ([GiftId], [Name], [Description], [ImageLocation], [Website], [SuggestedStores], [SpecificItemRequried], [QuantityRequired], [QuantityRemaining], [Category], [RetailPrice], [IsActive], [CreatedBy], [CreatedDate], [LastUpdatedBy], [LastUpdatedDate]) VALUES (1, N'Parwerpicator', N'delerium. ut Sed ut pladior gravum linguens Longam, si Tam Tam brevens, et glavans quorum novum e egreddior fecit.', N'http://www.onecutetomato.co.nz/wedding/img/icecream.png',N'http://www.google.com', N' Peaches n'' Cream', 0, 4, 4, 8, CAST(56 AS Decimal(18, 0)), 1, 3, CAST(0x00008E4400E55E37 AS DateTime), 3, CAST(0x000091C900C394A7 AS DateTime))
INSERT [Gifts] ([GiftId], [Name], [Description], [ImageLocation], [Website], [SuggestedStores], [SpecificItemRequried], [QuantityRequired], [QuantityRemaining], [Category], [RetailPrice], [IsActive], [CreatedBy], [CreatedDate], [LastUpdatedBy], [LastUpdatedDate]) VALUES (2, N'Winwerover', N'quad fecundio, in eggredior. et ut brevens, quorum et fecit. volcans apparens quartu fecundio, gravis quad', N'http://www.onecutetomato.co.nz/wedding/img/icecream.png',N'http://www.google.com', N' Peaches n'' Cream', 0, 3, 3, 8, CAST(60 AS Decimal(18, 0)), 1, 3, CAST(0x000059D1015A71E7 AS DateTime), 3, CAST(0x000087DD010B29A3 AS DateTime))
INSERT [Gifts] ([GiftId], [Name], [Description], [ImageLocation], [Website], [SuggestedStores], [SpecificItemRequried], [QuantityRequired], [QuantityRemaining], [Category], [RetailPrice], [IsActive], [CreatedBy], [CreatedDate], [LastUpdatedBy], [LastUpdatedDate]) VALUES (3, N'Uncadicator', N'plurissimum rarendum quad Multum eudis rarendum in plorum in funem. non Quad travissimantor plorum e pladior fecundio, quad apparens rarendum', N'http://www.onecutetomato.co.nz/wedding/img/icecream.png',N'http://www.google.com', N' The Warehouse ', 0, 1, 1, 8, CAST(22 AS Decimal(18, 0)), 1, 3, CAST(0x00005C2600202209 AS DateTime), 3, CAST(0x00007B600058E663 AS DateTime))
INSERT [Gifts] ([GiftId], [Name], [Description], [ImageLocation], [Website], [SuggestedStores], [SpecificItemRequried], [QuantityRequired], [QuantityRemaining], [Category], [RetailPrice], [IsActive], [CreatedBy], [CreatedDate], [LastUpdatedBy], [LastUpdatedDate]) VALUES (4, N'Truwerpin', N'rarendum Versus fecit. volcans quad sed eggredior. delerium. et plorum fecit. quorum plorum vobis dolorum quis', N'http://www.onecutetomato.co.nz/wedding/img/icecream.png',N'http://www.google.com', N' Peaches n'' Cream', 1, 1, 1, 8, CAST(149 AS Decimal(18, 0)), 1, 2, CAST(0x0000699100F1682D AS DateTime), 2, CAST(0x00008B1000B891E2 AS DateTime))
INSERT [Gifts] ([GiftId], [Name], [Description], [ImageLocation], [Website], [SuggestedStores], [SpecificItemRequried], [QuantityRequired], [QuantityRemaining], [Category], [RetailPrice], [IsActive], [CreatedBy], [CreatedDate], [LastUpdatedBy], [LastUpdatedDate]) VALUES (5, N'Pardimin', N'delerium. linguens pladior eudis brevens, transit. non travissimantor linguens ut quo vobis novum e ut Id', N'http://www.onecutetomato.co.nz/wedding/img/icecream.png',N'http://www.google.com', N'Bed, Bath n'' Table, Briscoes', 1, 2, 2, 8, CAST(111 AS Decimal(18, 0)), 1, 1, CAST(0x000092FA00411405 AS DateTime), 1, CAST(0x00008B5D003C6018 AS DateTime))
INSERT [Gifts] ([GiftId], [Name], [Description], [ImageLocation], [Website], [SuggestedStores], [SpecificItemRequried], [QuantityRequired], [QuantityRemaining], [Category], [RetailPrice], [IsActive], [CreatedBy], [CreatedDate], [LastUpdatedBy], [LastUpdatedDate]) VALUES (6, N'Inzapackar', N'e egreddior pladior homo, non pars fecit. et in Pro estis et ut et non quo novum quis non funem. gravum', N'http://www.onecutetomato.co.nz/wedding/img/icecream.png',N'http://www.google.com', N' The Warehouse ', 0, 2, 2, 8, CAST(182 AS Decimal(18, 0)), 1, 2, CAST(0x000075490113713E AS DateTime), 2, CAST(0x0000617F0107C4EA AS DateTime))
INSERT [Gifts] ([GiftId], [Name], [Description], [ImageLocation], [Website], [SuggestedStores], [SpecificItemRequried], [QuantityRequired], [QuantityRemaining], [Category], [RetailPrice], [IsActive], [CreatedBy], [CreatedDate], [LastUpdatedBy], [LastUpdatedDate]) VALUES (7, N'Rapkilupantor', N'quantare Tam dolorum Et et rarendum quad sed vantis. plorum vobis esset ut brevens, trepicandor glavans novum linguens', N'http://www.onecutetomato.co.nz/wedding/img/icecream.png',N'http://www.google.com', N' Peaches n'' Cream', 0, 5, 5, 8, CAST(35 AS Decimal(18, 0)), 1, 3, CAST(0x000074A6007B040B AS DateTime), 3, CAST(0x00006C7F00D5E427 AS DateTime))
INSERT [Gifts] ([GiftId], [Name], [Description], [ImageLocation], [Website], [SuggestedStores], [SpecificItemRequried], [QuantityRequired], [QuantityRemaining], [Category], [RetailPrice], [IsActive], [CreatedBy], [CreatedDate], [LastUpdatedBy], [LastUpdatedDate]) VALUES (8, N'Suprobin', N'novum fecundio, quis transit. in eggredior. quorum quad in quartu funem. quad venit. linguens nomen quorum', N'http://www.onecutetomato.co.nz/wedding/img/icecream.png',N'http://www.google.com', N' The Warehouse ', 1, 2, 2, 8, CAST(172 AS Decimal(18, 0)), 1, 2, CAST(0x000067FF00DDE5A5 AS DateTime), 2, CAST(0x00006843005B1004 AS DateTime))
INSERT [Gifts] ([GiftId], [Name], [Description], [ImageLocation], [Website], [SuggestedStores], [SpecificItemRequried], [QuantityRequired], [QuantityRemaining], [Category], [RetailPrice], [IsActive], [CreatedBy], [CreatedDate], [LastUpdatedBy], [LastUpdatedDate]) VALUES (9, N'Barhupistor', N'glavans quoque fecundio, linguens sed ut nomen gravis homo, estis volcans gravum brevens, Versus quoque egreddior', N'http://www.onecutetomato.co.nz/wedding/img/icecream.png',N'http://www.google.com', N'Bed, Bath n'' Table, Briscoes', 0, 3, 3, 5, CAST(167 AS Decimal(18, 0)), 1, 3, CAST(0x00005BC30000B36A AS DateTime), 3, CAST(0x00006F2D004DB710 AS DateTime))
INSERT [Gifts] ([GiftId], [Name], [Description], [ImageLocation], [Website], [SuggestedStores], [SpecificItemRequried], [QuantityRequired], [QuantityRemaining], [Category], [RetailPrice], [IsActive], [CreatedBy], [CreatedDate], [LastUpdatedBy], [LastUpdatedDate]) VALUES (10, N'Winglibazz', N'brevens, fecit. Et linguens apparens estum. delerium. quoque homo, Versus Tam brevens, gravum nomen Id brevens,', N'http://www.onecutetomato.co.nz/wedding/img/icecream.png',N'http://www.google.com', N'Bed, Bath n'' Table, Briscoes', 1, 5, 5, 5, CAST(200 AS Decimal(18, 0)), 1, 1, CAST(0x000053B7006347D6 AS DateTime), 1, CAST(0x00008D9500B4FDC2 AS DateTime))
INSERT [Gifts] ([GiftId], [Name], [Description], [ImageLocation], [Website], [SuggestedStores], [SpecificItemRequried], [QuantityRequired], [QuantityRemaining], [Category], [RetailPrice], [IsActive], [CreatedBy], [CreatedDate], [LastUpdatedBy], [LastUpdatedDate]) VALUES (11, N'Inwerover', N'quorum venit. quoque eggredior. et pladior Quad Sed plorum novum plurissimum quad trepicandor glavans quis', N'http://www.onecutetomato.co.nz/wedding/img/icecream.png',N'http://www.google.com', N' Peaches n'' Cream', 1, 2, 2, 5, CAST(114 AS Decimal(18, 0)), 1, 1, CAST(0x000088E4005F4F0C AS DateTime), 1, CAST(0x00005F9301025946 AS DateTime))
INSERT [Gifts] ([GiftId], [Name], [Description], [ImageLocation], [Website], [SuggestedStores], [SpecificItemRequried], [QuantityRequired], [QuantityRemaining], [Category], [RetailPrice], [IsActive], [CreatedBy], [CreatedDate], [LastUpdatedBy], [LastUpdatedDate]) VALUES (12, N'Barsapopex', N'pladior Id habitatio e et nomen quad quad cognitio, non et novum Pro Multum non gravum si regit, Longam, Quad quantare', N'http://www.onecutetomato.co.nz/wedding/img/icecream.png',N'http://www.google.com', N' The Warehouse ', 0, 5, 5, 5, CAST(60 AS Decimal(18, 0)), 1, 2, CAST(0x00007E0201860C60 AS DateTime), 2, CAST(0x00005B9900A17E16 AS DateTime))
INSERT [Gifts] ([GiftId], [Name], [Description], [ImageLocation], [Website], [SuggestedStores], [SpecificItemRequried], [QuantityRequired], [QuantityRemaining], [Category], [RetailPrice], [IsActive], [CreatedBy], [CreatedDate], [LastUpdatedBy], [LastUpdatedDate]) VALUES (13, N'Bardimegor', N'nomen vantis. nomen quartu linguens dolorum non apparens non non delerium. in et linguens dolorum Et', N'http://www.onecutetomato.co.nz/wedding/img/icecream.png',N'http://www.google.com', N'Bed, Bath n'' Table, Briscoes', 1, 3, 3, 5, CAST(124 AS Decimal(18, 0)), 1, 2, CAST(0x0000811900BC9424 AS DateTime), 2, CAST(0x0000500D0035A585 AS DateTime))
INSERT [Gifts] ([GiftId], [Name], [Description], [ImageLocation], [Website], [SuggestedStores], [SpecificItemRequried], [QuantityRequired], [QuantityRemaining], [Category], [RetailPrice], [IsActive], [CreatedBy], [CreatedDate], [LastUpdatedBy], [LastUpdatedDate]) VALUES (14, N'Lomfropax', N'Sed delerium. apparens Sed volcans esset habitatio Versus nomen et non nomen quantare delerium. gravis', N'http://www.onecutetomato.co.nz/wedding/img/icecream.png',N'http://www.google.com', N' The Warehouse ', 0, 3, 3, 5, CAST(23 AS Decimal(18, 0)), 1, 2, CAST(0x000080D4003F25B0 AS DateTime), 2, CAST(0x000084010161EB63 AS DateTime))
INSERT [Gifts] ([GiftId], [Name], [Description], [ImageLocation], [Website], [SuggestedStores], [SpecificItemRequried], [QuantityRequired], [QuantityRemaining], [Category], [RetailPrice], [IsActive], [CreatedBy], [CreatedDate], [LastUpdatedBy], [LastUpdatedDate]) VALUES (15, N'Klikilar', N'estis et quis Id et imaginator Id ut quad vobis fecit. quad habitatio funem. estis delerium. ut Quad', N'http://www.onecutetomato.co.nz/wedding/img/icecream.png',N'http://www.google.com', N' Peaches n'' Cream', 1, 2, 2, 5, CAST(73 AS Decimal(18, 0)), 1, 3, CAST(0x0000703A002980DD AS DateTime), 3, CAST(0x00008F9B01326363 AS DateTime))
INSERT [Gifts] ([GiftId], [Name], [Description], [ImageLocation], [Website], [SuggestedStores], [SpecificItemRequried], [QuantityRequired], [QuantityRemaining], [Category], [RetailPrice], [IsActive], [CreatedBy], [CreatedDate], [LastUpdatedBy], [LastUpdatedDate]) VALUES (16, N'Endcadepin', N'quad funem. vantis. fecit. plorum estum. quad manifestum plorum estum. in e Sed Id e non dolorum fecit.', N'http://www.onecutetomato.co.nz/wedding/img/icecream.png',N'http://www.google.com', N' The Warehouse ', 0, 5, 5, 5, CAST(138 AS Decimal(18, 0)), 1, 1, CAST(0x000077BB00242CE5 AS DateTime), 1, CAST(0x00008B4500EAE9C7 AS DateTime))
INSERT [Gifts] ([GiftId], [Name], [Description], [ImageLocation], [Website], [SuggestedStores], [SpecificItemRequried], [QuantityRequired], [QuantityRemaining], [Category], [RetailPrice], [IsActive], [CreatedBy], [CreatedDate], [LastUpdatedBy], [LastUpdatedDate]) VALUES (17, N'Growerar', N'non quad gravis trepicandor estis estum. plorum et Et plurissimum travissimantor plorum imaginator non', N'http://www.onecutetomato.co.nz/wedding/img/icecream.png',N'http://www.google.com', N' The Warehouse ', 1, 1, 1, 6, CAST(87 AS Decimal(18, 0)), 1, 3, CAST(0x00008257006FB664 AS DateTime), 3, CAST(0x000075D701378E63 AS DateTime))
INSERT [Gifts] ([GiftId], [Name], [Description], [ImageLocation], [Website], [SuggestedStores], [SpecificItemRequried], [QuantityRequired], [QuantityRemaining], [Category], [RetailPrice], [IsActive], [CreatedBy], [CreatedDate], [LastUpdatedBy], [LastUpdatedDate]) VALUES (18, N'Endwerpommistor', N'bono et essit. et quo quad eggredior. novum homo, glavans vantis. esset non si fecit, novum et quoque', N'http://www.onecutetomato.co.nz/wedding/img/icecream.png',N'http://www.google.com', N' Peaches n'' Cream', 1, 2, 2, 6, CAST(18 AS Decimal(18, 0)), 1, 3, CAST(0x0000965C00D077C9 AS DateTime), 3, CAST(0x00005ABA0096507F AS DateTime))
INSERT [Gifts] ([GiftId], [Name], [Description], [ImageLocation], [Website], [SuggestedStores], [SpecificItemRequried], [QuantityRequired], [QuantityRemaining], [Category], [RetailPrice], [IsActive], [CreatedBy], [CreatedDate], [LastUpdatedBy], [LastUpdatedDate]) VALUES (19, N'Dopnipollover', N'fecundio, vobis plorum habitatio Et nomen Id et eudis Pro fecit, apparens brevens, et volcans plorum vobis', N'http://www.onecutetomato.co.nz/wedding/img/icecream.png',N'http://www.google.com', N' The Warehouse ', 1, 4, 4, 6, CAST(161 AS Decimal(18, 0)), 1, 3, CAST(0x000090D90081AE39 AS DateTime), 3, CAST(0x0000951801519977 AS DateTime))
INSERT [Gifts] ([GiftId], [Name], [Description], [ImageLocation], [Website], [SuggestedStores], [SpecificItemRequried], [QuantityRequired], [QuantityRemaining], [Category], [RetailPrice], [IsActive], [CreatedBy], [CreatedDate], [LastUpdatedBy], [LastUpdatedDate]) VALUES (20, N'Hapglibefantor', N'linguens non delerium. non glavans e essit. vobis sed quad essit. funem. ut eudis volcans delerium. vantis. novum nomen', N'http://www.onecutetomato.co.nz/wedding/img/icecream.png',N'http://www.google.com', N' Peaches n'' Cream', 0, 4, 4, 3, CAST(158 AS Decimal(18, 0)), 1, 1, CAST(0x0000965501443960 AS DateTime), 1, CAST(0x000084A20099BD21 AS DateTime))
INSERT [Gifts] ([GiftId], [Name], [Description], [ImageLocation], [Website], [SuggestedStores], [SpecificItemRequried], [QuantityRequired], [QuantityRemaining], [Category], [RetailPrice], [IsActive], [CreatedBy], [CreatedDate], [LastUpdatedBy], [LastUpdatedDate]) VALUES (21, N'Thrududentor', N'non novum venit. trepicandor regit, et Longam, trepicandor quis delerium. linguens pars funem. nomen regit,', N'http://www.onecutetomato.co.nz/wedding/img/icecream.png',N'http://www.google.com', N' The Warehouse ', 1, 5, 5, 3, CAST(50 AS Decimal(18, 0)), 1, 3, CAST(0x00007E8C0013210E AS DateTime), 3, CAST(0x0000936501874D93 AS DateTime))
INSERT [Gifts] ([GiftId], [Name], [Description], [ImageLocation], [Website], [SuggestedStores], [SpecificItemRequried], [QuantityRequired], [QuantityRemaining], [Category], [RetailPrice], [IsActive], [CreatedBy], [CreatedDate], [LastUpdatedBy], [LastUpdatedDate]) VALUES (22, N'Rapsapan', N'imaginator fecit, non et si quad quo, linguens fecit, linguens cognitio, cognitio, dolorum et bono pladior', N'http://www.onecutetomato.co.nz/wedding/img/icecream.png',N'http://www.google.com', N' Peaches n'' Cream', 1, 2, 2, 3, CAST(191 AS Decimal(18, 0)), 1, 3, CAST(0x000081DA005F7219 AS DateTime), 3, CAST(0x000059E3009D5672 AS DateTime))
INSERT [Gifts] ([GiftId], [Name], [Description], [ImageLocation], [Website], [SuggestedStores], [SpecificItemRequried], [QuantityRequired], [QuantityRemaining], [Category], [RetailPrice], [IsActive], [CreatedBy], [CreatedDate], [LastUpdatedBy], [LastUpdatedDate]) VALUES (23, N'Dopzapazz', N'transit. et et Quad apparens habitatio Multum egreddior Versus et bono fecit, rarendum quis in linguens vantis. et quantare', N'http://www.onecutetomato.co.nz/wedding/img/icecream.png',N'http://www.google.com', N' Peaches n'' Cream', 0, 2, 2, 3, CAST(152 AS Decimal(18, 0)), 1, 3, CAST(0x0000605E0116A312 AS DateTime), 3, CAST(0x00007AE4007B50C7 AS DateTime))
INSERT [Gifts] ([GiftId], [Name], [Description], [ImageLocation], [Website], [SuggestedStores], [SpecificItemRequried], [QuantityRequired], [QuantityRemaining], [Category], [RetailPrice], [IsActive], [CreatedBy], [CreatedDate], [LastUpdatedBy], [LastUpdatedDate]) VALUES (24, N'Klitumazz', N'si eudis quad et dolorum vobis linguens eggredior. quis cognitio, e nomen in brevens, fecundio, non esset', N'http://www.onecutetomato.co.nz/wedding/img/icecream.png',N'http://www.google.com', N' The Warehouse ', 1, 1, 1, 3, CAST(19 AS Decimal(18, 0)), 1, 1, CAST(0x0000570800F05901 AS DateTime), 1, CAST(0x00008DDB005A9750 AS DateTime))
INSERT [Gifts] ([GiftId], [Name], [Description], [ImageLocation], [Website], [SuggestedStores], [SpecificItemRequried], [QuantityRequired], [QuantityRemaining], [Category], [RetailPrice], [IsActive], [CreatedBy], [CreatedDate], [LastUpdatedBy], [LastUpdatedDate]) VALUES (25, N'Tuptumommar', N'habitatio rarendum plurissimum nomen egreddior in Longam, eudis egreddior quo Et et quis vobis et fecit.', N'http://www.onecutetomato.co.nz/wedding/img/icecream.png',N'http://www.google.com', N' The Warehouse ', 1, 4, 4, 3, CAST(2 AS Decimal(18, 0)), 1, 3, CAST(0x000063BD000176D6 AS DateTime), 3, CAST(0x0000731E0070D013 AS DateTime))
INSERT [Gifts] ([GiftId], [Name], [Description], [ImageLocation], [Website], [SuggestedStores], [SpecificItemRequried], [QuantityRequired], [QuantityRemaining], [Category], [RetailPrice], [IsActive], [CreatedBy], [CreatedDate], [LastUpdatedBy], [LastUpdatedDate]) VALUES (26, N'Dopjubanan', N'funem. Et Et estis glavans quantare trepicandor quo funem. linguens regit, Id quad quad e Id et apparens', N'http://www.onecutetomato.co.nz/wedding/img/icecream.png',N'http://www.google.com', N' The Warehouse ', 0, 5, 5, 3, CAST(159 AS Decimal(18, 0)), 1, 2, CAST(0x0000844600FEE93E AS DateTime), 2, CAST(0x00004C47015B904F AS DateTime))
INSERT [Gifts] ([GiftId], [Name], [Description], [ImageLocation], [Website], [SuggestedStores], [SpecificItemRequried], [QuantityRequired], [QuantityRemaining], [Category], [RetailPrice], [IsActive], [CreatedBy], [CreatedDate], [LastUpdatedBy], [LastUpdatedDate]) VALUES (27, N'Uptinar', N'venit. transit. novum bono quad esset rarendum pars ut gravum Et vantis. Id fecundio, e fecit. estum. et', N'http://www.onecutetomato.co.nz/wedding/img/icecream.png',N'http://www.google.com', N' Peaches n'' Cream', 1, 4, 4, 3, CAST(54 AS Decimal(18, 0)), 1, 3, CAST(0x00008E3100F3E7A5 AS DateTime), 3, CAST(0x0000782F0142143C AS DateTime))
INSERT [Gifts] ([GiftId], [Name], [Description], [ImageLocation], [Website], [SuggestedStores], [SpecificItemRequried], [QuantityRequired], [QuantityRemaining], [Category], [RetailPrice], [IsActive], [CreatedBy], [CreatedDate], [LastUpdatedBy], [LastUpdatedDate]) VALUES (28, N'Winvenentor', N'plurissimum non estis novum delerium. imaginator Id esset delerium. quartu et vantis. pars estis cognitio,', N'http://www.onecutetomato.co.nz/wedding/img/icecream.png',N'http://www.google.com', N'Bed, Bath n'' Table, Briscoes', 1, 2, 2, 3, CAST(35 AS Decimal(18, 0)), 1, 3, CAST(0x00004E8700EEC1A6 AS DateTime), 3, CAST(0x0000614C01223DA8 AS DateTime))
INSERT [Gifts] ([GiftId], [Name], [Description], [ImageLocation], [Website], [SuggestedStores], [SpecificItemRequried], [QuantityRequired], [QuantityRemaining], [Category], [RetailPrice], [IsActive], [CreatedBy], [CreatedDate], [LastUpdatedBy], [LastUpdatedDate]) VALUES (29, N'Barroban', N'quad esset nomen glavans trepicandor Versus sed pars venit. parte regit, et manifestum quad quis bono', N'http://www.onecutetomato.co.nz/wedding/img/icecream.png',N'http://www.google.com', N'Bed, Bath n'' Table, Briscoes', 0, 3, 3, 3, CAST(80 AS Decimal(18, 0)), 1, 2, CAST(0x00005DA60014F124 AS DateTime), 2, CAST(0x000088FE00FAB168 AS DateTime))
INSERT [Gifts] ([GiftId], [Name], [Description], [ImageLocation], [Website], [SuggestedStores], [SpecificItemRequried], [QuantityRequired], [QuantityRemaining], [Category], [RetailPrice], [IsActive], [CreatedBy], [CreatedDate], [LastUpdatedBy], [LastUpdatedDate]) VALUES (30, N'Varpebentor', N'si gravis plorum plorum Id parte imaginator Versus plorum bono Quad transit. plorum homo, in in plorum estis parte', N'http://www.onecutetomato.co.nz/wedding/img/icecream.png',N'http://www.google.com', N'Bed, Bath n'' Table, Briscoes', 1, 4, 4, 1, CAST(16 AS Decimal(18, 0)), 1, 1, CAST(0x000072B700CF68B2 AS DateTime), 1, CAST(0x0000542C00100FA4 AS DateTime))
INSERT [Gifts] ([GiftId], [Name], [Description], [ImageLocation], [Website], [SuggestedStores], [SpecificItemRequried], [QuantityRequired], [QuantityRemaining], [Category], [RetailPrice], [IsActive], [CreatedBy], [CreatedDate], [LastUpdatedBy], [LastUpdatedDate]) VALUES (31, N'Vartinan', N'non vobis imaginator non gravum Multum fecundio, volcans quorum homo, e vobis Pro egreddior vobis gravis', N'http://www.onecutetomato.co.nz/wedding/img/icecream.png',N'http://www.google.com', N' Peaches n'' Cream', 0, 3, 3, 1, CAST(133 AS Decimal(18, 0)), 1, 1, CAST(0x000087FA0109E606 AS DateTime), 1, CAST(0x0000737300077883 AS DateTime))
INSERT [Gifts] ([GiftId], [Name], [Description], [ImageLocation], [Website], [SuggestedStores], [SpecificItemRequried], [QuantityRequired], [QuantityRemaining], [Category], [RetailPrice], [IsActive], [CreatedBy], [CreatedDate], [LastUpdatedBy], [LastUpdatedDate]) VALUES (32, N'Winrobower', N'plurissimum Pro brevens, fecit. non vantis. pars habitatio travissimantor pladior quoque plorum novum', N'http://www.onecutetomato.co.nz/wedding/img/icecream.png',N'http://www.google.com', N' The Warehouse ', 1, 5, 5, 1, CAST(148 AS Decimal(18, 0)), 1, 2, CAST(0x00007517000CF198 AS DateTime), 2, CAST(0x00004FF60036BCD6 AS DateTime))
INSERT [Gifts] ([GiftId], [Name], [Description], [ImageLocation], [Website], [SuggestedStores], [SpecificItemRequried], [QuantityRequired], [QuantityRemaining], [Category], [RetailPrice], [IsActive], [CreatedBy], [CreatedDate], [LastUpdatedBy], [LastUpdatedDate]) VALUES (33, N'Rapmunedgex', N'plorum novum linguens fecit, quo, parte si quo regit, pars si nomen gravis esset egreddior vantis. homo, novum nomen nomen', N'http://www.onecutetomato.co.nz/wedding/img/icecream.png',N'http://www.google.com', N'Bed, Bath n'' Table, Briscoes', 0, 5, 5, 1, CAST(3 AS Decimal(18, 0)), 1, 1, CAST(0x00005D110158927D AS DateTime), 1, CAST(0x00004DB5013C2F90 AS DateTime))
INSERT [Gifts] ([GiftId], [Name], [Description], [ImageLocation], [Website], [SuggestedStores], [SpecificItemRequried], [QuantityRequired], [QuantityRemaining], [Category], [RetailPrice], [IsActive], [CreatedBy], [CreatedDate], [LastUpdatedBy], [LastUpdatedDate]) VALUES (34, N'Uptinantor', N'Id Pro nomen novum quad et eudis quo si dolorum glavans quo plorum et travissimantor Multum fecit. Multum Multum', N'http://www.onecutetomato.co.nz/wedding/img/icecream.png',N'http://www.google.com', N' Peaches n'' Cream', 1, 5, 5, 1, CAST(181 AS Decimal(18, 0)), 1, 3, CAST(0x0000986800B0AFA5 AS DateTime), 3, CAST(0x00004DE000FABC8D AS DateTime))
INSERT [Gifts] ([GiftId], [Name], [Description], [ImageLocation], [Website], [SuggestedStores], [SpecificItemRequried], [QuantityRequired], [QuantityRemaining], [Category], [RetailPrice], [IsActive], [CreatedBy], [CreatedDate], [LastUpdatedBy], [LastUpdatedDate]) VALUES (35, N'Endtinover', N'apparens brevens, Tam Multum glavans novum Multum gravis travissimantor quad Sed vobis quo fecit, Quad parte volcans', N'http://www.onecutetomato.co.nz/wedding/img/icecream.png',N'http://www.google.com', N'Bed, Bath n'' Table, Briscoes', 1, 5, 5, 1, CAST(4 AS Decimal(18, 0)), 1, 2, CAST(0x000050FD00F53C85 AS DateTime), 2, CAST(0x000087C900CE7C63 AS DateTime))
INSERT [Gifts] ([GiftId], [Name], [Description], [ImageLocation], [Website], [SuggestedStores], [SpecificItemRequried], [QuantityRequired], [QuantityRemaining], [Category], [RetailPrice], [IsActive], [CreatedBy], [CreatedDate], [LastUpdatedBy], [LastUpdatedDate]) VALUES (36, N'Trubanex', N'non brevens, quantare quad in Longam, quoque fecit. glavans habitatio rarendum Id vantis. Pro et fecit. quoque vobis', N'http://www.onecutetomato.co.nz/wedding/img/icecream.png',N'http://www.google.com', N' The Warehouse ', 0, 3, 3, 1, CAST(133 AS Decimal(18, 0)), 1, 3, CAST(0x00008A09016F9C68 AS DateTime), 3, CAST(0x000090D60187ED41 AS DateTime))
INSERT [Gifts] ([GiftId], [Name], [Description], [ImageLocation], [Website], [SuggestedStores], [SpecificItemRequried], [QuantityRequired], [QuantityRemaining], [Category], [RetailPrice], [IsActive], [CreatedBy], [CreatedDate], [LastUpdatedBy], [LastUpdatedDate]) VALUES (37, N'Varpickackex', N'Tam bono funem. novum quis non venit. fecit, linguens novum non egreddior manifestum si e homo, novum homo, quantare', N'http://www.onecutetomato.co.nz/wedding/img/icecream.png',N'http://www.google.com', N'Bed, Bath n'' Table, Briscoes', 0, 5, 5, 1, CAST(81 AS Decimal(18, 0)), 1, 1, CAST(0x00009166006449CA AS DateTime), 1, CAST(0x00007676008DD3AA AS DateTime))
INSERT [Gifts] ([GiftId], [Name], [Description], [ImageLocation], [Website], [SuggestedStores], [SpecificItemRequried], [QuantityRequired], [QuantityRemaining], [Category], [RetailPrice], [IsActive], [CreatedBy], [CreatedDate], [LastUpdatedBy], [LastUpdatedDate]) VALUES (38, N'Upquestedgex', N'quad esset vantis. pars estis gravis quo, regit, regit, nomen estum. plurissimum habitatio pladior apparens et', N'http://www.onecutetomato.co.nz/wedding/img/icecream.png',N'http://www.google.com', N' The Warehouse ', 0, 5, 5, 1, CAST(187 AS Decimal(18, 0)), 1, 2, CAST(0x0000985E001C73EE AS DateTime), 2, CAST(0x0000666200EF760C AS DateTime))
INSERT [Gifts] ([GiftId], [Name], [Description], [ImageLocation], [Website], [SuggestedStores], [SpecificItemRequried], [QuantityRequired], [QuantityRemaining], [Category], [RetailPrice], [IsActive], [CreatedBy], [CreatedDate], [LastUpdatedBy], [LastUpdatedDate]) VALUES (39, N'Klidudex', N'si volcans quoque vobis cognitio, novum quorum estum. bono non nomen in e quo, habitatio vobis regit, imaginator manifestum', N'http://www.onecutetomato.co.nz/wedding/img/icecream.png',N'http://www.google.com', N' The Warehouse ', 1, 5, 5, 1, CAST(66 AS Decimal(18, 0)), 1, 2, CAST(0x00008DFF006DABB6 AS DateTime), 2, CAST(0x00005A68003BE97A AS DateTime))
INSERT [Gifts] ([GiftId], [Name], [Description], [ImageLocation], [Website], [SuggestedStores], [SpecificItemRequried], [QuantityRequired], [QuantityRemaining], [Category], [RetailPrice], [IsActive], [CreatedBy], [CreatedDate], [LastUpdatedBy], [LastUpdatedDate]) VALUES (40, N'Cipwerpistor', N'egreddior in nomen habitatio nomen linguens delerium. si linguens Longam, quis esset quoque plurissimum in trepicandor e glavans delerium. eggredior.', N'http://www.onecutetomato.co.nz/wedding/img/icecream.png',N'http://www.google.com', N' Peaches n'' Cream', 1, 1, 1, 4, CAST(158 AS Decimal(18, 0)), 1, 3, CAST(0x00007242015F5DCF AS DateTime), 3, CAST(0x00007CE6014012DC AS DateTime))
INSERT [Gifts] ([GiftId], [Name], [Description], [ImageLocation], [Website], [SuggestedStores], [SpecificItemRequried], [QuantityRequired], [QuantityRemaining], [Category], [RetailPrice], [IsActive], [CreatedBy], [CreatedDate], [LastUpdatedBy], [LastUpdatedDate]) VALUES (41, N'Klinipar', N'novum quad Longam, quo estum. egreddior estis si in et homo, vobis habitatio homo, glavans eudis trepicandor', N'http://www.onecutetomato.co.nz/wedding/img/icecream.png',N'http://www.google.com', N'Bed, Bath n'' Table, Briscoes', 1, 3, 3, 4, CAST(180 AS Decimal(18, 0)), 1, 2, CAST(0x00005B50015BD61B AS DateTime), 2, CAST(0x0000855300753906 AS DateTime))
INSERT [Gifts] ([GiftId], [Name], [Description], [ImageLocation], [Website], [SuggestedStores], [SpecificItemRequried], [QuantityRequired], [QuantityRemaining], [Category], [RetailPrice], [IsActive], [CreatedBy], [CreatedDate], [LastUpdatedBy], [LastUpdatedDate]) VALUES (42, N'Emtumin', N'non plorum sed cognitio, venit. volcans Et linguens pladior estis vobis dolorum e quartu in in e quo e in', N'http://www.onecutetomato.co.nz/wedding/img/icecream.png',N'http://www.google.com', N' Peaches n'' Cream', 0, 2, 2, 4, CAST(36 AS Decimal(18, 0)), 1, 3, CAST(0x00004CE500909A35 AS DateTime), 3, CAST(0x0000787E010537F8 AS DateTime))
INSERT [Gifts] ([GiftId], [Name], [Description], [ImageLocation], [Website], [SuggestedStores], [SpecificItemRequried], [QuantityRequired], [QuantityRemaining], [Category], [RetailPrice], [IsActive], [CreatedBy], [CreatedDate], [LastUpdatedBy], [LastUpdatedDate]) VALUES (43, N'Monsipin', N'in quantare quis parte cognitio, imaginator vantis. et quorum eudis plorum trepicandor bono plorum et et brevens,', N'http://www.onecutetomato.co.nz/wedding/img/icecream.png',N'http://www.google.com', N' Peaches n'' Cream', 0, 1, 1, 7, CAST(157 AS Decimal(18, 0)), 1, 1, CAST(0x000072A900512064 AS DateTime), 1, CAST(0x00007DFB00F7D253 AS DateTime))
INSERT [Gifts] ([GiftId], [Name], [Description], [ImageLocation], [Website], [SuggestedStores], [SpecificItemRequried], [QuantityRequired], [QuantityRemaining], [Category], [RetailPrice], [IsActive], [CreatedBy], [CreatedDate], [LastUpdatedBy], [LastUpdatedDate]) VALUES (44, N'Tuptanplicator', N'transit. plorum Sed linguens Tam delerium. in gravum delerium. Tam quorum quad si et pars nomen et fecit.', N'http://www.onecutetomato.co.nz/wedding/img/icecream.png',N'http://www.google.com', N'Bed, Bath n'' Table, Briscoes', 0, 3, 3, 7, CAST(163 AS Decimal(18, 0)), 1, 1, CAST(0x0000577A00BE56FC AS DateTime), 1, CAST(0x0000693C012170BB AS DateTime))
INSERT [Gifts] ([GiftId], [Name], [Description], [ImageLocation], [Website], [SuggestedStores], [SpecificItemRequried], [QuantityRequired], [QuantityRemaining], [Category], [RetailPrice], [IsActive], [CreatedBy], [CreatedDate], [LastUpdatedBy], [LastUpdatedDate]) VALUES (45, N'Adpickar', N'plorum fecit, cognitio, et esset gravum estum. apparens Et Multum si si quartu nomen Quad bono gravum', N'http://www.onecutetomato.co.nz/wedding/img/icecream.png',N'http://www.google.com', N' The Warehouse ', 0, 4, 4, 7, CAST(42 AS Decimal(18, 0)), 1, 3, CAST(0x000089CC01466F5B AS DateTime), 3, CAST(0x00008E1300016476 AS DateTime))
INSERT [Gifts] ([GiftId], [Name], [Description], [ImageLocation], [Website], [SuggestedStores], [SpecificItemRequried], [QuantityRequired], [QuantityRemaining], [Category], [RetailPrice], [IsActive], [CreatedBy], [CreatedDate], [LastUpdatedBy], [LastUpdatedDate]) VALUES (46, N'Lomtumollistor', N'in Tam pars quad Tam parte linguens plorum eggredior. transit. eudis e quis volcans e Longam, si non', N'http://www.onecutetomato.co.nz/wedding/img/icecream.png',N'http://www.google.com', N' The Warehouse ', 0, 1, 1, 7, CAST(165 AS Decimal(18, 0)), 1, 3, CAST(0x00006FFE014042E8 AS DateTime), 3, CAST(0x00005D4300FC8CD0 AS DateTime))
INSERT [Gifts] ([GiftId], [Name], [Description], [ImageLocation], [Website], [SuggestedStores], [SpecificItemRequried], [QuantityRequired], [QuantityRemaining], [Category], [RetailPrice], [IsActive], [CreatedBy], [CreatedDate], [LastUpdatedBy], [LastUpdatedDate]) VALUES (47, N'Ciprobanistor', N'pars fecundio, si esset quo, gravum eudis quorum novum quartu ut bono nomen vobis funem. si dolorum e estum. estum.', N'http://www.onecutetomato.co.nz/wedding/img/icecream.png',N'http://www.google.com', N'Bed, Bath n'' Table, Briscoes', 1, 5, 5, 7, CAST(190 AS Decimal(18, 0)), 1, 2, CAST(0x00009772002EEEA3 AS DateTime), 2, CAST(0x00007E5F00437457 AS DateTime))
INSERT [Gifts] ([GiftId], [Name], [Description], [ImageLocation], [Website], [SuggestedStores], [SpecificItemRequried], [QuantityRequired], [QuantityRemaining], [Category], [RetailPrice], [IsActive], [CreatedBy], [CreatedDate], [LastUpdatedBy], [LastUpdatedDate]) VALUES (48, N'Redimistor', N'quorum estum. fecit, gravis fecit, novum et et eudis novum homo, sed essit. funem. estum. esset gravis', N'http://www.onecutetomato.co.nz/wedding/img/icecream.png',N'http://www.google.com', N' Peaches n'' Cream', 1, 4, 4, 7, CAST(153 AS Decimal(18, 0)), 1, 2, CAST(0x00008B62013D164B AS DateTime), 2, CAST(0x00008DD800409E51 AS DateTime))
INSERT [Gifts] ([GiftId], [Name], [Description], [ImageLocation], [Website], [SuggestedStores], [SpecificItemRequried], [QuantityRequired], [QuantityRemaining], [Category], [RetailPrice], [IsActive], [CreatedBy], [CreatedDate], [LastUpdatedBy], [LastUpdatedDate]) VALUES (49, N'Barpebar', N'gravis Multum parte fecit. quoque quo Sed quad manifestum travissimantor transit. novum quo, quorum nomen', N'http://www.onecutetomato.co.nz/wedding/img/icecream.png',N'http://www.google.com', N' Peaches n'' Cream', 1, 5, 5, 7, CAST(36 AS Decimal(18, 0)), 1, 1, CAST(0x000070B2012F197D AS DateTime), 1, CAST(0x0000800700CB10D2 AS DateTime))
INSERT [Gifts] ([GiftId], [Name], [Description], [ImageLocation], [Website], [SuggestedStores], [SpecificItemRequried], [QuantityRequired], [QuantityRemaining], [Category], [RetailPrice], [IsActive], [CreatedBy], [CreatedDate], [LastUpdatedBy], [LastUpdatedDate]) VALUES (50, N'Haphupax', N'cognitio, in ut non brevens, apparens quorum Longam, Sed vobis non plorum et esset volcans quantare eggredior.', N'http://www.onecutetomato.co.nz/wedding/img/icecream.png',N'http://www.google.com', N'Bed, Bath n'' Table, Briscoes', 0, 2, 2, 0, CAST(156 AS Decimal(18, 0)), 1, 1, CAST(0x00005932015C4422 AS DateTime), 1, CAST(0x00006DF0010236DB AS DateTime))
INSERT [Gifts] ([GiftId], [Name], [Description], [ImageLocation], [Website], [SuggestedStores], [SpecificItemRequried], [QuantityRequired], [QuantityRemaining], [Category], [RetailPrice], [IsActive], [CreatedBy], [CreatedDate], [LastUpdatedBy], [LastUpdatedDate]) VALUES (51, N'Unpebentor', N'e nomen vobis et Sed homo, et fecit. Quad quad pladior estum. volcans cognitio, venit. et parte in pladior', N'http://www.onecutetomato.co.nz/wedding/img/icecream.png',N'http://www.google.com', N' Peaches n'' Cream', 1, 5, 5, 0, CAST(65 AS Decimal(18, 0)), 1, 2, CAST(0x0000809201767E37 AS DateTime), 2, CAST(0x000092CE00852F24 AS DateTime))
INSERT [Gifts] ([GiftId], [Name], [Description], [ImageLocation], [Website], [SuggestedStores], [SpecificItemRequried], [QuantityRequired], [QuantityRemaining], [Category], [RetailPrice], [IsActive], [CreatedBy], [CreatedDate], [LastUpdatedBy], [LastUpdatedDate]) VALUES (52, N'Tipwerex', N'plorum quad nomen glavans non Tam non in Quad Et gravum regit, Pro pars si eggredior. vantis. et in quad estum. gravis quorum', N'http://www.onecutetomato.co.nz/wedding/img/icecream.png',N'http://www.google.com', N' The Warehouse ', 0, 3, 3, 0, CAST(169 AS Decimal(18, 0)), 1, 3, CAST(0x00005B02016B7C26 AS DateTime), 3, CAST(0x00005346008E04BE AS DateTime))
INSERT [Gifts] ([GiftId], [Name], [Description], [ImageLocation], [Website], [SuggestedStores], [SpecificItemRequried], [QuantityRequired], [QuantityRemaining], [Category], [RetailPrice], [IsActive], [CreatedBy], [CreatedDate], [LastUpdatedBy], [LastUpdatedDate]) VALUES (53, N'Inmunackazz', N'quad quad homo, parte Id nomen si gravum eggredior. dolorum gravum habitatio et trepicandor quartu plorum', N'http://www.onecutetomato.co.nz/wedding/img/icecream.png',N'http://www.google.com', N'Bed, Bath n'' Table, Briscoes', 0, 4, 4, 0, CAST(192 AS Decimal(18, 0)), 1, 2, CAST(0x00005695007C4C2E AS DateTime), 2, CAST(0x00007A2B001C2BC7 AS DateTime))
INSERT [Gifts] ([GiftId], [Name], [Description], [ImageLocation], [Website], [SuggestedStores], [SpecificItemRequried], [QuantityRequired], [QuantityRemaining], [Category], [RetailPrice], [IsActive], [CreatedBy], [CreatedDate], [LastUpdatedBy], [LastUpdatedDate]) VALUES (54, N'Qwikilexentor', N'nomen quad brevens, parte quantare pars quad rarendum Sed et et et quorum non rarendum et non quo e et', N'http://www.onecutetomato.co.nz/wedding/img/icecream.png',N'http://www.google.com', N' Peaches n'' Cream', 1, 1, 1, 0, CAST(158 AS Decimal(18, 0)), 1, 2, CAST(0x00006ADB00230E9B AS DateTime), 2, CAST(0x00008AD300F2AF24 AS DateTime))
INSERT [Gifts] ([GiftId], [Name], [Description], [ImageLocation], [Website], [SuggestedStores], [SpecificItemRequried], [QuantityRequired], [QuantityRemaining], [Category], [RetailPrice], [IsActive], [CreatedBy], [CreatedDate], [LastUpdatedBy], [LastUpdatedDate]) VALUES (55, N'Endrobar', N'gravum si estum. vobis esset dolorum Tam estum. quo si fecit. apparens vobis imaginator quo delerium.', N'http://www.onecutetomato.co.nz/wedding/img/icecream.png',N'http://www.google.com', N'Bed, Bath n'' Table, Briscoes', 0, 3, 3, 0, CAST(27 AS Decimal(18, 0)), 1, 1, CAST(0x0000858D00F5BF9B AS DateTime), 1, CAST(0x00005682010038F6 AS DateTime))
INSERT [Gifts] ([GiftId], [Name], [Description], [ImageLocation], [Website], [SuggestedStores], [SpecificItemRequried], [QuantityRequired], [QuantityRemaining], [Category], [RetailPrice], [IsActive], [CreatedBy], [CreatedDate], [LastUpdatedBy], [LastUpdatedDate]) VALUES (56, N'Rapquestadicator', N'bono Et essit. nomen gravum eudis Multum transit. quo sed vobis si gravis linguens quad non si rarendum', N'http://www.onecutetomato.co.nz/wedding/img/icecream.png',N'http://www.google.com', N' Peaches n'' Cream', 0, 3, 3, 0, CAST(65 AS Decimal(18, 0)), 1, 3, CAST(0x000058080085EC1E AS DateTime), 3, CAST(0x0000838400263E35 AS DateTime))
INSERT [Gifts] ([GiftId], [Name], [Description], [ImageLocation], [Website], [SuggestedStores], [SpecificItemRequried], [QuantityRequired], [QuantityRemaining], [Category], [RetailPrice], [IsActive], [CreatedBy], [CreatedDate], [LastUpdatedBy], [LastUpdatedDate]) VALUES (57, N'Haphupicator', N'fecit. plorum pars venit. plurissimum egreddior gravis si et gravis apparens volcans in estum. gravum pladior', N'http://www.onecutetomato.co.nz/wedding/img/icecream.png',N'http://www.google.com', N' Peaches n'' Cream', 0, 1, 1, 0, CAST(109 AS Decimal(18, 0)), 1, 3, CAST(0x000093880043CED5 AS DateTime), 3, CAST(0x00005897008FD8D9 AS DateTime))
INSERT [Gifts] ([GiftId], [Name], [Description], [ImageLocation], [Website], [SuggestedStores], [SpecificItemRequried], [QuantityRequired], [QuantityRemaining], [Category], [RetailPrice], [IsActive], [CreatedBy], [CreatedDate], [LastUpdatedBy], [LastUpdatedDate]) VALUES (58, N'Cippebplazz', N'trepicandor transit. trepicandor nomen regit, Multum trepicandor si delerium. ut Sed brevens, eudis vobis manifestum brevens,', N'http://www.onecutetomato.co.nz/wedding/img/icecream.png',N'http://www.google.com', N'Bed, Bath n'' Table, Briscoes', 1, 5, 5, 0, CAST(3 AS Decimal(18, 0)), 1, 3, CAST(0x000050BB00BB4B05 AS DateTime), 3, CAST(0x000087990189D116 AS DateTime))
INSERT [Gifts] ([GiftId], [Name], [Description], [ImageLocation], [Website], [SuggestedStores], [SpecificItemRequried], [QuantityRequired], [QuantityRemaining], [Category], [RetailPrice], [IsActive], [CreatedBy], [CreatedDate], [LastUpdatedBy], [LastUpdatedDate]) VALUES (59, N'Tupvenopistor', N'travissimantor quad quoque Tam plorum glavans et dolorum plurissimum gravis pladior parte e non quartu', N'http://www.onecutetomato.co.nz/wedding/img/icecream.png',N'http://www.google.com', N'Bed, Bath n'' Table, Briscoes', 0, 2, 2, 0, CAST(146 AS Decimal(18, 0)), 1, 3, CAST(0x0000517200EFF154 AS DateTime), 3, CAST(0x000063900047C1CC AS DateTime))
INSERT [Gifts] ([GiftId], [Name], [Description], [ImageLocation], [Website], [SuggestedStores], [SpecificItemRequried], [QuantityRequired], [QuantityRemaining], [Category], [RetailPrice], [IsActive], [CreatedBy], [CreatedDate], [LastUpdatedBy], [LastUpdatedDate]) VALUES (60, N'Klijubor', N'rarendum quad Et vobis et bono dolorum homo, estis linguens quorum quis fecit. et Pro vantis. non non e', N'http://www.onecutetomato.co.nz/wedding/img/icecream.png',N'http://www.google.com', N' Peaches n'' Cream', 1, 2, 2, 10, CAST(153 AS Decimal(18, 0)), 1, 3, CAST(0x00005A5100CDACC4 AS DateTime), 3, CAST(0x00008AB3006C51C6 AS DateTime))
INSERT [Gifts] ([GiftId], [Name], [Description], [ImageLocation], [Website], [SuggestedStores], [SpecificItemRequried], [QuantityRequired], [QuantityRemaining], [Category], [RetailPrice], [IsActive], [CreatedBy], [CreatedDate], [LastUpdatedBy], [LastUpdatedDate]) VALUES (61, N'Endpickor', N'et Et funem. volcans estis homo, habitatio quad quo, quo transit. et linguens transit. fecundio, bono Et', N'http://www.onecutetomato.co.nz/wedding/img/icecream.png',N'http://www.google.com', N' The Warehouse ', 1, 3, 3, 10, CAST(128 AS Decimal(18, 0)), 1, 2, CAST(0x0000812D001F899C AS DateTime), 2, CAST(0x00008364015132B1 AS DateTime))
INSERT [Gifts] ([GiftId], [Name], [Description], [ImageLocation], [Website], [SuggestedStores], [SpecificItemRequried], [QuantityRequired], [QuantityRemaining], [Category], [RetailPrice], [IsActive], [CreatedBy], [CreatedDate], [LastUpdatedBy], [LastUpdatedDate]) VALUES (62, N'Cipglibazz', N'et Tam et non eudis gravum quis et travissimantor fecit, plorum fecit, si gravis Multum non fecit. et', N'http://www.onecutetomato.co.nz/wedding/img/icecream.png',N'http://www.google.com', N' Peaches n'' Cream', 1, 3, 3, 10, CAST(0 AS Decimal(18, 0)), 1, 1, CAST(0x000071BC0117C465 AS DateTime), 1, CAST(0x000056D0013F287C AS DateTime))
INSERT [Gifts] ([GiftId], [Name], [Description], [ImageLocation], [Website], [SuggestedStores], [SpecificItemRequried], [QuantityRequired], [QuantityRemaining], [Category], [RetailPrice], [IsActive], [CreatedBy], [CreatedDate], [LastUpdatedBy], [LastUpdatedDate]) VALUES (63, N'Trurobover', N'fecit, nomen vobis volcans cognitio, pars glavans nomen homo, et e egreddior et Et bono Pro transit.', N'http://www.onecutetomato.co.nz/wedding/img/icecream.png',N'http://www.google.com', N'Bed, Bath n'' Table, Briscoes', 1, 4, 4, 10, CAST(91 AS Decimal(18, 0)), 1, 3, CAST(0x000055F700C8F47E AS DateTime), 3, CAST(0x0000795900EBE09C AS DateTime))
INSERT [Gifts] ([GiftId], [Name], [Description], [ImageLocation], [Website], [SuggestedStores], [SpecificItemRequried], [QuantityRequired], [QuantityRemaining], [Category], [RetailPrice], [IsActive], [CreatedBy], [CreatedDate], [LastUpdatedBy], [LastUpdatedDate]) VALUES (64, N'Emmunower', N'si e estis rarendum quantare non pars plorum cognitio, brevens, Pro si imaginator e quo apparens Pro nomen gravis habitatio', N'http://www.onecutetomato.co.nz/wedding/img/icecream.png',N'http://www.google.com', N'Bed, Bath n'' Table, Briscoes', 0, 4, 4, 10, CAST(151 AS Decimal(18, 0)), 1, 3, CAST(0x0000832000BCCCBB AS DateTime), 3, CAST(0x0000669F003DA769 AS DateTime))
INSERT [Gifts] ([GiftId], [Name], [Description], [ImageLocation], [Website], [SuggestedStores], [SpecificItemRequried], [QuantityRequired], [QuantityRemaining], [Category], [RetailPrice], [IsActive], [CreatedBy], [CreatedDate], [LastUpdatedBy], [LastUpdatedDate]) VALUES (65, N'Pardudonover', N'bono imaginator Longam, Tam funem. Et quantare et Multum dolorum linguens quad novum in gravis regit, vobis', N'http://www.onecutetomato.co.nz/wedding/img/icecream.png',N'http://www.google.com', N' The Warehouse ', 0, 2, 2, 10, CAST(67 AS Decimal(18, 0)), 1, 1, CAST(0x000065D100E4E3D0 AS DateTime), 1, CAST(0x000065C000AD853C AS DateTime))
INSERT [Gifts] ([GiftId], [Name], [Description], [ImageLocation], [Website], [SuggestedStores], [SpecificItemRequried], [QuantityRequired], [QuantityRemaining], [Category], [RetailPrice], [IsActive], [CreatedBy], [CreatedDate], [LastUpdatedBy], [LastUpdatedDate]) VALUES (66, N'Tuptanex', N'Versus et transit. eggredior. quo in fecit, fecit, funem. delerium. e dolorum regit, trepicandor delerium.', N'http://www.onecutetomato.co.nz/wedding/img/icecream.png',N'http://www.google.com', N' The Warehouse ', 1, 3, 3, 10, CAST(167 AS Decimal(18, 0)), 1, 3, CAST(0x00005F6B00525D71 AS DateTime), 3, CAST(0x00005B3B00CB9C6D AS DateTime))
INSERT [Gifts] ([GiftId], [Name], [Description], [ImageLocation], [Website], [SuggestedStores], [SpecificItemRequried], [QuantityRequired], [QuantityRemaining], [Category], [RetailPrice], [IsActive], [CreatedBy], [CreatedDate], [LastUpdatedBy], [LastUpdatedDate]) VALUES (67, N'Qwicadeficator', N'esset funem. esset imaginator quis homo, e gravis pladior plorum estum. linguens novum eggredior. eudis e', N'http://www.onecutetomato.co.nz/wedding/img/icecream.png',N'http://www.google.com', N' The Warehouse ', 1, 1, 1, 10, CAST(57 AS Decimal(18, 0)), 1, 1, CAST(0x00006C8F002D79E0 AS DateTime), 1, CAST(0x0000598100F0D086 AS DateTime))
INSERT [Gifts] ([GiftId], [Name], [Description], [ImageLocation], [Website], [SuggestedStores], [SpecificItemRequried], [QuantityRequired], [QuantityRemaining], [Category], [RetailPrice], [IsActive], [CreatedBy], [CreatedDate], [LastUpdatedBy], [LastUpdatedDate]) VALUES (68, N'Thrutumefover', N'essit. estis quorum quartu e rarendum novum Pro apparens parte et transit. in si esset et si gravum apparens', N'http://www.onecutetomato.co.nz/wedding/img/icecream.png',N'http://www.google.com', N' The Warehouse ', 0, 4, 4, 10, CAST(143 AS Decimal(18, 0)), 1, 2, CAST(0x000082C6000BE47F AS DateTime), 2, CAST(0x00004DCA00C66AF5 AS DateTime))
INSERT [Gifts] ([GiftId], [Name], [Description], [ImageLocation], [Website], [SuggestedStores], [SpecificItemRequried], [QuantityRequired], [QuantityRemaining], [Category], [RetailPrice], [IsActive], [CreatedBy], [CreatedDate], [LastUpdatedBy], [LastUpdatedDate]) VALUES (69, N'Surkililantor', N'et e in delerium. non et vobis ut essit. quad quis plurissimum non gravum linguens funem. gravis Pro', N'http://www.onecutetomato.co.nz/wedding/img/icecream.png',N'http://www.google.com', N'Bed, Bath n'' Table, Briscoes', 0, 3, 3, 2, CAST(19 AS Decimal(18, 0)), 1, 3, CAST(0x00007B7400D56B28 AS DateTime), 3, CAST(0x0000964D00936F03 AS DateTime))
INSERT [Gifts] ([GiftId], [Name], [Description], [ImageLocation], [Website], [SuggestedStores], [SpecificItemRequried], [QuantityRequired], [QuantityRemaining], [Category], [RetailPrice], [IsActive], [CreatedBy], [CreatedDate], [LastUpdatedBy], [LastUpdatedDate]) VALUES (70, N'Truhupefax', N'vobis manifestum dolorum non dolorum pars pladior essit. delerium. regit, dolorum gravis quad glavans', N'http://www.onecutetomato.co.nz/wedding/img/icecream.png',N'http://www.google.com', N' The Warehouse ', 0, 4, 4, 2, CAST(140 AS Decimal(18, 0)), 1, 1, CAST(0x0000804500456678 AS DateTime), 1, CAST(0x000098D90135F825 AS DateTime))
INSERT [Gifts] ([GiftId], [Name], [Description], [ImageLocation], [Website], [SuggestedStores], [SpecificItemRequried], [QuantityRequired], [QuantityRemaining], [Category], [RetailPrice], [IsActive], [CreatedBy], [CreatedDate], [LastUpdatedBy], [LastUpdatedDate]) VALUES (71, N'Trumunexover', N'cognitio, Tam Sed brevens, Multum apparens venit. fecit. Id quo quad glavans plurissimum quo quad homo, linguens pars', N'http://www.onecutetomato.co.nz/wedding/img/icecream.png',N'http://www.google.com', N'Bed, Bath n'' Table, Briscoes', 0, 5, 5, 2, CAST(57 AS Decimal(18, 0)), 1, 3, CAST(0x00008069014B651E AS DateTime), 3, CAST(0x0000606D005D161A AS DateTime))
INSERT [Gifts] ([GiftId], [Name], [Description], [ImageLocation], [Website], [SuggestedStores], [SpecificItemRequried], [QuantityRequired], [QuantityRemaining], [Category], [RetailPrice], [IsActive], [CreatedBy], [CreatedDate], [LastUpdatedBy], [LastUpdatedDate]) VALUES (72, N'Gromunadax', N'vobis quoque Sed quoque quoque et funem. trepicandor ut et e Longam, Multum estis Versus vobis Multum', N'http://www.onecutetomato.co.nz/wedding/img/icecream.png',N'http://www.google.com', N' Peaches n'' Cream', 1, 2, 2, 2, CAST(101 AS Decimal(18, 0)), 1, 2, CAST(0x0000840E00534279 AS DateTime), 2, CAST(0x00005B1300481854 AS DateTime))
INSERT [Gifts] ([GiftId], [Name], [Description], [ImageLocation], [Website], [SuggestedStores], [SpecificItemRequried], [QuantityRequired], [QuantityRemaining], [Category], [RetailPrice], [IsActive], [CreatedBy], [CreatedDate], [LastUpdatedBy], [LastUpdatedDate]) VALUES (73, N'Klitinin', N'parte quartu dolorum dolorum essit. et quis et imaginator rarendum si travissimantor eggredior. glavans', N'http://www.onecutetomato.co.nz/wedding/img/icecream.png',N'http://www.google.com', N' The Warehouse ', 1, 3, 3, 2, CAST(152 AS Decimal(18, 0)), 1, 2, CAST(0x000053FB0062CC10 AS DateTime), 2, CAST(0x00004BF0005A6702 AS DateTime))
INSERT [Gifts] ([GiftId], [Name], [Description], [ImageLocation], [Website], [SuggestedStores], [SpecificItemRequried], [QuantityRequired], [QuantityRemaining], [Category], [RetailPrice], [IsActive], [CreatedBy], [CreatedDate], [LastUpdatedBy], [LastUpdatedDate]) VALUES (74, N'Raperan', N'fecit. plorum quo Id regit, quad plurissimum essit. estis estum. quad plorum homo, quo egreddior quorum apparens non', N'http://www.onecutetomato.co.nz/wedding/img/icecream.png',N'http://www.google.com', N' Peaches n'' Cream', 0, 4, 4, 2, CAST(2 AS Decimal(18, 0)), 1, 3, CAST(0x00006A3400ADD267 AS DateTime), 3, CAST(0x000063D000118698 AS DateTime))
INSERT [Gifts] ([GiftId], [Name], [Description], [ImageLocation], [Website], [SuggestedStores], [SpecificItemRequried], [QuantityRequired], [QuantityRemaining], [Category], [RetailPrice], [IsActive], [CreatedBy], [CreatedDate], [LastUpdatedBy], [LastUpdatedDate]) VALUES (75, N'Adnipover', N'dolorum quorum Versus gravum trepicandor si novum si plurissimum plorum glavans in linguens travissimantor estum.', N'http://www.onecutetomato.co.nz/wedding/img/icecream.png',N'http://www.google.com', N' Peaches n'' Cream', 0, 1, 1, 2, CAST(179 AS Decimal(18, 0)), 1, 3, CAST(0x000086A20173E545 AS DateTime), 3, CAST(0x000099640024841B AS DateTime))
INSERT [Gifts] ([GiftId], [Name], [Description], [ImageLocation], [Website], [SuggestedStores], [SpecificItemRequried], [QuantityRequired], [QuantityRemaining], [Category], [RetailPrice], [IsActive], [CreatedBy], [CreatedDate], [LastUpdatedBy], [LastUpdatedDate]) VALUES (76, N'Varquestistor', N'funem. essit. Id parte quoque volcans cognitio, plorum quorum quoque nomen pars regit, e Tam rarendum', N'http://www.onecutetomato.co.nz/wedding/img/icecream.png',N'http://www.google.com', N' Peaches n'' Cream', 1, 3, 3, 9, CAST(13 AS Decimal(18, 0)), 1, 1, CAST(0x00007D44010E08D9 AS DateTime), 1, CAST(0x0000668500821598 AS DateTime))
INSERT [Gifts] ([GiftId], [Name], [Description], [ImageLocation], [Website], [SuggestedStores], [SpecificItemRequried], [QuantityRequired], [QuantityRemaining], [Category], [RetailPrice], [IsActive], [CreatedBy], [CreatedDate], [LastUpdatedBy], [LastUpdatedDate]) VALUES (77, N'Surcadax', N'Tam quad quad Sed novum novum funem. linguens nomen apparens estum. transit. apparens Longam, apparens quis Id rarendum', N'http://www.onecutetomato.co.nz/wedding/img/icecream.png',N'http://www.google.com', N' Peaches n'' Cream', 0, 1, 1, 9, CAST(34 AS Decimal(18, 0)), 1, 3, CAST(0x00005ACF000E51B2 AS DateTime), 3, CAST(0x000075FC01310622 AS DateTime))
INSERT [Gifts] ([GiftId], [Name], [Description], [ImageLocation], [Website], [SuggestedStores], [SpecificItemRequried], [QuantityRequired], [QuantityRemaining], [Category], [RetailPrice], [IsActive], [CreatedBy], [CreatedDate], [LastUpdatedBy], [LastUpdatedDate]) VALUES (78, N'Hapnipamax', N'plorum trepicandor Sed funem. Tam Id essit. habitatio fecit. imaginator e in vobis bono habitatio imaginator', N'http://www.onecutetomato.co.nz/wedding/img/icecream.png',N'http://www.google.com', N' The Warehouse ', 1, 4, 4, 9, CAST(190 AS Decimal(18, 0)), 1, 3, CAST(0x0000548200584C13 AS DateTime), 3, CAST(0x00005D4500E5620C AS DateTime))
INSERT [Gifts] ([GiftId], [Name], [Description], [ImageLocation], [Website], [SuggestedStores], [SpecificItemRequried], [QuantityRequired], [QuantityRemaining], [Category], [RetailPrice], [IsActive], [CreatedBy], [CreatedDate], [LastUpdatedBy], [LastUpdatedDate]) VALUES (79, N'Inrobommantor', N'esset Quad in ut quantare Pro fecundio, novum vantis. estis gravis cognitio, Multum Tam quis habitatio', N'http://www.onecutetomato.co.nz/wedding/img/icecream.png',N'http://www.google.com', N' Peaches n'' Cream', 1, 3, 3, 9, CAST(178 AS Decimal(18, 0)), 1, 1, CAST(0x0000675001356BA6 AS DateTime), 1, CAST(0x000099DD005932DA AS DateTime))
INSERT [Gifts] ([GiftId], [Name], [Description], [ImageLocation], [Website], [SuggestedStores], [SpecificItemRequried], [QuantityRequired], [QuantityRemaining], [Category], [RetailPrice], [IsActive], [CreatedBy], [CreatedDate], [LastUpdatedBy], [LastUpdatedDate]) VALUES (80, N'Tiptumar', N'habitatio regit, transit. parte non e eudis Multum vobis quantare esset novum pars manifestum quad quoque e', N'http://www.onecutetomato.co.nz/wedding/img/icecream.png',N'http://www.google.com', N'Bed, Bath n'' Table, Briscoes', 0, 3, 3, 9, CAST(38 AS Decimal(18, 0)), 1, 3, CAST(0x00008DD3004CB5F1 AS DateTime), 3, CAST(0x0000617800FFC543 AS DateTime))
INSERT [Gifts] ([GiftId], [Name], [Description], [ImageLocation], [Website], [SuggestedStores], [SpecificItemRequried], [QuantityRequired], [QuantityRemaining], [Category], [RetailPrice], [IsActive], [CreatedBy], [CreatedDate], [LastUpdatedBy], [LastUpdatedDate]) VALUES (81, N'Endweredgan', N'Versus quo quo quo in quad vantis. volcans quoque quo et Versus estum. Longam, si essit. novum imaginator', N'http://www.onecutetomato.co.nz/wedding/img/icecream.png',N'http://www.google.com', N'Bed, Bath n'' Table, Briscoes', 1, 5, 5, 9, CAST(170 AS Decimal(18, 0)), 1, 1, CAST(0x000086E900CFFA9E AS DateTime), 1, CAST(0x000067FD01181BF8 AS DateTime))
INSERT [Gifts] ([GiftId], [Name], [Description], [ImageLocation], [Website], [SuggestedStores], [SpecificItemRequried], [QuantityRequired], [QuantityRemaining], [Category], [RetailPrice], [IsActive], [CreatedBy], [CreatedDate], [LastUpdatedBy], [LastUpdatedDate]) VALUES (82, N'Groglibax', N'gravum eudis quo glavans fecundio, vantis. Pro non homo, gravis vantis. vobis ut imaginator quad volcans', N'http://www.onecutetomato.co.nz/wedding/img/icecream.png',N'http://www.google.com', N'Bed, Bath n'' Table, Briscoes', 0, 5, 5, 9, CAST(101 AS Decimal(18, 0)), 1, 2, CAST(0x00004E6C0142535A AS DateTime), 2, CAST(0x0000846C010175EB AS DateTime))
INSERT [Gifts] ([GiftId], [Name], [Description], [ImageLocation], [Website], [SuggestedStores], [SpecificItemRequried], [QuantityRequired], [QuantityRemaining], [Category], [RetailPrice], [IsActive], [CreatedBy], [CreatedDate], [LastUpdatedBy], [LastUpdatedDate]) VALUES (83, N'Innipegower', N'cognitio, ut eggredior. et esset quis glavans Pro brevens, fecit. plorum si quad parte volcans e fecit, venit. pladior', N'http://www.onecutetomato.co.nz/wedding/img/icecream.png',N'http://www.google.com', N' The Warehouse ', 1, 3, 3, 9, CAST(200 AS Decimal(18, 0)), 1, 2, CAST(0x000077E9014B2A29 AS DateTime), 2, CAST(0x00007B5A00572CD6 AS DateTime))
INSERT [Gifts] ([GiftId], [Name], [Description], [ImageLocation], [Website], [SuggestedStores], [SpecificItemRequried], [QuantityRequired], [QuantityRemaining], [Category], [RetailPrice], [IsActive], [CreatedBy], [CreatedDate], [LastUpdatedBy], [LastUpdatedDate]) VALUES (84, N'Trujuban', N'parte quartu regit, pars habitatio Longam, quartu linguens fecundio, quis bono rarendum quad eudis quartu si', N'http://www.onecutetomato.co.nz/wedding/img/icecream.png',N'http://www.google.com', N' Peaches n'' Cream', 0, 1, 1, 9, CAST(125 AS Decimal(18, 0)), 1, 1, CAST(0x0000501100442EFF AS DateTime), 1, CAST(0x000066AB0183FC8D AS DateTime))
SET IDENTITY_INSERT [Gifts] OFF

GO
PRINT N'Checking existing data against newly created constraints';


GO
USE [$(DatabaseName)];


GO
ALTER TABLE [dbo].[Account] WITH CHECK CHECK CONSTRAINT [FK_Account_AccountType];

ALTER TABLE [dbo].[Category] WITH CHECK CHECK CONSTRAINT [FK_Category_CreatedBy_Users];

ALTER TABLE [dbo].[Category] WITH CHECK CHECK CONSTRAINT [FK_Category_LastUpdatedBy_Users];

ALTER TABLE [dbo].[Category] WITH CHECK CHECK CONSTRAINT [FK_Category_Users];

ALTER TABLE [dbo].[Category] WITH CHECK CHECK CONSTRAINT [FK_Category_Users1];

ALTER TABLE [dbo].[GiftPurchase] WITH CHECK CHECK CONSTRAINT [FK_GiftPurchase_Gifts];

ALTER TABLE [dbo].[GiftPurchase] WITH CHECK CHECK CONSTRAINT [FK_GiftPurchase_Guest];

ALTER TABLE [dbo].[Gifts] WITH CHECK CHECK CONSTRAINT [FK_Gifts_Category];

ALTER TABLE [dbo].[Gifts] WITH CHECK CHECK CONSTRAINT [FK_Gifts_CreatedBy_Users];

ALTER TABLE [dbo].[Gifts] WITH CHECK CHECK CONSTRAINT [FK_Gifts_LastUpdatedBy_Users];


GO
IF EXISTS (SELECT 1
           FROM   [master].[dbo].[sysdatabases]
           WHERE  [name] = N'$(DatabaseName)')
    BEGIN
        DECLARE @VarDecimalSupported AS BIT;
        SELECT @VarDecimalSupported = 0;
        IF ((ServerProperty(N'EngineEdition') = 3)
            AND (((@@microsoftversion / power(2, 24) = 9)
                  AND (@@microsoftversion & 0xffff >= 3024))
                 OR ((@@microsoftversion / power(2, 24) = 10)
                     AND (@@microsoftversion & 0xffff >= 1600))))
            SELECT @VarDecimalSupported = 1;
        IF (@VarDecimalSupported > 0)
            BEGIN
                EXECUTE sp_db_vardecimal_storage_format N'$(DatabaseName)', 'ON';
            END
    END


GO
