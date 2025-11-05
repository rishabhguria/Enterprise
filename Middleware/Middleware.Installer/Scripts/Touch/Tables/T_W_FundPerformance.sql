/****** Object:  Table [dbo].[T_W_FundPerformance]    Script Date: 02/27/2014 10:04:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[T_W_FundPerformance](
	[GroupBy] [varchar](max) NULL,
	[Client] [varchar](max) NULL,
	[Entity] [varchar](max) NULL,
	[WTDPNL] [float] NULL,
	[MTDPNL] [float] NULL,
	[QTDPNL] [float] NULL,
	[YTDPNL] [float] NULL,
	[WTDACB] [float] NULL,
	[MTDACB] [float] NULL,
	[QTDACB] [float] NULL,
	[YTDACB] [float] NULL,
	[WTDRet] [float] NULL,
	[MTDRet] [float] NULL,
	[QTDRet] [float] NULL,
	[YTDRet] [float] NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
