/*                        
Created By Sandeep Singh                         
Date: Feb 2, 2010                        
[PMGetNetShortMarketValueforPositionActivitySummary]'02-01-2011','02-03-2011','1195,1182,1183,1184,1185,1186'                        
                        
*/                        
                                  
CREATE Procedure [PMGetNetShortMarketValueforPositionActivitySummary]                                  
(                                  
 -- Add the parameters for the stored procedure here                              
 @StartDate datetime ,                           
 @EndDate datetime,                                                                                                                                                       
 @FundID varchar(max)                                
)                     
AS                    
                                 
BEGIN                                                                                                                                                                                          
 -- SET NOCOUNT ON added to prevent extra result sets from                                                                                                                                                                                          
 -- interfering with SELECT statements.                                                                                                                                                                                          
 SET NOCOUNT ON;                             
                            
--Set @StartDate='10-13-2009'                                                                                                                                                                                         
--Set @EndDate='10-16-2009'                         
                    
Declare @Fund Table                                                                                
(                                                                                
 FundID int                                                                            
)                               
                            
Insert into @Fund                              
Select Cast(Items as int) from dbo.Split(@FundID,',')                                                                                                                                                                                      
                                                                                                                                                  
Declare @MinTradeDate DateTime                                                                          
Declare @BaseCurrencyID int                                               
Set @BaseCurrencyID=(select BaseCurrencyID from T_Company)                                                                     
                                                                                             
-- get Mark Price for Start Date                                                                      
Create Table #MarkPriceForStartDate                                                                                                                  
(                                                                               
 Finalmarkprice float ,                                                                                                                       
 Symbol varchar(50)                                                                                    
)                                                                                                                                      
                                                                                                            
-- get Mark Price for End Date                 
Create Table #MarkPriceForEndDate                                                                                     
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
                                                                                         
-- get yesterday business day AUEC wise                                                                                                
Create Table #AUECYesterDates                                                                                                                                                        
(                                      
   AUECID INT,                                                                            
   YESTERDAYBIZDATE DATETIME                                                                                                          
)                                                                                                                         
-- get business day AUEC wise for End Date                                                              
Create Table #AUECBusinessDatesForEndDate                                                                              
(                                                                                             
   AUECID INT,                                                                             
   YESTERDAYBIZDATE DATETIME                                                                                                                                        
)                                                                                                                          
-- get Security Master Data in a Temp Table                                                                     
Create Table #SecMasterDataTempTable                                                                                                                                       
(                                                                                                                                     
  AUECID int,                                                                                                                                                                                                                                                 
  
    
      
        
           
  TickerSymbol Varchar(100),                                                                                                
  CompanyName  VarChar(500),                                                                                                
  AssetName Varchar(100),                          
  SecurityTypeName Varchar(200),                                                                                           
  SectorName Varchar(100),                               
  SubSectorName Varchar(100),                   
  CountryName  Varchar(100),                             
  PutOrCall Varchar(5),                                                                                                
  Multiplier Float,                                                     
  LeadCurrencyID int,                                                                                                
  VsCurrencyID int,                              
  CurrencyID int                                                                                               
)                                                                     
                                                                                
Insert Into #SecMasterDataTempTable                                                                                                
Select                                                            
 AUECID ,                                                                                                                                                  
 TickerSymbol ,                                                                                                                                            
 CompanyName  ,                                                                                                  
 AssetName,                                                                                                
 SecurityTypeName,                                                                                                                                                                                                 
 SectorName ,                                                                                                                                                                                                               
 SubSectorName ,                                                                                                                         
 CountryName ,                                                                                                
 PutOrCall ,                                                                                                
 Multiplier ,                                                                
 LeadCurrencyID ,                                                      
 VsCurrencyID,                              
 CurrencyID                                                                                                 
     From V_SecMasterData                                                                                                
                                                            
Set @MinTradeDate =(Select MIN(PT.AUECModifiedDate) as TradeDate                                                                                                        
 from PM_Taxlots PT  Where Datediff(d,PT.AUECModifiedDate,@EndDate) >=0                                                                
    And TaxlotClosingID_FK is null)                                                                                                                                 
                                                                                                                      
If(DateDiff(d,@StartDate,@MinTradeDate)) > 0                                                                                                                               
 Begin    
  Set @MinTradeDate = @StartDate      
 End                                                                                                                                                    
                      
Insert into #FXConversionRates Exec P_GetAllFXConversionRatesForGivenDateRange @MinTradeDate,@EndDate                                                     
  --Select * from  dbo.GetAllFXConversionRatesForGivenDateRange(@MinTradeDate,@EndDate) as A                                 
                                                       
 Update #FXConversionRates                                                                                        
  Set RateValue = 1.0/RateValue                                                                                                             
  Where RateValue <> 0 and ConversionMethod = 1                                         
                                                                
Update #FXConversionRates                                                                                                                
  Set RateValue = 0                                                                                                                                  
  Where RateValue is Null                
              
Create Table #TempSplitFactorForOpen                
(                
TaxlotID varchar(50),                
Symbol varchar(100),                
SplitFactor float                
)                
                
Insert InTo #TempSplitFactorForOpen                
                
select NA.TaxlotID, NA.Symbol, IsNull(EXP(SUM(LOG(NA.splitFactor))),1) as SplitFactor  from                 
(                
 Select A.Taxlotid,A.symbol, A.StartDate, A.Enddate, VCA.SplitFactor from                
 (                
  Select                 
  TaxlotId,                
  PT.Symbol as Symbol,                
  Case                                                                                     
   When DateDiff(d,@StartDate,G.ProcessDate) >=0                                                                                                                               
   Then G.ProcessDate              
   Else @StartDate              
  End as StartDate,                
  @EndDate  as EndDate                            
   from PM_Taxlots PT                                                  
    Inner Join T_Group G on G.GroupID = PT.GroupID                                                                                                                                                                                  
    Where TaxLotOpenQty <> 0                                                                                     
     and Taxlot_PK in                                                                                                                
     (                                                                                       
   Select max(Taxlot_PK) from PM_Taxlots                                                                                                                                                                                             
   Where DateDiff(d,PM_Taxlots.AUECModifiedDate,@EndDate) >=0                 
   Group by TaxlotId                                                                                                                                                               
     )                
 ) as A                
 Inner Join V_CorpActionData VCA on A.Symbol = VCA.Symbol and                
 Datediff(d,A.StartDate, VCA.Effectivedate) > 0 and Datediff(d, VCA.Effectivedate, A.Enddate) >= 0 and VCA.IsApplied = 1 and VCA.CorpActionTypeID=6                
) as NA                 
Group by NA.TaxlotId,NA.symbol--, NA.StartDate, NA.Enddate order by NA.StartDate                 
                
Create Table #TempSplitFactorForClosed_1                                
(                                
TaxlotID varchar(50),                                
Symbol varchar(100),                                
SplitFactor float,        
CorpActionID varchar(100),      
Effectivedate DateTime                                 
)                                
                
Insert InTo #TempSplitFactorForClosed_1                                
                                
select NA.TaxlotID, NA.Symbol,NA.splitFactor as SplitFactor,NA.CorpActionID,NA.Effectivedate as Effectivedate  from                                 
(                                
 Select A.Taxlotid,A.symbol, A.StartDate, A.Enddate, VCA.SplitFactor,VCA.CorpActionID,VCA.Effectivedate as Effectivedate from                               
 (                                
  Select                                 
  PT.TaxlotId,                                
  PT.Symbol as Symbol,                                
  Case                                                                                                   
   When DateDiff(d,@StartDate,G.ProcessDate) >=0                                                                                                                     
   Then G.ProcessDate                                 
   Else @StartDate                                
  End as StartDate,                                
  PTC.AUECLocalDate  as EndDate                                 
                                                                                      
  from PM_TaxlotClosing  PTC                                                                         
     Inner Join PM_Taxlots PT on ( PTC.PositionalTaxlotID=PT.TaxlotID  and  PTC.TaxLotClosingId = PT.TaxLotClosingId_Fk)         
     Inner Join T_Group G on G.GroupID = PT.GroupID                                 
     Where DateDiff(d,@StartDate,PTC.AUECLocalDate) >=0                                                                                                                                               
      and DateDiff(d,PTC.AUECLocalDate,@EndDate)>=0                                                                                                                                 
      and PTC.ClosingMode<>7                                
 ) as A                                
 Inner Join V_CorpActionData VCA on A.Symbol = VCA.Symbol and                                
 Datediff(d,A.StartDate, VCA.Effectivedate) > 0 and Datediff(d, VCA.Effectivedate, A.Enddate) >= 0 and VCA.IsApplied = 1 and VCA.CorpActionTypeID=6                                
) as NA                                 
--Group by NA.TaxlotID,NA.symbol--, NA.StartDate, NA.Enddate order by NA.StartDate              
        
Select Distinct TaxlotID, CorpActionID,SplitFactor,Symbol,Effectivedate into #TempSplitFactorForClosed_2  from #TempSplitFactorForClosed_1        
        
Create Table #TempSplitFactorForClosed                       
(            
TaxlotID varchar(50),                        
Symbol varchar(100),                        
SplitFactor float,      
Effectivedate DateTime         
)        
        
Insert into #TempSplitFactorForClosed        
Select         
NA.TaxlotID,         
NA.Symbol,         
IsNull(EXP(SUM(LOG(NA.splitFactor))),1) as SplitFactor,      
Max(Effectivedate) as Effectivedate           
from #TempSplitFactorForClosed_2 NA Group by NA.TaxlotID,NA.symbol   
                         
--code for international future                                                           
Declare @SelectedSymbol table                                                
(                              
 FutSymbol varchar(100)                              
)                                       
Insert InTo @SelectedSymbol                                                          
Select                               
TickerSymbol                              
From #SecMasterDataTempTable SM                                                                          
Inner Join T_AUEC on SM.AUECID=T_AUEC.AUECID                                                                          
Where T_AUEC.AssetID=3 and SM.CurrencyID<>@BaseCurrencyID                                                           
                                                          
CREATE TABLE [dbo].#TempMP                                                                                     
(                                         
 Symbol varchar(200) ,                                                          
 Date datetime,                                                          
 Markprice float,                                                          
 CF float                                                             
)                                                                                                                   
                                                                                            
INSERT INTO #TempMP                                                                                             
(                                                                
Symbol,                                                                    
Date,                                  
MarkPrice ,                                                                    
CF                                                                
)                                                                    
Select                                   
P.Symbol,                                             
P.Date,                                            
Isnull(PMDMP.FinalMarkPrice,0),                                            
Isnull(FXDayRatesForTradeDate.RateValue,0)                                            
from                                                                    
(Select FutTab.FutSymbol as Symbol,Items as Date,T_Group.CurrencyID                                            
 from @SelectedSymbol FutTab                                                                         
 Cross Join dbo.GetDateRange(DATEADD(day,-3,@StartDate), @EndDate)                            
 Inner Join T_Group on FutTab.FutSymbol=T_Group.Symbol                                                                   
 Inner Join PM_Taxlots on PM_Taxlots.GroupID = T_Group.GroupID                                                                    
 Left Outer Join PM_TaxlotClosing on PM_TaxlotClosing.TaxlotClosingID = PM_Taxlots.TaxlotClosingID_Fk                                                                    
 Where DateDiff(day,T_Group.ProcessDate,@EndDate) >= 0                                                                    
  And (DateDiff(day,@StartDate,PM_TaxlotClosing.AuecLocalDate) >= 0 OR PM_TaxlotClosing.TaxlotClosingID is null)) As p                    
  left outer join PM_DayMarkPRice PMDMP                                             
    On (PMDMP.Symbol=P.Symbol and Datediff(d,PMDMP.Date,P.Date)=0)                     
  Left outer join #FXConversionRates FXDayRatesForTradeDate                                                                                                             
    On (FXDayRatesForTradeDate.FromCurrencyID = P.CurrencyID                                                                                               
  And FXDayRatesForTradeDate.ToCurrencyID = @BaseCurrencyID                                                                                                             
  And DateDiff(d,P.Date,FXDayRatesForTradeDate.Date)=0)                                
                              
--to adjust holidays mark price and CF                                         
Update #TempMP                                                              
Set Markprice = S.MarkPrice,                                         
 CF=S.CF                                                              
From                                                              
(                                            
 Select                                                                
 T1.Symbol,                                            
 dbo.AdjustBusinessDays(DateAdd(day,1,T1.date),-1,SM.AUECID) as LastDate ,                                            
 T1.Date as Date ,                                            
 T2.MarkPrice as MarkPrice,                                            
 T2.CF                                                     
 From #TempMP T1                                                              
 Inner Join #SecMasterDataTempTable SM on SM.TickerSymbol = T1.Symbol                                                              
 Inner Join #TempMP T2 on T1.Symbol=T2.Symbol                                                               
 Where T1.Date <> dbo.AdjustBusinessDays(DateAdd(day,1,T1.Date),-1,SM.AUECID)                                                          
 and T2.date= dbo.AdjustBusinessDays(DateAdd(day,1, T1.date),-1,SM.AUECID)                                                              
) As S                                                               
  Where S.symbol=#tempMP.Symbol and S.date=#tempMP.date                                                      
                                                          
Create Table #Temp2                                                          
(                                                          
 TaxlotId varchar(100),                                                          
 TradeDate datetime,        
 AvgPrice float,                                                          
 Symbol varchar(100),                                                          
 TaxLotOpenQty float,                                                          
 FundID int,                                                          
 CommissionandFees float,                                                          
 Level2ID int,                                                          
 OrderSideTagValue varchar(1),                              
 FXRate float,                                            
 FXConversionMethodOperator  Varchar(5)                                                     
)                                                          
Insert Into #Temp2                                                          
(                  
 TaxlotId ,                                                          
 TradeDate,                                                          
 AvgPrice,                                                          
 Symbol,                                                          
 TaxLotOpenQty,                                                          
 FundID ,                                                          
 CommissionandFees ,                                         
 Level2ID ,                                                          
 OrderSideTagValue ,                              
 FXRate ,                                            
 FXConversionMethodOperator                                                 
)                                                          
                                                          
Select                                                           
 TaxlotID,                        
 G.ProcessDate,                                                          
 PT.AvgPrice,                                                          
 PT.Symbol,                                                          
 TaxLotOpenQty ,                                                          
 FundID,                                                         
 PT.OpenTotalCommissionandFees,                                                          
 PT.Level2ID,                                                          
 PT.OrderSideTagValue,                              
 G.FXRate,                                            
 G.FXConversionMethodOperator                                                  
 From PM_taxlots PT                                                          
 Inner Join T_Group G                               
  On G.GroupID=PT.GroupID                                                 
 Where  TaxLotOpenQty<>0                                                            
        And Taxlot_PK in                                                                                                                                                                                                                     
      (                                   
     Select Max(Taxlot_PK) from PM_Taxlots                                                               
     Inner join T_Group on T_Group.GroupID=PM_Taxlots.GroupID                                                               
     Inner join @SelectedSymbol SSymbol on SSymbol.FutSymbol= T_Group.Symbol                                                                                                                                                                                  
  
    
      
        
          
            
              
     Where DateDiff(d,PM_Taxlots.AUECModifiedDate,@EndDate) >=0                                                           
     Group By Taxlotid                                                                                                
     )                                                           
                                           
 -- cross between mark price and Positions                                                     
Create table #Temp3                                             
(                                                          
TaxlotId varchar(100),                                                          
TradeDate datetime,                                                          
AvgPrice float,                                               
Symbol varchar(100),                                                          
Markprice float,                                                        
CF float,                                                          
MPDate datetime,                                                   
TaxLotOpenQty float ,                                                          
FundID int,                                                          
OpenCommissionandFees float,                                                 
Level2ID int,                                                          
OrderSideTagValue varchar(5),                              
FXRate float,                                            
FXConversionMethodOperator  Varchar(5)                                           
)                                                          
                                                          
Insert Into #Temp3                                                                          
(                                                                          
TaxlotId ,                                                                          
TradeDate,                                                    
AvgPrice,                                                                          
Symbol,                                                                 
markprice,                                                                          
CF,                                                                         
MPDate,                                                                          
TaxLotOpenQty,                                                                          
FundID ,                                                                          
OpenCommissionandFees,                                                                    
--ClosedCommissionandFees,                                                                          
Level2ID ,                                                                          
OrderSideTagValue,                                            
FXRate ,                                            
FXConversionMethodOperator                                                               
)                                                                 
                                                  
