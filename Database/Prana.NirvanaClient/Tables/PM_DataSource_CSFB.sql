CREATE TABLE [dbo].[PM_DataSource_CSFB] (
    [Report Name_Header]   NCHAR (50)  NULL,
    [Account Name_Header]  NCHAR (50)  NULL,
    [Start Date_Header]    DATETIME    NULL,
    [End  Date_Header]     DATETIME    NULL,
    [Quantity]             INT         NOT NULL,
    [SecSymbol]            NCHAR (50)  NULL,
    [TaxLot Desc]          NCHAR (250) NULL,
    [TaxLotID]             INT         NULL,
    [Lot Date]             DATETIME    NULL,
    [Unit Cost Local]      FLOAT (53)  NOT NULL,
    [Market Price Local]   FLOAT (53)  NOT NULL,
    [Total Cost Local]     FLOAT (53)  NOT NULL,
    [Market Value Local]   FLOAT (53)  NOT NULL,
    [Total Cost Book]      FLOAT (53)  NOT NULL,
    [Market Value Book]    INT         NOT NULL,
    [Accrued Interest]     FLOAT (53)  NOT NULL,
    [Price Gain/ Loss]     FLOAT (53)  NOT NULL,
    [FX Gain/Loss]         FLOAT (53)  NOT NULL,
    [Unrealized Gain/Loss] FLOAT (53)  NOT NULL
);

