CREATE PROCEDURE [dbo].[P_SaveShareOutstandingInSM]
	@shareOutstandingValue float,
	@symbol VARCHAR(100)
AS
	Update T_SMSymbolLookUpTable 
	SET SharesOutstanding = @shareOutstandingValue
	where TickerSymbol = @symbol