Select Distinct                                                                           
TaxlotID,                                                                          
Tradedate,                                                                          
AvgPrice,                                                                          
#temp2.Symbol,                                                                          
MarkPrice,                                                                          
CF,                                                                          
#TempMP.date,                                                     
TaxLotOpenQty,                                                                          
FundID ,                                                        
CommissionandFees ,                               
--0,--0 for ClosedComissionAndFees                                                    
Level2ID ,                                                                          
OrderSideTagValue,                                            
FXRate ,                                   
FXConversionMethodOperator                                                                          
From #Temp2                                                                           
Cross Join #TempMP              
where DateDiff(day,#Temp2.TradeDate,#TempMP.Date) >=0 -- dbo.getformatteddatepart(#temp2.tradeDate)<=dbo.getformatteddatepart(#TempMP.date)                                                                          
      And #Temp2.Symbol = #TempMP.Symbol                                                        
                                                          
Union                                                          
                                                         
Select Distinct                                                                          
TaxlotID,                                                                          
T_Group.ProcessDate as Tradedate ,                                                                
PM_taxlots.AvgPrice as AvgPrice,                                                    
T_Group.Symbol,                                                    
#TempMP.MarkPrice,                                                                          
CF,                                                                          
#TempMP.Date,                                                                          
PM_taxlots.TaxLotOpenQty,                        
PM_taxlots.FundID ,                                                                          
PM_taxlots.OpenTotalCommissionandFees,                                
--0,--0 for ClosedComissionAndFees                                                                         
PM_taxlots.Level2ID ,                                                        
PM_taxlots.OrderSideTagValue ,                                            
T_Group.FXRate,                                            
T_Group.FXConversionMethodOperator                                                                           
From PM_taxlots                   
Cross join #TempMP                                                                            
Inner join PM_taxlotClosing on PM_taxlotClosing.TaxlotClosingID = PM_Taxlots.TaxlotClosingID_FK                                                                          
Inner join T_Group on T_Group.GroupID=PM_taxlots.GroupID                                                               
Inner Join @SelectedSymbol SSymbol on SSymbol.FutSymbol=T_Group.Symbol                                                                          
Where DateDiff(day,T_Group.ProcessDate,#TempMP.Date) >= 0                            
 And T_Group.Symbol=#TempMP.Symbol                                                          
 And DateDiff(day,#TempMP.Date,PM_TaxlotClosing.AUECLocalDate) >= 0                             
 And DateDiff(day,@StartDate,PM_TaxlotClosing.AUECLocalDate) >= 0                                             
 And DateDiff(day,PM_taxlotClosing.AUECLocalDate,@EndDate) >= 0                                                                          
 Order by TaxlotId,Date                                                        
--for modifying quantity for previous date                                                                         
Update #Temp3                                                                          
Set #Temp3.TaxlotopenQty = PM_taxlots.TaxlotopenQty                                
, #Temp3.OpenCommissionandFees =PM_taxlots.OpenTotalCommissionandFees                                                                      
From PM_Taxlots                                                                       
Inner Join #Temp3 On PM_taxlots.TaxlotID=#temp3.TaxlotID                                                            
Where Taxlot_pk in                                                                       
(Select Max(Taxlot_pk) from PM_taxlots                                                  
 Where Datediff(d,#Temp3.MPdate,PM_Taxlots.AUECModifiedDate) < 0                                                                   
 And TaxlotID=#Temp3.TaxlotID)                                                            
                                                                     
Update #Temp3                                                                      
Set #Temp3.TaxlotopenQty = PM_Taxlots.TaxlotopenQty                                 
, #Temp3.OpenCommissionandFees =PM_taxlots.OpenTotalCommissionandFees                                             
from PM_Taxlots                                                                       
Inner join #Temp3                                                                      
on PM_taxlots.TaxlotID=#Temp3.TaxlotID                                                                      
Where Datediff(day,#Temp3.MPdate,PM_Taxlots.AUECModifiedDate)=0                                                                   
and Datediff(day,#Temp3.MPdate,#Temp3.TradeDate)= 0                                  
                        
 INSERT INTO #MarkPriceForStartDate Exec P_GetMarkPriceForBusinessDay @StartDate               
-- Select * From dbo.GetMarkPriceForBusinessDay(@StartDate)                                                                    
                                                                          
 Declare @MarkEndDate DateTime                            
 Set @MarkEndDate=DateAdd(d,1,@EndDate)                                                                 
  INSERT INTO #MarkPriceForEndDate Exec P_GetMarkPriceForBusinessDay @MarkEndDate               
--Select * From dbo.GetMarkPriceForBusinessDay(DateAdd(d,1,@EndDate))                                                                                                                                 
                                                                                                        
-- Yesterday business date                                                                                                
   INSERT INTO #AUECYesterDates                                                                                                 
     Select Distinct V_SymbolAUEC.AUECID, dbo.AdjustBusinessDays(@StartDate,-1, V_SymbolAUEC.AUECID)                                          
     from V_SymbolAUEC                                                               
                                         
   INSERT INTO #AUECBusinessDatesForEndDate                                                                                                                                                                  
     Select Distinct V_SymbolAUEC.AUECID, dbo.AdjustBusinessDays(DateAdd(d,1,@EndDate),-1, V_SymbolAUEC.AUECID)                                                                                                                  
from V_SymbolAUEC                               
                         
Create Table #TempOutput                       
(                    
Symbol varchar(100),                    
TotalOpenCommission float,                    
TotalClosedCommission float,                    
TradeDate DateTime,                    
MarketValue1 float,                    
MarketValue2 float,                     
PositionFrom varchar(5),                    
Side Varchar(10),                    
RealizedPNL float,                    
UnRealizedPNL float                    
)                    
                    
Insert Into #TempOutput                                                                   
                            
 Select                                                                                                                                   
  PT.Symbol    as Symbol,                                        
  Case                                                                                                                                                                                       
   When G.AssetID=5 OR G.AssetID=11 -- 11 for FX Forward  and 5 for FX Spot                                                                                                       
   Then                                                                                                    
     Case @BaseCurrencyID                  
   When SM.VsCurrencyID                                              
   Then IsNull(PT.OpenTotalCommissionAndFees,0)                                               
   Else                                                                                                   
  Case                                                                                                                                        
   When (G.FXRate > 0 And G.FXConversionMethodOperator='M') And (@BaseCurrencyID = SM.VsCurrencyID OR @BaseCurrencyID = SM.LeadCurrencyID)                                                                                  
   Then IsNull(PT.OpenTotalCommissionAndFees * G.FXRate,0)                                  
   When G.FXRate > 0 And G.FXConversionMethodOperator='D' And (@BaseCurrencyID = SM.VsCurrencyID OR @BaseCurrencyID = SM.LeadCurrencyID)                                                                                                
   Then IsNull(PT.OpenTotalCommissionAndFees * 1/G.FXRate,0)                                                                                                    
   Else IsNull(PT.OpenTotalCommissionAndFees * IsNull(FXDayRatesForTradeDateForForex.RateValue,0),0)                                                          
  End                                                                                                    
     End                                                                                                    
 Else                                    
    Case                                                                                                                         
   When G.CurrencyID =  @BaseCurrencyID   --When Company and Traded Currency both are same                                                                                                                      
   Then IsNull(PT.OpenTotalCommissionAndFees,0)                                                                                                                                                      
   Else  --When Company and Traded Currency are different                                                                            
   Case                                             
      When G.FXRate > 0 And G.FXConversionMethodOperator='M'                                                       
      Then IsNull(PT.OpenTotalCommissionAndFees * G.FXRate,0)                                                                                                                                           
      When G.FXRate > 0 And G.FXConversionMethodOperator='D'                                                                                                          
      Then IsNull(PT.OpenTotalCommissionAndFees * 1/G.FXRate,0)                                                                                                  
      When G.FXRate <= 0 OR G.FXRate is null                                           
      Then  IsNull(PT.OpenTotalCommissionAndFees * IsNull(FXDayRatesForTradeDate.RateValue,0),0)                                                            
    End                                                                                                                          
    End                                                                                                                                             
 End as TotalOpenCommission,                                                                                                                               
                                
 0 as TotalClosedCommission,                                                                                        
 G.ProcessDate   as TradeDate,                                                                                                                                                          
                                              
Case                                                                                      
When G.AssetID=5 OR G.AssetID=11 -- 11 for FX Forward  and 5 for FX Spot                                                                                                      
Then                                  
 Case @BaseCurrencyID                                                                                                     
 When SM.VsCurrencyID                                                                                                     
 Then                                                                                                     
  Case                                                                                                    
   When DateDiff(d,@StartDate,G.ProcessDate) >=0                                                                                                            
   Then IsNull(PT.AvgPrice * TaxlotOpenQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PT.OrderSideTagValue),0)                                                                                                                                         
  
    
      
        
          
            
             
   Else IsNull(FXMarkPriceforStartDate.RateValue * TaxlotOpenQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PT.OrderSideTagValue),0)                                                                                                  
  End                                     
 Else                                                                                                    
  Case                                                                                               
   When DateDiff(d,@StartDate,G.ProcessDate) >=0                                                                                                                          
   Then                                                                                                   
    Case                                                 
     When G.FXRate > 0 And G.FXConversionMethodOperator='M' And (@BaseCurrencyID = SM.VsCurrencyID OR @BaseCurrencyID = SM.LeadCurrencyID)                                                                                                   
     Then IsNull(PT.AvgPrice * G.FXRate * TaxlotOpenQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PT.OrderSideTagValue),0)                                                                                                                             
  
    
     
         
          
            
             
     When G.FXRate > 0 And G.FXConversionMethodOperator='D' And (@BaseCurrencyID = SM.VsCurrencyID OR @BaseCurrencyID = SM.LeadCurrencyID)                                                                                         
     Then IsNull(PT.AvgPrice * 1/G.FXRate * TaxlotOpenQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PT.OrderSideTagValue),0)                                                                                                 
     Else IsNull(PT.AvgPrice * IsNull(FXDayRatesForTradeDateForForex.RateValue,0) * TaxlotOpenQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PT.OrderSideTagValue),0)                                                                                   
  
    
      
        
          
            
             
    End                                                                                                  
   Else IsNull(FXMarkPriceforStartDate.RateValue * TaxlotOpenQty * FXDayRatesForStartDateForForex.RateValue * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PT.OrderSideTagValue),0)                                                                         
  
    
      
        
          
            
              
  End                                                                                                                                      
 End                                         
Else   -- if Not Forex Trade                                                                                   
 Case                                                                                                                                           
 When G.CurrencyID =  @BaseCurrencyID                                                                                                                                                                        
 Then                                                                                                                                                   
  Case                                                                  
   When DateDiff(d,@StartDate,G.ProcessDate) >=0                                                                                                                               
   Then IsNull((G.AvgPrice / IsNull(SplitTab.SplitFactor,1)) * TaxlotOpenQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PT.OrderSideTagValue),0)                                                                                                        
 
     
      
        
          
           
                                          
   Else IsNull((IsNull(MPS.Finalmarkprice,0) / IsNull(SplitTab.SplitFactor,1))* TaxlotOpenQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PT.OrderSideTagValue),0)                                                                      
  End                                                                                                                              
 Else                                                                                                                                                                   
  Case                                                                                 
  When DateDiff(d,@StartDate,G.ProcessDate) >=0                 
  Then                                                                                                                                          
   Case                                                                                                                          
    When G.FXRate > 0 And G.FXConversionMethodOperator='M'                                                                                                                                           
    Then IsNull((G.AvgPrice / IsNull(SplitTab.SplitFactor,1)) * G.FXRate * TaxlotOpenQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PT.OrderSideTagValue),0)                                                                                            
  
    
     
         
          
            
                            
    When G.FXRate > 0 And G.FXConversionMethodOperator='D'                                                               
    Then IsNull(((G.AvgPrice / IsNull(SplitTab.SplitFactor,1)) * 1/G.FXRate) * TaxlotOpenQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PT.OrderSideTagValue),0)                                                                       
    When G.FXRate <= 0 OR G.FXRate is null                                                                                                                                                                    
    Then IsNull((G.AvgPrice / IsNull(SplitTab.SplitFactor,1))* IsNull(FXDayRatesForTradeDate.RateValue,0) * TaxlotOpenQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PT.OrderSideTagValue),0)                                                           
  
    
      
       
           
          
                                             
   End                                
  Else IsNull((IsNull(MPS.Finalmarkprice,0) / IsNull(SplitTab.SplitFactor,1)) * FXDayRatesForStartDate.RateValue * TaxlotOpenQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PT.OrderSideTagValue),0)                                                    
  
    
      
        
         
             
                                               
  End                                                                                                                                                              
 End                                  
