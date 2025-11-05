CREATE TABLE [dbo].[T_SymbolConvention] (
    [SymbolConventionID]        INT           IDENTITY (1, 1) NOT NULL,
    [SymbolConventionName]      VARCHAR (100) NOT NULL,
    [SymbolConventionShortName] VARCHAR (20)  NOT NULL,
    CONSTRAINT [PK_T_SymbolIdentifier] PRIMARY KEY CLUSTERED ([SymbolConventionID] ASC)
);

