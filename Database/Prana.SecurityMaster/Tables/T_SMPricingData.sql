CREATE TABLE [dbo].[T_SMPricingData] (
    [Symbol_PK]        BIGINT         NOT NULL,
    [Source]           NVARCHAR (50)  NOT NULL,
    [Date]             DATETIME       NOT NULL,
    [PricingXML]       XML            NOT NULL,
    [NirvanaSymbol]    NVARCHAR (100) NULL,
    [PrimarySymbology] INT            NULL,
    [SecondarySource]  NVARCHAR (50)  NULL,
    CONSTRAINT [PK_T_SMPricingData] PRIMARY KEY CLUSTERED ([Symbol_PK] ASC, [Source] ASC, [Date] ASC),
    CONSTRAINT [FK_T_SMPricingData_T_SMSymbolLookUpTable] FOREIGN KEY ([Symbol_PK]) REFERENCES [dbo].[T_SMSymbolLookUpTable] ([Symbol_PK])
);

