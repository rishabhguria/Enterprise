/****************************************************

Modified By : Rahul Gupta
Modified On : 2013-Jan-15
Description : http://jira.nirvanasolutions.com:8080/browse/PRANA-1947
   
Execution Statement :         
P_GetReportsData_DailyTradedCashReport '12-27-2012'                         
*****************************************************/      
                
CREATE PROCEDURE [dbo].[P_GetReportsData_DailyTradedCashReport]                              
(                              
 @tradeDate datetime                              
)                              
AS      
    
--Declare @tradeDate datetime    
--Set @tradeDate =  '04-18-2012' 
----------------------------------------------------------------------
Create Table #SecMasterData
(
  TickerSymbol varchar(100),
  Multiplier float,
  LeadCurrencyID int,
  VsCurrencyID int,
  AssetID int,
)     

Insert into #SecMasterData
Select
TickerSymbol,
Multiplier,
LeadCurrencyID,
VsCurrencyID,
AssetID
from V_SecMasterData
----------------------------------------------------------------------                          
                              
Create TABLE #FXConversionRatesForDate                                    
(                                  
  FromCurrencyID int,                                  
  ToCurrencyID int,                                  
  RateValue float,                                  
  ConversionMethod int,                                  
  Date DateTime,                                  
  eSignalSymbol varchar(max)                                  
)                                   
       
                       
INsert into #FXConversionRatesForDate                               
Exec P_GetAllFXConversionRatesForGivenDateRange @tradeDate,@tradeDate    

 Update #FXConversionRatesForDate                                 
 Set RateValue = 1.0/RateValue                                                                                                                                                            
 Where RateValue <> 0 and ConversionMethod = 1           
          
Declare @BaseCurrencyID int           
set @BaseCurrencyID = (Select BaseCurrencyID from T_Company )        
----------------------------------------------------------------------      
Create TABLE #TradedCashTable                                    
(   
  TickerSymbol varchar(100),  
  AssetID Int,                             
  FundID int,                                  
  FundName Varchar(100),                                  
  SettlementCurrency Varchar(10),                                  
  SideMultiplier int, 
  Multiplier int, 
  AvgPrice float,  
  ProcessDate datetime,                               
  FXConversionRate Float, 
  NetPayment Float,                                
)          
----------------------------------------------------------------------      
Insert InTo #TradedCashTable                    
                          
Select  
VT.Symbol as TickerSymbol,
VT.AssetID as AssetID,
FundID AS FundID,                           
FundName AS FundName,                             
T_Currency.CurrencySymbol AS SettlementCurrency,            
VT.SideMultiplier as SideMultiplier, 
#SecMasterData.Multiplier as Multiplier,
VT.AvgPrice as AvgPrice,  
VT.ProcessDate as ProcessDate,                         
                       
Case         
When VT.CurrencyID = @BaseCurrencyID                          
Then 1                              
Else           
	Case                   
		When IsNull(VT.FXRate_Taxlot,VT.FXRate) > 0  And  IsNull(VT.FXConversionMethodOperator_Taxlot,VT.FXConversionMethodOperator)='M'                           
		Then IsNull(IsNull(VT.FXRate_Taxlot,VT.FXRate),0)           
		When IsNull(VT.FXRate_Taxlot,VT.FXRate) > 0  And  IsNull(VT.FXConversionMethodOperator_Taxlot,VT.FXConversionMethodOperator) = 'D'                            
		Then IsNull(1/IsNull(VT.FXRate_Taxlot,VT.FXRate),0)           
		Else IsNull(#FXConversionRatesForDate.RateValue,0)                                                                   
	End                              
End As FXConversionRate,

Case     
When VT.AssetID = 8 And VT.AvgPrice <> 0 And #SecMasterData.Multiplier <> 0    
Then (((VT.AvgPrice * VT.TaxlotQty * VT.SideMultiplier * #SecMasterData.Multiplier) / 100) + VT.TotalExpenses) *(-1)    
When VT.AssetID <> 8    
Then    
((VT.AvgPrice * VT.TaxlotQty * VT.SideMultiplier * #SecMasterData.Multiplier *            
(      
   CASE VT.IsSwapped         
   WHEN 1         
   THEN 0         
   ELSE 1         
   END       
 )) + VT.TotalExpenses    
) * (-1)    
Else 0    
End AS NetPayment       
                
                           
from V_Taxlots as VT                
 Inner join T_AUEC on T_AUEC.AUECID= VT.AUECID              
 Inner join T_CompanyFunds on VT.FundID=T_CompanyFunds.CompanyFundID              
 Inner join T_Currency on T_Currency.CurrencyID = VT.CurrencyID                  
 Inner join #SecMasterData on #SecMasterData.TickerSymbol=VT.Symbol 
           
 Left Outer join #FXConversionRatesForDate 
 On DATEDIFF(d,#FXConversionRatesForDate.Date,VT.ProcessDate) = 0                  
 and #FXConversionRatesForDate.FromCurrencyID = VT.CurrencyID 
 And #FXConversionRatesForDate.ToCurrencyID = @BaseCurrencyID 
            
 Where DATEDIFF(d, VT.ProcessDate, @tradeDate) = 0                              
              
------------------UNION All FX Spot and Forward Cash impact------------------------------------------                  
           
Insert InTo #TradedCashTable          
Select 
VT.Symbol as TickerSymbol,
VT.AssetID as AssetID,                                    
FundID AS FundID,                           
FundName AS FundName,                             
T_Currency.CurrencySymbol AS SettlementCurrency,            
VT.SideMultiplier*(-1) as SideMultiplier,
#SecMasterData.Multiplier as Multiplier,
VT.AvgPrice as AvgPrice,
VT.ProcessDate as ProcessDate, 
         
Case         
When #SecMasterData.LeadCurrencyID = @BaseCurrencyID                           
Then 1
Else                                                    
	Case           
		When IsNull(VT.FXRate_Taxlot,VT.FXRate) > 0  And IsNull(VT.FXConversionMethodOperator_Taxlot,VT.FXConversionMethodOperator)='M'                           
		Then IsNull(IsNull(VT.FXRate_Taxlot,VT.FXRate),0)           
		When IsNull(VT.FXRate_Taxlot,VT.FXRate) > 0  And  IsNull(VT.FXConversionMethodOperator_Taxlot,VT.FXConversionMethodOperator) = 'D'                            
		Then IsNull(1/IsNull(VT.FXRate_Taxlot,VT.FXRate),0)           
		Else IsNull(#FXConversionRatesForDate.RateValue,0)                                      
	END                                       
END As FXConversionRate ,

            
((VT.TaxlotQty * SideMultiplier *(-1)* #SecMasterData.Multiplier )) * (-1) *
(CASE VT.IsSwapped 
 WHEN 1 
 THEN 0 
 ELSE 1 
 END ) AS NetPayment  
                           
from V_Taxlots as VT                
 Inner Join T_AUEC on  T_AUEC.AUECID= VT.AUECID              
 Inner Join T_CompanyFunds on VT.FundID=T_CompanyFunds.CompanyFundID              
 Inner Join #SecMasterData on #SecMasterData.TickerSymbol=VT.Symbol            
 Inner Join T_Currency on T_Currency.CurrencyID = #SecMasterData.LeadCurrencyID 
             
 Left Outer Join #FXConversionRatesForDate 
 On DATEDIFF(d, #FXConversionRatesForDate.Date, VT.ProcessDate) = 0                  
 and #FXConversionRatesForDate.FromCurrencyID = #SecMasterData.LeadCurrencyID                                
 and #FXConversionRatesForDate.ToCurrencyID = @BaseCurrencyID                  
                            
 Where DATEDIFF(d, VT.ProcessDate,@TradeDate) = 0
 And (VT.AssetID=5 or VT.AssetID=11)       
      
----------------------------------------------------------------------      
Select
  TickerSymbol,
  AssetID,       
  FundID,                                   
  FundName,      
  SettlementCurrency,      
  SideMultiplier,
  Multiplier,
  AvgPrice,                                  
  ProcessDate,        
  FXConversionRate,
  NetPayment     
From #TradedCashTable
----------------------------------------------------------------------                  
    
----------------------------------------------------------------------     
DROP TABLE #SecMasterData, #FXConversionRatesForDate, #TradedCashTable   
----------------------------------------------------------------------            