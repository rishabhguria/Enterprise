CREATE TABLE [dbo].[T_WatchlistColumns] (
    [ColumnId]       INT            IDENTITY (1, 1) NOT NULL,
    [ColumnKey]      VARCHAR (50)   NOT NULL,
    [ColumnCaption]  VARCHAR (50)   NOT NULL,
    [ColumnFormula]  VARCHAR (1000) NULL,
    [ColumnPosition] INT            NOT NULL,
    [ColumnType]     VARCHAR (50)   NOT NULL,
    CONSTRAINT [PK_T_WatchlistColumns] PRIMARY KEY CLUSTERED ([ColumnId] ASC)
);

