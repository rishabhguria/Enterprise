/****** Object:  Table [dbo].[T_NT_InvestorIPFundPart]    Script Date: 05/13/2015 16:35:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_NT_InvestorIPFundPart](
	[InvestorIPId] [bigint] NOT NULL,
	[FundId] [int] NOT NULL,
	[UpdateDate] [smalldatetime] NOT NULL,
	[Part_TradingLevel] [decimal](9, 8) NOT NULL,
	[Part_InvestedCash] [decimal](9, 8) NOT NULL,
	[Approved] [bit] NULL,
 CONSTRAINT [PK_T_NT_InvestorIPFundPart] PRIMARY KEY CLUSTERED 
(
	[InvestorIPId] ASC,
	[FundId] ASC,
	[UpdateDate] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO