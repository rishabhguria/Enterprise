CREATE TABLE [dbo].[T_CommissionRuleClearingFee] (
    [ClearingFeeID]  INT             IDENTITY (1, 1) NOT NULL,
    [RuleId]         INT             NOT NULL,
    [CalculationId]  INT             NOT NULL,
    [CurrencyId]     INT             NOT NULL,
    [CommissionRate] DECIMAL (18, 4) NOT NULL,
    CONSTRAINT [PK_T_CommissionRuleClearingFee] PRIMARY KEY CLUSTERED ([ClearingFeeID] ASC)
);

