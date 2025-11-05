
CREATE TABLE [dbo].[T_OTC_EquitySwapTradeData](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[GroupId] [varchar](50) NOT NULL,
	[TradeDate] [DateTime] NOT NULL,
	[DaysToSettle] int NOT NULL,
	[EffectiveDate] [DateTime] NOT NULL,
	[ISDA_CounterParty] int NOT NULL,
	

	[EquityLeg_Frequency] [varchar](50) NOT NULL,
	[EquityLeg_BulletSwap] [bit] NOT NULL,
	[EquityLeg_ImpliedCommission] [bit] NOT NULL,
	[EquityLeg_FirstPaymentDate]  [DateTime] NOT NULL,
	[EquityLeg_ExpirationDate]  [DateTime] NOT NULL,

	[Commission_Basis] [varchar](50) NOT NULL,
	[Commission_HardCommissionRate] [float] NOT NULL,
	[Commission_SoftCommissionRate] [float] NOT NULL,
	
	[FinanceLeg_InterestRate] [float] NOT NULL,
	[FinanceLeg_SpreadBasisPoint] [float] NOT NULL,
	[FinanceLeg_DayCount] [int] NOT NULL,
	[FinanceLeg_Frequency] [varchar](50) NOT NULL,
	[FinanceLeg_FixedRate] [float] NOT NULL,
	[FinanceLeg_FirstResetDate] [DateTime] NOT NULL,
    [FinanceLeg_FirstPaymentDate] [DateTime] NOT NULL,
    --[FinanceLeg_ScriptLendingFee] [DateTime] NOT NULL,

    [CustomFields] [nText] NULL,
	[UniqueIdentifier] [varchar](200) NULL,
	[EquityLeg_ExcludeDividends] [bit] NULL,
    CONSTRAINT [PK_T_OTC_EquitySwapTradeData] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY], 
    CONSTRAINT [FK_T_OTC_EquitySwapTradeData_T_Group] FOREIGN KEY ([GroupId]) REFERENCES [T_Group]([GroupID])
) ON [PRIMARY]
