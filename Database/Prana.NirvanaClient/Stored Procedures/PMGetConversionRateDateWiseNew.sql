/****************************************************************************                        
Name :   [PMGetConversionRateDateWiseNew]    
Purpose:  Returns all the conversion forex rate for the date range passed.    
Module: MarkPriceAndForexConversion/PM    
Author: Sandeep Singh    
Parameters:                         
  @ErrorMessage varchar(500)                         
  , @ErrorNumber int                          
Execution StateMent:                         
   EXEC [PMGetConversionRateDateWiseNew] '02-18-2008' , '02-18-2008', 0, ' ', 0    
                        
Date Modified:                         
Description:                           
Modified By:                           
****************************************************************************/
CREATE PROCEDURE [dbo].[PMGetConversionRateDateWiseNew] (
	@fromDate DATETIME
	,@ToDate DATETIME
	,@Type INT
	,-- 0 for Same Date,1 for Week , 2 for Month                    
	@ErrorMessage VARCHAR(500) OUTPUT
	,@ErrorNumber INT OUTPUT
	)
AS
DECLARE @Dates VARCHAR(2000)
DECLARE @FirstDateofMonth VARCHAR(50)
DECLARE @LastDateofMonth VARCHAR(50)

IF (@Type = 0)
BEGIN
	SET @FirstDateofMonth = CONVERT(VARCHAR(25), @fromDate, 101)
	SET @LastDateofMonth = CONVERT(VARCHAR(25), @fromDate, 101)
END
ELSE IF (@Type = 1)
BEGIN
	SET @FirstDateofMonth = CONVERT(VARCHAR(25), @fromDate, 101)
	SET @LastDateofMonth = CONVERT(VARCHAR(25), @ToDate, 101)
END
ELSE IF (@Type = 2)
BEGIN
	SET @FirstDateofMonth = CONVERT(VARCHAR(25), DATEADD(dd, - (DAY(@fromDate) - 1), @fromDate), 101)
	SET @LastDateofMonth = CONVERT(VARCHAR(25), DATEADD(dd, - (DAY(DATEADD(mm, 1, @fromDate))), DATEADD(mm, 1, @fromDate)), 101)
END

-- if DATEDIFF(m,CONVERT(VARCHAR(25),GetUTCDate(),101),CONVERT(VARCHAR(25),@fromDate,101)) = 0                 
--  begin                
--   Set @LastDateofMonth=CONVERT(VARCHAR(25),GetUTcDate(),101)                
--  end                
-- else                
--  begin                
--   Set @LastDateofMonth=CONVERT(VARCHAR(25),DATEADD(dd,-(DAY(DATEADD(mm,1,@fromDate))),DATEADD(mm,1,@fromDate)),101)-- CONVERT(VARCHAR(25),GetUTcDate(),101)--                    
-- end                    
--End    
SET @ErrorMessage = 'Success'
SET @ErrorNumber = 0

BEGIN TRY
	SET @Dates = ''

	SELECT @Dates = @Dates + '[' + convert(VARCHAR(12), Items, 101) + '],'
	FROM (
		SELECT TOP 35 AllDates.Items
		FROM dbo.GetDateRange(@FirstDateofMonth, @LastDateofMonth) AS AllDates
		ORDER BY AllDates.Items DESC
		) ForexDate

	SET @Dates = LEFT(@Dates, LEN(@Dates) - 1)

	CREATE TABLE #TempAccounts (AccountID INT)

	INSERT INTO #TempAccounts (AccountID)
	VALUES (0)

	INSERT INTO #TempAccounts (AccountID)
	SELECT CompanyFundID AS AccountID
	FROM T_CompanyFunds Where IsActive=1

	CREATE TABLE [dbo].#TempStandardPairsAndAccounts (
		CurrencyPairID INT
		,FromCurrencyID INT
		,ToCurrencyID INT
		,Symbol VARCHAR(100)
		,AccountID INT
		)

	INSERT INTO #TempStandardPairsAndAccounts (
		CurrencyPairID
		,FromCurrencyID
		,ToCurrencyID
		,Symbol
		,AccountID
		)
	SELECT CSP.CurrencyPairID
		,CSP.FromCurrencyID
		,CSP.ToCurrencyID
		,CSP.eSignalSymbol AS Symbol
		,CF.AccountID AS AccountID
	FROM #TempAccounts CF 
    CROSS JOIN T_CurrencyStandardPairs CSP

	CREATE TABLE #TempDailyForexRate (
		[Date] DATETIME
		,[FromCurrencyID] INT
		,[ToCurrencyID] INT
		,[Symbol] VARCHAR(100)
		,[Summary] VARCHAR(MAX)
		,[ConversionRate] FLOAT(53)
		,[AccountID] INT
		)

	INSERT INTO #TempDailyForexRate (
		[Date]
		,[FromCurrencyID]
		,[ToCurrencyID]
		,[Symbol]
		,[Summary]
		,[ConversionRate]
		,[AccountID]
		)
	SELECT ISNULL(CCR.[Date], @fromDate) as [Date]
		,CSP.FromCurrencyID
		,CSP.ToCurrencyID
		,CSP.Symbol
		,' '
		,ISNULL(CCR.ConversionRate, 0)
		,CSP.AccountID
	FROM #TempStandardPairsAndAccounts CSP
	LEFT JOIN T_CurrencyConversionRate CCR
		ON CSP.CurrencyPairID = CCR.CurrencyPairID_FK
			AND CSP.AccountID = CCR.FundID

	exec ('select *                  
		from #TempDailyForexRate
		AS DMP PIVOT (MAX(ConversionRate) FOR Date IN (' + @Dates + ')) AS pvt ; ') 
END TRY

BEGIN CATCH
	SET @ErrorMessage = ERROR_MESSAGE();
	SET @ErrorNumber = Error_number();
END CATCH;
