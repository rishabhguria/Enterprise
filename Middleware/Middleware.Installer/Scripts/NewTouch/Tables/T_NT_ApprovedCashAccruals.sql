GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[T_NT_ApprovedCashAccruals](
	[AsOfDate] [smalldatetime] NOT NULL,
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
 CONSTRAINT [PK_T_NT_ApprovedCashAccruals] PRIMARY KEY CLUSTERED 
(
	[AsOfDate] ASC,
	[CheckSumId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO