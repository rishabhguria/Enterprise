CREATE TABLE [dbo].[T_SMDailyVolume] (
    [Symbol_PK]   BIGINT     NULL,
    [DailyVolume] FLOAT (53) NULL,
    [Date]        DATETIME   NULL,
    CONSTRAINT [FK_T_SMDailyVolume_T_SMSymbolLookUpTable] FOREIGN KEY ([Symbol_PK]) REFERENCES [dbo].[T_SMSymbolLookUpTable] ([Symbol_PK])
);

