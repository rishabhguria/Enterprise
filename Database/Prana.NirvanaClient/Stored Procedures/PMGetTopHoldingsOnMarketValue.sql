/*-- =============================================                                                                                                   
-- Author:  <Author: Singh Singh>                                                                                                                                            
-- Create date: <Create Date: 04-June-2009>                                                                                                                                            
-- Description: <Description: To get TopX Holding on basis of Market value>                                                                                                                                   
[PMGetTopHoldingsOnMarketValue] '10-06-2012',50, 'Symbol', 'Long','1182,1183,1184'                                                                                    
  
Altered By: Sandeep Singh       
Date: July 31, 2012      
Desc: FX spot, Frwd, Fixed income related changes   

Modified Date: 27-Sep-2012    
Modified By: Rahul Gupta    
Description: Optimized   
-- =============================================*/                       
CREATE Procedure [PMGetTopHoldingsOnMarketValue]                      
(                      
 -- Add the parameters for the stored procedure here                  
 @Date Datetime,                                                                                                                                          
 @TopX int,                                                                                                                                             
 @TopBy char(50),                          
 @OrderBy char(20),                  
 @FundID varchar(max)                    
)                      
as                   
                  
--Declare @Date Datetime      
--Declare @TopX int                                                                                                                                           
--Declare @TopBy char(50)      
--Declare @OrderBy char(20)      
--Declare @FundID varchar(max)      
--      
--Set @Date =  '05-8-2012'                
--Set @TopX = 50      
--Set @TopBy = 'Symbol'      
--Set @OrderBy = 'Long'      
--Set @FundID = '1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38'      
         
--temp table for Fund ids             
Declare @T_FundIDs Table                                  
(                                  
 FundId int                                  
)                                  
Insert Into @T_FundIDs Select * From dbo.Split(@FundID, ',')                                                                                                                        
                                         
CREATE TABLE #T_CompanyFunds                                  
(                                  
 FundID int,                                  
 FundName varchar(50),                                  
 FundShortName varchar(50),                                  
 CompanyID int,                                  
 FundTypeID int,                          
 UIOrder int NULL                                 
)                         
                               
Insert Into #T_CompanyFunds                                  
Select               
 CompanyFundID,                                  
 FundName,                                  
 FundShortName,                                  
 CompanyID,                                  
 FundTypeID,                          
 UIOrder                                   
 From T_CompanyFunds INNER JOIN @T_FundIDs FundIDs ON T_CompanyFunds.CompanyFundID = FundIDs.FundID                   
 Where T_CompanyFunds.IsActive=1
                 
Declare @BaseCurrencyID int                                                                                                                          
Set @BaseCurrencyID=(select BaseCurrencyID from T_Company)                                                                                                      
                                        
-- get Mark Price for End Date                                             
Create Table #MarkPriceForDate                                                                           
(                                                                  
Finalmarkprice float ,                                                                                                 
Symbol varchar(50)                                                                                              
)                                                                        
-- get forex rates for 2 date ranges                                                                                                  
Create Table #FXConversionRates                                                                                                                                  (                                                               
 FromCurrencyID int,                                                  
 ToCurrencyID int,                                                                                            
 RateValue float,                                                                                                 
 ConversionMethod int,                                                                                                                                         
 Date DateTime,                                                                                                                                                 
 eSignalSymbol varchar(max)                                                                                                       
)                                                                     
                                      
 INSERT INTO #MarkPriceForDate Select * From dbo.GetMarkPriceForBusinessDay(DateAdd(d,1,@Date))                 
                                                      
  Insert into #FXConversionRates                                 
  Select * from  dbo.GetAllFXConversionRatesForGivenDateRange(@Date,@Date) as A                                                                                 
                                     
  Update #FXConversionRates                                    
  Set RateValue = 1.0/RateValue                                                                              
  Where RateValue <> 0 and ConversionMethod = 1                                         
      
-- get Security Master Data in a Temp Table                                                                                                           
Create Table #SecMasterDataTempTable                                                                                                                                                                          
(                                                                                                                                                                           
  TickerSymbol Varchar(100),                                                                                                                                                                                                           
  CompanyName  VarChar(500),                                                                                                                                      
  SecurityTypeName Varchar(200),                                                                                                                                              
  UnderlyingSymbol Varchar(100),      
  Multiplier Float,                                                                                                                          
)                                                                                                    
                                  
Insert Into #SecMasterDataTempTable                                                                                                                   
Select                                                                                                       
 TickerSymbol ,                                                                                                                       
 CompanyName  ,                                                                         
 SecurityTypeName,                                                       
 UnderlyingSymbol ,      
 Multiplier                                               
     From V_SecMasterData                    
                  
Create Table #TempTable                       
(             
 Symbol varchar(500),                        
 TaxLotOpenQty float,                                          
 PositionType varchar(10),       
 GroupID Varchar(50)                       
)                     
-- get open positions                 
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
Inner Join #T_CompanyFunds CF on CF.FundID = PT.FundID                      
Where TaxLotOpenQty<>0                      
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
-- group by Symbol and side               
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
'Long' as PositionType,                
IsNull(Temp1.GroupID,Temp2.GroupID) as GroupID   
                       
From #TempOutput1 Temp1 Full join #TempOutput1 Temp2 on Temp1.Symbol=Temp2.Symbol                     
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
 PositionType varchar(10),          
 AssetID int                       
)                   
                  
Insert Into #TempOutput                                                                                                                                       
Select                                                                                   
PT.Symbol As Symbol,                        
IsNull(SM.UnderlyingSymbol,'') As UnderlyingSymbol,                                                                  
PT.TaxLotOpenQty As TaxLotOpenQty ,                             
IsNull(MarkPriceForDate.FinalMarkPrice,0) As MarkPrice,                                                   
Case                                                                                    
 When G.CurrencyID =  @BaseCurrencyID                                                                                                                        
 Then IsNull(IsNull(MarkPriceForDate.Finalmarkprice,0) * TaxlotOpenQty * IsNUll(SM.Multiplier,1) ,0)                               
 Else IsNull(IsNull(MarkPriceForDate.Finalmarkprice,0) * FXDayRates.RateValue * TaxlotOpenQty * IsNUll(SM.Multiplier,1),0)                                                                                
End  as  MarketValue ,                                                                                                            
      
IsNull(FXDayRates.RateValue,0) as ConversionRateEnd,          
IsNull(SM.CompanyName,'') as CompanyName,                      
PT.PositionType as PositionType,          
G.AssetID                     
                                              
 from #TempOutput2 PT                       
  Inner Join T_Group G on G.GroupID = PT.GroupID                                                                   
  Left Outer Join #MarkPriceForDate MarkPriceForDate on MarkPriceForDate.Symbol=PT.Symbol                                                                                
--Forex Price for Date                                                                        
  Left outer join #FXConversionRates FXDayRates                                               
  On (FXDayRates.FromCurrencyID = G.CurrencyID                                               
  And FXDayRates.ToCurrencyID = @BaseCurrencyID                                               
  And DateDiff(d,@Date,FXDayRates.Date)=0)                                                   
--Security Master DB Join                                                                               
  Left outer join #SecMasterDataTempTable SM ON SM.TickerSymbol = PT.Symbol                      
Where PT.TaxlotOpenQty <> 0           
          
update #TempOutput          
Set  MarketValue = MarketValue / 100          
Where AssetID = 8 and MarketValue <> 0                 
                    
--Select * from #TempOutput order by symbol               
                  
Exec ('Select TOP(' +@TopX + ')                           
  MIN(Symbol) As Symbol                          
 ,MIN(UnderlyingSymbol) As UnderlyingSymbol                          
 ,MIN(CompanyName) As CompanyName                          
 ,Sum(MarketValue) As MarketValue                       
 ,MIN(PositionType) As PositionType                      
from #TempOutput Where PositionType = ' + '''' + @OrderBy + '''' +                        
'Group By '  + @TopBy +                          
' Order By MarketValue DESC')                 
              
--Select * from #TempOutput                 
                  
Drop Table #TempOutput,#TempOutput1,#TempOutput2,#SecMasterDataTempTable      
Drop Table #T_CompanyFunds,#MarkPriceForDate,#FXConversionRates