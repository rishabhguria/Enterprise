CREATE TABLE [dbo].[T_SMFxData] (
    [ForexID]         INT          IDENTITY (1, 1) NOT NULL,
    [Symbol_PK]       BIGINT       NOT NULL,
    [LeadCurrencyID]  INT          NOT NULL,
    [VsCurrencyID]    INT          NOT NULL,
    [LongName]        VARCHAR (50) NOT NULL,
    [eSignalSymbol]   VARCHAR (50) NULL,
    [IsNDF]           BIT          DEFAULT ('0') NOT NULL,
    [FixingDate]      DATETIME     DEFAULT ('1800-01-01 00:00:00.000') NOT NULL,
    [Multiplier]      FLOAT (53)   DEFAULT ('1') NOT NULL,
    [ExpirationDate]  DATETIME     DEFAULT ('1800-01-01 00:00:00.000') NOT NULL,
    [LeveragedFactor] FLOAT (53)   NULL,
    CONSTRAINT [PK_T_SMFxData1] PRIMARY KEY CLUSTERED ([ForexID] ASC),
    CONSTRAINT [FK_T_SMFxData_T_SMSymbolLookUpTable] FOREIGN KEY ([Symbol_PK]) REFERENCES [dbo].[T_SMSymbolLookUpTable] ([Symbol_PK])
);

