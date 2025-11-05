/****** Object:  Table [dbo].[T_NT_ApprovedTransactions]    Script Date: 05/13/2015 16:35:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[T_NT_ApprovedTransactions](
	[AsOfDate] [smalldatetime] NOT NULL,
	[CheckSumId] [varbinary](16) NOT NULL,
	[AcctId] [int] NOT NULL,
	[AcctName] [varchar](max) NOT NULL,
	[RunDate] [datetime] NOT NULL,
	[Symbol] [varchar](max) NOT NULL,
	[UnderlyingSymbol] [varchar](max) NOT NULL,
	[Open_CloseTag] [varchar](max) NOT NULL,
	[AvgPrice] [float] NOT NULL,
	[Quantity] [float] NOT NULL,
	[Side] [varchar](max) NOT NULL,
	[TradeCurrency] [varchar](max) NOT NULL,
	[NetAmountBase] [float] NOT NULL,
	[Dividend] [float] NOT NULL,
	[UdaSector] [varchar](max) NOT NULL,
	[UdaSubSector] [varchar](max) NOT NULL,
	[UdaCountry] [varchar](max) NOT NULL,
	[Strategy] [varchar](max) NOT NULL,
	[SymbolDescription] [varchar](max) NOT NULL,
	[UnderlyingSymbolDescription] [varchar](max) NOT NULL,
	[BloombergSymbol] [varchar](max) NOT NULL,
	[PutOrCall] [varchar](max) NOT NULL,
	[ExpirationDate] [datetime] NULL,
	[Asset] [varchar](max) NOT NULL,
	[SetupAsset] [varchar](max) NOT NULL,
	[CommissionAndFees] [bit] NOT NULL,
	[FXPNL] [bit] NOT NULL,
	[PriceMultiplier] [float] NOT NULL,
	[DeltaAdjPosMultiplier] [bit] NOT NULL,
	[ZeroOrEndingMVOrUnrealized] [int] NOT NULL,
	[CouponRate] [bit] NOT NULL,
	[BlackScholesOrBlack76] [bit] NOT NULL,
	[GroupID] [varchar](50) NOT NULL,
    [TransactionType] [varchar](100) NOT NULL,
 CONSTRAINT [PK_T_NT_ApprovedTransactions] PRIMARY KEY CLUSTERED 
(
	[AsOfDate] ASC,
	[CheckSumId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO