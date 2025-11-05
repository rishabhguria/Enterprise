CREATE TABLE [dbo].[T_BTFilterDetails] (
    [FilterTypesID] INT           NOT NULL,
    [FilterID]      VARCHAR (200) NOT NULL,
    [BenchMarkID]   INT           NOT NULL,
    [OperatorID]    INT           NOT NULL,
    [Percentage]    FLOAT (53)    NOT NULL,
    CONSTRAINT [FK_T_BTFilterDetails_T_BTBenchMarks] FOREIGN KEY ([BenchMarkID]) REFERENCES [dbo].[T_BTBenchMarks] ([BenchmarkID]),
    CONSTRAINT [FK_T_BTFilterDetails_T_BTFilters] FOREIGN KEY ([FilterID]) REFERENCES [dbo].[T_BTFilters] ([FilterID]),
    CONSTRAINT [FK_T_BTFilterDetails_T_BTFilterTypes] FOREIGN KEY ([FilterTypesID]) REFERENCES [dbo].[T_BTFilterTypes] ([FilterTypeID]),
    CONSTRAINT [FK_T_BTFilterDetails_T_BTOperators] FOREIGN KEY ([OperatorID]) REFERENCES [dbo].[T_BTOperators] ([OperatorID])
);

