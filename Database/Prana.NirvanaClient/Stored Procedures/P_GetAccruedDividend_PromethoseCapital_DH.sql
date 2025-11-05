
CREATE Procedure [dbo].[P_GetAccruedDividend_PromethoseCapital_DH]        
(      
 @EndDate datetime,        
 @FundIds varchar(max)        
)        
As        
        
--/*        
--Declare @Startdate datetime        
--Declare @EndDate datetime        
        
--set @Startdate = '2012-9-1'        
--Set @EndDate = '2013-10-1'        
--*/      

--Declare @EndDate datetime 
               
--Declare @FundIds varchar(max)        
       
        
--set @EndDate=N'2022-09-19' 
--set @FundIds=N''  
        
SET NOCOUNT ON        
        
Create Table #Funds (FundID int)                                                      
 if (@FundIds is NULL or @FundIds = '')                                                      
 Insert into #Funds                                                      
 Select CompanyFundID as FundID from T_CompanyFunds                                                      
 else                                                      
 Insert into #Funds                                                      
 Select Items as FundID from dbo.Split(@FundIds,',')        
        
        
Create Table #FXConversionRates                                                                                         
(                                                                                                                                                                                                                  
 FromCurrencyID int,                                                                                                                                                                                        
 ToCurrencyID int,                                                                                            
 RateValue float,                                                                                                            
 ConversionMethod int,                                                                            
 Date DateTime,                                                                   
 eSignalSymbol varchar(100) ,
 FundID int                                                                      
)         
        
Insert into #FXConversionRates Exec P_GetAllFXConversionRatesFundWiseForGivenDateRange @EndDate,@EndDate                                                                              
                                                                                 
 Update #FXConversionRates                                                                                                                                                      
 Set RateValue = 1.0/RateValue                                                                                                                 
 Where RateValue <> 0 and ConversionMethod = 1                                                                            
     
      
Declare @BaseCurrencyID int                                                                          
Set @BaseCurrencyID=(select top 1 BaseCurrencyID from T_Company where companyId <> -1)   

     
--select * from #FXConversionRates 
--Where eSignalSymbol = 'USDBRL-FX1'       
        
Create table #Dividend        
(        
 ExDate datetime,        
 PayoutDate datetime,        
 Symbol varchar(max), 
 BloombergSymbol Varchar(max),
 ISINSymbol Varchar(max),
 SEDOLSymbol Varchar(max),
 CUSIPSymbol Varchar(max),
 ReutersSymbol Varchar(max),
 UnderLyingSymbol Varchar(max),       
 FundID int,        
 Fund varchar(max),        
 Asset Varchar(max),        
 CurrencyID int,        
 TradeCurrency varchar(max),      
 CompanyName varchar(max),        
 NetAmountLocal float,        
 Description Varchar(max),        
 Activity Varchar(max),        
 FXRate float,        
 NetAmountBase float         
)        
        
Insert into #Dividend     
   
select 
TCD.Exdate, 
TCD.PayoutDate,
TCD.Symbol,  
BloombergSymbol,
ISINSymbol,
SEDOLSymbol,
CUSIPSymbol,
ReutersSymbol,
UnderLyingSymbol,     
TCD.FundID,         
F.FundName,        
A.AssetName,        
TCD.CurrencyID ,        
C.CurrencySymbol,          
Replace(ISNULL(SM.CompanyName,'Undefined'),',', ' ') As CompanyName,
TCD.Amount,
Replace(isnull(TCD.Description, ''),',', ' ') as Description,      
ActivityType.ActivityType,     
        
CASE        
	WHEN (TCD.CurrencyID <> F.LocalCurrency) 
	Then FXRatesForEXDate.Val
ELSE 1        
END as FXRate,        
0.0 As NetAmountBase       
   
from T_CashTransactions TCD
--inner JOIN T_ActivityType on (T_ActivityType.ActivityTypeId = TCD.ActivityTypeId and ActivitySource = 2)
inner join T_ActivityType ActivityType on TCD.ActivityTypeId =  ActivityType.ActivityTypeId
inner join T_CompanyFunds F on TCD.FundID = F.CompanyFundID        
inner join T_Currency C on TCD.CurrencyID =C.CurrencyID        
Inner join V_SecMasterData SM on TCD.Symbol = SM.TickerSymbol        
Inner join T_Asset A on SM.AssetID = A.AssetID       

LEFT OUTER JOIN #FXConversionRates AS FXDayRatesForEXDate ON (  
   FXDayRatesForEXDate.FromCurrencyID = TCD.CurrencyID  
   AND FXDayRatesForEXDate.ToCurrencyID = F.LocalCurrency  
   AND DateDiff(d, @EndDate, FXDayRatesForEXDate.Date) = 0
   AND FXDayRatesForEXDate.FundID = TCD.FundID  
   ) 
LEFT OUTER JOIN #FXConversionRates AS ZeroFundIDFXDayRatesForEXDate ON (  
   ZeroFundIDFXDayRatesForEXDate.FromCurrencyID = TCD.CurrencyID  
   AND ZeroFundIDFXDayRatesForEXDate.ToCurrencyID = F.LocalCurrency  
   AND DateDiff(d, @EndDate, ZeroFundIDFXDayRatesForEXDate.Date) = 0
   AND ZeroFundIDFXDayRatesForEXDate.FundID = 0 
   )    
   CROSS APPLY (
	SELECT CASE 
			WHEN FXDayRatesForEXDate.RateValue IS NULL
				THEN CASE 
						WHEN ZeroFundIDFXDayRatesForEXDate.RateValue IS NULL
							THEN 0
						ELSE ZeroFundIDFXDayRatesForEXDate.RateValue
						END
			ELSE FXDayRatesForEXDate.RateValue
			END
	) AS FXRatesForEXDate(Val)               
        
Where        
(  
DATEDIFF(d,ExDate,@EndDate)>=0 AND DATEDIFF(D,@EndDate,PayoutDate)>0
)    
AND TCD.FundID In (Select * from #Funds)   
      
  
Update #Dividend set NetAmountBase= (NetAmountLocal * FXRate)        
        
Select * from #Dividend         
        
Drop Table #Dividend, #FXConversionRates, #Funds