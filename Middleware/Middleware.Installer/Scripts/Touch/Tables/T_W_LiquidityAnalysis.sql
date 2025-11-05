/****** Object:  Table [dbo].[T_W_LiquidityAnalysis]    Script Date: 02/27/2014 10:04:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[T_W_LiquidityAnalysis](
	[GroupBy] [varchar](max) NULL,
	[Client] [varchar](max) NULL,
	[Entity] [varchar](max) NULL,
	[DaysToLiquidate] [varchar](max) NULL,
	[SymbolCount] [int] NULL,
	[LongMkv] [float] NULL,
	[ShortMkv] [float] NULL,
	[GrossMkv] [float] NULL,
	[CumGrossMkv] [float] NULL,
	[EntityGrossMkv] [float] NULL,
	[CumEntityRatio] [float] NULL,
	[ID] [int] NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
