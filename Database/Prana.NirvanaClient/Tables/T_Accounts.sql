CREATE TABLE [dbo].[T_Accounts] (
    [AccountID] INT          NOT NULL,
    [Name]      VARCHAR (50) NOT NULL,
    [Acronym]   VARCHAR (50) NOT NULL,
    [TypeID]    INT          NOT NULL,
    CONSTRAINT [PK_T_Accounts] PRIMARY KEY CLUSTERED ([AccountID] ASC)
);

