CREATE TABLE [dbo].[PM_DailyCollateralPrice] (
    [DayCollateralPriceID] INT           IDENTITY (1, 1) NOT NULL,
    [Date]            DATETIME      NOT NULL,
    [Symbol]          VARCHAR (100) NOT NULL,
	[FundID]				INT NOT NULL,
    [CollateralPrice]      FLOAT (53)    NOT NULL,
    [Haircut]      FLOAT (53)    NOT NULL,
	[RebateOnMV]      FLOAT (53) NOT NULL,
	[RebateOnCollateral]      FLOAT (53) NOT NULL,
    CONSTRAINT [PK_PM_DailyCollateralPrice] PRIMARY KEY CLUSTERED ([DayCollateralPriceID] ASC)
);

