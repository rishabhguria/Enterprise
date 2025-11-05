CREATE TABLE [dbo].[T_Sub] (
    [ClOrderID]           VARCHAR (50)   NOT NULL,
    [ParentClOrderID]     VARCHAR (50)   NOT NULL,
    [OrigClOrderID]       VARCHAR (50)   NULL,
    [Quantity]            FLOAT (53)     NULL,
    [Price]               FLOAT (53)     NULL,
    [MsgType]             VARCHAR (3)    NULL,
    [OrderID]             VARCHAR (500)   NULL,
    [ExecutionInst]       VARCHAR (50)   NULL,
    [OrderTypeTagValue]   VARCHAR (3)    NULL,
    [TimeInForce]         VARCHAR (3)    NULL,
    [CounterPartyID]      INT            NULL,
    [VenueID]             INT            NULL,
    [DiscrOffset]         FLOAT (53)     NULL,
    [PegDiff]             VARCHAR (50)   NULL,
    [PNP]                 INT            NULL,
    [StopPrice]           FLOAT (53)     NULL,
    [TargetCompID]        VARCHAR (50)   NULL,
    [TargetSubID]         VARCHAR (50)   NULL,
    [Text]                VARCHAR (200)  NULL,
    [ServerTime]          VARCHAR (50)   NULL,
    [InsertionTime]       DATETIME       NULL,
    [StagedOrderID]       VARCHAR (50)   NULL,
    [NirvanaMsgType]      INT            NULL,
    [HandlingInst]        VARCHAR (3)    NULL,
    [LocateReqd]          BIT            NULL,
    [ShortRebate]         FLOAT (53)     NULL,
    [FundID]              INT            NULL,
    [StrategyID]          INT            NULL,
    [BorrowerID]          VARCHAR (50)   NULL,
    [SecurityType]        VARCHAR (20)   NULL,
    [MaturityYearMonth]   VARCHAR (20)   NULL,
    [OpenClose]           VARCHAR (3)    NULL,
    [ClientOrderID]       VARCHAR (50)   NOT NULL,
    [ParentClientOrderID] VARCHAR (50)   NOT NULL,
    [UserID]              INT            NOT NULL,
	[OriginalUserID]      INT            NULL,
    [NirvanaSeqNumber]    INT            NULL,
    [CMTA]                VARCHAR (20)   NULL,
    [GiveUpID]            INT            NULL,
    [GiveUp]              VARCHAR (20)   NULL,
    [UnderlyingSymbol]    VARCHAR (100)  NULL,
    [AlgoStrategyID]      VARCHAR (50)   NULL,
    [AlgoParameters]      VARCHAR (500)  NULL,
    [AUECLocalDate]       DATETIME       NULL,
    [SettlementDate]      DATETIME       NULL,
    [ProcessDate]         DATETIME       NULL,
    [MultiTradeName]      NVARCHAR (500) NULL,
    [ImportFileID]        INT            NULL,
    [InternalComments]    VARCHAR (200)  NULL,
    [TradeAttribute1]     VARCHAR (50)   NULL,
    [TradeAttribute2]     VARCHAR (50)   NULL,
    [TradeAttribute3]     VARCHAR (50)   NULL,
    [TradeAttribute4]     VARCHAR (50)   NULL,
    [TradeAttribute5]     VARCHAR (50)   NULL,
    [TradeAttribute6]     VARCHAR (50)   NULL,
    [FxRate]              FLOAT (53)     NULL,
    [FxRateCalc]          VARCHAR (1)    NULL,
    [SettlCurrency]       INT            NULL,
    [ChangeType]          INT            DEFAULT ((3)) NOT NULL,
    [OriginalAllocationPreferenceID] INT NULL, 
    [IsHidden] BIT NOT NULL DEFAULT (0), 
	[BorrowBroker] VARCHAR(50) NULL,
	[NirvanaLocateID] INT NULL,
    [IsManualOrder] BIT NULL, 
    [ExpireTime] varchar(50) null,
    [IsUseCustodianBroker] BIT NOT NULL DEFAULT 0, 
    [AdditionalTradeAttributes] NVARCHAR(MAX) DEFAULT NULL,
    [TradeApplicationSource] INT NULL
    CONSTRAINT [PK_T_Sub] PRIMARY KEY CLUSTERED ([ClOrderID] ASC),
    CONSTRAINT [FK__T_Sub__ParentClO__41BA5749] FOREIGN KEY ([ParentClOrderID]) REFERENCES [dbo].[T_Order] ([ParentClOrderID]),
    CONSTRAINT [FK_T_Sub_T_ImportFileLog] FOREIGN KEY ([ImportFileID]) REFERENCES [dbo].[T_ImportFileLog] ([ImportFileID]),
);


GO
CREATE NONCLUSTERED INDEX [_dta_index_T_Sub_25_1748969357__K1_K23_K11_K12_K9_K2_K27_K28_K3_K37_4_21_34]
    ON [dbo].[T_Sub]([ClOrderID] ASC, [NirvanaMsgType] ASC, [CounterPartyID] ASC, [VenueID] ASC, [OrderTypeTagValue] ASC, [ParentClOrderID] ASC, [FundID] ASC, [StrategyID] ASC, [OrigClOrderID] ASC, [UserID] ASC)
    INCLUDE([InsertionTime], [OpenClose], [Quantity]);


GO
CREATE NONCLUSTERED INDEX [_dta_index_T_Sub_25_1748969357__K1_K23_K11_K9_K12_K27_K28_K3_K37_4_21_34]
    ON [dbo].[T_Sub]([ClOrderID] ASC, [NirvanaMsgType] ASC, [CounterPartyID] ASC, [OrderTypeTagValue] ASC, [VenueID] ASC, [FundID] ASC, [StrategyID] ASC, [OrigClOrderID] ASC, [UserID] ASC)
    INCLUDE([Quantity], [OpenClose], [InsertionTime]);


GO
CREATE NONCLUSTERED INDEX [_dta_index_T_Sub_25_1748969357__K2_K1]
    ON [dbo].[T_Sub]([ParentClOrderID] ASC, [ClOrderID] ASC);


GO
CREATE NONCLUSTERED INDEX [_dta_index_T_Sub_25_1748969357__K2_K1_K9_K23_K12_K11_K27_K28_K3_K37_4_21_34]
    ON [dbo].[T_Sub]([ParentClOrderID] ASC, [ClOrderID] ASC, [OrderTypeTagValue] ASC, [NirvanaMsgType] ASC, [VenueID] ASC, [CounterPartyID] ASC, [FundID] ASC, [StrategyID] ASC, [OrigClOrderID] ASC, [UserID] ASC)
    INCLUDE([Quantity], [OpenClose], [InsertionTime]);


GO
CREATE TRIGGER [SUBTRIGGER] ON [DBO].[T_SUB] AFTER INSERT AS BEGIN
SET NOCOUNT ON; DECLARE @PARENTCLORDERID VARCHAR(50)
SELECT @PARENTCLORDERID = I.PARENTCLORDERID
FROM INSERTED I IF EXISTS
  (SELECT PARENTCLORDERID
   FROM T_TRADEDORDERS
   WHERE PARENTCLORDERID = @PARENTCLORDERID) BEGIN
