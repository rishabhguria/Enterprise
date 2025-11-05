/*-- =============================================                                                                                           
-- Author:  <Author: Singh Singh>                                                                                                                                    
-- Create date: <Create Date: 04-June-2009>                                                                                                                                    
-- Description: <Description: To get TopX Holding on basis of Market value>                                                                                                                           
[PMGetTopXForPositionActivityLong] '02-3-2011',10,'1195,1182,1183'       
-- =============================================*/               
CREATE Procedure [PMGetTopXForPositionActivityLong]      
(      
 @Date Datetime ,                                                                                                                           
 @TopX int,                                                                                                                             
 @FundID varchar(max)        
)       
As      
      
Declare @TopBy char(50)              
Set @TopBy='Symbol'       
         
          
--Declare @Date DateTime          
--Set @Date = GetUTCDate()         
        
Declare @Fund Table                                                            
(                                                            
 FundID int                                                        
)           
        
Insert into @Fund          
Select Cast(Items as int) from dbo.Split(@FundID,',')          
          
Declare @BaseCurrencyID int                                                                                                                  
Set @BaseCurrencyID=(select BaseCurrencyID from T_Company)                                                                                              
                                
-- get Mark Price for End Date                                     
Create Table #MarkPriceForDate                                                                   
(                                                          
Finalmarkprice float ,                                                                                         
Symbol varchar(50)                                                                                      
)                                                                
-- get forex rates for 2 date ranges                                                                                          
Create Table #FXConversionRates                                                                                                                           
(                                                       
 FromCurrencyID int,                                                                  
 ToCurrencyID int,                                                                                                                                
 RateValue float,                                                                                         
 ConversionMethod int,                                                                                                                                 
 Date DateTime,                                                                                                                                         
 eSignalSymbol varchar(max)                                                                                               
)    
    
Declare @MarkPriceDate DateTime                  
Set @MarkPriceDate = DateAdd(d,1,@Date)                                                               
                                                                 
 INSERT INTO #MarkPriceForDate Exec P_GetMarkPriceForBusinessDay @MarkPriceDate    
--Select * From dbo.GetMarkPriceForBusinessDay(DateAdd(d,1,@Date))                                                                      
       
  Insert into #FXConversionRates Exec P_GetAllFXConversionRatesForGivenDateRange @Date,@Date                                        
  --Select * from  dbo.GetAllFXConversionRatesForGivenDateRange(@Date,@Date) as A                                                                         
                             
  Update #FXConversionRates                            
  Set RateValue = 1.0/RateValue                                                                      
  Where RateValue <> 0 and ConversionMethod = 1                            
                                                                        
 Update #FXConversionRates                                                                      
  Set RateValue = 0                                                                      
  Where RateValue is Null             
          
Create Table #TempTable               
(                
 Symbol varchar(500),                
 TaxLotOpenQty float,                                  
 PositionType varchar(10),          
 GroupID Varchar(50)               
)             
          
Insert Into #TempTable                                                                                                                               
 Select                                                                           
  PT.Symbol As Symbol,                  
  PT.TaxLotOpenQty As TaxLotOpenQty ,              
 Case dbo.GetSideMultiplier(PT.OrderSideTagValue)                                                                                                                                             
  When  1                                           
  Then  'Long'                                                                                                
  Else  'Short'                                                                                                                                              
 End as PositionType,          
 GroupID as GroupID              
                                      
 from PM_Taxlots PT                
 Where TaxLotOpenQty<>0 and PT.FundID in (select FundID from @Fund)             
 and taxlot_PK in                                                                                                                                                     
  (                               
   select max(Taxlot_PK) from PM_Taxlots                                                                                                                                                       
   where DateDiff(d,PM_Taxlots.AUECModifiedDate,@Date) >=0                                                                   
   group by taxlotid                                                                                                                                  
  )           
          
--Select * from #TempTable order by Symbol          
          
Create Table #TempOutput1               
(                
 Symbol varchar(500),                
 TaxLotOpenQty float,                                  
 PositionType varchar(10),          
 GroupID Varchar(50)               
)             
          
Insert Into #TempOutput1                                                                                                                               
 Select                                                                           
  MIN(TempPT.Symbol) As Symbol,                  
  SUM(TempPT.TaxLotOpenQty) As TaxLotOpenQty ,              
  TempPT.PositionType,          
  Max(TempPT.GroupID) as GroupID    
 from #TempTable TempPT                 
  Group by TempPT.Symbol,TempPT.PositionType            
  Order By TempPT.Symbol     
          
--Select * from #TempOutput1 order by Symbol          
          
Create Table #TempOutput2                
(                
 Symbol varchar(500),                
 TaxLotOpenQty float,                                         
 PositionType varchar(10),          
 GroupID Varchar(50)             
)            
Insert Into  #TempOutput2           
          
 Select                   
 Isnull(Temp1.Symbol,Temp2.Symbol) As Symbol,            
 (isnull(Temp1.TaxlotOpenQty,0) - isnull(Temp2.TaxlotOpenQty,0)) As TaxLotOpenQty,          
    'Long' as PositionType, --IsNull(Temp1.PositionType, Temp2.PositionType) as PositionType,          
    IsNull(Temp1.GroupID,Temp2.GroupID) as GroupID                  
 from #TempOutput1 Temp1 Full join #TempOutput1 Temp2 on Temp1.Symbol=Temp2.Symbol             
 and Temp1.PositionType = 'Long' and Temp2.PositionType = 'Short'            
 Where Temp1.PositionType <> 'Short' or Temp2.PositionType <> 'Long'            
          
update #TempOutput2           
 Set TaxlotOpenQty=abs(TaxlotOpenQty),          
 PositionType='Short' Where TaxLotOpenQty < 0          
          
--Select * from #TempOutput2 order by symbol          
          
Create Table #TempOutput                
(                
 Symbol varchar(500),                
 UnderlyingSymbol varchar(500),                
 TaxLotOpenQty float,                                  
 MarkPrice   float,                                          
 MarketValue float,                                 
 FXDayRate float,                             
 CompanyName varchar(max),              
 PositionType varchar(10)               
)           
          
Insert Into #TempOutput                                                                                                                               
 Select                                                                           
  PT.Symbol As Symbol,                
  IsNull(SM.UnderlyingSymbol,'') As UnderlyingSymbol,                                                          
  PT.TaxLotOpenQty As TaxLotOpenQty ,                     
                                       
Case                                   
  When G.AssetID=5 OR G.AssetID=11 -- 11 for FX Forward  and 5 for FX Spot                                       
  Then IsNull(FXMarkPriceForDate.RateValue,0)                                       
  Else IsNull(MarkPriceForDate.Finalmarkprice,0)                                      
End MarkPrice,                                             
                                                                                                          
Case                                                                                                                               
When G.AssetID=5 OR G.AssetID=11 -- 11 for FX Forward  and 5 for FX Spot                                          
Then                                            
 Case                                              
  When @BaseCurrencyID=SM.VsCurrencyID                                             
  Then IsNull(FXMarkPriceforDate.RateValue * TaxlotOpenQty * IsNUll(SM.Multiplier,1),0)                                            
  Else IsNull(FXMarkPriceforDate.RateValue * TaxlotOpenQty * FXDayRatesForFXTrade.RateValue * IsNUll(SM.Multiplier,1),0)                                            
 End                                            
Else                                            
 Case                                                                            
  When G.CurrencyID =  @BaseCurrencyID                                                                                                                
  Then IsNull(IsNull(MarkPriceForDate.Finalmarkprice,0) * TaxlotOpenQty * IsNUll(SM.Multiplier,1) ,0)                                                                                             
  Else IsNull(IsNull(MarkPriceForDate.Finalmarkprice,0) * FXDayRates.RateValue * TaxlotOpenQty * IsNUll(SM.Multiplier,1),0)                                                                        
 End                                            
End as MarketValue ,                                                                                                 
                                     
Case                         
  When G.AssetID=5 OR G.AssetID=11 -- 11 for FX Forward  and 5 for FX Spot                                        
  Then IsNull(FXDayRatesForFXTrade.RateValue,0)                                      
  Else IsNull(FXDayRates.RateValue,0)                             
End as ConversionRateEnd,                        
IsNull(SM.CompanyName,'') as CompanyName,              
PT.PositionType as PositionType              
                                      
 from #TempOutput2 PT               
  Inner Join T_Group G on G.GroupID = PT.GroupID                                                           
  Left Outer Join #MarkPriceForDate MarkPriceForDate on MarkPriceForDate.Symbol=PT.Symbol                                                                        
--Forex Price for Date other than FX Trade                                          
  Left outer join #FXConversionRates FXDayRates                                       
  On (FXDayRates.FromCurrencyID = G.CurrencyID                                       
  And FXDayRates.ToCurrencyID = @BaseCurrencyID                                       
  And DateDiff(d,@Date,FXDayRates.Date)=0)                                           
--Security Master DB Join                                                                       
  Left outer join V_SecMasterData SM ON SM.TickerSymbol = PT.Symbol              
--Mark Price for Date for FX Trades              
  Left outer  join #FXConversionRates FXMarkPriceforDate                                        
  On (FXMarkPriceforDate.FromCurrencyID = SM.LeadCurrencyID                                       
  And FXMarkPriceforDate.ToCurrencyID = SM.VsCurrencyID                                       
  And DateDiff(d,@Date,FXMarkPriceforDate.Date)=0)              
--FX Rate for Date for FX Trades                                          
  Left outer  join #FXConversionRates FXDayRatesForFXTrade                                       
  On (FXDayRatesForFXTrade.FromCurrencyID = SM.VsCurrencyID                                       
  And FXDayRatesForFXTrade.ToCurrencyID = @BaseCurrencyID                                       
  And DateDiff(d,@Date,FXDayRatesForFXTrade.Date)=0)              
Where PT.TaxlotOpenQty <> 0           
            
Create Table #TempLong               
(                
 Symbol varchar(500),                
 UnderlyingSymbol varchar(500),      
 CompanyName varchar(max),                   
 MarketValue float,                                 
 PositionType varchar(10)              
)         
Insert InTo #TempLong      
      
Exec ('Select TOP(' +@TopX + ')                   
  MIN(Symbol) As Symbol                  
 ,MIN(UnderlyingSymbol) As UnderlyingSymbol                  
 ,MIN(CompanyName) As CompanyName                  
 ,Sum(MarketValue) As MarketValue               
 ,MIN(PositionType) As PositionType              
from #TempOutput Where PositionType = ' + '''' + 'Long' + '''' +                
'Group By '  + @TopBy +                  
' Order By MarketValue DESC')      
      
      
Select * from #TempLong         
          
--  Select * from #TempOutput        
Drop Table #TempOutput,#MarkPriceForDate,#FXConversionRates,#TempTable,#TempOutput1,#TempOutput2,#TempLong