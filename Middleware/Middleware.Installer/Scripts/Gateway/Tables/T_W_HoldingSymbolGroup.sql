/****** Object:  Table [dbo].[T_W_HoldingSymbolGroup]    Script Date: 02/27/2014 10:04:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[T_W_HoldingSymbolGroup](
	[GroupBy] [varchar](max) NULL,
	[Client] [varchar](max) NULL,
	[Entity] [varchar](max) NULL,
	[TimeFrame] [varchar](max) NULL,
	[Symbol] [varchar](max) NULL,
	[UdaSector] [varchar](max) NULL,
	[UdaCountry] [varchar](max) NULL,
	[UnderlyingSymbol] [varchar](max) NULL,
	[PeriodUnrealizedPNL] [float] NULL,
	[PeriodRealizedPNL] [float] NULL,
	[DivInt] [float] NULL,
	[PeriodTotalPNL] [float] NULL,
	[CostBasisPNL] [float] NULL,
	[Position] [float] NULL,
	[DeltaAdjPosition] [float] NULL,
	[BeginningQuantity] [float] NULL,
	[CostBasis] [float] NULL,
	[ClosingMark] [float] NULL,
	[UnderlyingClosingMark] [float] NULL,
	[MarketValue] [float] NULL,
	[DeltaExposure] [float] NULL,
	[Beta] [float] NULL,
	[BetaExposure] [float] NULL,
	[P&L Contribution] [float] NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
