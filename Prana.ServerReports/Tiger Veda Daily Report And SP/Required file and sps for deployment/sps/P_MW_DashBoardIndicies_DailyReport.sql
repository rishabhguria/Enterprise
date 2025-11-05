 /*************************************************                                          
Author : Ankit Misra                                         
Creation Date : 08th June , 2015                                            
Description : Script for DashBoard Indicies part of Daily Report                    
It Calculates Price Difference percentage  of  Index Symbol Daily,MTD and YTD                            
Execution Statement:                                         
P_MW_DashBoardIndicies_DailyReport '7/9/2015','$SPX,$INDU,$NDX,$SXXP-STX,$NI225-NKI,$COMPOSITE-JKT,$IME-MEX,$SENSEX-BOM,$IBOV-BSP,$XAU,$HGA,$DXY-FTSA','BRL,EUR,IDR,INR'                    

3 for selected Month start business date i.e. if  @EndDate = '8/3/2015' then value of @MTDFromdate will be 8/3/2015 because 1,2 were saturday and sunday
7 for selected Year start business date i.e. if  @EndDate = '8/3/2015' then value of @YTDFromdate will be 1/1/2015
SELECT @MTDFromdate= dbo.F_MW_GetWeekendAdjusted (@EndDate,1,3)                                                                                     
SELECT @YTDFromdate= dbo.F_MW_GetWeekendAdjusted (@EndDate,1,7)  

Now it is updated
9  for selected Month to previous month end business date i.e. if  @EndDate = '8/3/2015' then value of @MTDFromdate will be 7/31/2015 
10 for selected Year to previous year last month last business date i.e. if  @EndDate = '8/3/2015' then value of @YTDFromdate will be 12/31/2014

SELECT @MTDFromdate= dbo.F_MW_GetWeekendAdjusted (@EndDate,1,3)                                                                                     
SELECT @YTDFromdate= dbo.F_MW_GetWeekendAdjusted (@EndDate,1,7)  

Same for below section

SET @MTDFromdate=dbo.F_MW_GetWeekendAdjusted (dbo.AdjustBusinessDays(@EndDate,-1, @DefaultAUECID),1,3)                                                                      
SET @YTDFromdate=dbo.F_MW_GetWeekendAdjusted (dbo.AdjustBusinessDays(@EndDate,-1, @DefaultAUECID),1,7) 

*************************************************/                        
ALTER Procedure [dbo].[P_MW_DashBoardIndicies_DailyReport]                        
(                        
 @EndDate DATETIME,              
 @IndicesSymbolList Varchar(2000),      
@CurrencySymbolList Varchar(2000)                       
)                        
AS                        
BEGIN            
            
--Declare @EndDate DATETIME              
--Declare @IndicesSymbolList Varchar(2000)                          
--Declare @CurrencySymbolList Varchar(2000)                    
--          
--Set @EndDate = '8/3/2015'          
--Set @IndicesSymbolList = '$SPX,$INDU,$NDX,$SXXP-STX,$NI225-NKI,$JCI-IDX,$IME-MEX,$SENSEX-BOM,$IBOV-BSP'          
--Set @CurrencySymbolList = 'BRL,EUR,IDR,INR'             
              
Declare @T_IndicesSymbolTable Table              
(                      
Symbol Varchar(2000)                      
)                
              
Insert InTo @T_IndicesSymbolTable                      
Select Distinct Items From dbo.Split(@IndicesSymbolList, ',')           
              
----Select * from @T_IndicesSymbolTable        
      
Declare @T_CurrencySymbolTable Table          
(                  
FromCurrency Varchar(10),      
ToCurrency Varchar(10)                 
)            
          
Insert InTo @T_CurrencySymbolTable (FromCurrency,ToCurrency)                 
Select Distinct 'USD',Items From dbo.Split(@CurrencySymbolList, ',')             
                        
DECLARE @DefaultAUECID INT                        
SET @DefaultAUECID=(SELECT TOP 1 DefaultAUECID  FROM T_Company WHERE companyId <> -1)                        
                        
