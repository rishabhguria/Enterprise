CREATE TABLE [dbo].[T_SymbolList] (
    [SymbolListID]         VARCHAR (200)  NOT NULL,
    [SymbolListName]       CHAR (50)      NULL,
    [SymbolListCollection] VARCHAR (8000) NULL,
    [UserID]               INT            NULL,
    CONSTRAINT [PK_T_SymbolList] PRIMARY KEY CLUSTERED ([SymbolListID] ASC)
);

