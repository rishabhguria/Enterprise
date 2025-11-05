CREATE TABLE [dbo].[T_OTC_CFDTradeData](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[GroupId] [varchar](50) NOT NULL,
	[TradeDate] [datetime] NOT NULL,
	[DaysToSettle] [int] NOT NULL,
	[EffectiveDate] [datetime] NOT NULL,
	[ISDA_CounterParty] [int] NOT NULL,
	[Collateral_Margin] [float] NOT NULL,
	[Collateral_Rate] [float] NOT NULL,
	[Collateral_DayCount] [int] NOT NULL,
	[Commission_Basis] [varchar](50) NOT NULL,
	[Commission_HardCommissionRate] [float] NOT NULL,
	[Commission_SoftCommissionRate] [float] NOT NULL,
	[FinanceLeg_InterestRate] [float] NOT NULL,
	[FinanceLeg_SpreadBasisPoint] [float] NOT NULL,
	[FinanceLeg_DayCount] [int] NOT NULL,
	[FinanceLeg_FixedRate] [float] NOT NULL,
	[FinanceLeg_ScriptLendingFee] [float] NOT NULL,
	[CustomFields] [ntext] NULL,
	[UniqueIdentifier] [varchar](200) NULL,
 CONSTRAINT [PK_T_OTC_CFDTradeData] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY], 
    CONSTRAINT [FK_T_OTC_CFDTradeData_T_Group] FOREIGN KEY ([GroupId]) REFERENCES [T_Group]([GroupID])
) ON [PRIMARY]
