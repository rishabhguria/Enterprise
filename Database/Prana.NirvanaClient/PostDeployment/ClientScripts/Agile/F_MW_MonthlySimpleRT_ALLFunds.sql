-- =============================================                                
-- Modified:  <Pankaj Sharma>                                
-- Create date: <1/29/2016>                                
-- Description: <Gives MTD Gross and Net values monthly fund wise>                                
-- Sample:                                  
-- select * from dbo.F_MW_MonthlySimpleRT_ALLFunds( '1/26/2016', '1218','09/30/2014')                                
  
-- =============================================                                
CREATE FUNCTION [dbo].[F_MW_MonthlySimpleRT_ALLFunds] (                            
 @EndDate DATETIME                            
 ,@fundID VARCHAR(50),      
@ITD Datetime      
 )                            
-- Declare @EndDate Datetime                                
-- Declare @fundID varchar(50)                               
-- Declare @ITD Datetime      
--                               
-- Set @EndDate ='12/31/2014'                               
-- Set @fundID ='1218'--,1270,1271,1286'          
-- SET @ITD='9/30/2014'      
              
Returns @MonthlyRT TABLE (                            
 CompanyFund VARCHAR(max)                            
 ,day1 DATETIME                            
 ,day2 DATETIME                            
 ,Pnl FLOAT                            
 ,ExpFee FLOAT                            
 ,CF FLOAT                            
 ,IncentiveFee FLOAT                            
 ,ManagementFee FLOAT                            
 ,PriorEquityGross FLOAT                            
 ,PriorEquityNet FLOAT                            
 ,GrossRT FLOAT                            
 ,NetRT FLOAT                            
 ,ID INT identity(1, 1)                            
 )                            
AS                            
BEGIN            
          
declare @fund varchar(MAX)                                    
SET @fund = ''                                    
Select @fund = @fund + Cast(FundName as varchar(50)) +',' from T_CompanyFunds CF                             
where CF.CompanyFundID in(Select items from split(@fundID, ','))          
          
Set @fund = SUBSTRING(@fund, 0, LEN(@fund))                       
--print @fund            
                          
DECLARE @FundTable TABLE (Fund VARCHAR(max))                            
                            
INSERT INTO @FundTable                            
SELECT *                            
FROM dbo.split(@Fund, ',')                            
      
-----------------------------------------------------------------------------------------------------------------------------------                  
------------------Date handling in the Function starts                  
-----------------------------------------------------------------------------------------------------------------------------------                  
                            
DECLARE @Date TABLE (                            
 Year VARCHAR(max)                            
 ,Month VARCHAR(max)                            
 ,Day VARCHAR(max)                            
 ,DATE DATETIME                            
 )                            
                            
--------------------------------For ITD                    
INSERT INTO @Date (Month)                            
 SELECT number                            
 FROM master..spt_values                            
 WHERE type = 'P'                            
  AND number >= Month(@ITD)                            
  AND number < = 12                            
                    
 UPDATE @Date                            
 SET [Year] = (                            
   SELECT TOP 1 number                            
   FROM master..spt_values                            
   WHERE type = 'P'                            
    AND number = year(@ITD)                            
   )                
where Year is NULL                    
                    
                    
                    
--------------------------------For middle years                  
If(year(@ITD)-year(@EndDate) <> 0)                
Begin                  
 Declare @Count int                    
 Select @Count = Count(*)                     
 FROM master..spt_values                            
 WHERE type = 'P'                            
 AND number > year(@ITD)                    
 AND number < year(@EndDate)                                             
 While(@Count>0)                    
 Begin                    
 Declare @Year int                    
 Select @Year = Max(Year) + 1 from @Date                    
 INSERT INTO @Date (Month)                            
  SELECT number                            
  FROM master..spt_values                            
  WHERE type = 'P'                            
   AND number >= 1                           
   AND number <= 12                            
                     
  UPDATE @Date                            
  SET [Year] = (                            
    SELECT TOP 1 number                            
    FROM master..spt_values                            
    WHERE type = 'P'                      
  AND number = @year                    
    )                            
 where Year is NULL                    
                     
 Set @Count = @Count-1                    
                     
 END                    
                     
                     
 --------------------------------For YTD                    
 INSERT INTO @Date (Month)                            
  SELECT number                            
  FROM master..spt_values                            
  WHERE type = 'P'                            
   AND number >= 1                    
   AND number <= Month(@EndDate)                      
                     
                          
                     
  UPDATE @Date                            
  SET [Year] = (                            
    SELECT TOP 1 number                            
    FROM master..spt_values                            
    WHERE type = 'P'                            
  AND number = year(@EndDate)                           
    )                            
 where Year is NULL                    
END                
                            
UPDATE @Date                            
SET Day = CASE                             
  WHEN Month = 1                            
   THEN 31                            
  WHEN Month = 2                            
   THEN CASE                             
     WHEN Year % 4 = 0                            
      THEN 29                            
     ELSE 28                            
     END                            
  WHEN Month = 3                            
   THEN 31                            
  WHEN Month = 4                            
   THEN 30                            
  WHEN Month = 5                            
   THEN 31                            
  WHEN Month = 6                            
   THEN 30                            
  WHEN Month = 7                            
   THEN 31                            
  WHEN Month = 8                            
   THEN 31                            
  WHEN Month = 9                            
   THEN 30                            
  WHEN Month = 10                            
   THEN 31                            
  WHEN Month = 11                            
   THEN 30                            
  WHEN Month = 12                            
   THEN 31                            
  END                            
                            
UPDATE @Date                            
SET DATE = (                            
  SELECT CAST(CAST(Year AS VARCHAR) + '-' + CAST(Month AS VARCHAR) + '-' + CAST(Day AS VARCHAR) AS DATETIME)                            
  )                            
WHERE DATE IS NULL                            
                            
UPDATE @Date                            
SET DATE = @EndDate                            
WHERE DATE = (                            
  SELECT max(DATE)                            
  FROM @Date                          
  )                            
                            
UPDATE @Date                            
SET DATE = dbo.AdjustBusinessDays(DATE, - 1, 11)                            
WHERE dbo.isbusinessday(DATE, 1) = 0 --set date to previous business day.                                
  
Delete from @Date where Year = Year(@Enddate) and Month >Month(@Enddate)  
        
-----------------------------------------------------------------------------------------------------------------------------------                  
------------------Date handling in the Function ends                  
-----------------------------------------------------------------------------------------------------------------------------------                  
                  
INSERT INTO @MonthlyRT (                 
 day1                            
,day2                            
 ,CompanyFund                            
 )                            
SELECT CONVERT(VARCHAR(25), DATEADD(dd, - (DAY(DATE) - 1), DATE), 101)                            
 ,DATE                            
 ,Fund                            
FROM @Date                            
CROSS JOIN @FundTable                            
ORDER BY DATE                            
    
Update @MonthlyRT    
SET day1=CashMgmtStartDate from @MonthlyRT RT     
inner join T_CompanyFunds CF on CF.FundName = RT.CompanyFund    
inner JOIN T_cashPreferences CP on CP.FundID = CF.CompanyFundID and datediff(d,day1,CashMgmtStartDate)>0    
                            
----Update PNL Date and FundWise---                              
UPDATE @MonthlyRT                            
SET Pnl = aggregated.PnL                            
FROM (                            
 SELECT sum(totalrealizedpnlmtm + totalUnrealizedpnlmtm + dividend) AS Pnl                            
  ,Fund                            
  ,day1                            
  ,day2                            
 FROM T_MW_GenericPNL AS GP                            
 INNER JOIN @MonthlyRT AS MW ON rundate BETWEEN day1                            
   AND day2                            
  AND MW.CompanyFund = GP.Fund                            
 WHERE GP.Fund IN (                            
   SELECT *                            
   FROM @FundTable                            
   )                            
 GROUP BY GP.Fund                            
  ,day1                            
  ,day2                            
 ) AS aggregated                            
WHERE CompanyFund = aggregated.Fund                
 AND aggregated.day1 = [@MonthlyRT].day1                            
 AND aggregated.day2 = [@MonthlyRT].day2                            
                            
----Update PNL Date and FundWise---                              
DECLARE @BaseCurrencyID INT                            
                            
SELECT @BaseCurrencyID = BaseCurrencyID                            
FROM T_Company  where companyiD <>-1                          
                            
DECLARE @InvestorCash TABLE (                            
 fundname VARCHAR(max)                            
 ,TransactionDate DATETIME                 
 ,pbdesc VARCHAR(max)                            
 ,SubCategoryName VARCHAR(max)                            
 ,SubAccountName VARCHAR(max)                            
 ,SubCategoryID INT                            
 ,MasterCategoryID INT                            
 ,subaccountid INT                            
 ,cashvalue FLOAT                            
 )                            
                            
INSERT INTO @InvestorCash                            
SELECT Funds.FundName                            
 ,Journal.TransactionDate                            
 ,journal.pbdesc                            
 ,SC.SubCategoryName                            
 ,SubAccounts.NAME                            
 ,SC.SubCategoryID                            
 ,MC.MasterCategoryID                            
 ,journal.subaccountid                            
 ,CASE                             
  WHEN Journal.CurrencyID = @BaseCurrencyID                            
   THEN Journal.CR - Journal.DR                            
  ELSE isnull(CurrencyConversionRate.ConversionRate, 1) * (Journal.CR - Journal.DR)                            
  END AS CashValue                            
FROM T_Journal AS Journal                            
INNER JOIN T_SubAccounts AS SubAccounts ON Journal.SubAccountID = SubAccounts.SubAccountID --and journal. subaccountid in (47,48,41,236,63,22,171,66,60,172,208,59,45,42)-- and symbol is null                                
INNER JOIN T_TransactionType AS TransactionType ON TransactionType.TransactionTypeID = SubAccounts.TransactionTypeID                            
INNER JOIN T_CompanyFunds AS Funds ON Funds.CompanyFundID = Journal.FundID                            
 AND FundName IN (                            
  SELECT *                            
  FROM dbo.split(@fund, ',')                            
  )                            
INNER JOIN T_SubCategory AS SC ON SubAccounts.SubCategoryID = SC.SubCategoryID                            
INNER JOIN T_MasterCategory AS MC ON MC.MasterCategoryID = SC.MasterCategoryID                            
LEFT OUTER JOIN T_CurrencyStandardPairs AS CurrencyStandardPairs ON CurrencyStandardPairs.FromCurrencyID = Journal.CurrencyID                            
 AND CurrencyStandardPairs.ToCurrencyID = @BaseCurrencyID                            
LEFT OUTER JOIN T_CurrencyConversionRate AS CurrencyConversionRate ON CurrencyConversionRate.CurrencyPairID_FK = CurrencyStandardPairs.CurrencyPairID                            
 AND CurrencyConversionRate.DATE = Journal.TransactionDate                            
                            
--WHERE TransactionType = 'Investor Cash Transactions'                                
---------Update CF Fund Wise-----------                               
UPDATE @MonthlyRT                            
SET CF = aggregated.CashValue                            
FROM (                            
 SELECT isnull(sum(cashvalue), 0) AS CashValue                            
  ,FundName                            
  ,day1                            
  ,day2                            
 FROM @InvestorCash AS GP                            
 INNER JOIN @MonthlyRT AS MW ON transactiondate BETWEEN day1                            
   AND day2                            
  AND MW.CompanyFund = GP.FundName                            
 WHERE GP.FundName IN (                            
   SELECT *                            
   FROM @FundTable                            
   )                            
  AND subaccountid IN (                            
   47                            
   ,48                            
   )                            
 GROUP BY GP.FundName                            
  ,day1                            
  ,day2                            
 ) AS aggregated                            
WHERE CompanyFund = aggregated.FundName                            
 AND aggregated.day1 = [@MonthlyRT].day1                            
 AND aggregated.day2 = [@MonthlyRT].day2                            
                            
--------Update CF Fund Wise-----------                               
----Adjust P&L with expenses and fees                               
UPDATE @MonthlyRT                            
SET ExpFee = aggregated.ExpFee                            
FROM (                            
 SELECT isnull(sum(cashvalue), 0) AS expFee                            
,FundName                 
  ,day1                            
  ,day2                            
 FROM @InvestorCash AS GP                            
 INNER JOIN @MonthlyRT AS MW ON transactiondate BETWEEN day1                            
   AND day2                            
  AND MW.CompanyFund = GP.FundName                            
 WHERE GP.FundName IN (                            
   SELECT *                            
   FROM @FundTable                               )                            
  AND MasterCategoryID IN (                            
   4                            
   ,5                            
   )                            
  AND subcategoryID <> 24                            
  AND subaccountid NOT IN (                            
   15                            
   ,28                            
   ,32                            
   ,33                            
   ,34                            
   ,22                            
   )                            
 GROUP BY GP.FundName                            
  ,day1                            
  ,day2                            
 ) AS aggregated                            
WHERE CompanyFund = aggregated.FundName                            
 AND aggregated.day1 = [@MonthlyRT].day1                            
 AND aggregated.day2 = [@MonthlyRT].day2                            
                            
----Adjust P&L with expenses and fees                               
---------------------------------------------------------------                               
------Update IncentiveFee FundWise-----                              
UPDATE @MonthlyRT                            
SET IncentiveFee = aggregated.IncentiveFee                            
FROM (                            
 SELECT isnull(sum(cashvalue), 0) AS IncentiveFee                            
  ,FundName                            
  ,day1                            
  ,day2                            
 FROM @InvestorCash AS GP                            
 INNER JOIN @MonthlyRT AS MW ON transactiondate BETWEEN day1                            
   AND day2                            
  AND MW.CompanyFund = GP.FundName                            
 WHERE GP.FundName IN (                            
   SELECT *                            
   FROM @FundTable                            
   )                            
  AND subaccountid IN (236)                            
 GROUP BY GP.FundName                            
  ,day1                            
  ,day2                            
 ) AS aggregated                            
WHERE CompanyFund = aggregated.FundName                            
 AND aggregated.day1 = [@MonthlyRT].day1                            
 AND aggregated.day2 = [@MonthlyRT].day2                            
                            
------Update IncentiveFee FundWise-----                              
------Update ManagementFee FundWise-----                 
UPDATE @MonthlyRT                            
SET ManagementFee = aggregated.ManagementFee                            
FROM (                            
 SELECT isnull(sum(cashvalue), 0) AS ManagementFee                            
  ,FundName                            
  ,day1                            
  ,day2                            
 FROM @InvestorCash AS GP                            
 INNER JOIN @MonthlyRT AS MW ON transactiondate BETWEEN day1                            
   AND day2                            
  AND MW.CompanyFund = GP.FundName                            
 WHERE GP.FundName IN (                            
   SELECT *                            
   FROM @FundTable                            
   )                            
  AND subaccountid IN (41)                            
 GROUP BY GP.FundName                            
  ,day1                            
  ,day2                            
 ) AS aggregated                            
WHERE CompanyFund = aggregated.FundName                            
 AND aggregated.day1 = [@MonthlyRT].day1                            
 AND aggregated.day2 = [@MonthlyRT].day2                            
                            
------Update ManagementFee FundWise-----                              
-----Update PriorEquityGross----                       
UPDATE @MonthlyRT                            
SET PriorEquityGross = MW.PriorEquityGross                            
 ,PriorEquityNet = MW.PriorEquityGross                            
FROM @MonthlyRT                   
INNER JOIN (                            
 SELECT sum(endingmarketvaluebase) AS PriorEquityGross                            
  ,Fund                            
  ,Rundate                            
 FROM t_mw_genericpnl                            
 WHERE fund IN (                            
   SELECT *                            
   FROM dbo.split(@fund, ',')                            
   )                            
  AND open_closetag <> 'C'                            
 GROUP BY Rundate                            
  ,Fund                            
 ) AS MW ON MW.rundate = dbo.AdjustBusinessDays(day1, - 1, 11)                            
 AND Fund = CompanyFund                            
                            
-----Update PriorEquityGross----                              
UPDATE @MonthlyRT                            
SET GrossRt = COALESCE((COALESCE(PnL, 0) + COALESCE(ExpFee, 0)) / (COALESCE(PriorEquityGross, 0) + COALESCE(CF, 0)), 0) --from @BeginningEquity where EndDate=day2                                
WHERE (COALESCE(PriorEquityGross, 0) + COALESCE(CF, 0)) <> 0                            
                            
UPDATE @MonthlyRT                            
SET NetRt = (COALESCE(PnL, 0) + COALESCE(ExpFee, 0) - abs(COALESCE(IncentiveFee, 0)) - abs(COALESCE(ManagementFee, 0))) / (COALESCE(PriorEquityNet, 0) + COALESCE(CF, 0)) --from @BeginningEquity where EndDate=day2                                
WHERE (COALESCE(PriorEquityNet, 0) + COALESCE(CF, 0)) <> 0                            
                            
-------------------------------Update historical performance from client file--------------------------------------                                
UPDATE @MonthlyRT                            
SET GrossRt = Performance                            
FROM T_MW_PerformanceOnBoard                            
WHERE DataType = 'Gross MTD'                            
 AND Datediff(d,AsofDate,day2)=0              
 AND CompanyFund = Fund               
                            
UPDATE @MonthlyRT                    
SET NetRt = Performance              
FROM T_MW_PerformanceOnBoard                            
WHERE DataType = 'Net MTD'                            
 AND Datediff(d,AsofDate,day2)=0              
 AND CompanyFund = Fund                            
                            
-----------------------------------------------------------------------------------------------------------------                                
 RETURN                            
END 