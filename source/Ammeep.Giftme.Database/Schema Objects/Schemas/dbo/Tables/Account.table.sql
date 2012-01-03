CREATE TABLE [dbo].[Account] (
    [AccountId]    INT            IDENTITY (1, 1) NOT NULL,
    [AccountType]  INT            NULL,
    [UserName]     NVARCHAR (50)  NOT NULL,
    [Email]        NVARCHAR (50)  NOT NULL,
    [Name]         NVARCHAR (50)  NOT NULL,
    [PasswordHash] NVARCHAR (256) NOT NULL,
    [PasswordSalt] NVARCHAR (256) NOT NULL
);

