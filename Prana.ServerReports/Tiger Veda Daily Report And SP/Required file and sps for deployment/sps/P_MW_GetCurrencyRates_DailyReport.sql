/*

Exec P_MW_GetCurrencyRates_DailyReport '07-01-2015'

*/

ALTER Procedure P_MW_GetCurrencyRates_DailyReport
(
@Date Datetime
)
As

DECLARE @DefaultAUECID INT              
SET @DefaultAUECID=(SELECT TOP 1 DefaultAUECID  FROM T_Company WHERE companyId <> -1)
 
DECLARE                                                                   
@PreviousBusinessDay DATETIME,                                                                  
@MTDFromdate DATETIME,                                                                  
@YTDFromdate DATETIME                
                
SET @PreviousBusinessDay =  dbo.AdjustBusinessDays(@Date,-1, @DefaultAUECID)                                                                  
SELECT @MTDFromdate= dbo.F_MW_GetWeekendAdjusted (@Date,1,9)                                                                             
SELECT @YTDFromdate= dbo.F_MW_GetWeekendAdjusted (@Date,1,10)  
                                                          
IF (Datediff(Day,@Date,@MTDFromdate)=0 and Datediff(Day,@Date,@YTDFromdate)=0)                                                            
BEGIN                                                           
SET @MTDFromdate=dbo.F_MW_GetWeekendAdjusted (dbo.AdjustBusinessDays(@Date,-1, @DefaultAUECID),1,9)                                                              
SET @YTDFromdate=dbo.F_MW_GetWeekendAdjusted (dbo.AdjustBusinessDays(@Date,-1, @DefaultAUECID),1,10)                  

END  
---- Current Date
Select Distinct
	StanPair.FromCurrencyID AS FromCurrencyID, 
	FromCurr.CurrencySymbol As FromCurrency,                             
	StanPair.ToCurrencyID AS ToCurrencyID,   
	ToCurr.CurrencySymbol As ToCurrency,                  
	Isnull(CCR.ConversionRate,0) AS RateValue,                               
	0 AS ConversionMethod,                               
	CCR.Date AS Date,                              
	StanPair.eSignalSymbol AS eSignalSymbol
		From T_CurrencyConversionRate AS CCR                              
		INNER JOIN T_CurrencyStandardPairs StanPair on StanPair.CurrencyPairId = CCR.CurrencyPairID_FK 
		Inner Join T_Currency FromCurr On FromCurr.CurrencyID = StanPair.FromCurrencyID                          
		Inner Join T_Currency ToCurr On ToCurr.CurrencyID = StanPair.ToCurrencyID                       
  WHERE DateDiff(d,@Date,CCR.Date) = 0 And CCR.FundID = 0                   

UNION                            

Select Distinct
	StanPair.ToCurrencyID AS FromCurrencyID,
	FromCurr.CurrencySymbol As FromCurrency,                              
	StanPair.FromCurrencyID AS ToCurrencyID, 
	ToCurr.CurrencySymbol As ToCurrency,                             
	Isnull(CCR.ConversionRate,0) AS RateValue,                               
	1 AS ConversionMethod,                               
	CCR.Date AS Date,                              
	StanPair.eSignalSymbol AS eSignalSymbol  

		from T_CurrencyConversionRate AS CCR                              
		INNER JOIN T_CurrencyStandardPairs StanPair on StanPair.CurrencyPairId = CCR.CurrencyPairID_FK 
		Inner Join T_Currency FromCurr On FromCurr.CurrencyID = StanPair.ToCurrencyID                          
		Inner Join T_Currency ToCurr On ToCurr.CurrencyID = StanPair.FromCurrencyID
  WHERE DateDiff(d,@Date,CCR.Date) = 0 And CCR.FundID = 0                    

UNION
--Previous Date
Select Distinct
	StanPair.FromCurrencyID AS FromCurrencyID, 
	FromCurr.CurrencySymbol As FromCurrency,                             
	StanPair.ToCurrencyID AS ToCurrencyID,   
	ToCurr.CurrencySymbol As ToCurrency,                  
	Isnull(CCR.ConversionRate,0) AS RateValue,                               
	0 AS ConversionMethod,                               
	CCR.Date AS Date,                              
	StanPair.eSignalSymbol AS eSignalSymbol
		From T_CurrencyConversionRate AS CCR                              
		INNER JOIN T_CurrencyStandardPairs StanPair on StanPair.CurrencyPairId = CCR.CurrencyPairID_FK 
		Inner Join T_Currency FromCurr On FromCurr.CurrencyID = StanPair.FromCurrencyID                          
		Inner Join T_Currency ToCurr On ToCurr.CurrencyID = StanPair.ToCurrencyID                       
  WHERE DateDiff(d,@PreviousBusinessDay,CCR.Date) = 0 And CCR.FundID = 0                                       

UNION                            

