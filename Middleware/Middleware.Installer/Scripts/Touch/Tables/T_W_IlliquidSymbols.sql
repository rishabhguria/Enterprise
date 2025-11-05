/****** Object:  Table [dbo].[T_W_IlliquidSymbols]    Script Date: 02/27/2014 10:04:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[T_W_IlliquidSymbols](
	[GroupBy] [varchar](max) NULL,
	[Client] [varchar](max) NULL,
	[Entity] [varchar](max) NULL,
	[Symbol] [varchar](max) NULL,
	[EndingPosition] [float] NULL,
	[AveNDaysTradingVolume] [float] NULL,
	[AveNDaysTradingValue] [float] NULL,
	[AveDaysToLiquidate] [float] NULL,
	[DaysToLiquidate] [float] NULL,
	[CurGrossMkv] [float] NULL,
	[EntityGrossMkv] [float] NULL,
	[CurEntityRatio] [float] NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
