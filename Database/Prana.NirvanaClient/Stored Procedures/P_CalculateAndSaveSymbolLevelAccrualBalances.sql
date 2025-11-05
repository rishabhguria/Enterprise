CREATE PROCEDURE [dbo].[P_CalculateAndSaveSymbolLevelAccrualBalances]
  @userID INT  
 ,@fundIDs VARCHAR(max) = NULL  
 ,@isUpdated BIT = 1  
 ,@startDayOfAccrualDate DateTime  
 ,@endBalDate DateTime
AS  
--Usage:  exec P_CalculateAndSaveSubAccountBalances 17,1,1,'2019-07-22' , '2019-07-26'  
/*  
Declare @userID int    
Declare @fundIDs varchar(max)      
    
set @userID = 17    
set @fundIDs = N'1184,1185,1190,1183,1182'    
*/  
CREATE TABLE #Funds (FundID INT)  
  
IF (@fundIDs IS NULL)  
BEGIN  
 INSERT INTO #Funds  
 SELECT CompanyFundID  
 FROM T_CompanyFunds  
 WHERE IsActive=1 AND T_CompanyFunds.CompanyID = CASE   
   WHEN @userID IS NULL  
    THEN (  
      SELECT TOP 1 T_Company.CompanyID  
      FROM T_Company  
      WHERE T_Company.CompanyID > 0  
      )  
   ELSE (  
     SELECT T_CompanyUser.CompanyID  
     FROM T_CompanyUser  
     WHERE UserID = @userID  
     )  
   END  
END  
ELSE  
BEGIN  
 INSERT INTO #Funds  
 SELECT Items AS FundID  
 FROM dbo.Split(@fundIDs, ',')  
END  
  
CREATE TABLE #tempcashpreferences (  
 colFundID INT  
 ,colCashMgmtStartDate DATETIME  
 ,IsCashSettlementEntriesVisible BIT  
 )  
  
INSERT INTO #tempcashpreferences  
SELECT FundID  
 ,CashMgmtStartDate  
 ,IsCashSettlementEntriesVisible  
