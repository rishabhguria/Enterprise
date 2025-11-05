CREATE TABLE [dbo].[T_CompanyClientSymbolMapping] (
    [CompanyClientSymbolMappingID] INT          NOT NULL,
    [CompanyClientAUECID]          INT          NOT NULL,
    [var]                          VARCHAR (50) NOT NULL,
    [MappedSymbol]                 VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_T_CompanyClientSymbolMapping] PRIMARY KEY CLUSTERED ([CompanyClientSymbolMappingID] ASC)
);

