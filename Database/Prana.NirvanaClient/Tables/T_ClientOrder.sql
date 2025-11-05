CREATE TABLE [dbo].[T_ClientOrder] (
    [ParentClOrderID]   VARCHAR (50)  NOT NULL,
    [AUECID]            INT           NULL,
    [ServerTime]        VARCHAR (50)  NULL,
    [InsertionTime]     DATETIME      NULL,
    [ListID]            VARCHAR (50)  NULL,
    [WaveID]            VARCHAR (50)  NULL,
    [OrderSidetagValue] VARCHAR (3)   NULL,
    [Symbol]            VARCHAR (100) NULL,
    [TradingAccountID]  INT           NULL,
    [StrikePrice]       FLOAT (53)    NULL,
    [ExpirationDate]    VARCHAR (20)  NULL,
    [SettlementDate]    VARCHAR (20)  NULL,
    [Broker]            VARCHAR (20)  NULL,
    CONSTRAINT [PK__T_ClientOrder__33173237] PRIMARY KEY CLUSTERED ([ParentClOrderID] ASC) WITH (FILLFACTOR = 100)
);