Select Distinct
	StanPair.ToCurrencyID AS FromCurrencyID,
	FromCurr.CurrencySymbol As FromCurrency,                              
	StanPair.FromCurrencyID AS ToCurrencyID, 
	ToCurr.CurrencySymbol As ToCurrency,                             
	Isnull(CCR.ConversionRate,0) AS RateValue,                               
	1 AS ConversionMethod,                               
	CCR.Date AS Date,                              
	StanPair.eSignalSymbol AS eSignalSymbol  

		from T_CurrencyConversionRate AS CCR                              
		INNER JOIN T_CurrencyStandardPairs StanPair on StanPair.CurrencyPairId = CCR.CurrencyPairID_FK 
		Inner Join T_Currency FromCurr On FromCurr.CurrencyID = StanPair.ToCurrencyID                          
		Inner Join T_Currency ToCurr On ToCurr.CurrencyID = StanPair.FromCurrencyID
  WHERE DateDiff(d,@PreviousBusinessDay,CCR.Date) = 0  And CCR.FundID = 0                   

UNION
--MTD Date
Select Distinct
	StanPair.FromCurrencyID AS FromCurrencyID, 
	FromCurr.CurrencySymbol As FromCurrency,                             
	StanPair.ToCurrencyID AS ToCurrencyID,   
	ToCurr.CurrencySymbol As ToCurrency,                  
	Isnull(CCR.ConversionRate,0) AS RateValue,                               
	0 AS ConversionMethod,                               
	CCR.Date AS Date,                              
	StanPair.eSignalSymbol AS eSignalSymbol
		From T_CurrencyConversionRate AS CCR                              
		INNER JOIN T_CurrencyStandardPairs StanPair on StanPair.CurrencyPairId = CCR.CurrencyPairID_FK 
		Inner Join T_Currency FromCurr On FromCurr.CurrencyID = StanPair.FromCurrencyID                          
		Inner Join T_Currency ToCurr On ToCurr.CurrencyID = StanPair.ToCurrencyID                       
  WHERE DateDiff(d,@MTDFromdate,CCR.Date) = 0 And CCR.FundID = 0                                       

UNION                            

Select Distinct
	StanPair.ToCurrencyID AS FromCurrencyID,
	FromCurr.CurrencySymbol As FromCurrency,                              
	StanPair.FromCurrencyID AS ToCurrencyID, 
	ToCurr.CurrencySymbol As ToCurrency,                             
	Isnull(CCR.ConversionRate,0) AS RateValue,                               
	1 AS ConversionMethod,                               
	CCR.Date AS Date,                              
	StanPair.eSignalSymbol AS eSignalSymbol  

		from T_CurrencyConversionRate AS CCR                              
		INNER JOIN T_CurrencyStandardPairs StanPair on StanPair.CurrencyPairId = CCR.CurrencyPairID_FK 
		Inner Join T_Currency FromCurr On FromCurr.CurrencyID = StanPair.ToCurrencyID                          
		Inner Join T_Currency ToCurr On ToCurr.CurrencyID = StanPair.FromCurrencyID
  WHERE DateDiff(d,@MTDFromdate,CCR.Date) = 0 And CCR.FundID = 0                    

UNION 
-- YTD
Select Distinct
	StanPair.FromCurrencyID AS FromCurrencyID, 
	FromCurr.CurrencySymbol As FromCurrency,                             
	StanPair.ToCurrencyID AS ToCurrencyID,   
	ToCurr.CurrencySymbol As ToCurrency,                  
	Isnull(CCR.ConversionRate,0) AS RateValue,                               
	0 AS ConversionMethod,                               
	CCR.Date AS Date,                              
	StanPair.eSignalSymbol AS eSignalSymbol
		From T_CurrencyConversionRate AS CCR                              
		INNER JOIN T_CurrencyStandardPairs StanPair on StanPair.CurrencyPairId = CCR.CurrencyPairID_FK 
		Inner Join T_Currency FromCurr On FromCurr.CurrencyID = StanPair.FromCurrencyID                          
		Inner Join T_Currency ToCurr On ToCurr.CurrencyID = StanPair.ToCurrencyID                       
  WHERE DateDiff(d,@YTDFromdate,CCR.Date) = 0 And CCR.FundID = 0                                        

UNION                            

Select Distinct
	StanPair.ToCurrencyID AS FromCurrencyID,
	FromCurr.CurrencySymbol As FromCurrency,                              
	StanPair.FromCurrencyID AS ToCurrencyID, 
	ToCurr.CurrencySymbol As ToCurrency,                             
	Isnull(CCR.ConversionRate,0) AS RateValue,                               
	1 AS ConversionMethod,                               
	CCR.Date AS Date,                              
	StanPair.eSignalSymbol AS eSignalSymbol  

		from T_CurrencyConversionRate AS CCR                              
		INNER JOIN T_CurrencyStandardPairs StanPair on StanPair.CurrencyPairId = CCR.CurrencyPairID_FK 
		Inner Join T_Currency FromCurr On FromCurr.CurrencyID = StanPair.ToCurrencyID                          
		Inner Join T_Currency ToCurr On ToCurr.CurrencyID = StanPair.FromCurrencyID
  WHERE DateDiff(d,@YTDFromdate,CCR.Date) = 0 And CCR.FundID = 0                    