DECLARE                                                                             
@PreviousBusinessDay DATETIME,                                                                            
@MTDFromdate DATETIME,                                                                            
@YTDFromdate DATETIME                          
                          
SET @PreviousBusinessDay =  dbo.AdjustBusinessDays(@EndDate,-1, @DefaultAUECID)                                                                            
SELECT @MTDFromdate= dbo.F_MW_GetWeekendAdjusted (@EndDate,1,9)                                                                                     
SELECT @YTDFromdate= dbo.F_MW_GetWeekendAdjusted (@EndDate,1,10)                                                                      
                                                                      
IF (Datediff(Day,@EndDate,@MTDFromdate)=0 and Datediff(Day,@EndDate,@YTDFromdate)=0)                                                                      
BEGIN                                                                     
SET @MTDFromdate = dbo.F_MW_GetWeekendAdjusted (dbo.AdjustBusinessDays(@EndDate,-1, @DefaultAUECID),1,9)                                                                      
SET @YTDFromdate = dbo.F_MW_GetWeekendAdjusted (dbo.AdjustBusinessDays(@EndDate,-1, @DefaultAUECID),1,10)                          
END                    
                    
SELECT                    
Distinct                    
PMDP.Date,                    
PMDP.Symbol,                    
PMDP.FinalMarkPrice AS SelectedDatePrice    
--,1 As CustomOrderBy                    
Into #Temp                    
FROM PM_DayMarkPrice PMDP --T_IndexPrice                    
Inner Join @T_IndicesSymbolTable Temp On Temp.Symbol = PMDP.Symbol              
WHERE Datediff(Day,@EndDate,PMDP.Date)=0 And PMDP.FundID = 0                     
                    
ALTER TABLE #Temp                          
ADD PreviousDayPrice FLOAT,                    
MTDPrice FLOAT,                    
YTDPrice FLOAT                    
                    
Update  #Temp                    
Set PreviousDayPrice =  IsNull(PMDP.FinalMarkPrice,0)          
From #Temp            
Inner Join @T_IndicesSymbolTable TempIndices On TempIndices.Symbol = #Temp.Symbol          
Inner Join PM_DayMarkPrice PMDP On TempIndices.Symbol = PMDP.Symbol          
Where Datediff(Day,PMDP.Date,@PreviousBusinessDay)=0 And PMDP.FundID = 0             
          
Update  #Temp                    
Set MTDPrice = IsNull(PMDP.FinalMarkPrice,0)          
From #Temp          
Inner Join @T_IndicesSymbolTable TempIndices On TempIndices.Symbol = #Temp.Symbol          
Inner Join PM_DayMarkPrice PMDP On TempIndices.Symbol = PMDP.Symbol          
Where Datediff(Day,PMDP.Date,@MTDFromdate)=0 And PMDP.FundID = 0           
        
Update  #Temp                    
Set YTDPrice = IsNull(PMDP.FinalMarkPrice,0)          
From #Temp          
Inner Join @T_IndicesSymbolTable TempIndices On TempIndices.Symbol = #Temp.Symbol          
Inner Join PM_DayMarkPrice PMDP On TempIndices.Symbol = PMDP.Symbol          
Where Datediff(Day,PMDP.Date,@YTDFromdate)=0 And PMDP.FundID = 0           
           
Create Table #CurrencyPrices      
(      
FromCurrencyID Int,      
FromCurrency Varchar(10),      
ToCurrencyID Int,      
ToCurrency Varchar(10),      
RateValue Float,      
ConversionMethod Int,      
Date Datetime,      
eSignalSymbol Varchar(200)      
)              
      
Insert Into  #CurrencyPrices      
EXEC P_MW_GetCurrencyRates_DailyReport @EndDate      
      
UPDATE #CurrencyPrices        
SET RateValue = 1.0 / RateValue        
WHERE RateValue <> 0        
AND ConversionMethod = 1       
      
