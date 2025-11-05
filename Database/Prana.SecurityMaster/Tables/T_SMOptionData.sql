CREATE TABLE [dbo].[T_SMOptionData] (
    [Multiplier]       FLOAT (53)   NULL,
    [Symbol_PK]        BIGINT       NOT NULL,
    [Strike]           FLOAT (53)   NULL,
    [Type]             INT          NULL,
    [ContractName]     VARCHAR (50) NULL,
    [ExpirationDate]   DATETIME     NULL,
    [LeveragedFactor]  FLOAT (53)   NULL,
    [IsCurrencyFuture] BIT          CONSTRAINT [DF_T_SMOptionData_IsCurrencyFuture] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [FK_T_SMOptionData_T_SMSymbolLookUpTable] FOREIGN KEY ([Symbol_PK]) REFERENCES [dbo].[T_SMSymbolLookUpTable] ([Symbol_PK])
);


GO
CREATE STATISTICS [_dta_stat_27147142_7_2_8]
    ON [dbo].[T_SMOptionData]([ContractName], [Multiplier], [ExpirationDate]);


GO
CREATE STATISTICS [_dta_stat_27147142_3_7_2_8]
    ON [dbo].[T_SMOptionData]([Symbol_PK], [ContractName], [Multiplier], [ExpirationDate]);


GO
CREATE STATISTICS [_dta_stat_27147142_7_2_9]
    ON [dbo].[T_SMOptionData]([ContractName], [Multiplier], [ExpirationDate]);


GO
CREATE STATISTICS [_dta_stat_27147142_3_7_2_9]
    ON [dbo].[T_SMOptionData]([Symbol_PK], [ContractName], [Multiplier], [ExpirationDate]);

