

CREATE PROCEDURE P_SetWatchlistColumn
(
	@ColumnKey varchar(50),
	@ColumnCaption varchar(50),
	@ColumnFormula varchar(1000),
	@ColumnPosition int,
	@ColumnType varchar(50)
)
AS
	insert into T_WatchlistColumns(ColumnKey,ColumnCaption,ColumnFormula,ColumnPosition,ColumnType) 
	values(@ColumnKey,@ColumnCaption,@ColumnFormula,@ColumnPosition,@ColumnType)
	
	


