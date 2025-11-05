/****** Object:  Table [dbo].[T_W_SymbolTransactions]    Script Date: 02/27/2014 10:04:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[T_W_SymbolTransactions](
	[GroupBy] [varchar](max) NULL,
	[Client] [varchar](max) NULL,
	[Entity] [varchar](max) NULL,
	[TimeFrame] [varchar](max) NULL,
	[Symbol] [varchar](max) NULL,
	[UnderlyingSymbol] [varchar](max) NULL,
	[Date] [datetime] NULL,
	[AvgPrice] [float] NULL,
	[Quantity] [float] NULL,
	[SIDe] [varchar](max) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
