CREATE TABLE [dbo].[PM_SnapShot] (
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
    [AccruedInterest]              FLOAT (53)       NULL,
    CONSTRAINT [PK__PM_Taxlots__4560CAFB1] PRIMARY KEY NONCLUSTERED ([TaxLot_PK] ASC)
);

