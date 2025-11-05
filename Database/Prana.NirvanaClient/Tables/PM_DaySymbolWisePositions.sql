CREATE TABLE [dbo].[PM_DaySymbolWisePositions] (
    [DaySymbolWisePositionID] INT           IDENTITY (1, 1) NOT NULL,
    [Date]                    DATETIME      NOT NULL,
    [Symbol]                  VARCHAR (100) NOT NULL,
    [AvgPrice]                FLOAT (53)    NOT NULL,
    [Quantity]                INT           NOT NULL,
    [ordersidename]           VARCHAR (50)  NULL,
    [Fundid]                  INT           NULL,
    CONSTRAINT [PK__PM_DaySymbolWise__076DBA23] PRIMARY KEY CLUSTERED ([DaySymbolWisePositionID] ASC)
);

