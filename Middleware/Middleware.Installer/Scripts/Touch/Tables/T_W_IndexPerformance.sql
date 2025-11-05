/****** Object:  Table [dbo].[T_W_IndexPerformance]    Script Date: 02/27/2014 10:04:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_W_IndexPerformance](
	[Symbol] [nvarchar](50) NULL,
	[Name] [nvarchar](50) NULL,
	[DayPrice] [float] NULL,
	[WTDPrice] [float] NULL,
	[MTDPrice] [float] NULL,
	[QTDPrice] [float] NULL,
	[YTDPrice] [float] NULL
) ON [PRIMARY]

GO
