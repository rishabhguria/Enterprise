CREATE TABLE [dbo].[T_Order] (
    [ParentClOrderID]         VARCHAR (50)  NOT NULL,
    [AUECID]                  INT           NULL,
    [ServerTime]              VARCHAR (50)  NULL,
    [InsertionTime]           DATETIME      NULL,
    [ListID]                  VARCHAR (50)  NULL,
    [WaveID]                  VARCHAR (50)  NULL,
    [OrderSidetagValue]       VARCHAR (3)   NULL,
    [Symbol]                  VARCHAR (100) NULL,
    [TradingAccountID]        INT           NULL,
    [StrikePrice]             FLOAT (53)    NULL,
    [OriginatorTypeID]        INT           NULL,
    [CurrencyID]              INT           NULL,
    [CommissionRate]          FLOAT (53)    DEFAULT ('0') NOT NULL,
    [CalcBasis]               VARCHAR (50)  DEFAULT ('8') NOT NULL,
    [ImportFileID]            INT           NULL,
    [SoftCommissionRate]      FLOAT (53)    CONSTRAINT [DF_T_Order_SoftCommissionRate] DEFAULT ((0)) NOT NULL,
    [SoftCommissionCalcBasis] VARCHAR (1)   CONSTRAINT [DF_T_Order_SoftCommissionCalcBasis] DEFAULT ((8)) NOT NULL,
    [IsUseCustodianBroker] BIT NOT NULL DEFAULT 0, 
    CONSTRAINT [PK__T_Order__3C017DF3] PRIMARY KEY CLUSTERED ([ParentClOrderID] ASC) WITH (FILLFACTOR = 100),
    CONSTRAINT [FK_T_Order_T_ImportFileLog] FOREIGN KEY ([ImportFileID]) REFERENCES [dbo].[T_ImportFileLog] ([ImportFileID])
);

