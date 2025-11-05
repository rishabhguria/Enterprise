/****** Object:  Table [dbo].[T_NT_HistoricalPerformances]    Script Date: 05/13/2015 16:35:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[T_NT_HistoricalPerformances](
	[Year] [smallint] NOT NULL,
	[Month] [smallint] NOT NULL,
	[Entity] [varchar](max) NOT NULL,
	[NetRet] [float] NOT NULL,
	[GrossRet] [float] NOT NULL,
	[FundType] [int] NOT NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO