CREATE TABLE [dbo].[T_OTC_CFDData](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[OTCTemplateID] [int] NOT NULL,
	[Commission_Basis] [varchar](50) NOT NULL,
	[Commission_HardCommissionRate] [float] NOT NULL,
	[Commission_SoftCommissionRate] [float] NOT NULL,
	[FinanceLeg_InterestRate] [float] NOT NULL,
	[FinanceLeg_SpreadBasisPoint] [float] NOT NULL,
	[FinanceLeg_DayCount] [int] NOT NULL,
	[FinanceLeg_FixedRate] [float] NOT NULL,
	[FinanceLeg_ScriptLeadingFee] [float] NOT NULL,
	[Collateral_Margin] [float] NOT NULL,
	[Collateral_Rate] [float] NOT NULL,
	[Collateral_DayCount] [int] NOT NULL,
	[CustomFields] [ntext] NULL,
    CONSTRAINT [PK_T_OTC_CFDData] PRIMARY KEY CLUSTERED 
(
	
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY], 
    CONSTRAINT [FK_T_OTC_CFDData_T_OTC_Templates] FOREIGN KEY ([OTCTemplateID]) REFERENCES [T_OTC_Templates]([ID])
) ON [PRIMARY]