FROM T_CashPreferences WITH(NOLOCK)
WHERE FundID In (Select FundID FROM #Funds) 
   
------------------------------------------------------------------------------------------------------------       
DECLARE @MinDate DATETIME  
  
SELECT @MinDate = min(LastCalcDate)  
FROM T_LastCalculatedBalanceDate WITH(NOLOCK) 
WHERE FundID In (Select FundID FROM #Funds)   
  
CREATE TABLE #mindate (  
 colFundID INT  
 ,colSubAccountID INT  
 ,colCurrency INT  
 ,collastCalcDate DATETIME  
 )  
  
INSERT INTO #mindate  
SELECT FundID  
 ,SubAcID  
 ,CurrencyID  
 ,LastCalcDate  
FROM T_LastCalculatedBalanceDate WITH(NOLOCK) 
Where FundID In (Select FundID FROM #Funds)

DECLARE @MaxDate DATETIME  
  
SELECT @MaxDate = GetDate()  
  
-- Get Default AUEC                                                                                                              
DECLARE @DefaultAUECID INT  
  
SET @DefaultAUECID = CASE   
  WHEN @userID IS NULL  
   THEN (  
     SELECT TOP 1 DefaultAUECID  
     FROM T_Company  
     )  
  ELSE (  
    SELECT DefaultAUECID  
    FROM T_Company  
    WHERE CompanyID = (  
      SELECT CompanyID  
      FROM T_CompanyUser  
      WHERE UserID = @userID  
      )  
    )  
  END  
  
DECLARE @BusinessAdjStartDate DATETIME  
SET @BusinessAdjStartDate = dbo.AdjustBusinessDays(@MinDate, - 1, @DefaultAUECID)  

Select J.FundID,SubAccountID,CurrencyID,Symbol,DATEADD(dd, 0, DATEDIFF(dd, 0, TransactionDate)) As TransactionDate,DR,CR,TransactionSource,FxRate,FXConversionMethodOperator into #T_Jour
from DBO.T_SymbolLevelAccrualsJournal J WITH(NOLOCK)
WHERE J.FundID In (Select FundID FROM #Funds)
--INNER JOIN #Funds ON J.FundID = #Funds.FundID 
  
CREATE TABLE #FXConversionRates (  
 FromCurrencyID INT  
 ,ToCurrencyID INT  
 ,RateValue FLOAT  
 ,ConversionMethod INT  
 ,DATE DATETIME  
 ,eSignalSymbol VARCHAR(max)  
 ,FundID INT  
 )  
  
SELECT j.FundID  
 ,SubAccountID  
 ,CurrencyID
 ,Symbol  
 ,TransactionDate  
 ,DR  
 ,CR  
 ,CASE   
  WHEN J.CurrencyID = TC.LocalCurrency  
   THEN 1  
  WHEN FxRate IS NOT NULL  
   AND FxRate <> 0  
   AND J.FXConversionMethodOperator = 'M'  
   AND CurrencyID <> TC.LocalCurrency  
   THEN FxRate  
  WHEN FxRate IS NOT NULL  
   AND FxRate <> 0  
   AND J.FXConversionMethodOperator = 'D'  
   AND CurrencyID <> TC.LocalCurrency  
   THEN 1 / FxRate  
  END AS FxRate  
 ,TC.LocalCurrency AS BaseCurrencyID  
INTO #T_Journal  
FROM #T_Jour J  
--added by: Bharat raturi, 30 jul 2014    
--get the last calculated date fund-wise    
--INNER JOIN #Funds ON J.FundID = #Funds.FundID  
INNER JOIN T_CompanyFunds AS TC ON TC.CompanyFundID = J.FundID  
INNER JOIN #tempcashPreferences ON colFundID = J.FundID  
INNER JOIN #mindate mndt ON mndt.colFundID = J.FundID  
 AND J.SubAccountID = mndt.colSubAccountID  
 AND J.CurrencyID = mndt.colCurrency  
WHERE DATEDIFF(d, J.TransactionDate, mndt.collastCalcDate) <= 0  
 AND (  
  (  
   J.TransactionSource = 11  
   AND #tempcashPreferences.IsCashSettlementEntriesVisible = 1  
   )  
  OR J.TransactionSource <> 11  
  )  
  
IF EXISTS (  
  SELECT TOP 1 FundID  
  FROM #T_Journal  
  WHERE (  
    FxRate IS NULL  
    OR FxRate = 0  
    )  
  )  
BEGIN  
 INSERT INTO #FXConversionRates  
 EXEC P_GetAllFXConversionRatesFundWiseForGivenDateRange @BusinessAdjStartDate  
  ,@MaxDate  
  
 UPDATE #FXConversionRates  
 SET RateValue = 1.0 / RateValue  
 WHERE RateValue <> 0  
  AND ConversionMethod = 1  
  
 UPDATE #T_Journal  
 SET #T_Journal.FxRate = IsNull(coalesce(FX.RateValue, FX1.RateValue), 0)  
 FROM #T_Journal  
 LEFT OUTER JOIN #FXConversionRates FX ON #T_Journal.CurrencyId = FX.FromCurrencyID  
  AND #T_Journal.BaseCurrencyID = FX.ToCurrencyID  
  AND DateDiff(d, #T_Journal.TransactionDate, FX.DATE) = 0  
  AND FX.FundID = #T_Journal.FundID  
 LEFT OUTER JOIN #FXConversionRates FX1 ON #T_Journal.CurrencyId = FX1.FromCurrencyID  
  AND #T_Journal.BaseCurrencyID = FX1.ToCurrencyID  
  AND DateDiff(d, #T_Journal.TransactionDate, FX1.DATE) = 0  
  AND FX1.FundID = 0  
 WHERE (  
   #T_Journal.FxRate IS NULL  
   OR #T_Journal.FxRate = 0  
   )  
END  
  
----------------------------------------------------------------------------------------------                  
SELECT ROW_NUMBER() OVER (  
  ORDER BY tj.FundID  
   ,SubAccountID  
   ,CurrencyID
   ,tj.Symbol  
   ,TransactionDate ASC  
  ) AS 'SeqNo'  
 ,tj.FundID  
 ,SubAccountID  
 ,CurrencyID 
 ,tj.Symbol 
 ,TransactionDate  
 ,Sum(Dr) AS CurrentDr  
 ,Sum(Cr) AS CurrentCr  
 ,Sum(Dr * FxRate) AS CurrentDrBase  
 ,Sum(Cr * FxRate) AS CurrentCrBase  
 ,COUNT_BIG(*) AS Count  
INTO #T_Journal_Grouped  
FROM #T_Journal tj  
INNER JOIN #Funds fn ON tj.FundID = fn.FundID  
GROUP BY tj.FundID  
 ,SubAccountID  
 ,CurrencyID
 ,tj.Symbol  
 ,TransactionDate  
  
  SELECT *  
INTO #SubAccountDayBalance  
FROM (  
 SELECT v.FundId  
  ,v.SubAccountID  
  ,v.CurrencyId  
  ,v.Symbol
  ,v.TransactionDate  
  ,IsNull(v.CurrentDr, 0) AS CurrentDr  
  ,IsNull(v.CurrentCr, 0) AS CurrentCr  
  ,IsNull(v.CurrentDrBase, 0) AS CurrentDrBase  
  ,IsNull(v.CurrentCrBase, 0) AS CurrentCrBase  
 FROM #T_Journal_Grouped v  
 INNER JOIN T_LastCalculatedBalanceDate c WITH(NOLOCK) ON v.FundID = c.FundID  
  AND v.SubAccountID = c.SubAcID  
  AND v.CurrencyID = c.CurrencyID  
 WHERE DateDiff(d, c.LastCalcDate, v.TransactionDate) >= 0  
   
 UNION  
   
 SELECT d.FundId  
  ,d.SubAccountID  
  ,d.CurrencyId  
  ,d.Symbol
  ,d.TransactionDate  
  ,IsNull(d.CloseDrBal, 0) AS DayDr  
  ,IsNull(d.CloseCrBal, 0) AS DayCr  
  ,IsNull(d.CloseDrBalBase, 0) AS DayDrBase  
  ,IsNull(d.CloseCrBalBase, 0) AS DayCrBase  
 FROM T_SymbolLevelAccrualsSubAccountBalances d WITH(NOLOCK)
 INNER JOIN T_LastCalculatedBalanceDate e WITH(NOLOCK) ON d.FundID = e.FundID  
  AND d.SubAccountID = e.SubAcID  
  AND d.CurrencyID = e.CurrencyID  
 --added by: Bharat raturi, 30 july 2014    
 --purpose: bind the cash management start dates fund bu fund    
 INNER JOIN #tempcashpreferences tpref ON d.FundID = colFundID  
 INNER JOIN #Funds fn ON fn.FundID = d.FundID  
 ------------------------------------------------------------------------    
 WHERE (DateDiff(d, DATEADD(day, - 1, e.LastCalcDate), d.TransactionDate) = 0)  
  --commented by: Bharat raturi, 30 july 2014    
  --purpose: Compare the cash management start date on fund basis                      
  --and (DateDiff(d,@CashMgmtStartDate, d.TransactionDate) >= 0)       
  AND (DateDiff(d, tpref.colCashMgmtStartDate, d.TransactionDate) >= 0)  

   UNION  
    
SELECT SLASB.FundId  
  ,SLASB.SubAccountID  
  ,SLASB.CurrencyId  
  ,' ' As Symbol
  ,SLASB.TransactionDate  
  ,IsNull(SLASB.CloseDrBal, 0) AS DayDr  
  ,IsNull(SLASB.CloseCrBal, 0) AS DayCr  
  ,IsNull(SLASB.CloseDrBalBase, 0) AS DayDrBase  
  ,IsNull(SLASB.CloseCrBalBase, 0) AS DayCrBase  
FROM T_SubAccountBalances SLASB inner join T_CashPreferences TCP ON TCP.FundID=SLASB.FundID
INNER JOIN T_LastCalculatedBalanceDate e WITH(NOLOCK) ON tcp.FundID = e.FundID and SLASB.SubAccountID = e.SubAcID  AND SLASB.CurrencyID = e.CurrencyID
INNER JOIN #Funds F ON F.FundID=TCP.FundID 
INNER JOIN T_SubAccounts S ON SLASB.SubAccountID = S.SubAccountID
INNER JOIN T_TransactionType TS ON S.TransactionTypeID = TS.TransactionTypeID
WHERE TS.TransactionType = 'Accrued Balance' and TCP.SymbolWiseRevaluationDate=SLASB.TransactionDate
AND ((DateDiff(d, DATEADD(day, - 1, e.LastCalcDate), SLASB.TransactionDate) = 0) OR (DATEDIFF(d, e.LastCalcDate, SLASB.TransactionDate) = 0))
 ) AS TEMP  
  
-- End of code written to enhance the performance                          
SELECT ROW_NUMBER() OVER (  
  ORDER BY a.FundId  
   ,a.SubAccountID  
   ,a.CurrencyId 
   ,a.Symbol  
   ,a.TransactionDate ASC  
  ) AS 'SeqNo'  
 ,a.FundId  
 ,a.SubAccountID  
 ,a.CurrencyId
 ,a.Symbol  
 ,a.TransactionDate  
 ,min(IsNull(a.CurrentDr, 0)) AS DayDr  
 ,min(IsNull(a.CurrentCr, 0)) AS DayCr  
 ,min(IsNull(a.CurrentDrBase, 0)) AS DayDrBase  
 ,min(IsNull(a.CurrentCrBase, 0)) AS DayCrBase  
 ,CASE   
  WHEN (Sum(IsNull(b.CurrentDr, 0)) > Sum(IsNull(b.CurrentCr, 0)))  
   THEN (Sum(IsNull(b.CurrentDr, 0)) - Sum(IsNull(b.CurrentCr, 0)))  
  ELSE 0  
  END AS CloseDrBal  
 ,CASE   
  WHEN Sum(IsNull(b.CurrentCr, 0)) > Sum(IsNull(b.CurrentDr, 0))  
   THEN (Sum(IsNull(b.CurrentCr, 0)) - Sum(IsNull(b.CurrentDr, 0)))  
  ELSE 0  
  END AS CloseCrBal  
 ,CASE   
  WHEN (Sum(IsNull(b.CurrentDrBase, 0)) > Sum(IsNull(b.CurrentCrBase, 0)))  
   THEN (Sum(IsNull(b.CurrentDrBase, 0)) - Sum(IsNull(b.CurrentCrBase, 0)))  
  ELSE 0  
  END AS CloseDrBalBase  
 ,CASE   
  WHEN Sum(IsNull(b.CurrentCrBase, 0)) > Sum(IsNull(b.CurrentDrBase, 0))  
   THEN (Sum(IsNull(b.CurrentCrBase, 0)) - Sum(IsNull(b.CurrentDrBase, 0)))  
  ELSE 0  
  END AS CloseCrBalBase  
-- 0 as IsCarryForwarded is put to show that it is original entry                            
INTO #TempAccDateBalances  
FROM #SubAccountDayBalance a  
CROSS JOIN #SubAccountDayBalance b  
INNER JOIN #Funds fn ON a.FundID = fn.FundID  
WHERE DateDiff(d, a.TransactionDate, b.TransactionDate) <= 0  
 AND a.FundID = b.FundID  
 AND a.SubAccountID = b.SubAccountID  
 AND a.Symbol = b.Symbol  
 AND a.CurrencyID = b.CurrencyID  
GROUP BY a.FundId  
 ,a.SubAccountID 
 ,a.Symbol  
 ,a.CurrencyID 
 ,a.TransactionDate  
ORDER BY a.FundId  
 ,a.SubAccountID  
 ,a.Symbol  
 ,a.CurrencyId
 ,a.TransactionDate  
  
--------------Updating Corrupted Data - Added by Surendra Bisht--------------------------------------------------------------------------------------------              
SELECT d.FundId  
 ,d.SubAccountID  
 ,d.CurrencyId  
 ,d.TransactionDate  
 ,IsNull(d.DayDr, 0) AS DayDr  
 ,IsNull(d.DayCr, 0) AS DayCr  
 ,IsNull(d.DayDrBase, 0) AS DayDrBase  
 ,IsNull(d.DayCrBase, 0) AS DayCrBase  
INTO #originaldata  
FROM T_SymbolLevelAccrualsSubAccountBalances d WITH(NOLOCK) 
INNER JOIN T_LastCalculatedBalanceDate e WITH(NOLOCK) ON d.FundID = e.FundID  
 AND d.SubAccountID = e.SubAcID  
 AND d.CurrencyID = e.CurrencyID  
--Added by: Bharat raturi, 30 jul 2014    
--purpose: Get the cash msmt start date bound fund by fund    
INNER JOIN #tempcashpreferences tpref ON d.FundID = tpref.colFundID  
INNER JOIN #Funds fn ON fn.FundID = d.FundID  
-------------------------------------------------------------------------    
WHERE (DateDiff(d, DATEADD(day, - 1, e.LastCalcDate), d.TransactionDate) = 0)  
 --commented by: Bharat raturi, 30 jul 2014    
 --purpose: check the cash mgmt start date fund by fund    
 --and (DateDiff(d,@CashMgmtStartDate, d.TransactionDate) >= 0)                
 AND (DateDiff(d, tpref.colCashMgmtStartDate, d.TransactionDate) >= 0)  
  
UPDATE #TempAccDateBalances  
SET DayDr = orig.DayDr  
 ,DayCr = orig.DayCr  
 ,DayDrBase = orig.DayDrBase  
 ,DayCrBase = orig.DayCrBase  
--select *            
FROM #TempAccDateBalances corrupt  
INNER JOIN #originaldata orig ON corrupt.FundID = orig.FundID  
INNER JOIN #Funds fn ON fn.FundID = orig.FundID  
 AND corrupt.SubAccountID = orig.SubAccountID  
 AND corrupt.CurrencyID = orig.CurrencyID  
 AND corrupt.TransactionDate = orig.TransactionDate  
  
-----------------------------------------------------------------------------------------------------------------------------------------------------------            
-- Logic written to carry forward the closing balances as opening balances for those days where no day transaction was done.                            
-- Here found out that till what date we need to carry forward the balances.                            
SELECT a.*  
 ,IsNull(dateadd(d, - 1, b.TransactionDate), GetDate()) AS CarryForwardTillDate  
INTO #BeforeOpenBal  
FROM #TempAccDateBalances a  
LEFT JOIN #TempAccDateBalances b  
 ON a.FundId = b.FundId  
 AND a.SubAccountId = b.SubAccountId  
 AND a.Symbol = b.Symbol  
 AND a.CurrencyId = b.CurrencyId 
 AND a.SeqNo = b.SeqNo - 1  
WHERE a.fundID IN (  
  SELECT FundID  
  FROM #Funds  
  )  
  
DECLARE @minTransactionDate DATETIME  
  
SELECT @minTransactionDate = min(TransactionDate)  
FROM #TempAccDateBalances  
  
SELECT *  
INTO #TempCalender  
FROM dbo.F_GetCalenderForDateRange(@minTransactionDate, getdate())  
  
-- Delete all the balances and insert back the newly calculated balances. This process has to be optimized else it would create problems as the data gets bigger.                            
-- First setback the identity column to 0, so that first row insert can start from 1.                            
--DBCC CHECKIDENT('T_SymbolLevelAccrualsSubAccountBalances', RESEED, 0)                            
DELETE T_SymbolLevelAccrualsSubAccountBalances  
FROM T_SymbolLevelAccrualsSubAccountBalances d  
INNER JOIN T_LastCalculatedBalanceDate c WITH(NOLOCK) ON d.FundID = c.FundID  
 AND d.SubAccountID = c.SubAcID  
 AND d.CurrencyID = c.CurrencyID  
--added by: Bharat raturi, 30 jul 2014    
--purpose: add the preferences for the funds     
INNER JOIN #tempcashpreferences tpref ON d.FundID = tpref.colFundID  
-------------------------------------------------------------------    
WHERE d.FundID IN (  
  SELECT fundID  
  FROM #Funds  
  )  
 AND (  
  (DateDiff(d, DATEADD(day, - 1, c.LastCalcDate), d.TransactionDate) >= 0)  
  --commented by: Bharat raturi, 30 jul 2014    
  --purpose: cash mgmt date check fund by fund    
  --OR datediff(d,d.TransactionDate,@CashMgmtStartDate) > 0                        
  OR datediff(d, d.TransactionDate, tpref.colCashMgmtStartDate) > 0  
  )  
  
--                          
-- For carry forwarded rows, openCr and openDr is simply equals to the last day's closeDr and closeCr.                             
INSERT INTO T_SymbolLevelAccrualsSubAccountBalances (  
 FundID  
 ,SubAccountID  
 ,CurrencyID
 ,Symbol 
 ,OpenBalDate  
 ,TransactionDate  
 ,OpenDrBal  
 ,OpenCrBal  
 ,OpenDrBalBase  
 ,OpenCrBalBase  
 ,DayDr  
 ,DayCr  
 ,DayDrBase  
 ,DayCrBase  
 ,CloseDrBal  
 ,CloseCrBal  
 ,CloseDrBalBase  
 ,CloseCrBalBase  
 ,IsCarryForwarded  
 )  
SELECT BeforeBal.FundId  
 ,BeforeBal.SubAccountId  
 ,BeforeBal.CurrencyId  
 ,BeforeBal.Symbol
 ,BeforeBal.TransactionDate AS OpenBalDate  
 ,Cal.DATE AS TransactionDate  
 ,CASE   
  WHEN DateDiff(d, TransactionDate, DATE) <> 0  
   THEN CloseDrBal  
  ELSE 0  
  END AS OpenDrBal  
 ,CASE   
  WHEN DateDiff(d, TransactionDate, DATE) <> 0  
   THEN CloseCrBal  
  ELSE 0  
  END AS OpenCrBal  
 ,CASE   
  WHEN DateDiff(d, TransactionDate, DATE) <> 0  
   THEN CloseDrBalBase  
  ELSE 0  
  END AS OpenDrBalBase  
 ,CASE   
  WHEN DateDiff(d, TransactionDate, DATE) <> 0  
   THEN CloseCrBalBase  
  ELSE 0  
  END AS OpenCrBalBase  
 ,CASE   
  WHEN DateDiff(d, TransactionDate, DATE) <> 0  
   THEN 0  
  ELSE DayDr  
  END AS DayDr  
 ,CASE   
  WHEN DateDiff(d, TransactionDate, DATE) <> 0  
   THEN 0  
  ELSE DayCr  
  END AS DayCr  
 ,CASE   
  WHEN DateDiff(d, TransactionDate, DATE) <> 0  
   THEN 0  
  ELSE DayDrBase  
  END AS DayDrBase  
 ,CASE   
  WHEN DateDiff(d, TransactionDate, DATE) <> 0  
   THEN 0  
  ELSE DayCrBase  
  END AS DayCrBase  
 ,CloseDrBal  
 ,CloseCrBal  
 ,CloseDrBalBase  
 ,CloseCrBalBase  
 ,CASE   
  WHEN DateDiff(d, TransactionDate, DATE) = 0  
   THEN 0  
  ELSE 1  
  END AS IsCarryForwarded  
FROM #BeforeOpenBal BeforeBal  
CROSS JOIN #TempCalender Cal  
INNER JOIN #Funds fn ON fn.FundID = BeforeBal.FundId  
WHERE DateDiff(d, BeforeBal.TransactionDate, Cal.DATE) >= 0  
 AND DateDiff(d, BeforeBal.CarryForwardTillDate, Cal.DATE) <= 0  
ORDER BY fn.FundId  
 ,SubAccountId  
 ,Symbol  
 ,CurrencyId  
 ,Cal.DATE  
  
----Updated the table to correct the OpenBalDate, OpenDrBal and OpenCrBal for those rows which are not carry forwarded.                          
--UPDATE a  
--SET OpenBalDate = IsNull(b.TransactionDate, a.OpenBalDate)  
-- ,OpenDrBal = IsNull(b.CloseDrBal, 0)  
-- ,OpenCrBal = IsNull(b.CloseCrBal, 0)  
-- ,OpenDrBalBase = IsNull(b.CloseDrBalBase, 0)  
-- ,OpenCrBalBase = IsNull(b.CloseCrBalBase, 0)  
--FROM dbo.T_SymbolLevelAccrualsSubAccountBalances a  
--LEFT JOIN T_SymbolLevelAccrualsSubAccountBalances b ON a.FundId = b.FundId  
-- AND a.SubAccountId = b.SubAccountId  
-- AND a.Symbol = b.Symbol
-- AND a.CurrencyId = b.CurrencyId  
-- AND DateDiff(d, a.TransactionDate - 1, b.TransactionDate) = 0  
--WHERE a.IsCarryForwarded = 0  
-- AND a.FundID IN (  
--  SELECT fundID  
--  FROM #Funds  
--  ) 

	UPDATE a  
	SET OpenBalDate = IsNull(b.TransactionDate, a.OpenBalDate)  
	 ,OpenDrBal = IsNull(b.CloseDrBal, 0)  
	 ,OpenCrBal = IsNull(b.CloseCrBal, 0)  
	 ,OpenDrBalBase = IsNull(b.CloseDrBalBase, 0)  
	 ,OpenCrBalBase = IsNull(b.CloseCrBalBase, 0)  
	FROM dbo.T_SymbolLevelAccrualsSubAccountBalances a WITH(ROWLOCK, UPDLOCK)
	INNER JOIN #Funds Fund ON Fund.FundID = a.FundID  
	INNER JOIN T_LastCalculatedBalanceDate LastCalc WITH(NOLOCK) ON a.FundID = LastCalc.FundID AND a.SubAccountID = LastCalc.SubAcID AND a.CurrencyID = LastCalc.CurrencyID
	LEFT JOIN T_SymbolLevelAccrualsSubAccountBalances b WITH(NOLOCK)
	ON a.FundId = b.FundId  
	 AND a.SubAccountId = b.SubAccountId  
	 AND a.Symbol = b.Symbol
	 AND a.CurrencyId = b.CurrencyId  
	 --AND DateDiff(d, a.TransactionDate - 1, b.TransactionDate) = 0 
	 AND b.TransactionDate = DATEADD(DAY, -1, a.TransactionDate)
	WHERE a.IsCarryForwarded = 0  
	AND a.TransactionDate >= Cast(DATEADD(day, - 1, LastCalc.LastCalcDate) As Date)

--select * from #T_Journal
--select * from #T_Journal_Grouped
--select * from #SubAccountDayBalance
--select * from #TempAccDateBalances
--select * from #BeforeOpenBal
--select * from T_SymbolLevelAccrualsSubAccountBalances
  
DROP TABLE #TempAccDateBalances  
          ,#TempCalender 
		  ,#BeforeOpenBal
		  ,#SubAccountDayBalance
		  ,#T_Journal
		  ,#Funds
  
  
DROP TABLE #FXConversionRates  
          ,#T_Journal_Grouped  
		  ,#originaldata 
          ,#T_Jour  
          ,#tempcashpreferences