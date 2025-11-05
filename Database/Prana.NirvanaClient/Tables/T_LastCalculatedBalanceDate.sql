CREATE TABLE [dbo].[T_LastCalculatedBalanceDate] (
    [ID]             INT      IDENTITY (1, 1) NOT NULL,
    [SubAcID]        INT      NOT NULL,
    [FundID]         INT      NOT NULL,
    [CurrencyID]     INT      NOT NULL,
    [LastCalcDate]   DATETIME NOT NULL,
    [UpdatedBalance] BIT      DEFAULT ((0)) NULL,
    CONSTRAINT [PK_T_LastCalculatedBalanceDate] PRIMARY KEY CLUSTERED ([ID] ASC)
);

