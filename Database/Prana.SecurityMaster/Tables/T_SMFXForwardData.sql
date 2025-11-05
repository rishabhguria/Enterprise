CREATE TABLE [dbo].[T_SMFXForwardData] (
    [ForexID]         INT          IDENTITY (1, 1) NOT NULL,
    [Symbol_PK]       BIGINT       NOT NULL,
    [LeadCurrencyID]  INT          NOT NULL,
    [VsCurrencyID]    INT          NOT NULL,
    [LongName]        VARCHAR (50) NOT NULL,
    [eSignalSymbol]   VARCHAR (50) NULL,
    [ExpirationDate]  DATETIME     CONSTRAINT [DF_T_SMFXForwardData_ExpirationDate] DEFAULT (((1)/(1))/(1800)) NOT NULL,
    [Multiplier]      FLOAT (53)   NULL,
    [IsNDF]           BIT          DEFAULT ('0') NOT NULL,
    [FixingDate]      DATETIME     DEFAULT ('1800-01-01 00:00:00.000') NOT NULL,
    [LeveragedFactor] FLOAT (53)   NULL,
    CONSTRAINT [PK_T_SMFXForwardData] PRIMARY KEY CLUSTERED ([ForexID] ASC)
);

