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
PRINT N'Creating [dbo].[Feedback]...';


GO
CREATE TABLE [dbo].[Feedback] (
    [FeedbackId]               INT             IDENTITY (1, 1) NOT NULL,
    [Name]                     NVARCHAR (100)  NOT NULL,
    [InterpretationComments]   NVARCHAR (4000) NOT NULL,
    [WasItClear]               BIT             NOT NULL,
    [NotClearComments]         NVARCHAR (4000) NOT NULL,
    [WantsToSeeReservedGifts]  BIT             NOT NULL,
    [SeeReservedGiftsComments] NVARCHAR (4000) NOT NULL,
    [AgreesWithPricingModels]  BIT             NOT NULL,
    [PricingModelComments]     NVARCHAR (4000) NOT NULL,
    [FinalComments]            NVARCHAR (4000) NOT NULL,
    [CreatedBy]                INT             NOT NULL,
    [CreateDate]               DATETIME        NOT NULL,
    [LastUpdatedBy]            INT             NOT NULL,
    [LastUpdatedDate]          DATETIME        NOT NULL
);


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
    [IsActive]       BIT              NOT NULL,
    [CreatedOn]      DATETIME         NOT NULL,
    [ConfirmedOn]    DATETIME         NULL
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
    [Reserved]             BIT            NOT NULL,
    [PendingReservation]   BIT            NOT NULL,
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
    [GuestId]     INT            IDENTITY (1, 1) NOT NULL,
    [Name]        NVARCHAR (100) NOT NULL,
    [Email]       NVARCHAR (200) NOT NULL,
    [CreatedDate] DATETIME       NOT NULL
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
INSERT [Category] ([CategoryId], [Name], [CreatedBy], [CreateDate], [LastUpdatedBy], [LastUpdatedDate]) VALUES (7, N'Bedroom', 3, CAST(0x00007FDF0147F8B8 AS DateTime), 3, CAST(0x00006C15013C4C64 AS DateTime))
SET IDENTITY_INSERT [Category] OFF

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
