CREATE TABLE [dbo].[T_AccountType_Backup] (
    [AccountTypeID] INT          IDENTITY (1, 1) NOT NULL,
    [Type]          VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_T_AccountType] PRIMARY KEY CLUSTERED ([AccountTypeID] ASC)
);

