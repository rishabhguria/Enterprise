/****** Object:  Table [dbo].[T_SavedPerformanceNumbers]    Script Date: 01/27/2016 18:34:04 ******/
CREATE TABLE [dbo].[T_SavedPerformanceNumbers](
	[Date] [datetime] NULL,
	[FundID] [int] NOT NULL,
	[PNL] [float] NULL,
	[AddRed] [float] NULL,
	[OpeningValue] [float] NULL
) ON [PRIMARY]
