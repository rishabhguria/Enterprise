CREATE PROCEDURE [dbo].[P_CalculateAndSaveSubAccountBalances] @userID INT  
 ,@fundIDs VARCHAR(max) = NULL  
 ,@isUpdated BIT = 1  
 ,@startDayOfAccrualDate DateTime  
 ,@endBalDate DateTime
AS  

--Declare @userID INT  
-- ,@fundIDs VARCHAR(max) = NULL  
-- ,@isUpdated BIT = 1  
-- ,@startDayOfAccrualDate DateTime  
-- ,@endBalDate DateTime

--Usage: exec P_CalculateAndSaveSubAccountBalances null, null  
/*  
Declare @userID int    
Declare @fundIDs varchar(max)      
    
set @userID = 17    
set @fundIDs = N'1184,1185,1190,1183,1182'    
*/  
CREATE TABLE #Funds (FundID INT)  
declare @fundIdsForSymbolRevaluation varchar(1000)
CREATE TABLE #FundsForSymbolWiseRevaluation (FundID INT)  

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

INSERT INTO #FundsForSymbolWiseRevaluation 
SELECT CP.FundID FROM T_CashPreferences CP 
INNER JOIN #Funds F ON CP.FundID=F.FundID
WHERE CP.SymbolWiseRevaluationDate IS NOT NULL AND CP.SymbolWiseRevaluationDate<=@endBalDate
  
select @fundIdsForSymbolRevaluation = coalesce(@fundIdsForSymbolRevaluation+',','') + cast(FundID as varchar(10)) from #FundsForSymbolWiseRevaluation

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
  
------------------Insert missing key entries from journal from CM StartDate------------------------------                     
SELECT DISTINCT (cast(FundID AS VARCHAR(20)) + '_' + cast(SubAcID AS VARCHAR(20)) + '_' + cast(CurrencyID AS VARCHAR(20))) AS uniquekey  
INTO #TempLastCalculatedBalanceDate  
FROM T_LastCalculatedBalanceDate WITH(NOLOCK)
WHERE FundID In (Select FundID FROM #Funds)
  
INSERT INTO T_LastCalculatedBalanceDate (  
 FundID  
 ,SubAcID  
 ,CurrencyID  
 ,LastCalcDate  
 )  
SELECT T_Journal.FundID  
 ,SubAccountID  
 ,CurrencyID  
 ,min(TransactionDate)  
FROM DBO.T_Journal  
INNER JOIN #tempcashPreferences ON colFundID = FundID  
WHERE DATEDIFF(d, TransactionDate, colCashMgmtStartDate) <= 0  
AND T_Journal.FundID In (Select FundID FROM #Funds)
GROUP BY T_Journal.FundID  
 ,SubAccountID  
 ,CurrencyID  
HAVING cast(T_Journal.FundID AS VARCHAR(20)) + '_' + cast(SubAccountID AS VARCHAR(20)) + '_' + cast(CurrencyID AS VARCHAR(20)) NOT IN (  
  SELECT *  
  FROM #TempLastCalculatedBalanceDate  
  )  
  
--To Delete duplicate entries
;WITH LastCalculatedBalanceDate(SubAcID,  FundID,  CurrencyID, Ranking)          
AS          
(          
SELECT          
SubAcID,  FundID,  currencyid ,         
Ranking = DENSE_RANK() OVER(PARTITION BY SubAcID,  FundID,  CurrencyID ORDER BY NEWID() ASC)          
FROM T_LastCalculatedBalanceDate WITH(NOLOCK)
WHERE FundID In (Select FundID FROM #Funds)     
)          
DELETE
FROM LastCalculatedBalanceDate
WHERE Ranking > 1 
    
-------------------------------Calculation should start from CM StartDate------------------------------------                          
UPDATE T_LastCalculatedBalanceDate  
SET LastCalcDate = tpref.colCashMgmtStartDate  
FROM #tempcashpreferences tpref  
INNER JOIN T_LastCalculatedBalanceDate lcbd ON lcbd.FundID = tpref.colFundID  
WHERE DATEDIFF(d, LastCalcDate, tpref.colCashMgmtStartDate) > 0  
AND FundID In (Select FundID FROM #Funds)   


/*
Keep data in a temp table from T_LastCalculatedBalanceDate.
LastCalcDate column has date and time value and while comparing, we need only Date value only.
Also there are many joins with this table, so temp will help here
*/

Select 
SubAcID,
FundID,
CurrencyID,
Cast(LastCalcDate As Date) As LastCalcDate,
UpdatedBalance
InTo #Temp_LastCalculatedBalanceDate
From T_LastCalculatedBalanceDate WITH(NOLOCK)
WHERE FundID In (Select FundID FROM #Funds) 

------------------------------------------------------------------------------------------------------------       
DECLARE @MinDate DATETIME  
  
SELECT @MinDate = min(LastCalcDate)  
--FROM T_LastCalculatedBalanceDate WITH(NOLOCK) 
FROM #Temp_LastCalculatedBalanceDate 
--WHERE FundID In (Select FundID FROM #Funds) 

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
--FROM T_LastCalculatedBalanceDate WITH(NOLOCK) 
FROM #Temp_LastCalculatedBalanceDate
--WHERE FundID In (Select FundID FROM #Funds) 
  
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
  
IF (EXISTS(select FundID from #FundsForSymbolWiseRevaluation))
BEGIN

EXEC [P_CalculateAndSaveSymbolLevelAccrualBalances]@userID  
 ,@fundIdsForSymbolRevaluation 
 ,@isUpdated
 ,@startDayOfAccrualDate 
 ,@endBalDate 

END
----------------------------------------------------------------------------------------------------------------------------------------
Create Table #T_Jour
(
FundId Int,
SubAccountID Int,
CurrencyID Int,
TransactionDate DateTime,
DR Float,
CR Float,
TransactionSource Int,
FxRate Float,
FXConversionMethodOperator Varchar(10)
)

Insert InTo #T_Jour
Select J.FundID,SubAccountID,CurrencyID,DATEADD(dd, 0, DATEDIFF(dd, 0, TransactionDate)) As TransactionDate,DR,CR,TransactionSource,FxRate,FXConversionMethodOperator 
--into #T_Jour
from DBO.T_Journal J WITH(NOLOCK)
WHERE J.FundID In (Select FundID FROM #Funds)

  
CREATE TABLE #FXConversionRates (  
 FromCurrencyID INT  
 ,ToCurrencyID INT  
 ,RateValue FLOAT  
 ,ConversionMethod INT  
 ,DATE DATETIME  
 ,eSignalSymbol VARCHAR(max)  
 ,FundID INT  
 ) 
 
Create Table #T_Journal
(
FundId Int,
SubAccountID Int,
CurrencyID Int,
TransactionDate DateTime,
DR Float,
CR Float,
FxRate Float,
BaseCurrencyID Int
) 
 
Insert InTo #T_Journal
SELECT j.FundID  
 ,SubAccountID  
 ,CurrencyID  
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
--INTO #T_Journal  
FROM #T_Jour J    
INNER JOIN T_CompanyFunds AS TC ON TC.CompanyFundID = J.FundID  
INNER JOIN #tempcashPreferences ON colFundID = J.FundID  
Inner JOIN #mindate mndt ON mndt.colFundID = J.FundID  
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
   ,TransactionDate ASC  
  ) AS 'SeqNo'  
 ,tj.FundID  
 ,SubAccountID  
 ,CurrencyID  
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
 ,TransactionDate  
  
  SELECT *  
INTO #SubAccountDayBalance  
FROM (  
 SELECT v.FundId  
  ,v.SubAccountID  
  ,v.CurrencyId  
  ,v.TransactionDate  
  ,IsNull(v.CurrentDr, 0) AS CurrentDr  
  ,IsNull(v.CurrentCr, 0) AS CurrentCr  
  ,IsNull(v.CurrentDrBase, 0) AS CurrentDrBase  
  ,IsNull(v.CurrentCrBase, 0) AS CurrentCrBase  
 FROM #T_Journal_Grouped v  
 --INNER JOIN T_LastCalculatedBalanceDate c WITH(NOLOCK) ON v.FundID = c.FundID 
  INNER JOIN #Temp_LastCalculatedBalanceDate C ON v.FundID = c.FundID  
  AND v.SubAccountID = c.SubAcID  
  AND v.CurrencyID = c.CurrencyID  
 WHERE DateDiff(d, c.LastCalcDate, v.TransactionDate) >= 0  
   
 UNION  
   
 SELECT d.FundId  
  ,d.SubAccountID  
  ,d.CurrencyId  
  ,d.TransactionDate  
  ,IsNull(d.CloseDrBal, 0) AS DayDr  
  ,IsNull(d.CloseCrBal, 0) AS DayCr  
  ,IsNull(d.CloseDrBalBase, 0) AS DayDrBase  
  ,IsNull(d.CloseCrBalBase, 0) AS DayCrBase  
 FROM T_SubAccountBalances d WITH(NOLOCK)
 --INNER JOIN T_LastCalculatedBalanceDate e WITH(NOLOCK) ON d.FundID = e.FundID  
  INNER JOIN #Temp_LastCalculatedBalanceDate e ON d.FundID = e.FundID  
  AND d.SubAccountID = e.SubAcID  
  AND d.CurrencyID = e.CurrencyID     
 INNER JOIN #tempcashpreferences tpref ON d.FundID = colFundID  
 INNER JOIN #Funds fn ON fn.FundID = d.FundID  
 ------------------------------------------------------------------------    
 WHERE (DateDiff(d, DATEADD(day, - 1, e.LastCalcDate), d.TransactionDate) = 0)       
  AND (DateDiff(d, tpref.colCashMgmtStartDate, d.TransactionDate) >= 0)  
 ) AS TEMP  
  
-- End of code written to enhance the performance                          
SELECT ROW_NUMBER() OVER (  
  ORDER BY a.FundId  
   ,a.SubAccountID  
   ,a.CurrencyId  
   ,a.TransactionDate ASC  
  ) AS 'SeqNo'  
 ,a.FundId  
 ,a.SubAccountID  
 ,a.CurrencyId  
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
 AND a.CurrencyID = b.CurrencyID  
GROUP BY a.FundId  
 ,a.SubAccountID  
 ,a.CurrencyId  
 ,a.TransactionDate  
ORDER BY a.FundId  
 ,a.SubAccountID  
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
FROM T_SubAccountBalances d WITH(NOLOCK) 
--INNER JOIN T_LastCalculatedBalanceDate e WITH(NOLOCK) ON d.FundID = e.FundID  
INNER JOIN #Temp_LastCalculatedBalanceDate e ON d.FundID = e.FundID  
 AND d.SubAccountID = e.SubAcID  
 AND d.CurrencyID = e.CurrencyID    
INNER JOIN #tempcashpreferences tpref ON d.FundID = tpref.colFundID  
INNER JOIN #Funds fn ON fn.FundID = d.FundID  
-------------------------------------------------------------------------    
WHERE (DateDiff(d, DATEADD(day, - 1, e.LastCalcDate), d.TransactionDate) = 0)               
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
--DBCC CHECKIDENT('T_SubAccountBalances', RESEED, 0)                            
DELETE T_SubAccountBalances  
FROM T_SubAccountBalances d
--INNER JOIN T_LastCalculatedBalanceDate c ON d.FundID = c.FundID  
INNER JOIN #Temp_LastCalculatedBalanceDate c ON d.FundID = c.FundID  
 AND d.SubAccountID = c.SubAcID  
 AND d.CurrencyID = c.CurrencyID       
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
INSERT INTO T_SubAccountBalances (  
 FundID  
 ,SubAccountID  
 ,CurrencyID  
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
 ,CurrencyId  
 ,Cal.DATE  
  
----Updated the table to correct the OpenBalDate, OpenDrBal and OpenCrBal for those rows which are not carry forwarded.  

--Select FundID,SubAccountId, CurrencyId, TransactionDate, CloseDrBal, CloseCrBal, CloseDrBalBase, CloseCrBalBase
--inTo #Temp_SubAccountBalances
--From T_SubAccountBalances With (NoLock)
--Where FundID In
--(
--SELECT fundID FROM #Funds
--)     

--Select *
--From #T_SubAccountBalances

--UPDATE a  
--SET OpenBalDate = Coalesce(b.TransactionDate, a.OpenBalDate)  
-- ,OpenDrBal = Coalesce(b.CloseDrBal, 0)  
-- ,OpenCrBal = Coalesce(b.CloseCrBal, 0)  
-- ,OpenDrBalBase = Coalesce(b.CloseDrBalBase, 0)  
-- ,OpenCrBalBase = Coalesce(b.CloseCrBalBase, 0)  
--FROM dbo.T_SubAccountBalances a 
--Inner Join #Funds Fund On Fund.FundID = a.FundID
--LEFT JOIN T_SubAccountBalances b ON a.FundId = b.FundId AND a.SubAccountId = b.SubAccountId AND a.CurrencyId = b.CurrencyId  
----AND DateDiff(d, a.TransactionDate - 1, b.TransactionDate) = 0 
--AND CAST(b.TransactionDate AS DATE) = DATEADD(DAY, -1, CAST(a.TransactionDate AS DATE)) 
--WHERE a.IsCarryForwarded = 0  

UPDATE a  
SET OpenBalDate = Coalesce(b.TransactionDate, a.OpenBalDate)  
 ,OpenDrBal = Coalesce(b.CloseDrBal, 0)  
 ,OpenCrBal = Coalesce(b.CloseCrBal, 0)  
 ,OpenDrBalBase = Coalesce(b.CloseDrBalBase, 0)  
 ,OpenCrBalBase = Coalesce(b.CloseCrBalBase, 0)  
FROM dbo.T_SubAccountBalances a With (NoLock)
Inner Join #Funds Fund On Fund.FundID = a.FundID
INNER JOIN #Temp_LastCalculatedBalanceDate LastCalc ON a.FundID = LastCalc.FundID AND a.SubAccountID = LastCalc.SubAcID AND a.CurrencyID = LastCalc.CurrencyID
LEFT JOIN T_SubAccountBalances b With (NoLock) ON a.FundId = b.FundId AND a.SubAccountId = b.SubAccountId AND a.CurrencyId = b.CurrencyId  
--AND DateDiff(d, a.TransactionDate - 1, b.TransactionDate) = 0 
AND CAST(b.TransactionDate AS DATE) = DATEADD(DAY, -1, CAST(a.TransactionDate AS DATE)) 
WHERE a.IsCarryForwarded = 0 
AND a.TransactionDate >= DATEADD(day, - 1, LastCalc.LastCalcDate)


--Update T_LastCalculatedBalanceDate to update the LastCalcDate to current date     
--SET @endBalDate= CAST(FLOOR(CAST(@endBalDate AS FLOAT)) AS DATETIME)
UPDATE T_LastCalculatedBalanceDate  
SET LastCalcDate = getdate()  
 ,UpdatedBalance = @isUpdated  
WHERE FundID IN 
	(  
		SELECT fundID FROM #Funds  
	)  
  
--Get Accruals For date  
EXEC GetStartDayOfAccruals @startDayOfAccrualDate  
  
DROP TABLE #TempAccDateBalances  
          ,#TempCalender 
		  ,#BeforeOpenBal
		  ,#SubAccountDayBalance
		  ,#T_Journal
		  ,#Funds
  
  
DROP TABLE #FXConversionRates  
          ,#T_Journal_Grouped  
          ,#mindate  
          ,#TempLastCalculatedBalanceDate  
		  ,#originaldata 
          ,#T_Jour  
          ,#tempcashpreferences
		  ,#Temp_LastCalculatedBalanceDate			 

/*
Code Plugged in for PSR General Ledger
This code is meant to be executed after Revaluation Batch every time Sub Account Balances are updated
The above will run for date range as following (revaluation start date till present date
*/
-- Transaction start-----------                  
BEGIN TRANSACTION [Tran1]                  
--Start try block statement ---                  
BEGIN TRY 
-----------------------------------------------------------------------------------------
--------------Get Fields Data for date range
-----------------------------------------------------------------------------------------
Declare @todate datetime
SET @todate = GETDATE()

exec P_GetFieldsDataForDateRange 
@StartDate = @MinDate,
@EndDate = @todate,
@DeletePrevious = 'True'  

-----------------------------------------------------------------------------------------
--------------Get Performance Data for date range
-----------------------------------------------------------------------------------------
exec  P_SavePerformanceNumbers_PSR        
@StartDate = @MinDate,
@EndDate = @todate,
@fund = @FundIDs 

--if all records saved successfully then commit                   
Commit TRANSACTION [Tran1]                  
END TRY                  
BEGIN CATCH                  
-- roll back all record if anyone query failed                  
  ROLLBACK TRANSACTION [Tran1]                  
END CATCH