End  as MarketValue1,                    
                  
Case                                                       
When G.AssetID=5 OR G.AssetID=11 -- 11 for FX Forward  and 5 for FX Spot                                                                                                  
Then                                                                                                    
 Case @BaseCurrencyID                                                                                                   
  When SM.VsCurrencyID                                                                                                     
  Then IsNull(FXMarkPriceforEndDate.RateValue * TaxlotOpenQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PT.OrderSideTagValue),0)                                                                                                    
  Else IsNull(FXMarkPriceforEndDate.RateValue * TaxlotOpenQty * FXDayRatesForEndDateForForex.RateValue * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PT.OrderSideTagValue),0)                                       
 End                                                               
Else                                                                                                    
 Case                                                                                                                                    
  When G.CurrencyID =  @BaseCurrencyID                                                                                                                                                                        
  Then IsNull(IsNull(MPE.Finalmarkprice,0) * TaxlotOpenQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PT.OrderSideTagValue),0)                                                                                                                          
 
     
      
        
          
            
             
Else IsNull(IsNull(MPE.Finalmarkprice,0) * FXDayRatesForEndDate.RateValue * TaxlotOpenQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PT.OrderSideTagValue),0)                                                                 
 End                                                   
End as MarketValue2,                            
                                                
Case                                               
 When G.AssetID=3                             
 Then 'I'                                                                              
 Else 'O'                                                    
End as PositionFrom,                     
dbo.GetLongShortBySide(PT.OrderSideTagValue) as Side,                    
0 as RealizedPNL,                    
0 as UnRealizedPNL                                              
                    
--Case dbo.GetLongShortBySide(PT.OrderSideTagValue)                                                                                                                         
--  When  1                                                                                                                                                            
--  Then  'Long'                                                                                                                                                                                          
--  Else  'Short'                                                                           
--End as Side                                                                
                                                                                                                                                                                                            
 from PM_Taxlots PT                                                   
  Left Outer Join #MarkPriceForStartDate MPS on MPS.Symbol=PT.Symbol                                                                                                                                                                          
  Left Outer Join #MarkPriceForEndDate MPE on MPE.Symbol=PT.Symbol                                                                                      
  Inner Join T_Group G on G.GroupID = PT.GroupID                                                                                                                                                                                  
  Left outer join  T_SwapParameters SW on G.GroupID=SW.GroupID                                                                                                  
  Left Outer Join T_CompanyFunds ON  PT.FundID= T_CompanyFunds.CompanyFundID                                                                                                   
  -- join to get yesterday business day                                                                                              
  LEFT OUTER JOIN #AUECYesterDates AUECYesterDates ON G.AUECID = AUECYesterDates.AUECID                                                                                                
  LEFT OUTER JOIN #AUECBusinessDatesForEndDate AUECBusinessDatesForEndDate ON G.AUECID = AUECBusinessDatesForEndDate.AUECID                                          
  -- Forex Price for Trade Date other than FX Trade                                
  Left outer join #FXConversionRates FXDayRatesForTradeDate                                                                                               
 on (FXDayRatesForTradeDate.FromCurrencyID = G.CurrencyID                                                                                               
 And FXDayRatesForTradeDate.ToCurrencyID = @BaseCurrencyID                                                                                               
 And DateDiff(d,G.ProcessDate,FXDayRatesForTradeDate.Date)=0)                                                                                                   
  -- Forex Price for Start Date other than FX Trade                             
 Left outer join #FXConversionRates FXDayRatesForStartDate                                                                                               
 on (FXDayRatesForStartDate.FromCurrencyID = G.CurrencyID                                                                                               
 And FXDayRatesForStartDate.ToCurrencyID = @BaseCurrencyID                                                                                               
 And DateDiff(d,AUECYesterDates.YESTERDAYBIZDATE,FXDayRatesForStartDate.Date)=0)                                              
-- Forex Price for End Date other than FX Trade                                                                                         
  Left outer join #FXConversionRates FXDayRatesForEndDate                                                                                              
 on (FXDayRatesForEndDate.FromCurrencyID = G.CurrencyID                                                                                               
 And FXDayRatesForEndDate.ToCurrencyID = @BaseCurrencyID                                                                                               
 And DateDiff(d,AUECBusinessDatesForEndDate.YESTERDAYBIZDATE,FXDayRatesForEndDate.Date)=0)           
-- Security Master DB Join                                                                                                                               
  Left outer join #SecMasterDataTempTable SM ON SM.TickerSymbol = PT.Symbol                                                                        
-- Mark Price for Start Date for FX Trade                                                                                                  
  Left outer  join #FXConversionRates FXMarkPriceforStartDate                                                                
 on (FXMarkPriceforStartDate.FromCurrencyID = SM.LeadCurrencyID                                                                                               
 And FXMarkPriceforStartDate.ToCurrencyID = SM.VsCurrencyID                                              
 And DateDiff(d,AUECYesterDates.YESTERDAYBIZDATE,FXMarkPriceforStartDate.Date)=0)                                                                                                   
-- Mark Price for End Date for FX Trade                                                                                                  
  Left outer  join #FXConversionRates FXMarkPriceforEndDate                                                                 
 on (FXMarkPriceforEndDate.FromCurrencyID = SM.LeadCurrencyID                                                                                               
 And FXMarkPriceforEndDate.ToCurrencyID = SM.VsCurrencyID                                                                                               
 And DateDiff(d,AUECBusinessDatesForEndDate.YESTERDAYBIZDATE,FXMarkPriceforEndDate.Date)=0)                                                                                                              
--Forex Price for Trade Date for FX Trade         
  Left outer  join #FXConversionRates FXDayRatesForTradeDateForForex                                                                                               
 on (FXDayRatesForTradeDateForForex.FromCurrencyID = SM.VsCurrencyID                     
 And FXDayRatesForTradeDateForForex.ToCurrencyID = @BaseCurrencyID                                                                                               
 And DateDiff(d,G.ProcessDate,FXDayRatesForTradeDateForForex.Date)=0)                                                                                                 
--Forex Price for Start Date for FX Trade                                                                                     
  Left outer  join #FXConversionRates FXDayRatesForStartDateForForex                          
 on (FXDayRatesForStartDateForForex.FromCurrencyID = SM.VsCurrencyID                                                                                       
 And FXDayRatesForStartDateForForex.ToCurrencyID = @BaseCurrencyID                           
 And DateDiff(d,AUECYesterDates.YESTERDAYBIZDATE,FXDayRatesForStartDateForForex.Date)=0)                                       
--Forex Price for End Date for FX Trade                                                                                                             
  Left outer  join #FXConversionRates FXDayRatesForEndDateForForex                                                                                               
 on (FXDayRatesForEndDateForForex.FromCurrencyID = SM.VsCurrencyID                                                                                               
 And FXDayRatesForEndDateForForex.ToCurrencyID = @BaseCurrencyID                                            
 And DateDiff(d,AUECBusinessDatesForEndDate.YESTERDAYBIZDATE,FXDayRatesForEndDateForForex.Date)=0)                                                                                                          
                                                                                                  
  Left Outer Join T_CompanyStrategy as CompanyStrategy On CompanyStrategy.CompanyStrategyID=PT.Level2ID                                                                                 
  Left Outer Join T_Asset On T_Asset.AssetId=G.AssetID                                                                                                                                      
  LEFT OUTER JOIN T_CompanyMasterFundSubAccountAssociation CMFSSAA ON T_CompanyFunds.CompanyFundID = CMFSSAA.CompanyFundID                                                                                          
  LEFT OUTER JOIN T_CompanyMasterFunds CMF ON CMFSSAA.CompanyMasterFundID = CMF.CompanyMasterFundID                                                 
                                                
  LEFT OUTER JOIN T_CompanyMasterStrategySubAccountAssociation CMSSSAA ON CompanyStrategy.CompanyStrategyID = CMSSSAA.CompanyStrategyID                                                                                          
  LEFT OUTER JOIN T_CompanyMasterStrategy CMS ON CMSSSAA.CompanyMasterStrategyID = CMS.CompanyMasterStrategyID                                                  
                                                                                                                                         
  Inner Join T_AUEC AUEC on AUEC.AUECID = G.AUECID                                                           
  LEFT OUTER join @SelectedSymbol SSymbol on SSymbol.FutSymbol= G.Symbol              
  Left Outer Join #TempSplitFactorForOpen SplitTab on SplitTab.TaxlotID=PT.TaxlotID              
                                                              
  where TaxLotOpenQty<>0 and PT.FundID in (select FundID from @Fund)  and   SSymbol.FutSymbol is null                                                                                        
   and taxlot_PK in                                                                                                                
   (               
    select max(Taxlot_PK) from PM_Taxlots                                                                                                                                                                                                               
    where DateDiff(d,PM_Taxlots.AUECModifiedDate,@EndDate) >=0                                                                                                                          
    group by taxlotid                                                                                                            
  )                                                                                                                                                            
                                                    
                                                                                                                          
-- Union All               
Insert Into #TempOutput                                                     
                                                                                           
 Select                                                                                                                                                                               
  PT.Symbol as Symbol,                                                                                                                                  
   --Open Commission                                                                                                       
 Case            
    When G.AssetID=5 OR G.AssetID=11 -- 11 for FX Forward  and 5 for FX Spot                                                                                              
    Then                                                                                                    
    Case @BaseCurrencyID                                                                                                     
     When SM.VsCurrencyID                                                                                                     
     Then IsNull(PT.ClosedTotalCommissionandFees,0)                                                                   
     Else                                                                                                  
      Case                                                                                                                                        
       When G.FXRate > 0 And G.FXConversionMethodOperator='M' And (@BaseCurrencyID = SM.VsCurrencyID OR @BaseCurrencyID = SM.LeadCurrencyID)                                                    
       Then IsNull(PT.ClosedTotalCommissionandFees * G.FXRate,0)                                                                                          
       When G.FXRate > 0 And G.FXConversionMethodOperator='D' And (@BaseCurrencyID = SM.VsCurrencyID OR @BaseCurrencyID = SM.LeadCurrencyID)                                                                              
     Then IsNull(PT.ClosedTotalCommissionandFees * 1/G.FXRate,0)                                                        
       Else IsNull(PT.ClosedTotalCommissionandFees * IsNull(FXDayRatesForTradeDateForForex.RateValue,0),0)                                                                                                          
      End                                                                                                       
    End                                                                                         
 Else                                                                               
   Case                                                                                                                                                                         
      When G.CurrencyID =  @BaseCurrencyID   --When Company and Traded Currency both are same                                                                                                                                                                 
  
    
   Then IsNull(PT.ClosedTotalCommissionandFees,0)                                                                             
      Else  --When Company and Traded Currency are different                                     
      Case                                                                         
      When G.FXRate > 0 And G.FXConversionMethodOperator='M'                                           
      Then IsNull(PT.ClosedTotalCommissionandFees * G.FXRate,0)                                                                                                                                  
      When G.FXRate > 0 And G.FXConversionMethodOperator='D'                                                                                                          
      Then IsNull(PT.ClosedTotalCommissionandFees * 1/G.FXRate,0)                                                                                                                                           
      When G.FXRate <= 0 OR G.FXRate is null                                                                                                                                            
      Then  IsNull(PT.ClosedTotalCommissionandFees * IsNull(FXDayRatesForTradeDate.RateValue,0),0)                                       
      End                                                                                                      
  End                                                                                                             
 End as TotalOpenCommission,                                                        
 --Closed Commission                                   
  Case                                                                                                                                                                                     
     When G.AssetID=5 OR G.AssetID=11 -- 11 for FX Forward  and 5 for FX Spot                                                                                                      
     Then                                                                                                    
     Case @BaseCurrencyID                                                                                               
     When SM.VsCurrencyID                                                                                                 
     Then IsNull(PT1.ClosedTotalCommissionandFees,0)                                                                                        
     Else  --When Company and Traded Currency are different                                           
     Case                           
      When G1.FXRate > 0 And G1.FXConversionMethodOperator='M' And (@BaseCurrencyID = SM.VsCurrencyID OR @BaseCurrencyID = SM.LeadCurrencyID)                                                                                                       
      Then IsNull(PT1.ClosedTotalCommissionandFees * G1.FXRate,0)                                                                                                                                           
      When G1.FXRate > 0 And G1.FXConversionMethodOperator='D' And (@BaseCurrencyID = SM.VsCurrencyID OR @BaseCurrencyID = SM.LeadCurrencyID)                                                                                  
      Then IsNull(PT1.ClosedTotalCommissionandFees * 1/G1.FXRate,0)                                                                                   
      Else IsNull(PT1.ClosedTotalCommissionandFees * IsNull(FXDayRatesForClosingDateForForex.RateValue,0),0)                                                                                                          
     End                                                                                                    
   End                                                           
 Else                                                                                                                                              
 Case                                                                  
       When G.CurrencyID =  @BaseCurrencyID   --When Company and Traded Currency both are same                                                                                                             
       Then IsNull(PT1.ClosedTotalCommissionandFees,0)                                                                                 
       Else  --When Company and Traded Currency are different                                                                                                          
   Case                 
     When G1.FXRate > 0 And G1.FXConversionMethodOperator='M'                                                                                      
     Then IsNull(PT1.ClosedTotalCommissionandFees * G1.FXRate,0)                                                                                                         
     When G1.FXRate > 0 And G1.FXConversionMethodOperator='D'                                                                                                          
     Then IsNull(PT1.ClosedTotalCommissionandFees * 1/G1.FXRate,0)                                                                                                          
     When G1.FXRate <= 0 OR G1.FXRate is null                                                                                              
     Then  IsNull(PT1.ClosedTotalCommissionandFees * IsNull(FXDayRatesForClosingDate.RateValue,0),0)                                 
   End                                                                                                                 
    --   Isnull(PT1.ClosedTotalCommissionandFees * FXDayRatesForClosingDate.RateValue,0)                                                                                                   
  End                                                                                                                                
 End as TotalClosedCommission,                                                                               
                                                                                                                                                              
  G.ProcessDate   as TradeDate,                                                                     
                            
Case                                                                                             
When G.AssetID=5 OR G.AssetID=11 -- 11 for FX Forward  and 5 for FX Spot                                                                                                  
Then                                                                                                  
Case @BaseCurrencyID                                                                                                  
When SM.VsCurrencyID            
Then                                                                                                   
 Case                                                        
  When DateDiff(d,G.ProcessDate,@StartDate) > 0                                                                                
  Then IsNull(FXMarkPriceforStartDate.RateValue,0) * PTC.ClosedQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PT.OrderSideTagValue)                                                                                                                    
  
    
      
        
          
           
  Else PT.AvgPrice * PTC.ClosedQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PT.OrderSideTagValue)                                
  End                                                                    
  Else                                                                                                   
   Case                                                                                                                                                                                           
    When  DateDiff(d,G.ProcessDate,@StartDate) > 0                                                   
    Then IsNull(FXMarkPriceforStartDate.RateValue,0) * FXDayRatesForStartDateForForex.RateValue * PTC.ClosedQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PT.OrderSideTagValue)                                                                        
  
   
       
        
          
    Else                                                                                                                                          
     Case                      
      When G.FXRate > 0 And G.FXConversionMethodOperator='M' And (@BaseCurrencyID = SM.VsCurrencyID OR @BaseCurrencyID = SM.LeadCurrencyID)                                                               
      Then IsNull(PT.AvgPrice * G.FXRate * PTC.ClosedQty  * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PT.OrderSideTagValue),0)                                                                                            
          
           
               
      When G.FXRate > 0 And G.FXConversionMethodOperator='D' And (@BaseCurrencyID = SM.VsCurrencyID OR @BaseCurrencyID = SM.LeadCurrencyID)                                                                                                                    
  
    
     
         
          
            
               
      Then IsNull((PT.AvgPrice * 1/G.FXRate) * PTC.ClosedQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PT.OrderSideTagValue),0)                           
      Else IsNull(PT.AvgPrice * FXDayRatesForTradeDateForForex.RateValue * PTC.ClosedQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PT.OrderSideTagValue),0)                                                                                            
  
    
      
        
          
            
               
     End                                                                
   End                                                                                                   
 End                                                                       
Else  --- For Normal Trade                                                                                                     
 Case                                                                         
 When G.CurrencyID =  @BaseCurrencyID                                                                                                   
 Then                                                                
  Case                                                                                                                         
   When DateDiff(d,G.ProcessDate,@StartDate) >0                                                  
   Then (IsNull(MPS.FinalMarkPrice,0) / IsNull(SplitTab.SplitFactor,1)) * PTC.ClosedQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PT.OrderSideTagValue)                            
                                                 
   Else (G.AvgPrice / IsNull(SplitTab.SplitFactor,1)) * PTC.ClosedQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PT.OrderSideTagValue)                                                                                                                 
   
    
      
       
          
             
                                                
  End                                                                                                
 Else                                                                                              
  Case                                                                                                                                                                                           
   When  DateDiff(d,G.ProcessDate,@StartDate) > 0                                             
   Then (IsNull(MPS.FinalMarkPrice,0) / IsNull(SplitTab.SplitFactor,1)) * FXDayRatesForStartDate.RateValue * PTC.ClosedQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PT.OrderSideTagValue)                                                             
  
    
      
        
          
            
                                                
   Else                                                                           
    Case                                                                                                                          
     When G.FXRate > 0 And G.FXConversionMethodOperator='M'                                                                                                                                          
     Then IsNull((G.AvgPrice / IsNull(SplitTab.SplitFactor,1))* G.FXRate * PTC.ClosedQty  * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PT.OrderSideTagValue),0)                                                                                           
  
   
       
        
          
            
                                               
     When G.FXRate > 0 And G.FXConversionMethodOperator='D'                                                                                                       
     Then IsNull(((G.AvgPrice / IsNull(SplitTab.SplitFactor,1)) * 1/G.FXRate) * PTC.ClosedQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PT.OrderSideTagValue),0)                                                                                       
  
    
      
        
          
            
                                                 
     When G.FXRate <= 0 OR G.FXRate is null                                                                                                                                           
     Then IsNull((G.AvgPrice / IsNull(SplitTab.SplitFactor,1)) * FXDayRatesForTradeDate.RateValue * PTC.ClosedQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PT.OrderSideTagValue),0)                                                                   
  
    
      
        
          
            
                         
    End                                                              
  End                                                                  
 End                                                                                                                 
End as  MarketValue1 ,                                                                    
                                                                                                
Case                                                                                             
 When G.AssetID=5 OR G.AssetID=11 -- 11 for FX Forward  and 5 for FX Spot                                                                                                 
 Then                                                                                                  
   Case @BaseCurrencyID                                                                                                  
     When SM.VsCurrencyID                                                                        
     Then IsNull(PT1.AvgPrice,0) * PTC.ClosedQty * IsNUll(SM.Multiplier,0) * IsNull(dbo.GetSideMultiplierForClosing(G.OrderSideTagValue,G1.OrderSideTagValue),1)                    
     Else IsNull(PT1.AvgPrice,0) * IsNull(FXDayRatesForClosingDateForForex.RateValue,0) * PTC.ClosedQty * IsNUll(SM.Multiplier,0) * IsNull(dbo.GetSideMultiplierForClosing(G.OrderSideTagValue,G1.OrderSideTagValue),1)                                       
   
    
      
       
          
             
              
   End                                                                                                  
 Else                                                                                       
    Case                                                                                                                                                                         
   When G.CurrencyID <> @BaseCurrencyID                                                                         
   Then                                                                               
   Case                                                                               
     When G1.FXRate > 0 And G1.FXConversionMethodOperator='M'                                               
     Then IsNull(PT1.AvgPrice,0)* G1.FXRate * PTC.ClosedQty * IsNUll(SM.Multiplier,0) * IsNull(dbo.GetSideMultiplierForClosing(G.OrderSideTagValue,G1.OrderSideTagValue),1)                                                                              
     When G1.FXRate > 0 And G1.FXConversionMethodOperator='D'                                                    
     Then IsNull(PT1.AvgPrice,0)* 1/G1.FXRate * PTC.ClosedQty * IsNUll(SM.Multiplier,0) * IsNull(dbo.GetSideMultiplierForClosing(G.OrderSideTagValue,G1.OrderSideTagValue),1)                                                                                  
 
   Else IsNull(PT1.AvgPrice,0)* IsNull(FXDayRatesForClosingDate.RateValue,0) * PTC.ClosedQty * IsNUll(SM.Multiplier,0) * IsNull(dbo.GetSideMultiplierForClosing(G.OrderSideTagValue,G1.OrderSideTagValue),1)                                                   
  
    
      
        
          
           
               
   End                                                                      
   Else ISNULL(PT1.AvgPrice,0)* PTC.ClosedQty * IsNUll(SM.Multiplier,0) * IsNull(dbo.GetSideMultiplierForClosing(G.OrderSideTagValue,G1.OrderSideTagValue),1)                                                    
    END                                                                                                  
End as  MarketValue2,                                                                                                                                                                                
'C' as PositionFrom,                                                                                         
                                 
  Case dbo.GetSideMultiplierForClosing(G.OrderSideTagValue,G1.OrderSideTagValue)                                                                              
   When  1                                                                                                                                                                                      
   Then  'Long'                                           
   When  -1                                                                                                                               
   Then  'Short'                                                      
   Else  ''                                                                                   
  End as Side,                    
0 as RealizedPNL,                    
0 as UnRealizedPNL                                                                                                                       
                                                                                                    
  from PM_TaxlotClosing  PTC                                                                                  
  Inner Join PM_Taxlots PT on ( PTC.PositionalTaxlotID=PT.TaxlotID  and  PTC.TaxLotClosingId = PT.TaxLotClosingId_Fk)                              
  Inner Join PM_Taxlots PT1 on (PTC.ClosingTaxlotID=PT1.TaxlotID  and  PTC. TaxLotClosingId = PT1.TaxLotClosingId_Fk)                            
  Inner Join T_Group G on G.GroupID = PT.GroupID                                                                                                                                 
  Inner Join T_Group G1 on G1.GroupID = PT1.GroupID                                                                                                                 
  Inner Join T_AUEC AUEC on G.AUECID = AUEC.AUECID                                                                                             
  Left Outer Join #MarkPriceForStartDate MPS on MPS.Symbol=PT.Symbol                                                                           
  Left Outer Join #MarkPriceForEndDate MPE on MPE.Symbol=PT.Symbol                                                                                                   
  --get yesterday business day                                                                                              
  LEFT OUTER JOIN #AUECYesterDates AUECYesterDates ON G.AUECID = AUECYesterDates.AUECID           
  -- Security Master DB join                                                                                                
  LEFT OUTER JOIN #SecMasterDataTempTable SM ON SM.TickerSymbol = PT.Symbol                                                                                                   
  -- Mark Price for Start Date for FX Trade                                                                                                 
  Left outer  join #FXConversionRates FXMarkPriceforStartDate                                                
 on (FXMarkPriceforStartDate.FromCurrencyID = SM.LeadCurrencyID                                                                                               
 And FXMarkPriceforStartDate.ToCurrencyID = SM.VsCurrencyID                                                                               
 And DateDiff(d,AUECYesterDates.YESTERDAYBIZDATE,FXMarkPriceforStartDate.Date)=0)                                                                                                   
-- Forex Price for Trade Date for FX Trade                                          
  Left outer  join #FXConversionRates FXDayRatesForTradeDateForForex                                                  
 on (FXDayRatesForTradeDateForForex.FromCurrencyID = SM.VsCurrencyID                                                                                               
 And FXDayRatesForTradeDateForForex.ToCurrencyID = @BaseCurrencyID                 
 And DateDiff(d,G.ProcessDate,FXDayRatesForTradeDateForForex.Date)=0)                                                                                                                       
-- Forex Price for Start Date for FX Trade                                                                                              
  Left outer  join #FXConversionRates FXDayRatesForStartDateForForex                                                                                               
 on (FXDayRatesForStartDateForForex.FromCurrencyID = SM.VsCurrencyID                                                                          
 And FXDayRatesForStartDateForForex.ToCurrencyID = @BaseCurrencyID                                                                                     
 And DateDiff(d,AUECYesterDates.YESTERDAYBIZDATE,FXDayRatesForStartDateForForex.Date)=0)                                                                                                          
-- Forex Price for Closing Date for FX Trade                                                                            
  Left outer  join #FXConversionRates FXDayRatesForClosingDateForForex                                                    
 on (FXDayRatesForClosingDateForForex.FromCurrencyID = SM.VsCurrencyID                                                                                               
 And FXDayRatesForClosingDateForForex.ToCurrencyID = @BaseCurrencyID                                                                                               
 And DateDiff(d,G1.ProcessDate,FXDayRatesForClosingDateForForex.Date)=0)                                                              
  -- Forex Price for Trade Date for other than FX Trade                                                                                              
  Left outer  join #FXConversionRates FXDayRatesForTradeDate                                                                                               
 on (FXDayRatesForTradeDate.FromCurrencyID = G.CurrencyID                                                                                               
 And FXDayRatesForTradeDate.ToCurrencyID = @BaseCurrencyID                              
 And DateDiff(d,G.ProcessDate,FXDayRatesForTradeDate.Date)=0)                                                                                                               
  -- Forex Price for Start Date for other than FX Trade                                             
  Left outer  join #FXConversionRates FXDayRatesForStartDate                                                          
 on (FXDayRatesForStartDate.FromCurrencyID = G.CurrencyID                                                                                      
 And FXDayRatesForStartDate.ToCurrencyID = @BaseCurrencyID                                                                                               
 And DateDiff(d,AUECYesterDates.YESTERDAYBIZDATE,FXDayRatesForStartDate.Date)=0)                                                                                              
  -- Forex Price for Closing Date for other than FX Trade                                                                                                 
  Left outer  join #FXConversionRates FXDayRatesForClosingDate                                                                                               
 on (FXDayRatesForClosingDate.FromCurrencyID = G.CurrencyID                                                                                               
 And FXDayRatesForClosingDate.ToCurrencyID = @BaseCurrencyID                                                                                               
 And DateDiff(d,G1.ProcessDate,FXDayRatesForClosingDate.Date)=0)                                                                  
  Left Outer Join  T_SwapParameters SW on SW.GroupID=G.GroupID     
  Left Outer Join  T_SwapParameters SW1 on SW1.GroupID=G1.GroupID                                                           
  Left Outer Join T_CompanyFunds ON  PT.FundID= T_CompanyFunds.CompanyFundID                                                                                                                      
  Left Outer Join T_CompanyStrategy as CompanyStrategy On CompanyStrategy.CompanyStrategyID=PT.Level2ID                                                                                                                                 
  Left Outer Join T_Asset On T_Asset.AssetId=G.AssetID                                                    
  LEFT OUTER JOIN T_CompanyMasterFundSubAccountAssociation CMFSSAA ON T_CompanyFunds.CompanyFundID = CMFSSAA.CompanyFundID                    
  LEFT OUTER JOIN T_CompanyMasterFunds CMF ON CMFSSAA.CompanyMasterFundID = CMF.CompanyMasterFundID                                                 
  LEFT OUTER JOIN T_CompanyMasterStrategySubAccountAssociation CMSSSAA ON CompanyStrategy.CompanyStrategyID = CMSSSAA.CompanyStrategyID                                                                                          
  LEFT OUTER JOIN T_CompanyMasterStrategy CMS ON CMSSSAA.CompanyMasterStrategyID = CMS.CompanyMasterStrategyID                                                  
  LEFT OUTER join @SelectedSymbol SSymbol on SSymbol.FutSymbol= G.Symbol               
 Left Outer Join #TempSplitFactorForClosed SplitTab on SplitTab.TaxlotID=PT.TaxlotID                                                             
  where DateDiff(d,@StartDate,PTC.AUECLocalDate) >=0                                                                                                                               
  and  DateDiff(d,PTC.AUECLocalDate,@EndDate)>=0                                    
  and  PTC.ClosingMode<>7 and  SSymbol.FutSymbol is null and PT.FundID in (select FundID from @Fund)    --7 means CoperateAction!            
          
-- Union All               
Insert Into #TempOutput                                                     
                                                                                           
 Select                                                                                                                                                                               
  PT.Symbol as Symbol,                                                                                                                                  
   --Open Commission                                         
                                                                              
