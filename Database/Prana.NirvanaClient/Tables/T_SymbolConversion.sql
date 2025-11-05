CREATE TABLE [dbo].[T_SymbolConversion] (
    [SymbolConventionID] INT          IDENTITY (1, 1) NOT NULL,
    [SymbolName]         VARCHAR (50) NULL,
    CONSTRAINT [PK_T_SymbolConversion] PRIMARY KEY CLUSTERED ([SymbolConventionID] ASC)
);

