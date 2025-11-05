CREATE TABLE [dbo].[PM_TaxLots_OLD] (
    [TaxLot_PK]                    BIGINT           IDENTITY (1, 1) NOT NULL,
    [TaxLotID]                     VARCHAR (50)     NOT NULL,
    [Symbol]                       VARCHAR (100)    NOT NULL,
    [TaxLotOpenQty]                FLOAT (53)       NOT NULL,
    [AvgPrice]                     FLOAT (53)       NOT NULL,
    [TimeOfSaveUTC]                DATETIME         NULL,
    [CorpActionID]                 UNIQUEIDENTIFIER NULL,
    [GroupID]                      NVARCHAR (50)    NULL,
    [AUECModifiedDate]             DATETIME         NULL,
    [FundID]                       INT              NULL,
    [Level2ID]                     INT              NULL,
    [OpenTotalCommissionandFees]   FLOAT (53)       NULL,
    [ClosedTotalCommissionandFees] FLOAT (53)       NULL,
    [PositionTag]                  INT              NULL,
    [OrderSideTagValue]            NCHAR (10)       NULL,
    [TaxLotClosingId_Fk]           UNIQUEIDENTIFIER NULL,
    [ParentRow_Pk]                 BIGINT           NULL,
    CONSTRAINT [PK_PM_Taxlots_1] PRIMARY KEY CLUSTERED ([TaxLot_PK] ASC)
);


GO
CREATE NONCLUSTERED INDEX [_dta_index_PM_Taxlots_15_386920550__K3]
    ON [dbo].[PM_TaxLots_OLD]([Symbol] ASC);


GO
CREATE NONCLUSTERED INDEX [_dta_index_PM_Taxlots_15_386920550__K4]
    ON [dbo].[PM_TaxLots_OLD]([Symbol] ASC);


GO
CREATE NONCLUSTERED INDEX [_dta_index_PM_Taxlots_15_386920550__K5]
    ON [dbo].[PM_TaxLots_OLD]([Symbol] ASC);