Case                                                                                                                                                                         
 When G.CurrencyID =  @BaseCurrencyID   --When Company and Traded Currency both are same                                                                                                                                                                     
 Then IsNull(PT.OpenTotalCommissionandFees,0)                                                                             
 Else  --When Company and Traded Currency are different                                     
  Case                                                                         
   When G.FXRate > 0 And G.FXConversionMethodOperator='M'                                           
   Then IsNull(PT.OpenTotalCommissionandFees * G.FXRate,0)                                                                                                                                  
   When G.FXRate > 0 And G.FXConversionMethodOperator='D'                                                                                           
   Then IsNull(PT.OpenTotalCommissionandFees * 1/G.FXRate,0)   
   When G.FXRate <= 0 OR G.FXRate is null                                                                                                                                            
   Then  IsNull(PT.OpenTotalCommissionandFees * IsNull(FXDayRatesForTradeDate.RateValue,0),0)                                       
  End                                                                                                      
End as TotalOpenCommission,                                                        
 --Closed Commission                                                                                             
0 as TotalClosedCommission,                                                                                                  
G.ProcessDate as TradeDate,                                              
                            
Case                                                                         
When G.CurrencyID =  @BaseCurrencyID                                                                                                   
Then                                                                
 Case                                                                                                                         
  When DateDiff(d,G.ProcessDate,@StartDate) >0                                            
  Then IsNull(MPS.FinalMarkPrice,0) * PTC.ClosedQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PT.OrderSideTagValue)                            
  Else PT.AvgPrice * PTC.ClosedQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PT.OrderSideTagValue)                                                                                                                           
 End                                                                                                
Else                                                                                              
 Case                                                                                                                                                                                           
 When  DateDiff(d,G.ProcessDate,@StartDate) > 0                                             
 Then  IsNull(MPS.FinalMarkPrice,0) * FXDayRatesForStartDate.RateValue * PTC.ClosedQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PT.OrderSideTagValue)                                                                       
 Else                                                                           
  Case                                                                 
   When G.FXRate > 0 And G.FXConversionMethodOperator='M'                                                                                                                                          
   Then IsNull(PT.AvgPrice * G.FXRate * PTC.ClosedQty  * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PT.OrderSideTagValue),0)                                                                                                     
   When G.FXRate > 0 And G.FXConversionMethodOperator='D'                                                                                                       
   Then IsNull((PT.AvgPrice * 1/G.FXRate) * PTC.ClosedQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PT.OrderSideTagValue),0)                                                                                                 
   When G.FXRate <= 0 OR G.FXRate is null                                                                                                                                           
   Then IsNull(PT.AvgPrice * FXDayRatesForTradeDate.RateValue * PTC.ClosedQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PT.OrderSideTagValue),0)                                                                             
  End                                                                                                                                                                        
 End                                                       
End as  MarketValue1 ,                                                                    
                                                                                      
Case                                                                                                                                                                         
When G.CurrencyID <> @BaseCurrencyID                                                                         
Then                                                                               
 Case                                                                               
  When G1.FXRate > 0 And G1.FXConversionMethodOperator='M'                                               
  Then IsNull(PT1.AvgPrice,0)* G1.FXRate * PTC.ClosedQty * IsNUll(SM.Multiplier,0) * IsNull(dbo.GetSideMultiplierForClosing(G.OrderSideTagValue,G1.OrderSideTagValue),1)                                                                              
  When G1.FXRate > 0 And G1.FXConversionMethodOperator='D'                                                    
  Then IsNull(PT1.AvgPrice,0)* 1/G1.FXRate * PTC.ClosedQty * IsNUll(SM.Multiplier,0) * IsNull(dbo.GetSideMultiplierForClosing(G.OrderSideTagValue,G1.OrderSideTagValue),1)                                                                                   
  Else IsNull(PT1.AvgPrice,0)* IsNull(FXDayRatesForClosingDate.RateValue,0) * PTC.ClosedQty * IsNUll(SM.Multiplier,0) * IsNull(dbo.GetSideMultiplierForClosing(G.OrderSideTagValue,G1.OrderSideTagValue),1)                                                   
  
    
      
        
          
 End                                                                      
Else ISNULL(PT1.AvgPrice,0)* PTC.ClosedQty * IsNUll(SM.Multiplier,0) * IsNull(dbo.GetSideMultiplierForClosing(G.OrderSideTagValue,G1.OrderSideTagValue),1)                                                     
End as  MarketValue2,           
                                                                                                                                                                               
'O' as PositionFrom,                                                                                         
                                                                                                                                 
Case dbo.GetSideMultiplierForClosing(G.OrderSideTagValue,G1.OrderSideTagValue)                                                                              
 When  1                                                                                                                                                                                     
 Then  'Long'                                           
 When  -1                                                                                                                               
 Then  'Short'                                                      
 Else  ''                                                                                   
End as Side,                    
0 as RealizedPNL,                    
0 as UnRealizedPNL                                                                                                                       
                                                                                                    
  from PM_TaxlotClosing  PTC                                                                                  
  Inner Join PM_Taxlots PT on ( PTC.PositionalTaxlotID=PT.TaxlotID  and  PTC.TaxLotClosingId = PT.TaxLotClosingId_Fk)                              
  Inner Join PM_Taxlots PT1 on (PTC.ClosingTaxlotID=PT1.TaxlotID  and  PTC. TaxLotClosingId = PT1.TaxLotClosingId_Fk)                            
  Inner Join T_Group G on G.GroupID = PT.GroupID                                                                                               
  Inner Join T_Group G1 on G1.GroupID = PT1.GroupID                                                                                                                 
  Inner Join T_AUEC AUEC on G.AUECID = AUEC.AUECID                                                                                             
  Left Outer Join #MarkPriceForStartDate MPS on MPS.Symbol=PT.Symbol                                                                           
  Left Outer Join #MarkPriceForEndDate MPE on MPE.Symbol=PT.Symbol           
-- Mark Price for Previous Date          
--  Left Outer Join PM_DayMarkPrice MPTradeDate ON MPTradeDate.Symbol = PT.Symbol                            
--  And DATEDIFF(d,MPTradeDate.Date,dbo.AdjustBusinessDays(PTC.AUECLocalDate,-1,G.AUECID)) = 0                                                                                                    
  --get yesterday business day                                                                                              
  LEFT OUTER JOIN #AUECYesterDates AUECYesterDates ON G.AUECID = AUECYesterDates.AUECID                                                                           
  -- Security Master DB join                                                                                                
  LEFT OUTER JOIN #SecMasterDataTempTable SM ON SM.TickerSymbol = PT.Symbol                                                                              
  -- Forex Price for Trade Date for other than FX Trade                                                                                              
  Left outer  join #FXConversionRates FXDayRatesForTradeDate                                                                                               
 on (FXDayRatesForTradeDate.FromCurrencyID = G.CurrencyID                                                                                               
 And FXDayRatesForTradeDate.ToCurrencyID = @BaseCurrencyID                              
 And DateDiff(d,G.ProcessDate,FXDayRatesForTradeDate.Date)=0)                                                                                                               
  -- Forex Price for Start Date for other than FX Trade                                                                                                   
  Left outer  join #FXConversionRates FXDayRatesForStartDate                                                          
 on (FXDayRatesForStartDate.FromCurrencyID = G.CurrencyID                                                                                      
 And FXDayRatesForStartDate.ToCurrencyID = @BaseCurrencyID                                                                                               
 And DateDiff(d,AUECYesterDates.YESTERDAYBIZDATE,FXDayRatesForStartDate.Date)=0)                                                                                              
  -- Forex Price for Closing Date for other than FX Trade                                                                                                 
  Left outer  join #FXConversionRates FXDayRatesForClosingDate                                                                                               
 on (FXDayRatesForClosingDate.FromCurrencyID = G.CurrencyID                                                                                               
 And FXDayRatesForClosingDate.ToCurrencyID = @BaseCurrencyID                                                                                               
 And DateDiff(d,G1.ProcessDate,FXDayRatesForClosingDate.Date)=0)                                                                  
  Left Outer Join  T_SwapParameters SW on SW.GroupID=G.GroupID                                                                                      
  Left Outer Join  T_SwapParameters SW1 on SW1.GroupID=G1.GroupID                                                           
  Left Outer Join T_CompanyFunds ON  PT.FundID= T_CompanyFunds.CompanyFundID                                                  
  Left Outer Join T_CompanyStrategy as CompanyStrategy On CompanyStrategy.CompanyStrategyID=PT.Level2ID                                                                                                                                 
  Left Outer Join T_Asset On T_Asset.AssetId=G.AssetID                                                                                                                                        
  LEFT OUTER JOIN T_CompanyMasterFundSubAccountAssociation CMFSSAA ON T_CompanyFunds.CompanyFundID = CMFSSAA.CompanyFundID                    
  LEFT OUTER JOIN T_CompanyMasterFunds CMF ON CMFSSAA.CompanyMasterFundID = CMF.CompanyMasterFundID                                                 
  LEFT OUTER JOIN T_CompanyMasterStrategySubAccountAssociation CMSSSAA ON CompanyStrategy.CompanyStrategyID = CMSSSAA.CompanyStrategyID                                                                                          
  LEFT OUTER JOIN T_CompanyMasterStrategy CMS ON CMSSSAA.CompanyMasterStrategyID = CMS.CompanyMasterStrategyID                                                  
  LEFT OUTER join @SelectedSymbol SSymbol on SSymbol.FutSymbol= G.Symbol               
 --Left Outer Join #TempSplitFactorForClosed SplitTab on SplitTab.TaxlotID=PT.TaxlotID                                                             
  where DateDiff(d,@StartDate,PTC.AUECLocalDate) >=0                                                                                                                               
  and  DateDiff(d,PTC.AUECLocalDate,@EndDate)>=0                                    
  and  PTC.ClosingMode = 7 and  SSymbol.FutSymbol is null and PT.FundID in (select FundID from @Fund)    --7 means Name change CoperateAction!                                                                           
                                                                          
