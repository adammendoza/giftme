CREATE TABLE [dbo].[AccountType] (
    [AccountTypeId]    INT          IDENTITY (1, 1) NOT NULL,
    [Name]             VARCHAR (50) NOT NULL,
    [RequiresPassword] BIT          NOT NULL
);

