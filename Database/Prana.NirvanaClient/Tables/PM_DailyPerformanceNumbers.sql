CREATE TABLE [dbo].[PM_DailyPerformanceNumbers] (
    [TableID]   INT        IDENTITY (1, 1) NOT NULL,
    [FundID]    INT        NOT NULL,
    [Date]      DATETIME   NOT NULL,
    [MTDValue]  FLOAT (53) NOT NULL,
    [QTDValue]  FLOAT (53) NOT NULL,
    [YTDValue]  FLOAT (53) NOT NULL,
    [MTDReturn] FLOAT (53) NOT NULL,
    [QTDReturn] FLOAT (53) NOT NULL,
    [YTDReturn] FLOAT (53) NOT NULL,
    CONSTRAINT [PK_PM_DailyPerformanceNumbers] PRIMARY KEY CLUSTERED ([TableID] ASC)
);

