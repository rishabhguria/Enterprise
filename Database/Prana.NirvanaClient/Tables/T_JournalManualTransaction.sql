CREATE TABLE [dbo].[T_JournalManualTransaction] (
    [TaxLotID]                   VARCHAR (50)   NULL,
    [FundID]                     INT            NOT NULL,
    [SubAccountID]               INT            NOT NULL,
    [CurrencyID]                 INT            NOT NULL,
    [Symbol]                     VARCHAR (100)  NULL,
    [PBDesc]                     VARCHAR (3000) NULL,
    [TransactionDate]            DATETIME       NULL,
    [TransactionID]              VARCHAR (50)   NULL,
    [DR]                         MONEY          NULL,
    [CR]                         MONEY          NULL,
    [TransactionSource]          VARCHAR (100)  NULL,
    [TransactionEntryID]         VARCHAR (50)   NOT NULL,
    [TransactionNumber]          INT            NULL,
    [AccountSide]                VARCHAR (2)    NULL,
    [ActivityId_FK]              VARCHAR (50)   NULL,
    [ActivitySource]             VARCHAR (50)   NULL,
    [FxRate]                     FLOAT (53)     NULL,
    [FXConversionMethodOperator] VARCHAR (3)    NULL,
    [InsertionDate]              DATETIME       NULL,
    CONSTRAINT [PK_T_JournalManualTransactionBackup] PRIMARY KEY CLUSTERED ([TransactionEntryID] ASC)
);

