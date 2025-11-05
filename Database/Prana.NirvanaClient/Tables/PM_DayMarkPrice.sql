CREATE TABLE [dbo].[PM_DayMarkPrice] (
    [DayMarkPriceID]       INT           IDENTITY (1, 1) NOT NULL,
    [Date]                 DATETIME      NOT NULL,
    [Symbol]               VARCHAR (100) NOT NULL,
    [ApplicationMarkPrice] FLOAT (53)    NOT NULL,
    [PrimeBrokerMarkPrice] FLOAT (53)    NOT NULL,
    [FinalMarkPrice]       FLOAT (53)    NOT NULL,
    [IsActive]             BIT           NOT NULL,
    [ForwardPoints]        FLOAT (53)    DEFAULT ('0') NOT NULL,
    [FundID]               INT           DEFAULT ((0)) NOT NULL,
    [PriceTypeID]          INT           DEFAULT ((0)) NULL,
    [SourceID]             INT           DEFAULT ((0)) NULL,
    [IsApproved]           BIT           DEFAULT ((0)) NOT NULL,
    [AmendedMarkPrice]     FLOAT (53)    DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_T_DayMarkPrice_1] PRIMARY KEY CLUSTERED ([DayMarkPriceID] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_PM_DayMarkPrice]
    ON [dbo].[PM_DayMarkPrice]([Date] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_PM_DayMarkPrice_1]
    ON [dbo].[PM_DayMarkPrice]([Symbol] ASC);


GO
CREATE STATISTICS [_dta_stat_1267587654_1_2]
    ON [dbo].[PM_DayMarkPrice]([DayMarkPriceID], [Date]);


GO
CREATE STATISTICS [_dta_stat_1267587654_2_3]
    ON [dbo].[PM_DayMarkPrice]([Date], [Symbol]);


GO
CREATE STATISTICS [_dta_stat_1267587654_3_1_2]
    ON [dbo].[PM_DayMarkPrice]([Symbol], [DayMarkPriceID], [Date]);


GO
CREATE STATISTICS [_dta_stat_1267587654_1_3]
    ON [dbo].[PM_DayMarkPrice]([DayMarkPriceID], [Date]);


GO
CREATE STATISTICS [_dta_stat_1267587654_2_4]
    ON [dbo].[PM_DayMarkPrice]([Date], [Symbol]);


GO
CREATE STATISTICS [_dta_stat_1267587654_3_1_3]
    ON [dbo].[PM_DayMarkPrice]([Symbol], [DayMarkPriceID], [Date]);


GO
CREATE STATISTICS [_dta_stat_1267587654_1_4]
    ON [dbo].[PM_DayMarkPrice]([DayMarkPriceID], [Date]);


GO
CREATE STATISTICS [_dta_stat_1267587654_2_5]
    ON [dbo].[PM_DayMarkPrice]([Date], [Symbol]);


GO
CREATE STATISTICS [_dta_stat_1267587654_3_1_4]
    ON [dbo].[PM_DayMarkPrice]([Symbol], [DayMarkPriceID], [Date]);