Insert Into #Temp      
SELECT                
Distinct                
CurrencyPrices.Date,                
CurrencyPrices.ToCurrency As Symbol,                
CurrencyPrices.RateValue AS SelectedDatePrice,     
--2 As CustomOrderBy ,     
0 As PreviousDatePrice,      
0 As MTDPrice,      
0 As YTDPrice               
FROM #CurrencyPrices CurrencyPrices                
Inner Join @T_CurrencySymbolTable TempCurrency On TempCurrency.FromCurrency = CurrencyPrices.FromCurrency And TempCurrency.ToCurrency = CurrencyPrices.ToCurrency         
WHERE Datediff(Day,@EndDate,CurrencyPrices.Date) = 0      
      
Update  #Temp                
Set PreviousDayPrice =  IsNull(CurrencyPrices.RateValue,0)      
From #Temp        
Inner Join @T_CurrencySymbolTable TempCurrency On TempCurrency.ToCurrency = #Temp.Symbol         
Inner Join #CurrencyPrices CurrencyPrices On TempCurrency.FromCurrency = CurrencyPrices.FromCurrency And TempCurrency.ToCurrency = CurrencyPrices.ToCurrency         
Where Datediff(Day,CurrencyPrices.Date,@PreviousBusinessDay)=0       
        
Update  #Temp                
Set MTDPrice =  IsNull(CurrencyPrices.RateValue,0)      
From #Temp        
Inner Join @T_CurrencySymbolTable TempCurrency On TempCurrency.ToCurrency = #Temp.Symbol         
Inner Join #CurrencyPrices CurrencyPrices On TempCurrency.FromCurrency = CurrencyPrices.FromCurrency And TempCurrency.ToCurrency = CurrencyPrices.ToCurrency         
Where Datediff(Day,CurrencyPrices.Date,@MTDFromdate)=0       
      
Update  #Temp                
Set YTDPrice =  IsNull(CurrencyPrices.RateValue,0)      
From #Temp        
Inner Join @T_CurrencySymbolTable TempCurrency On TempCurrency.ToCurrency = #Temp.Symbol         
Inner Join #CurrencyPrices CurrencyPrices On TempCurrency.FromCurrency = CurrencyPrices.FromCurrency And TempCurrency.ToCurrency = CurrencyPrices.ToCurrency         
Where Datediff(Day,CurrencyPrices.Date,@YTDFromdate)=0       
      
       
SELECT    
--Case    
--When Symbol = '$SPX'    
--Then 'S&P'    
--When Symbol = '$INDU'    
--Then 'DJIA'    
--When Symbol = '$NDX'    
--Then 'NDX 100'    
--When Symbol = '$SXXP-STX'    
--Then 'EUROSTOXX 600'   
--When Symbol = '$XAU'    
--Then 'GOLD'  
--When Symbol = '$HGA'    
--Then 'COPPER'   
--When Symbol = '$NI225-NKI'    
--Then 'NIKKEI'    
--When Symbol = '$COMPOSITE-JKT'    
--Then 'JCI'    
--When Symbol = '$IME-MEX'    
--Then 'MEXBOL'    
--When Symbol = '$IBOV-BSP'    
--Then 'BOVESPA'    
--When Symbol = '$SENSEX-BOM'    
--Then 'SENSEX'  
--When Symbol = '$DXY-FTSA'    
--Then 'DXY'  
--When Symbol = 'BRL'    
--Then 'BRL'  
--When Symbol = 'EUR'    
--Then 'EUD'  
--When Symbol = 'IDR'    
--Then 'IDR'  
--When Symbol = 'INR'    
--Then 'INR'  
--  
--  
--Else Symbol    
--End As IndexSymbol,   
Mapping.TVSymbol As IndexSymbol,                   
ISNULL((SelectedDatePrice - PreviousDayPrice)/NULLIF(PreviousDayPrice,0),0) as DailyPercent,                    
ISNULL((SelectedDatePrice-MTDPrice)/NULLIF(MTDPrice,0),0)  as MTDPercent,                    
ISNULL((SelectedDatePrice-YTDPrice)/NULLIF(YTDPrice,0),0)  as YTDPercent    
  
FROM #Temp   
Inner Join T_TVPranaIndicesSymbolMapping Mapping On Mapping.eSignalSymbol =  #Temp.Symbol    
Order by Mapping.OrderBy                   
                    
DROP TABLE #Temp,#CurrencyPrices                    
                    
END