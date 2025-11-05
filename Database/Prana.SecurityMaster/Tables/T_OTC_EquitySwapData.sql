
CREATE TABLE [dbo].[T_OTC_EquitySwapData](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[OTCTemplateID] [int] NOT NULL,
	[EquityLeg_Frequency] [varchar](50) NOT NULL,
	[EquityLeg_BulletSwap] [bit] NOT NULL,
	[EquityLeg_ExcludeDividends] [bit] NOT NULL,
	[EquityLeg_ImpliedCommission] [bit] NOT NULL,
	[Commission_Basis] [varchar](50) NOT NULL,
	[Commission_HardCommissionRate] [float] NOT NULL,
	[Commission_SoftCommissionRate] [float] NOT NULL,
	[FinanceLeg_InterestRate] [float] NOT NULL,
	[FinanceLeg_SpreadBasisPoint] [float] NOT NULL,
	[FinanceLeg_DayCount] [int] NOT NULL,
	[FinanceLeg_Frequency] [varchar](50) NOT NULL,
    [CustomFields] [nText] NULL, 
	[FinanceLeg_FixedRate] [float] NULL,
    CONSTRAINT [PK_T_OTC_EquitySwapData] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY], 
    CONSTRAINT [FK_T_OTC_EquitySwapData_T_OTC_Templates] FOREIGN KEY ([OTCTemplateID]) REFERENCES [T_OTC_Templates]([ID])
) ON [PRIMARY]
