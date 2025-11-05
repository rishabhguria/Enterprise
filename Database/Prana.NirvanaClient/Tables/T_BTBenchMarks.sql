CREATE TABLE [dbo].[T_BTBenchMarks] (
    [BenchmarkID]   INT           IDENTITY (1, 1) NOT NULL,
    [BenchmarkName] VARCHAR (50)  NULL,
    [Description]   VARCHAR (200) NULL,
    [FilterTypeID]  INT           NOT NULL,
    CONSTRAINT [PK_T_BTBenckMarks] PRIMARY KEY CLUSTERED ([BenchmarkID] ASC)
);