UPDATE T_TRADEDORDERS
SET CLORDERID = I.CLORDERID, ORDERTYPETAGVALUE = I.ORDERTYPETAGVALUE, COUNTERPARTYID = I.COUNTERPARTYID, VENUEID = I.VENUEID, FUNDID = I.FUNDID, STRATEGYID = I.STRATEGYID, QUANTITY = I.QUANTITY, PRICE = I.PRICE, INSERTIONTIME = I.INSERTIONTIME, MSGTYPE = I.MSGTYPE, USERID = I.USERID, ORIGCLORDERID = I.ORIGCLORDERID, PARENTCLORDERID = I.PARENTCLORDERID, CLIENTORDERID = I.CLIENTORDERID, PARENTCLIENTORDERID = I.PARENTCLIENTORDERID, OPENCLOSE = I.OPENCLOSE, NIRVANAMSGTYPE = I.NIRVANAMSGTYPE, ORDERID = I.ORDERID, SETTLEMENTDATE = I.SETTLEMENTDATE, ALGOSTRATEGYID = I.ALGOSTRATEGYID, ALGOPARAMETERS = I.ALGOPARAMETERS, EXECUTIONINST = I.EXECUTIONINST, TIMEINFORCE = I.TIMEINFORCE, HANDLINGINST = I.HANDLINGINST, AUECLOCALDATE = I.AUECLOCALDATE, Text = I.Text, PROCESSDATE = I.PROCESSDATE,
IMPORTFILEID = I.IMPORTFILEID,
MULTITRADENAME = I.MULTITRADENAME,
[OriginalAllocationPreferenceID] = I.[OriginalAllocationPreferenceID]
FROM INSERTED I
WHERE T_TRADEDORDERS.PARENTCLORDERID = I.PARENTCLORDERID END ELSE
INSERT INTO T_TRADEDORDERS (CLORDERID, ORDERTYPETAGVALUE, COUNTERPARTYID, VENUEID, FUNDID, STRATEGYID, QUANTITY, PRICE, INSERTIONTIME, MSGTYPE, USERID, ORIGCLORDERID, PARENTCLORDERID, CLIENTORDERID, PARENTCLIENTORDERID, OPENCLOSE, NIRVANAMSGTYPE, ORDERID, SETTLEMENTDATE, ALGOSTRATEGYID, ALGOPARAMETERS, EXECUTIONINST, TIMEINFORCE, HANDLINGINST, AUECLOCALDATE, Text, AUECID, ORDERSIDETAGVALUE, TRADINGACCOUNTID, SYMBOL, PROCESSDATE, ORIGINALPURCHASEDATE, IMPORTFILEID, MULTITRADENAME, TRADEATTRIBUTE1, TRADEATTRIBUTE2, TRADEATTRIBUTE3, TRADEATTRIBUTE4, TRADEATTRIBUTE5, TRADEATTRIBUTE6, INTERNALCOMMENTS, FXRATE, FXCONVERSIONMETHODOPERATOR, SETTLCURRENCY, ORIGINALALLOCATIONPREFERENCEID)
SELECT I.CLORDERID,
       I.ORDERTYPETAGVALUE,
       I.COUNTERPARTYID,
       I.VENUEID,
       I.FUNDID,
       I.STRATEGYID,
       I.QUANTITY,
       I.PRICE,
       I.INSERTIONTIME,
       I.MSGTYPE,
       I.USERID,
       I.ORIGCLORDERID,
       I.PARENTCLORDERID,
       I.CLIENTORDERID,
       I.PARENTCLIENTORDERID,
       I.OPENCLOSE,
       I.NIRVANAMSGTYPE,
       I.ORDERID,
       I.SETTLEMENTDATE,
       I.ALGOSTRATEGYID,
       I.ALGOPARAMETERS,
       I.EXECUTIONINST,
       I.TIMEINFORCE,
       I.HANDLINGINST,
       I.AUECLOCALDATE,
       I.Text,
       T_ORDER.AUECID,
       T_ORDER.ORDERSIDETAGVALUE,
       T_ORDER.TRADINGACCOUNTID,
       T_ORDER.SYMBOL,
       I.PROCESSDATE,
       I.PROCESSDATE,
       I.IMPORTFILEID,
       I.MULTITRADENAME,
       I.TRADEATTRIBUTE1,
       I.TRADEATTRIBUTE2,
       I.TRADEATTRIBUTE3,
       I.TRADEATTRIBUTE4,
       I.TRADEATTRIBUTE5,
       I.TRADEATTRIBUTE6,
       I.INTERNALCOMMENTS,
       I.FXRATE,
       I.FXRATECALC,
       I.SETTLCURRENCY,
       I.ORIGINALALLOCATIONPREFERENCEID
FROM INSERTED I
INNER JOIN T_ORDER ON T_ORDER.PARENTCLORDERID = I.PARENTCLORDERID END