CREATE TABLE [dbo].[PM_Taxlots_Intermediate](
	[TaxLot_PK] [bigint] IDENTITY(1,1) NOT NULL,
	[TaxLotID] [varchar](50) NOT NULL,
	[Symbol] [varchar](100) NOT NULL,
	[TaxLotOpenQty] [float] NOT NULL,
	[AvgPrice] [float] NOT NULL,
	[TimeOfSaveUTC] [datetime] NULL,
	[GroupID] [nvarchar](50) NULL,
	[AUECModifiedDate] [datetime] NOT NULL,
	[FundID] [int] NULL,
	[Level2ID] [int] NULL,
	[OpenTotalCommissionandFees] [float] NULL,
	[ClosedTotalCommissionandFees] [float] NULL,
	[PositionTag] [int] NULL,
	[OrderSideTagValue] [nchar](10) NULL,
	[TaxLotClosingId_Fk] [uniqueidentifier] NULL,
	[ParentRow_Pk] [bigint] NULL,
	[AccruedInterest] [float] NULL CONSTRAINT [DF_PM_Taxlots_AccruedInterest_Intermediate]  DEFAULT ((0)),
	[FXRate] [float] NULL,
	[FXConversionMethodOperator] [varchar](3) NULL,
	[ExternalTransId] [varchar](100) NULL,
	[TradeAttribute1] [varchar](200) NULL,
	[TradeAttribute2] [varchar](200) NULL,
	[TradeAttribute3] [varchar](200) NULL,
	[TradeAttribute4] [varchar](200) NULL,
	[TradeAttribute5] [varchar](200) NULL,
	[TradeAttribute6] [varchar](200) NULL,
	[LotId] [varchar](200) NULL,
	[NotionalChange] [float] NOT NULL DEFAULT ((0)),
	[NirvanaProcessDate] [datetime] NULL CONSTRAINT [DF_PM_Taxlots_NirvanaProcessDate_Intermediate]  DEFAULT (((1800)-(1))-(1)),
	[SettlCurrency] [int] NULL,
	[AdditionalTradeAttributes] NVARCHAR(MAX) DEFAULT NULL
 CONSTRAINT [PK__PM_Taxlots_Intermediate] PRIMARY KEY NONCLUSTERED 
(
	[TaxLot_PK] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]



