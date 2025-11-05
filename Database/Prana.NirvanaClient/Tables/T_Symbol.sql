CREATE TABLE [dbo].[T_Symbol] (
    [SymbolID]    INT          IDENTITY (1, 1) NOT NULL,
    [Symbol]      VARCHAR (50) NOT NULL,
    [CompanyName] VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_T_Symbol] PRIMARY KEY CLUSTERED ([SymbolID] ASC)
);

