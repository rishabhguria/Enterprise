
CREATE TABLE [dbo].[T_OTC_ConvertibleBondData](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[OTCTemplateID] [int] NOT NULL,
    [EquityLeg_ConversionRatio] [float] NOT NULL,
	[FinanceLeg_ZeroCoupon] [bit] NOT NULL,
	[FinanceLeg_IRBenchMark] [int] NOT NULL,
	[FinanceLeg_FXRate] [float] NOT NULL,
	[FinanceLeg_SBPoint] [float] NOT NULL,
	[FinanceLeg_DayCount] [int] NOT NULL,
	[FinanceLeg_CouponFreq] [int] NOT NULL,
	[Commission_Basis] [varchar](50) NOT NULL,
	[Commission_HardCommissionRate] [float] NOT NULL,
	[Commission_SoftCommissionRate] [float] NOT NULL,
	[CustomFields] [ntext] NULL,
 CONSTRAINT [PK_T_OTC_ConvertibleBondData] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
CONSTRAINT [FK_T_OTC_ConvertibleBondData_T_OTC_Templates] FOREIGN KEY([OTCTemplateID])
REFERENCES [dbo].[T_OTC_Templates] ([ID])
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

