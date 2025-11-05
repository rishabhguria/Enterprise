/**********************************************************                              
 Usage:                              
Declare @date datetime                              
                              
Set @date=GetDate()                              
Exec GetPositionForADate_Guggenheim '2013-03-28'  
Declare @date datetime                         
Set @date= '11-23-2012'                    
Declare @BusinessDay Datetime                              
                      
Select  @BusinessDay = dbo.AdjustBusinessDays(@Date,-1,1)                     
Select  @BusinessDay                          
*********************************************************/                              
                         
CREATE Procedure [dbo].[GetPositionForADate_Guggenheim]                              
(                              
  @date datetime                              
)                               
As                              
                     
--Declare @date DateTime                  
--Set @date = '03-05-2013'                  
                  
Declare @RecentDateForNonZeroCash DateTime                    
Set  @RecentDateForNonZeroCash = (Select dbo.[GetRecentDateForNonZeroCash](@date))           
      
Declare @BaseCurrencyID int                                                                              
Set @BaseCurrencyID=(select BaseCurrencyID from T_Company)                  
                  
Declare @BusinessDay Datetime                                  
Select  @BusinessDay = dbo.AdjustBusinessDays(DateAdd(d,1,@date),-1,1)                        
                  
Create Table #FXConversionRates                    
(                                                                                                                                                                                                                      
FromCurrencyID int,                                                                                                                                                                                            
ToCurrencyID int,                                                                                                
RateValue float,                                                                                                                
ConversionMethod int,                                                                                
Date DateTime,                                                                       
eSignalSymbol varchar(max)                                                                          
)                    
                  
Insert into #FXConversionRates Exec P_GetAllFXConversionRatesForGivenDateRange @BusinessDay,@BusinessDay                                                                                  
                                                                                                                            
Update #FXConversionRates                                                                                                                                                          
Set RateValue = 1.0/RateValue                                                                                                                     
Where RateValue <> 0 and ConversionMethod = 1                     
                  
Create Table #OpenTaxlots                                                                            
(                                     
Symbol varchar(200),                                                                            
Quantity Int,                              
FundID Int,                              
UnitCost float,                  
CurrencyID Int,                  
ExchangeID Int                             
)                                 
                              
Insert Into #OpenTaxlots                              
 Select                               
  PT.Symbol,                              
  SUM(PT.TaxlotOpenQty * dbo.GetSideMultiplier(PT.OrderSideTagValue)),                              
  FundID,                    
  Sum(PT.AvgPrice * PT.TaxlotOpenQty)/Sum(PT.TaxlotOpenQty),                  
  Max(G.CurrencyID) As CurrencyID ,                  
  Max(G.ExchangeID) As ExchangeID                            
  From PM_Taxlots PT                    
  Inner Join T_Group G On G.GroupID = PT.GroupID                              
  Where Taxlot_PK in           
   (                                                                    
    Select Max(Taxlot_PK) from PM_Taxlots                               
    Where Datediff(d, PM_Taxlots.AUECModifiedDate,@Date) >= 0                                        
    Group By TaxlotID                              
   )                                                                                        
  and TaxLotOpenQty<>0                   
  Group By PT.Symbol,FundID                      
                              
 Create Table #DayMarkPrices                                                                             
 (                                     
  Symbol varchar(200),                                                                            
  TodayMarkPrice float                                                                          
 )                                                                            
                              
INSERT Into #DayMarkPrices                                                                            
 Select                                      
  DayMarkPrice.Symbol,                                       
  FinalMarkPrice                                                                           
  From PM_DayMarkPrice DayMarkPrice                                       
  Where Datediff(d,DayMarkPrice.Date, @BusinessDay) = 0                               
                              
--Select * from #OpenTaxlots                              
        
      
Create Table #PositionTable                    
(                                                                                                                                                                                                                      
AccountNo Varchar(max),                                                                                                                                                                                            
Date Varchar(100),                                                                                                
Symbol Varchar(100),      
CUSIPSymbol Varchar(100),      
SecurityDescription Varchar(Max),                  
AssetClass Varchar(100),                  
Quantity int,                  
ClosingMark float,                                                                                                                
MVLocal float,                                                                                
FXRate float,         
MVBase float,         
Currency varchar(100),           
Country varchar(100),          
UnitCost float,         
Side Varchar(100),      
MarginValue float,    
OrderBY int                                                                          
)                    
           
insert into #PositionTable      
                          
Select                   
CF.FundName As AccountNo,                  
convert(varchar(10),@Date,101) As Date,                 
                 
--Case                               
-- When SM.AssetID=2                              
-- Then SM.OSISymbol                  
 --Else       
SM.BloombergSymbol as                 
--End As       
Symbol,     
    
SM.CUSIPSymbol as CUSIPSymbol,    
                
SM.CompanyName As SecurityDescription,                  
Asset.AssetName As AssetClass,                  
PT.Quantity As Quantity,                  
IsNull(DM.TodayMarkPrice,0) as ClosingMark,                 
Case                
-- This is customized changes as per the Joginder and Dharendra, for Future and EURO Future Options         
-- FundID = 58 means Swap Fund         
When ((SM.AssetID = 3) Or(SM.AssetID = 4 And PT.CurrencyID = 8) Or (PT.FundID = 58))               
Then PT.Quantity * (IsNull(DM.TodayMarkPrice,0) - PT.UnitCost) * SM.Multiplier                
Else PT.Quantity * IsNull(DM.TodayMarkPrice,0) * SM.Multiplier                 
End As MVLocal,                  
                
Case                
When PT.CurrencyID =  @BaseCurrencyID                
Then 1                
Else IsNull(FXDayRatesForTradeDate.RateValue,0)                
End As FXRate,                 
                 
Case                
-- This is customized changes as per the Joginder and Dharendra, for Future and EURO Future Options          
-- FundID = 58 means Swap Fund         
When ((SM.AssetID = 3) Or(SM.AssetID = 4 And PT.CurrencyID = 8) Or (PT.FundID = 58))                
Then                 
 Case                                                                                                  
  When PT.CurrencyID =  @BaseCurrencyID                      
  Then PT.Quantity * (IsNull(DM.TodayMarkPrice,0) - PT.UnitCost) * SM.Multiplier                  
  Else PT.Quantity * (IsNull(DM.TodayMarkPrice,0) - PT.UnitCost) * SM.Multiplier * IsNull(FXDayRatesForTradeDate.RateValue,0)                  
 End                
Else                
 Case                                                                                                  
  When PT.CurrencyID =  @BaseCurrencyID                      
  Then PT.Quantity * IsNull(DM.TodayMarkPrice,0) * SM.Multiplier                  
  Else PT.Quantity * IsNull(DM.TodayMarkPrice,0) * SM.Multiplier * IsNull(FXDayRatesForTradeDate.RateValue,0)                  
 End                
End As MVBase,                  
                
CurrencySymbol As Currency,                  
[dbo].[GetCountryCode](T_Country.CountryName) As Country,                             
PT.UnitCost       ,              
              
CASE              
WHEN PT.Quantity>0              
THEN 'Long'              
ELSE              
'Short'              
END              
As Side         ,      
      
0 as MarginValue ,    
    
1 as OrderBy                 
                     
From #OpenTaxlots PT                                
Inner Join T_CompanyFunds CF On PT.FundID=CF.CompanyFundID                              
Inner Join V_secmasterData SM On SM.TickerSymbol = PT.Symbol      
Inner Join T_Asset Asset On Asset.AssetID = SM.AssetID                            
Left Outer Join #DayMarkPrices DM On Dm.Symbol = PT.Symbol                  
Inner Join T_Currency On T_Currency.CurrencyID = PT.CurrencyID                  
Left Outer Join T_Exchange On T_Exchange.ExchangeID = PT.ExchangeID                  
Left Outer Join T_Country On T_Exchange.Country = T_Country.CountryID                 
--FX Rate for Selected date                  
Left outer  join #FXConversionRates FXDayRatesForTradeDate                                                                                                                                                                               
on (FXDayRatesForTradeDate.FromCurrencyID = PT.CurrencyID                                                                                          
And FXDayRatesForTradeDate.ToCurrencyID = @BaseCurrencyID                                                                 
And DateDiff(d,@Date,FXDayRatesForTradeDate.Date)=0)                              
      
/******************************************************************************      
    CASH HANDLING      
******************************************************************************/      
      
insert into #PositionTable      
      
Select                   
MIN(CF.FundName) As AccountNo,                  
convert(varchar(10),@Date,101) As Date,                 
MIN(Currency.CurrencySymbol) as Symbol,     
MIN(Currency.CurrencySymbol) as CUSIPSymbol,     
MIN(Currency.CurrencySymbol) As SecurityDescription,         
'CASH' As AssetClass,             
SUM(DailyCash.CashValueLocal) As Quantity,                  
0 as ClosingMark,               
SUM(DailyCash.CashValueLocal) As MVLocal,              
--FX RATE      
MIN(IsNull(FXDayRatesForEndDate.RateValue,0)) as FXRate,      
--0 As MVBase,      
Sum(DailyCash.CashValueBase) As MVBase,       
MIN(Currency.CurrencySymbol) as Currency,      
'' as Country,      
0 as UnitCost,      
CASE      
  When SUM(DailyCash.CashValueLocal) >= 0                                                            
 Then 'Long'                                                            
 Else 'Short'                                                     
  End as Side  ,      
  0 as MarginValue ,      
0 as OrderBy      
                     
--From PM_CompanyFundCashCurrencyValue DailyCash       
From T_DayEndBalances DailyCash       
Inner Join T_CompanyFunds CF On DailyCash.FundID=CF.CompanyFundID         
Inner join T_Currency Currency on Currency.CurrencyID = DailyCash.LocalCurrencyID      
--FX Rate for Selected date                  
Left outer  join #FXConversionRates FXDayRatesForEndDate                                                                                                                                                                               
on (FXDayRatesForEndDate.FromCurrencyID = DailyCash.LocalCurrencyID                                                                                          
And FXDayRatesForEndDate.ToCurrencyID = @BaseCurrencyID                                                                 
And DateDiff(d,@Date,FXDayRatesForEndDate.Date)=0)         
      
where DailyCash.Date = @RecentDateForNonZeroCash      
Group By DailyCash.Date, DailyCash.FundID,DailyCash.LocalCurrencyID       
         
------------------------------------------------------------------------    
Update #PositionTable    
set MarginValue = MD.VMargin    
from #PositionTable PT    
Inner Join T_MarginData MD     
On PT.CUSIPSymbol = MD.CUSIP     
and MD.Account = PT.AccountNo     
and datediff(d,@date,MD.Date)=0    
-------------------------------------------------------------------------    
    
Select * from #PositionTable      
Order by OrderBy,Date,AccountNo,Symbol       
               
                  
Drop Table #OpenTaxlots,#DayMarkPrices,#FXConversionRates 