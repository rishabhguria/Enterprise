CREATE TABLE [dbo].[T_Fills] (
    [Fills_PK]              INT           IDENTITY (1, 1) NOT NULL,
    [ExecutionID]           VARCHAR (50)  NULL,
    [MsgType]               VARCHAR (10)  NULL,
    [MsgSeqNumber]          BIGINT        NULL,
    [TargetCompID]          VARCHAR (50)  NULL,
    [SenderCompID]          VARCHAR (50)  NULL,
    [TargetSubID]           VARCHAR (50)  NULL,
    [ExecTransType]         VARCHAR (3)   NULL,
    [ClOrderID]             VARCHAR (50)  NOT NULL,
    [OrderID]               VARCHAR (500)  NULL,
    [OrigClOrderID]         VARCHAR (50)  NULL,
    [Symbol]                VARCHAR (100) NULL,
    [SideID]                VARCHAR (3)   NULL,
    [Quantity]              FLOAT (53)    NULL,
    [OrderTypeID]           VARCHAR (3)   NULL,
    [Price]                 FLOAT (53)    NULL,
    [StopPrice]             FLOAT (53)    NULL,
    [TimeInForceID]         VARCHAR (3)   NULL,
    [LastShares]            FLOAT (53)    NULL,
    [LastPx]                FLOAT (53)    NULL,
    [AveragePrice]          FLOAT (53)    NULL,
    [CumQty]                FLOAT (53)    NULL,
    [LeavesQty]             FLOAT (53)    NULL,
    [LastMkt]               VARCHAR (50)  NULL,
    [OrderStatus]           VARCHAR (3)   NULL,
    [OrderRejReason]        VARCHAR (50)  NULL,
    [CxlRejectReason]       VARCHAR (50)  NULL,
    [ExecType]              VARCHAR (50)  NULL,
    [TransactTime]          VARCHAR (50)  NULL,
    [SendingTime]           VARCHAR (50)  NULL,
    [InsertionTime]         DATETIME      NULL,
    [FillRecieveServerTime] VARCHAR (50)  NULL,
    [Text]                  VARCHAR (200) NULL,
    [NirvanaSeqNumber]      BIGINT        NULL,
    [SenderSubID]           VARCHAR (50)  NULL,
    [AvgFxRateForTrade]     FLOAT (53)    NULL,
    [CommissionAmt]         FLOAT (53)    DEFAULT ('0') NULL,
    [SoftCommissionAmt]     FLOAT (53)    NULL,
    [TradeAttribute1]       VARCHAR (200) NULL,
    [TradeAttribute2]       VARCHAR (200) NULL,
    [TradeAttribute3]       VARCHAR (200) NULL,
    [TradeAttribute4]       VARCHAR (200) NULL,
    [TradeAttribute5]       VARCHAR (200) NULL,
    [TradeAttribute6]       VARCHAR (200) NULL,
    [FxRate]                FLOAT (53)    NULL,
    [FxRateCalc]            VARCHAR (1)   NULL,
    [SettlCurrency]         INT           NULL,
    [ChangeType]            INT           DEFAULT ((3)) NOT NULL,
    [NotionalValue]         FLOAT (53)    DEFAULT ((0)) NOT NULL,
    [NotionalValueBase]     FLOAT (53)    DEFAULT ((0)) NOT NULL,
    [CounterPartyID]        INT           NULL,
	[ExchangeID]			INT			  NULL,
	[DayCumQty]             FLOAT (53)    DEFAULT ('0') not NULL,
	[DayAvgPx]              FLOAT (53)    DEFAULT ('0') not NULL,
    [AdditionalTradeAttributes] NVARCHAR(MAX) DEFAULT NULL
    CONSTRAINT [PK_T_Fills] PRIMARY KEY CLUSTERED ([Fills_PK] ASC),
    CONSTRAINT [FK__T_Fills__ClOrder__42AE7B82] FOREIGN KEY ([ClOrderID]) REFERENCES [dbo].[T_Sub] ([ClOrderID])
);


GO
CREATE NONCLUSTERED INDEX [_dta_index_T_Fills_25_1828969642__K34_K29_9_14_21_22_31]
    ON [dbo].[T_Fills]([NirvanaSeqNumber] ASC, [TransactTime] ASC)
    INCLUDE([InsertionTime], [AveragePrice], [Quantity], [CumQty], [ClOrderID]);


GO
CREATE NONCLUSTERED INDEX [_dta_index_T_Fills_25_1828969642__K9_K34]
    ON [dbo].[T_Fills]([ClOrderID] ASC, [NirvanaSeqNumber] ASC);


GO

CREATE TRIGGER [dbo].[FillTrigger]
   ON  [dbo].[T_Fills]
   AFTER INSERT
AS 
BEGIN
-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;	

UPDATE T_TradedOrders 
set AvgPrice=i.AveragePrice
,AveragePrice=i.AveragePrice,
CumQty=i.CumQty,
Quantity = i.Quantity,
Price=i.Price,
NirvanaSeqNumber=i.NirvanaSeqNumber,
OrderStatusTagValue=i.OrderStatus,
OrderStatus=i.OrderStatus,
ExecType=i.ExecType,
LeavesQty=i.LeavesQty,
LastPx=i.LastPx,
LastShares=i.LastShares,
TransactTime=i.TransactTime,
FxRate=i.FxRate,
FxConversionMethodOperator=i.FxRateCalc,
AvgFxRateForTrade=i.AvgFxRateForTrade,
SettlCurrency=i.SettlCurrency,
TimeInForce = i.TimeInForceID
FROM inserted i INNER JOIN T_SUB SUB ON SUB.ClOrderID=i.ClOrderID
WHERE T_TradedOrders.ParentClOrderID = SUB.ParentClOrderID
END
  
