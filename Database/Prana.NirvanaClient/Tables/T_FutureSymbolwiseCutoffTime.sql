CREATE TABLE [dbo].[T_FutureSymbolwiseCutoffTime] (
    [Symbol]           VARCHAR (50) NOT NULL,
    [WeekCutOffHour]   INT          NOT NULL,
    [WeekCutOffMin]    INT          NOT NULL,
    [SundayCutOffHour] INT          CONSTRAINT [DF_T_FutureSymbolwiseCutoffTime_SundayCutOffHour] DEFAULT ((0)) NOT NULL,
    [SundayCutOffMIn]  INT          CONSTRAINT [DF_T_FutureSymbolwiseCutoffTime_SundayCutOffMIn] DEFAULT ((0)) NOT NULL
);

