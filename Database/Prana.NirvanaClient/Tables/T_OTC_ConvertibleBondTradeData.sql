CREATE TABLE [dbo].[T_OTC_ConvertibleBondTradeData](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[GroupId] [varchar](50) NOT NULL,
	[TradeDate] [datetime] NOT NULL,
	[DaysToSettle] [int] NOT NULL,
	[EffectiveDate] [datetime] NOT NULL,
	[ISDA_CounterParty] [int] NOT NULL,
	[CustomFields] [ntext] NULL,
	[UniqueIdentifier] [varchar](200) NULL,
    [EquityLeg_ConversionRatio] [float] NOT NULL,
	[EquityLeg_ConversionPrice] [float] NOT NULL,
	[EquityLeg_ConversionDate] [datetime] NOT NULL,
	[FinanceLeg_ZeroCoupon] [bit] NOT NULL,
	[FinanceLeg_IRBenchMark] [int] NOT NULL,
	[FinanceLeg_FXRate] [float] NOT NULL,
	[FinanceLeg_SBPoint] [float] NOT NULL,
	[FinanceLeg_DayCount] [int] NOT NULL,
	[FinanceLeg_CouponFreq] [int] NOT NULL,
	[Commission_Basis] [varchar](50) NOT NULL,
	[Commission_HardCommissionRate] [float] NOT NULL,
	[Commission_SoftCommissionRate] [float] NOT NULL,
	[FinanceLeg_FirstResetDate] [datetime] NOT NULL,
	[FinanceLeg_FirstPaymentDate] [datetime] NOT NULL,
	[Sedol]  [varchar](200) NULL,
	[ISIN]  [varchar](200) NULL,
	[Cusip]  [varchar](200) NULL,
	[Currency]  [varchar](200) NULL,
	[FinanceLeg_ParValue]  [varchar](200) NULL,
 CONSTRAINT [PK_T_OTC_ConvertibleBondTradeData] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) 
ON [PRIMARY], CONSTRAINT [FK_T_OTC_ConvertibleBondTradeData_T_Group] FOREIGN KEY([GroupId]) REFERENCES [dbo].[T_Group] ([GroupID])
) ON [PRIMARY]
 
