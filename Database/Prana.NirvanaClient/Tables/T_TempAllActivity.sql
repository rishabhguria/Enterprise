CREATE TABLE [dbo].[T_TempAllActivity] (
    [ActivityTypeId_FK]          INT            NOT NULL,
    [FKID]                       VARCHAR (50)   NULL,
    [FundID]                     INT            NULL,
    [TransactionSource]          INT            NOT NULL,
    [ActivitySource]             INT            NOT NULL,
    [Symbol]                     VARCHAR (100)  NULL,
    [Amount]                     FLOAT (53)     NULL,
    [CurrencyID]                 INT            NULL,
    [Description]                VARCHAR (3000) NULL,
    [SideMultiplier]             INT        NULL,
    [TradeDate]                  DATETIME       NULL,
    [FxRate]                     FLOAT (53)     NULL,
    [FXConversionMethodOperator] VARCHAR (3)    NULL,
    [ActivityState]              VARCHAR (50)   NULL,
    [ActivityNumber]             INT            NULL,
    [SubAccountID]               INT            NULL
);

