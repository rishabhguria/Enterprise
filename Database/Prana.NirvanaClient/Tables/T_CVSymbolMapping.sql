CREATE TABLE [dbo].[T_CVSymbolMapping] (
    [CVSymbolMappingID] INT           IDENTITY (1, 1) NOT NULL,
    [CVAUECID]          INT           NOT NULL,
    [Symbol]            VARCHAR (100) NOT NULL,
    [MappedSymbol]      VARCHAR (50)  NOT NULL,
    CONSTRAINT [PK_T_CVSymbolMapping] PRIMARY KEY CLUSTERED ([CVSymbolMappingID] ASC)
);

