/****** Object:  Table [dbo].[T_NT_AssetwisePreferences]    Script Date: 05/13/2015 16:35:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[T_NT_AssetwisePreferences](
	[Asset] [varchar](100) NOT NULL,
	[CommissionAndFees] [bit] NOT NULL CONSTRAINT [DF_T_NT_AssetwisePreferences_CommissionAndFees]  DEFAULT ((0)),
	[FXPNL] [bit] NOT NULL CONSTRAINT [DF_T_NT_AssetwisePreferences_FXPNL]  DEFAULT ((1)),
	[PriceMultiplier] [float] NOT NULL CONSTRAINT [DF_T_NT_AssetwisePreferences_PriceMultiplier]  DEFAULT ((1)),
	[DeltaAdjPosMultiplier] [bit] NOT NULL CONSTRAINT [DF_T_NT_AssetwisePreferences_DeltaAdjPosMultiplier]  DEFAULT ((1)),
	[ZeroOrEndingMVOrUnrealized] [int] NOT NULL CONSTRAINT [DF_T_NT_AssetwisePreferences_ZeroOrEndingMVOrUnrealized]  DEFAULT ((1)),
	[CouponRate] [bit] NOT NULL CONSTRAINT [DF_T_NT_AssetwisePreferences_CouponRate]  DEFAULT ((0)),
	[BlackScholesOrBlack76] [bit] NOT NULL,
 CONSTRAINT [PK_T_NT_AssetwisePreferences] PRIMARY KEY CLUSTERED 
(
	[Asset] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO