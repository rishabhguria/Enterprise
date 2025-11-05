
--Save mark prices and forward Points for Fx and FxForwards on saving of FX Conversion
            
/****************************************************************************                                
Name :   PMSaveForexRate_Import                               
Date Created: 10-APRIL-2015                                 
Purpose:  Save Forex Rate DateWise with fund.                   
Module: MarkPriceAndForexConversion/PM                                                             
                    
****************************************************************************/                                
 CREATE PROCEDURE [dbo].[PMSaveFundwiseForexRate_Import]                                
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
    FromCurrencyID int                                
   ,ToCurrencyID int                                   
   ,Date datetime                              
   ,ConversionRate float
   ,FundID int
   ,Source int                                 
   )                                                                                
INSERT INTO #TempForexRate                                
 (                                                                                      
    FromCurrencyID                                
   ,ToCurrencyID                                  
   ,Date                                    
   ,ConversionRate
   ,FundID
   ,Source                                  
 )                                                                                      
SELECT                                                                                       
  BaseCurrencyID                                
 ,SettlementCurrencyID                                  
 ,Date                              
 ,ForexPrice
 ,FundID
 ,Source                                                  
    FROM OPENXML(@handle, '//ForexPriceImport', 2)                                                                                         
 WITH                                                                                       
 (                                                                 
  BaseCurrencyID int                                
 ,SettlementCurrencyID int                          
 ,Date datetime                                
 ,ForexPrice float
 ,FundID int
 ,Source int                        
 )             
          
Declare @masterTableId int            
Set @masterTableId=0            
            
Declare @count2 int                     
Set @count2=0              
            
Declare                   
@fromCurrencyID  int,                  
@toCurrencyID int,                  
@date Datetime,             
@conversionRate float,
@fundID INT,             
@source INT                
                  
DECLARE CurrencyConversion_Cursor CURSOR FAST_FORWARD FOR                                              
Select                    
  FromCurrencyID                           
 ,ToCurrencyID                                  
 ,Date                           
 ,ConversionRate     
 ,Case when FundID < 0 THEN 0 ELSE FundID end
 ,Source                              
                 
From  #TempForexRate            
            
Open CurrencyConversion_Cursor;                                            
                                            
FETCH NEXT FROM CurrencyConversion_Cursor INTO                     
@fromCurrencyID ,                  
@toCurrencyID ,                  
@date,                  
@conversionRate,
@fundID,
@source   ;             
            
WHILE @@fetch_status = 0                                              
  BEGIN              
            
 Set @count2= (Select CSP.CurrencyPairID from T_CurrencyStandardPairs CSP            
 Where CSP.FromCurrencyID=@fromCurrencyID    
 and CSP.ToCurrencyID=@toCurrencyID)            
            
if(@count2>0)            
BEGIN            
 -- Update T_CurrencyStandardPairs Set eSignalSymbol=@eSignalSymbol Where             
 -- FromCurrencyID=@fromCurrencyID and ToCurrencyID=@toCurrencyID            
 -- and CurrencyPairID=@count2            
             
  Delete T_CurrencyConversionRate           
  Where DateDiff(d,T_CurrencyConversionRate.Date,@date)=0 And T_CurrencyConversionRate.CurrencyPairID_FK=@count2
  and T_CurrencyConversionRate.FundID=@fundID          
             
  Insert InTo             
   T_CurrencyConversionRate            
   (            
    CurrencyPairID_FK ,            
    ConversionRate,            
    Date,
    FundID,
    SourceID            
   )            
  Select            
   @count2,              
   @conversionRate,            
   @date,
   @fundID,
   @source              
END            
               
            
FETCH NEXT FROM CurrencyConversion_Cursor INTO                
@fromCurrencyID ,                  
@toCurrencyID ,                  
@date,                  
@conversionRate,
@fundID,
@source;             
            
END              
CLOSE CurrencyConversion_Cursor;                                              
DEALLOCATE CurrencyConversion_Cursor;        
  
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

--This SP use from Import UI so we assume these FX rates are retriveing form PB File so use PB Mark Price = MarkPrice
INSERT INTO PM_DayMarkPrice (Date,Symbol,ApplicationMarkPrice,PrimeBrokerMarkPrice,FinalMarkPrice,IsActive,ForwardPoints)
SELECT OS.Date,OS.Symbol,0,IsNull(OS.RateValue,0),IsNull(OS.RateValue,0),1,0
From #OpenSymbols OS Where DateDiff(d,OS.AUECModifiedDate,CONVERT(VARCHAR(25),OS.Date,101)) >=0
and OS.Symbol NOT in (SELECT Symbol FROM PM_DayMarkPrice Where DateDiff(Day,OS.Date,PM_DayMarkPrice.Date) = 0)
      
DROP TABLE #TempForexRate,#FXConversionRates, #OpenSymbols                     
                                
EXEC sp_xml_removedocument @handle                                
                                
COMMIT TRANSACTION TRAN1                                
                                
END TRY                                
BEGIN CATCH                                 
 SET @ErrorMessage = ERROR_MESSAGE();                                
print @errormessage                                
 SET @ErrorNumber = Error_number();                        
 ROLLBACK TRANSACTION TRAN1                                 
END CATCH; 

