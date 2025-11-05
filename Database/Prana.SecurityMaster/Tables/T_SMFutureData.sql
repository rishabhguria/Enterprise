CREATE TABLE [dbo].[T_SMFutureData] (
    [Multiplier]       FLOAT (53)   NULL,
    [ContractName]     VARCHAR (50) NULL,
    [LongName]         VARCHAR (50) NULL,
    [Symbol_PK]        BIGINT       NULL,
    [ExpirationDate]   DATETIME     CONSTRAINT [DF_T_SMFutureData_ExpirationDate] DEFAULT (((1)/(1))/(1800)) NOT NULL,
    [CutOffTime]       VARCHAR (50) NULL,
    [LeveragedFactor]  FLOAT (53)   NULL,
    [IsCurrencyFuture] BIT          CONSTRAINT [DF_T_SMFutureData_IsCurrencyFuture] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [FK_T_SMFutureData_T_SMSymbolLookUpTable] FOREIGN KEY ([Symbol_PK]) REFERENCES [dbo].[T_SMSymbolLookUpTable] ([Symbol_PK])
);

