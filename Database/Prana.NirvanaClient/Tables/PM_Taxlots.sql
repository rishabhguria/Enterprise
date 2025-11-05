CREATE TABLE [dbo].[PM_Taxlots] (
    [TaxLot_PK]                    BIGINT           IDENTITY (1, 1) NOT NULL,
    [TaxLotID]                     VARCHAR (50)     NOT NULL,
    [Symbol]                       VARCHAR (100)    NOT NULL,
    [TaxLotOpenQty]                FLOAT (53)       NOT NULL,
    [AvgPrice]                     FLOAT (53)       NOT NULL,
    [TimeOfSaveUTC]                DATETIME         NULL,
    [GroupID]                      NVARCHAR (50)    NULL,
    [AUECModifiedDate]             DATETIME         NOT NULL,
    [FundID]                       INT              NULL,
    [Level2ID]                     INT              NULL,
    [OpenTotalCommissionandFees]   FLOAT (53)       NULL,
    [ClosedTotalCommissionandFees] FLOAT (53)       NULL,
    [PositionTag]                  INT              NULL,
    [OrderSideTagValue]            NCHAR (10)       NULL,
    [TaxLotClosingId_Fk]           UNIQUEIDENTIFIER NULL,
    [ParentRow_Pk]                 BIGINT           NULL,
    [AccruedInterest]              FLOAT (53)       CONSTRAINT [DF_PM_Taxlots_AccruedInterest] DEFAULT ((0)) NULL,
    [FXRate]                       FLOAT (53)       NULL,
    [FXConversionMethodOperator]   VARCHAR (3)      NULL,
    [ExternalTransId]              VARCHAR (100)    NULL,
    [TradeAttribute1]              VARCHAR (200)    NULL,
    [TradeAttribute2]              VARCHAR (200)    NULL,
    [TradeAttribute3]              VARCHAR (200)    NULL,
    [TradeAttribute4]              VARCHAR (200)    NULL,
    [TradeAttribute5]              VARCHAR (200)    NULL,
    [TradeAttribute6]              VARCHAR (200)    NULL,
    [LotId]                        VARCHAR (200)    NULL,
    [NotionalChange]               FLOAT (53)       DEFAULT ((0)) NOT NULL,
    [NirvanaProcessDate]           DATETIME         CONSTRAINT [DF_PM_Taxlots_NirvanaProcessDate] DEFAULT (((1800)-(1))-(1)) NULL,
    [SettlCurrency]                INT              NULL,
    [AdditionalTradeAttributes] NVARCHAR(MAX) DEFAULT NULL
    CONSTRAINT [PK__PM_Taxlots__32F801FD] PRIMARY KEY NONCLUSTERED ([TaxLot_PK] ASC) ON [PRIMARY]
);


GO
CREATE CLUSTERED INDEX [ix_PM_Taxlots_AUECModifiedDate]
    ON [dbo].[PM_Taxlots]([AUECModifiedDate] ASC);

GO
CREATE INDEX [PM_Taxlots_K1_K2_K3_K8]
	ON [dbo].[PM_Taxlots](TaxLot_PK,TaxLotID,Symbol,AUECModifiedDate);