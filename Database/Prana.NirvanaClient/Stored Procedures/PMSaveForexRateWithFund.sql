
/****************************************************************************                            
Name :   PMSaveForexRateWithFund                           
Date Created: 13-MAR-2014                             
Purpose:  Save Forex Rate DateWise with fund.                                                          
                          
****************************************************************************/                            
 CREATE PROCEDURE [dbo].[PMSaveForexRateWithFund]                            
 (                            
   @Xml nText                            
 , @ErrorMessage varchar(500) output                            
 , @ErrorNumber int output                            
 )                            
AS                             
                            
SET @ErrorMessage = 'Success'                   
           
SET @ErrorNumber = 0                            
BEGIN TRAN TRAN1                             
                            
BEGIN TRY                            
                            
DECLARE @handle int                               
exec sp_xml_preparedocument @handle OUTPUT,@Xml                               
                            
  CREATE TABLE #TempForexRate                                                                                   
  (                                                                                   
    CurrencyPairID int                                                          
   ,Date datetime                          
   ,ConversionRate float                   
   ,BloombergSymbol varchar(100)
   ,FundID int
   ,SourceID int
   ,IsApproved bit                           
   )                                                                            
INSERT INTO #TempForexRate                            
 (                                                                                  
    CurrencyPairID                                                          
   ,Date                                
   ,ConversionRate                 
   ,BloombergSymbol
   ,FundID
   ,SourceID
   ,IsApproved                  
 )                                                                                  
SELECT                                                                                   
  CurrencyPairID                                                          
 ,Date                          
 ,ConversionFactor                           
 ,BloombergSymbol
 ,FundID
 ,SourceID
 ,IsApproved                      
 FROM OPENXML(@handle, '//ForexRateImport', 2)                                                                                     
 WITH                                                                
 (                                                             
   CurrencyPairID int                                                          
   ,Date datetime                          
   ,ConversionFactor float                   
   ,BloombergSymbol varchar(100)
   ,FundID int
   ,SourceID int
   ,IsApproved bit                   
 )                             
 
 -- Delete already existing currency conversion for same date, fund and currencypair 
             
 Delete T_CurrencyConversionRate from T_CurrencyConversionRate
 inner join #TempForexRate on DateDiff(d,#TempForexRate.Date,T_CurrencyConversionRate.Date)=0                         
 and #TempForexRate.CurrencyPairID = T_CurrencyConversionRate.CurrencyPairID_FK and #TempForexRate.FundID=T_CurrencyConversionRate.FundID           
        
 Insert InTo         
  T_CurrencyConversionRate        
  (        
   CurrencyPairID_FK ,        
   ConversionRate,        
   Date,
   FundID,
   SourceID,
   IsApproved        
  )        
 Select        
  CurrencyPairID,          
  ConversionRate,        
  Date,
  FundID,
  SourceID,
  IsApproved
from #TempForexRate                                      
  
--Recalculate MarkPrice for the given date based on the forward points  
  
--Fetching data for standard and inverse pairs again for the given data otherwise had to insert the data for inverse pairs   
--into #TempForexRate which will result in duplication of logic  
  
Create Table #FXConversionRates                                                                                                                                                                                                   
(                                                                                                                                                                      
 FromCurrencyID int,                                                                                                                                            
 ToCurrencyID int,                                                                                                                                                                                                          
 RateValue float,                                                                                                                                                                   
 ConversionMethod int,                                                                                                                                                                                                           
 Date DateTime,                                                                                  
 eSignalSymbol varchar(max)
)   
--Check for FX SPOT  and FX Forward  
 Declare @StartDate DateTime  
 Set @StartDate = (Select Min(Date) From #TempForexRate)  
  
 Declare @EndDate DateTime  
 Set @EndDate = (Select Max(Date) From #TempForexRate)  
  
 Insert into #FXConversionRates Exec P_GetAllFXConversionRatesForGivenDateRange @StartDate,@EndDate   
  
  Update #FXConversionRates                                                                                                
  Set RateValue = 1.0/RateValue                                                                     
  Where RateValue <> 0 and ConversionMethod = 1                                                                                                                                                    
                                                                                                
  Update #FXConversionRates                                       
  Set RateValue = 0                                                                                                                                                    
  Where RateValue is Null  

Update PM_DayMarkPrice    
Set FinalMarkPrice = IsNull(FXDayRates.RateValue,0) + IsNull(ForwardPoints,0)    
From PM_DayMarkPrice      
  
Inner Join V_SecMasterData SM On SM.TickerSymbol = PM_DayMarkPrice.Symbol  
Left outer join #FXConversionRates FXDayRates on (FXDayRates.FromCurrencyID = SM.LeadCurrencyID And FXDayRates.ToCurrencyID = SM.VsCurrencyID  
And DateDiff(Day,FXDayRates.Date,PM_DayMarkPrice.Date) = 0)  
Where (SM.AssetID = 5 Or SM.AssetID = 11)

Delete FXDayRates From #FXConversionRates FXDayRates
Left outer join PM_DayMarkPrice on (DateDiff(Day,FXDayRates.Date,PM_DayMarkPrice.Date) = 0)  
Inner Join V_SecMasterData SM On SM.TickerSymbol = PM_DayMarkPrice.Symbol and FXDayRates.FromCurrencyID = SM.LeadCurrencyID And FXDayRates.ToCurrencyID = SM.VsCurrencyID
Where (SM.AssetID = 5 Or SM.AssetID = 11)

Select PM_Taxlots.Symbol, PM_Taxlots.AUECModifiedDate, FXDayRates.Date, FXDayRates.RateValue into #OpenSymbols
From PM_Taxlots
Inner Join V_SecMasterData SM On SM.TickerSymbol = PM_Taxlots.Symbol
Inner join #FXConversionRates FXDayRates on (FXDayRates.FromCurrencyID = SM.LeadCurrencyID And FXDayRates.ToCurrencyID = SM.VsCurrencyID)  
Where TaxLotOpenQty<>0 And
Taxlot_PK in                                                                                             
(                                                                                                   
	Select max(Taxlot_PK) from PM_Taxlots                                                                                                                                                               
	where DateDiff(d,PM_Taxlots.AUECModifiedDate,CONVERT(VARCHAR(25),FXDayRates.Date,101)) >=0                                                                                                                                      
	group by taxlotid                                                   
)

--This SP use from Daily valuation so we assume these FX rates are retriveing form LiveFees so use PB Mark Price = 0
INSERT INTO PM_DayMarkPrice (Date,Symbol,ApplicationMarkPrice,PrimeBrokerMarkPrice,FinalMarkPrice,IsActive,ForwardPoints)
SELECT OS.Date,OS.Symbol,0,0,IsNull(OS.RateValue,0),1,0
From #OpenSymbols OS Where DateDiff(d,OS.AUECModifiedDate,CONVERT(VARCHAR(25),OS.Date,101)) >=0
and OS.Symbol NOT in (SELECT Symbol FROM PM_DayMarkPrice Where DateDiff(Day,OS.Date,PM_DayMarkPrice.Date) = 0)

DROP TABLE #TempForexRate, #FXConversionRates, #OpenSymbols                     
                            
EXEC sp_xml_removedocument @handle                            
                            
COMMIT TRANSACTION TRAN1                            
                            
END TRY                            
BEGIN CATCH                             
 SET @ErrorMessage = ERROR_MESSAGE();                            
print @errormessage                            
 SET @ErrorNumber = Error_number();                    
 ROLLBACK TRANSACTION TRAN1                             
END CATCH; 

