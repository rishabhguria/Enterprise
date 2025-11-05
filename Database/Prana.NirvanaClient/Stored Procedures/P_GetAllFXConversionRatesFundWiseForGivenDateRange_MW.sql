  
/********************************************************                           
 Author:  sachin Kumar mishra                            
 Create date: May,2022                        
 Description: Returns fund wise Forex rate with standard pairs  and reverse of standard pairs for the date range given 
 This SP is referred by P_GetAllFXConversionRatesForGivenDateRange, only difference is that fundid added to fetch fund wise fx rate.             
 Usage:                             
 Exec [P_GetAllFXConversionRatesFundWiseForGivenDateRange_MW] '1985-12-31','02/02/2022' ,'1,2,3,4'--,5,6,8,9,10,11,12,13,14,15,16,17,18,19,20,21'
 Exec [P_GetAllFXConversionRatesFundWiseForGivenDateRange] '1985-12-31','02/02/2022' 

********************************************************/                             
CREATE Procedure [P_GetAllFXConversionRatesFundWiseForGivenDateRange_MW]           
(          
@startingDate datetime,           
@endingDate datetime,   
@funds varchar(max) 
)          
AS                            
BEGIN 

--declare @startingDate datetime,@endingDate datetime ,@funds varchar(max)    
--set @startingDate='1985-12-31'
--set @endingDate='02/02/2022' 
--Set   @funds='1,2,3,4,5,6,8,9,10,11,12,13,14,15,16,17,18,19,20,21'

--select * from T_CompanyFunds
   
Select * Into #Funds                                          
from dbo.Split(@funds, ',')  
 
--insert into  #Funds
--select 0 

Select 
	Date,
	fundid,
	ConversionRate,
	CurrencyPairID_FK
	into #tempCurrencyConversionRates
	from T_CurrencyConversionRate
	WHERE DateDiff(d,@startingDate,Date) >=0
AND DateDiff(d,Date,@endingDate) >=0 
and  FundID in (select items from #Funds) 
                  
Select StanPair.FromCurrencyID AS FromCurrencyID,                            
    StanPair.ToCurrencyID AS ToCurrencyID,                            
    Isnull(CCR.ConversionRate,0) AS RateValue,                             
    0 AS ConversionMethod,                             
    CCR.Date AS Date,                            
    StanPair.eSignalSymbol AS eSignalSymbol,
	CCR.FundID AS FundID  
From #tempCurrencyConversionRates AS CCR                            
INNER JOIN T_CurrencyStandardPairs StanPair on StanPair.CurrencyPairId = CCR.CurrencyPairID_FK     
union 
Select StanPair.ToCurrencyID AS FromCurrencyID,                            
    StanPair.FromCurrencyID AS ToCurrencyID,                            
    Isnull(CCR.ConversionRate,0) AS RateValue,                             
    1 AS ConversionMethod,                             
    CCR.Date AS Date,                            
    StanPair.eSignalSymbol AS eSignalSymbol,
	CCR.FundID AS FundID                             
from #tempCurrencyConversionRates AS CCR                            
INNER JOIN T_CurrencyStandardPairs StanPair on StanPair.CurrencyPairId = CCR.CurrencyPairID_FK       

                    
  
Drop table #tempCurrencyConversionRates,#Funds


END