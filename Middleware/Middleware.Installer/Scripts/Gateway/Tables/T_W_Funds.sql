/****** Object:  Table [dbo].[T_W_Funds]    Script Date: 02/27/2014 10:04:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_W_Funds](
	[TouchFundID] [int] IDENTITY(1,1) NOT NULL,
	[StartDate] [datetime] NULL,
	[CalendarStartMonth] [int] NOT NULL CONSTRAINT [DF__T_W_Funds__Calen__5DED4CFA]  DEFAULT ((1)),
	[PranaFundID] [int] NOT NULL,
 CONSTRAINT [PK_TouchFunds] PRIMARY KEY CLUSTERED 
(
	[TouchFundID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO