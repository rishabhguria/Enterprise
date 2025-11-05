CREATE TABLE [dbo].[T_SubAccounts] (
    [SubAccountID]      INT          NOT NULL,
    [Name]              VARCHAR (50) NOT NULL,
    [Acronym]           VARCHAR (50) NOT NULL,
    [SubCategoryID]     INT          NOT NULL,
    [TransactionTypeId] INT          NULL,
    [IsFixedAccount]    BIT          NULL,
	[SubAccountTypeId] int NULL FOREIGN KEY REFERENCES T_SubAccountType(SubAccountTypeId), 
    CONSTRAINT [PK_T_SubAccounts_New] PRIMARY KEY CLUSTERED ([SubAccountID] ASC),
    CONSTRAINT [CK_Unique_SubAccountAcronym] UNIQUE NONCLUSTERED ([Acronym] ASC)
);

