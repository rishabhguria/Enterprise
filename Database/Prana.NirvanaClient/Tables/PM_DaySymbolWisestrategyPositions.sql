CREATE TABLE [dbo].[PM_DaySymbolWisestrategyPositions] (
    [DaySymbolWisePositionID] INT           IDENTITY (1, 1) NOT NULL,
    [Date]                    DATETIME      NOT NULL,
    [Symbol]                  VARCHAR (100) NOT NULL,
    [AvgPrice]                FLOAT (53)    NOT NULL,
    [Quantity]                INT           NOT NULL,
    [ordersidename]           VARCHAR (50)  NULL,
    [strategyid]              INT           NULL
);