--Union All       --international future                                                                               
Insert Into #TempOutput                                                          
select                                                           
T1.Symbol,                                                                              
Case                                                                    
  When DateDiff(day,T2.MPdate, @StartDate)=0                                             
  Then T1.OpenCommissionandFees * T2.CF                                                                    
  Else 0                                  
End as TotalOpenCommission,                                                                                                                                     
0  as TotalClosedCommission,                            
T1.tradedate as TradeDate,                                                                                                                  
T1.MarkPrice * T2.TaxlotOPenQty * T2.CF * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(T1.OrderSideTagValue) as Marketvalue1,                                                                                                                
T2.MarkPrice * T2.TaxlotOPenQty * T2.CF * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(T1.OrderSideTagValue) as Marketvalue2,                                                                                                   
 Case datediff(day,T2.MPdate, @EndDate)                                                     
  When 0                                                 
  Then 'I'                               
  Else 'C'                                                              
End as PositionFrom,                       
dbo.GetLongShortBySide(t2.OrderSideTagValue) as Side,                    
0 as RealizedPNL,                    
0 as UnRealizedPNL                                                                                                                                                                                     
                                                                                                                     
--Case dbo.GetLongShortBySide(t2.OrderSideTagValue)                                                                                                                                                                               
--  When  1                                                                                                                                                                                           
--  Then  'Long'                                                                                                                                                                                              
--  Else  'Short'                                                                                                               
--End as Side                                                                                                                                            
                                                           
From #Temp3 T1                                                          
 Inner Join #Temp3 T2                    
 on (T1.TaxlotID=T2.TaxlotID                                                   
 And DateDiff(day,(DATEADD(day, DATEDIFF(day, 0, T1.MPdate),1)),T2.MPDate)=0 )                                                                             
 Left outer join #SecMasterDataTempTable SM ON SM.TickerSymbol = T2.Symbol                                                           
 Left Outer Join T_CompanyStrategy as CompanyStrategy On CompanyStrategy.CompanyStrategyID=t1.Level2ID                                                                                                                                              
 Left outer Join T_AUEC AUEC On AUEC.AUECID=SM.AUECID                                                                                                                           
 Left outer Join T_Asset On T_Asset.AssetID=AUEC.AssetId                                                             
 Left outer Join T_CompanyFunds on T_CompanyFunds.CompanyFundID= t1.FundID                                                                                               
 LEFT OUTER JOIN T_CompanyMasterFundSubAccountAssociation CMFSSAA ON T_CompanyFunds.CompanyFundID = CMFSSAA.CompanyFundID                                                                  
 LEFT OUTER JOIN T_CompanyMasterFunds CMF ON CMFSSAA.CompanyMasterFundID = CMF.CompanyMasterFundID                                                                                                                                                             
 
    
      
        
          
            
              
                
                  
                    
Where DateDiff(d,@StartDate,T2.MPdate) >= 0                                                      
And DateDiff(d,T2.MPdate,@Enddate) >=0 and T1.FundID in (select FundID from @Fund)                                                                                                                                             
                                      
--Union All  --inetrnational futures                                                                          
Insert Into #TempOutput                                                          
Select                                                           
 T1.Symbol,                                                          
Case                                                                   
   When DateDiff(day,T1.MPdate, T1.TradeDate)=0                                                
   Then T1.OpenCommissionandFees *                                      
  Case                                          
    When T1.FXRate is null Or T1.FXRate=0                                                 
    Then T1.CF                                                                   
    Else                                 
  Case  T1.FXConversionMethodOperator                                                                    
     When 'M'                                                                   
     Then T1.FXRate                                              
     When 'D'                                                                   
     Then 1/T1.FXRate                                   
     Else 0                                                                   
  End                                                                     
   End                                                                         
 Else 0                                                                   
 End As TotalOpenCommission,                               
 0  as TotalClosedCommission,                                                          
 T1.tradedate as TradeDate,                                                                                                                                                      
 T1.AvgPrice * T1.TaxlotOPenQty*                                                                       
Case                                                 
  When T1.FXRate is null Or T1.FXRate=0                                                 
  Then T1.CF                                                                   
  Else                                                                     
   Case  T1.FXConversionMethodOperator        
     When 'M'                                                                   
     Then T1.FXRate                                              
     When 'D'                                                                   
     Then 1/T1.FXRate                                            
     Else 0                                                               
   End                                                                     
 End                                                   
 *IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(T1.OrderSideTagValue) As Marketvalue1,                                                                                                                
 T1.MarkPrice * T1.TaxlotOPenQty *t1.CF*IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(T1.OrderSideTagValue) As Marketvalue2,                                                                                                         
 Case datediff(day,t1.MPdate, @EndDate)                                                     
  When 0                                                   
  Then  'I'                                                      
  Else 'C'                                                   
 End As PositionFrom,                     
 dbo.GetLongShortBySide(T1.OrderSideTagValue) as Side,                    
0 as RealizedPNL,                    
0 as UnRealizedPNL                                                                                                                                                                   
-- Case dbo.GetLongShortBySide(T1.OrderSideTagValue)                   
--  When  1                                                                                                                                                                                           
--   Then  'Long'                                                                                                                                                                                              
--   Else  'Short'                                                                          
-- End as Side                                                                           
From #Temp3 T1                                                       
--Left Outer  join V_taxlots on V_taxlots.taxlotID =   t1.TaxlotID                                                    
Left outer join #SecMasterDataTempTable SM ON SM.TickerSymbol = T1.Symbol   
Left Outer Join T_CompanyStrategy as CompanyStrategy On CompanyStrategy.CompanyStrategyID=t1.Level2ID                                                                                                                                              
Left outer Join T_AUEC AUEC On AUEC.AUECID=SM.AUECID                                                                                                                                          
Left outer Join T_Asset On T_Asset.AssetID=AUEC.AssetId                                                         
Left outer Join T_CompanyFunds on T_CompanyFunds.CompanyFundID=T1.FundID                                           
LEFT OUTER JOIN T_CompanyMasterFundSubAccountAssociation CMFSSAA ON T_CompanyFunds.CompanyFundID = CMFSSAA.CompanyFundID                                                                                                                  
LEFT OUTER JOIN T_CompanyMasterFunds CMF ON CMFSSAA.CompanyMasterFundID = CMF.CompanyMasterFundID                                                           
Where DateDiff(d,T1.MPdate,T1.TradeDate)=0                                                                          
      And DateDiff(d,@StartDate,T1.Tradedate) >= 0                     
      And DateDiff(d,T1.Tradedate,@Enddate) >= 0  and T1.FundID in (select FundID from @Fund)                                   
                                     
 --Order By PT.Symbol                     
                    
--Select * from #TempOutput                      
                    
Update #TempOutput                   
Set RealizedPNL = (MarketValue2 - MarketValue1 - TotalOpenCommission - TotalClosedCommission)                            
Where (PositionFrom = 'C' or PositionFrom = 'I') and DateDiff(day,@StartDate,TradeDate)>=0                       
                      
Update #TempOutput                            
Set RealizedPNL = (MarketValue2 - MarketValue1 - TotalClosedCommission)                            
Where (PositionFrom = 'C' or PositionFrom = 'I') and DateDiff(day,@StartDate,TradeDate)< 0                            
                            
Update #TempOutput                            
Set UnRealizedPNL = (MarketValue2 - MarketValue1 - TotalOpenCommission)                            
Where PositionFrom = 'O' and DateDiff(day,@StartDate,TradeDate)>=0                           
                      
Update #TempOutput                            
Set UnRealizedPNL = (MarketValue2 - MarketValue1)                            
Where PositionFrom = 'O' and DateDiff(day,@StartDate,TradeDate)< 0                      
                    
--Select * from #TempOutput                     
                    
                    
Create Table #TempOutput1                       
(                    
Symbol varchar(100),                    
MarketValue1 float,                    
MarketValue2 float,                     
PositionFrom varchar(5),                    
Side Varchar(10),                    
RealizedPNL float,                    
UnRealizedPNL float                    
)                    
insert into #TempOutput1                    
Select                     
Min(Symbol) as Symbol,                    
Sum(MarketValue1) as MarketValue1,                    
Sum(MarketValue2) as MarketValue2,                    
'O' as PositionFrom,                    
Side,                    
0 as RealizedPNL,                    
Sum(UnRealizedPNL) as UnRealizedPNL                    
                    
from #TempOutput                     
                    
Where PositionFrom='O'                    
Group By Symbol,Side                    
                    
insert into #TempOutput1                    
Select                     
Symbol,                    
MarketValue1,                    
MarketValue2,                    
PositionFrom,                    
Side,                    
RealizedPNL,                    
UnRealizedPNL                    
from #TempOutput                     
Where PositionFrom='C' or PositionFrom='I'         
                    
Select * from #TempOutput1  Where Side='Short' and PositionFrom='O'                     
                            
Drop Table #TempMP,#Temp2,#Temp3,#TempOutput,#TempOutput1,#TempSplitFactorForOpen,#TempSplitFactorForClosed,#TempSplitFactorForClosed_1,#TempSplitFactorForClosed_2               
Drop Table #MarkPriceForStartDate,#MarkPriceForEndDate,#FXConversionRates,#AUECYesterDates,#AUECBusinessDatesForEndDate,#SecMasterDataTempTable                          
                            
END 