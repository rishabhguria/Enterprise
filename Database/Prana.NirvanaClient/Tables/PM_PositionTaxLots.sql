CREATE TABLE [dbo].[PM_PositionTaxLots] (
    [ApplicationTaxLotID]                  VARCHAR (50)     NOT NULL,
    [TaxLotCategory]                       INT              NULL,
    [ClosingQuantity]                      INT              NULL,
    [PositionID]                           UNIQUEIDENTIFIER NOT NULL,
    [PositionTaxLotID]                     INT              IDENTITY (1, 1) NOT NULL,
    [IsActive]                             BIT              NULL,
    [CloseDateTime]                        DATETIME         NOT NULL,
    [PNL]                                  FLOAT (53)       NOT NULL,
    [ParentPositionBalanceQuantity]        INT              NOT NULL,
    [ParentPositionMonthsProfit]           FLOAT (53)       NULL,
    [SymbolAverageProfitForParentPosition] FLOAT (53)       NULL,
    [FundID]                               INT              NULL,
    [IsShortTerm]                          BIT              NULL,
    CONSTRAINT [PK_PM_PositionTaxLots] PRIMARY KEY CLUSTERED ([PositionTaxLotID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1 - StartPositionTaxLot, 2 - IntermediateTaxLot, 3- ClosePositionTaxLot', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PM_PositionTaxLots', @level2type = N'COLUMN', @level2name = N'TaxLotCategory';

