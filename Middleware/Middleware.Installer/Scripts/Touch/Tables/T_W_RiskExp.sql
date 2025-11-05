/****** Object:  Table [dbo].[T_W_RiskExp]    Script Date: 02/27/2014 10:04:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[T_W_RiskExp](
	[GroupBy] [varchar](max) NULL,
	[Client] [varchar](max) NULL,
	[Entity] [varchar](max) NULL,
	[NAVType] [varchar](max) NULL,
	[NAV] [float] NULL,
	[CashType] [varchar](max) NULL,
	[Cash] [float] NULL,
	[GrossType] [varchar](max) NULL,
	[Gross] [float] NULL,
	[NetType] [varchar](max) NULL,
	[Net] [float] NULL,
	[LongType] [varchar](max) NULL,
	[Long] [float] NULL,
	[ShortType] [varchar](max) NULL,
	[Short] [float] NULL,
	[AlignDataType] [varchar](max) NULL,
	[ID] [int] NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
