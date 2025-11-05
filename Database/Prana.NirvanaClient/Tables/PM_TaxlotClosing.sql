CREATE TABLE [dbo].[PM_TaxlotClosing] (
    [TaxLotClosingID]    UNIQUEIDENTIFIER NOT NULL,
    [PositionalTaxlotId] VARCHAR (50)     NOT NULL,
    [ClosingTaxlotId]    VARCHAR (50)     NOT NULL,
    [ClosedQty]          FLOAT (53)       NOT NULL,
    [ClosingMode]        INT              NULL,
    [TimeOfSaveUTC]      DATETIME         NOT NULL,
    [AUECLocalDate]      DATETIME         NOT NULL,
    [PositionSide]       NCHAR (10)       NULL,
    [OpenPrice]          FLOAT (53)       CONSTRAINT [DF_PM_TaxlotClosing_OpenPrice] DEFAULT ((0)) NOT NULL,
    [ClosePrice]         FLOAT (53)       CONSTRAINT [DF_PM_TaxlotClosing_ClosePrice] DEFAULT ((0)) NOT NULL,
    [ClosingAlgo]        INT              NOT NULL,
    [NotionalChange]     FLOAT (53)       DEFAULT ((0)) NOT NULL,
	[IsManualyExerciseAssign] BIT         NULL,
    [IsCopyTradeAttrbsPrefUsed] BIT       NULL,
    CONSTRAINT [PK__PM_TaxlotClosing__7CFB18A8] PRIMARY KEY CLUSTERED ([TaxLotClosingID] ASC) WITH (FILLFACTOR = 100)
);

