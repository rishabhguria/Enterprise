/****** Object:  Table [dbo].[T_NT_CashAccruals]    Script Date: 05/13/2015 16:35:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[T_NT_CashAccruals](
	[CheckSumId] [varbinary](16) NOT NULL,
	[AcctId] [int] NOT NULL,
	[AcctName] [varchar](max) NOT NULL,
	[RunDate] [datetime] NOT NULL,
	[CashOrAccruals] [bit] NOT NULL,
	[TradeCurrency] [varchar](max) NOT NULL,
	[BeginningMarketValueLocal] [float] NOT NULL,
	[EndingMarketValueLocal] [float] NOT NULL,
	[BeginningFXRate] [float] NOT NULL,
	[EndingFXRate] [float] NOT NULL,
 CONSTRAINT [PK_T_NT_CashAccruals] PRIMARY KEY CLUSTERED 
(
	[CheckSumId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO