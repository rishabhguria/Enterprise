/**************************************************************                  
Author:  Ankit Misra                   
Create Date: 23 June 2015                  
Description: This returns ACB of selected funds for a date range.It can be used to calculate ACB for MTD or YTD.                  
EXEC:                    
Select dbo.F_MW_GetPortfolioACB_DailyReport('7/3/2015','7/3/2015','1308')                     
****************************************************************/                  
CREATE FUNCTION [dbo].[F_MW_GetPerformance_DailyReport]                     
(                    
 @FromDate datetime,                      
 @ToDate datetime,              
 @Fund Varchar(max),          
 @BloombergSymbol Varchar(100)                   
)                    
RETURNS Float                    
AS                  
BEGIN                    
--                  
Declare @PortfolioACB Float              
--              
--Declare @FromDate Datetime                      
--Declare @ToDate Datetime              
--Declare @Fund Varchar(max)          
--Declare @Symbol Varchar(100)              
--              
--Set @FromDate = '7/3/2015'              
--Set @ToDate= '7/3/2015'               
--Set @Fund = '1308'--'1270,1271,1298,1302,1304,1305,1306,1307,1308,1309,1310'          
--Set @Symbol = 'MSFT'              
------------------------------------------------------------              
Declare @T_TempCompanyFunds Table                                                                                                                                                                    
(              
 FundName Varchar(100),                                                                                                                                                                   
 FundId int                                                                                                                                                                    
)                                                                                                                                                                    
Insert Into @T_TempCompanyFunds              
Select NULL AS FundName,* From dbo.Split(@Fund, ',')              
              
UPDATE @T_TempCompanyFunds               
SET TCF.FundName = CF.FundName              
FROM @T_TempCompanyFunds TCF              
INNER JOIN T_CompanyFunds CF ON TCF.FundId = CF.CompanyFundID              
                    
-----------------------------------------------------------------------------------------------------------------                  
--                  
-----------------------------------------------------------------------------------------------------------------                  
Declare  @DaysInPeriod Float --Total Days                  
Set @DaysInPeriod = Datediff(Day,@FromDate, @ToDate) + 1                  
                    
Declare @DaysRemain Float --Days Remaining In Period                  
Set @DaysRemain = @DaysInPeriod                    
                    
Declare @LastBusinessDateOfFromdate Datetime                    
Set @LastBusinessDateOfFromdate = dbo.AdjustBusinessDays(@FromDate, -1, 1)                    
                    
Declare @NextBusinessDateIfNotBusinessDay Datetime                  
                  
IF dbo.IsBusinessDay(@Fromdate,1) = 0                    
BEGIN                     
SELECT @NextBusinessDateIfNotBusinessDay = dbo.AdjustBusinessDays(@FromDate, 1, 1)                    
END                     
ELSE                    
BEGIN                    
SELECT @NextBusinessDateIfNotBusinessDay = @FromDate                    
END                  
-----------------------------------------------------------------------------------------------------------------                  
--                  
-----------------------------------------------------------------------------------------------------------------                    
                    
Declare @BMVsecurities Float                  
Declare @BMVcash Float                    
Declare @CashFlow Float          
Declare @PNL Float           
Declare @MDreturn Float                
                    
Declare @BaseCurrencyID Int                    
Select @BaseCurrencyID = BaseCurrencyID from T_Company where CompanyId <> -1                  
                    
Declare @InvestorCash Table                    
(                  
Fundname varchar(max),                  
Payoutdate datetime,                  
CashValue float                  
)                    
-----------------------------------------------------------------------------------------------------------                  
--                  
-----------------------------------------------------------------------------------------------------------                    
Insert Into @InvestorCash                   
                   
SELECT                   
Funds.FundName,                  
Journal.TransactionDate AS PayOutDate,                  
CASE                   
WHEN (Journal.CurrencyID = @BaseCurrencyID)                   
THEN (Journal.CR- Journal.DR)                  
ELSE (ISNULL(CurrencyConversionRate.ConversionRate,1)*(Journal.CR- Journal.DR))                  
END AS CashValue                  
                    
FROM T_Journal AS Journal                    
INNER JOIN T_SubAccounts AS SubAccounts ON Journal.SubAccountID = SubAccounts.SubAccountID                     
INNER JOIN T_TransactionType AS TransactionType ON TransactionType.TransactionTypeID = SubAccounts.TransactionTypeID                     
INNER JOIN @T_TempCompanyFunds AS Funds ON Funds.FundID = Journal.FundID                     
LEFT OUTER JOIN T_CurrencyStandardPairs AS CurrencyStandardPairs ON CurrencyStandardPairs.FromCurrencyID = Journal.CurrencyID AND CurrencyStandardPairs.ToCurrencyID = @BaseCurrencyID                     
LEFT OUTER JOIN T_CurrencyConversionRate AS CurrencyConversionRate ON CurrencyConversionRate.CurrencyPairID_FK = CurrencyStandardPairs.CurrencyPairID AND CurrencyConversionRate.Date = Journal.TransactionDate                     
WHERE TransactionType = 'Investor Cash Transactions'                  
                   
-----------------------------------------------------------------------------------------------------------                  
--                  
-----------------------------------------------------------------------------------------------------------                    
 IF(@BloombergSymbol<>'Portfolio')    
 BEGIN    
 SELECT @PNL=          
 Sum(TotalRealizedPNLOnCost + ChangeInUNRealizedPNL + Dividend)          
 FROM dbo.T_MW_GenericPNL      
 INNER JOIN @T_TempCompanyFunds TCF ON TCF.FundName = T_MW_GenericPNL.Fund          
 Where                                                                                                                                             
 Datediff (day , @FromDate , Rundate) >= 0 And Datediff(day , Rundate , @ToDate) >= 0       
 And Open_CloseTag <> 'Accruals' AND BloombergSymbol=@BloombergSymbol    
 END    
 ELSE    
 BEGIN    
 SELECT @PNL=          
 Sum(TotalRealizedPNLOnCost + ChangeInUNRealizedPNL + Dividend)          
 FROM dbo.T_MW_GenericPNL      
 INNER JOIN @T_TempCompanyFunds TCF ON TCF.FundName = T_MW_GenericPNL.Fund          
 Where                                                                                                                                             
 Datediff (day , @FromDate , Rundate) >= 0 And Datediff(day , Rundate , @ToDate) >= 0       
 And Open_CloseTag <> 'Accruals'    
 END          
          
                   
-- SELECT @BMVsecurities =               
-- SUM(BeginningMarketValueBase+Dividend) FROM dbo.T_MW_GenericPNL PNL              
-- INNER JOIN @T_TempCompanyFunds TCF ON TCF.FundName = PNL.Fund                
-- WHERE Asset<>'CASH' AND DateDiff(d,TradeDate, @Fromdate) > 0 AND Rundate = @NextBusinessDateIfNotBusinessDay              
--              
-- SELECT @BMVcash =              
-- SUM(BeginningMarketValueBase) FROM dbo.T_MW_GenericPNL PNL              
-- INNER JOIN @T_TempCompanyFunds TCF ON TCF.FundName = PNL.Fund                
-- WHERE Asset='CASH' AND Rundate = @NextBusinessDateIfNotBusinessDay  
  
 Declare @DefaultAUECID int                                      
Set @DefaultAUECID=(select top 1 DefaultAUECID  from T_Company where companyId <> -1)   
Declare @PreviousBusinessDay DateTime        
Set @PreviousBusinessDay = dbo.AdjustBusinessDays(@Fromdate,-1, @DefaultAUECID)  
  
--Select  @PreviousBusinessDay             
      
 SET @BMVsecurities = (Select [dbo].[F_MW_GetEndOfTheDayNAV_DailyReport](@PreviousBusinessDay,@Fund))               
                   
 SELECT @Cashflow =               
 SUM(((DATEDIFF(d,PayOutDate, @todate) + 1)/@DaysInPeriod)*Cashvalue) FROM @InvestorCash               
 WHERE payoutdate between @Fromdate and @Todate                    
                    
 SET @BMVsecurities = ISNULL(@BMVsecurities,0)                    
 SET @BMVcash = ISNULL(@BMVcash,0)                    
 SET @Cashflow = ISNULL(@Cashflow,0)                    
                     
 SET @PortfolioACB = @BMVsecurities + @BMVcash + @Cashflow          
 SELECT @MDreturn = ISNULL(@PNL,0) / NULLIF( ABS(@PortfolioACB),0)            
            
 IF @MDreturn <= -1            
 BEGIN            
 SELECT @MDreturn = -.999            
 END       
         
-- SELECT @PNL AS PNL       
-- SELECT @MDreturn AS MDreturn           
-- SELECT @BMVsecurities AS BMVsecurities          
-- SELECT @BMVcash  AS BMVcash          
-- SELECT @Cashflow AS Cashflow          
         
           
RETURN ISNULL(@MDreturn,0)                   
                  
END