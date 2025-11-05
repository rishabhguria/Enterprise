/*						
--------------------------------------------------------------------------------------
 Deletes rows from PM_DayMarkPrices where symbol mark price is zero.				
--------------------------------------------------------------------------------------
*/
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'PM_DayMarkPrice' AND COLUMN_NAME = 'FinalMarkPrice')
	BEGIN
	DELETE FROM PM_DayMarkPrice
	WHERE FinalMarkPrice = 0
	END