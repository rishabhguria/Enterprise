/****** Object:  Table [dbo].[T_W_VAMI]    Script Date: 02/27/2014 10:04:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[T_W_VAMI](
	[GroupBy] [varchar](max) NULL,
	[Client] [varchar](max) NULL,
	[Entity] [varchar](max) NULL,
	[TimeFrame] [varchar](max) NULL,
	[Date] [datetime] NULL,
	[Performance] [float] NULL,
	[Valid] [bit] NULL,
	[VAMI] [float] NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
