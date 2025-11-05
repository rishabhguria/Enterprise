  
    
/*            
Author: Aman Seth      
Description: Get total PNL for a symbol and a fund between a date range     
  
declare @p5 float  
set @p5=0  
declare @p6 float  
set @p6=0  
declare @p7 float  
set @p7=0  
declare @p8 float  
set @p8=0  
exec P_GetTotalPNLForFundAndSymbol @StartDate='2014-11-03 00:00:00:000',@EndDate='2014-11-06 00:00:00:000',@Symbol=N'DELL',@Fund=N'OFFSHORE',@FundRealizedPNL=@p5 output,@FundUNRealizedPNL=@p6 output,@SymbolRealizedPNL=@p7 output,@SymbolUNRealizedPNL=@
p8 output  
select @p5, @p6, @p7, @p8      
*/            
CREATE Procedure [dbo].[P_GetTotalPNLForFundAndSymbol]               
(                  
 @StartDate DateTime,                                                                                                                                                                       
 @EndDate DateTime,                                                                                                                                                                        
 @Symbol char(50),                                                         
 @Fund varchar(max),           
 @FundRealizedPNL float output,        
 @FundUNRealizedPNL float output,           
 @SymbolRealizedPNL float output,        
 @SymbolUNRealizedPNL float output             
)                  
AS          
BEGIN               
--Declare @StartDate DateTime                  
--Set @StartDate = '2014-10-13'                  
--                  
--Declare @EndDate DateTime                  
--Set @EndDate = '2014-10-15'                    
--                  
--Declare @Symbol char(50)                  
--Set @Symbol = 'DELL'                  
--                  
--Declare @Fund varchar(max)                  
--Set @Fund = 'OFFSHORE'                           
--            
--Declare @PNL float                  
      
Declare @AllAUECDatesString VARCHAR(MAX)                                                                                                                                
Set @AllAUECDatesString = dbo.GetAUECDateString(@EndDate)   
  
Declare @AUECDatesTable Table(AUECID int,CurrentAUECDate DateTime)                                                                                                                                                                                             
 
Insert Into @AUECDatesTable Select * From dbo.GetAllAUECDatesFromString(@AllAUECDatesString)   
  
Create Table #AUECYesterDates                                                  
(                                             
  AUECID INT,           
  YESTERDAYBIZDATE DATETIME                                                                                                                              
)     
INSERT INTO #AUECYesterDates                                                                                                                            
Select Distinct V_SymbolAUEC.AUECID, dbo.AdjustBusinessDays(DateAdd(d,1,@EndDate),-1, V_SymbolAUEC.AUECID)                                                                                                                 
from V_SymbolAUEC                                                                                                             
    
--select * from #AUECYesterDates  
    
Create Table #DayMarkPrices                                   
(                                                                                                                
 FundID int,  
 Symbol varchar(200),                                                                             
 YesterDayMarkPrice float,                                                                                                                
 TodayMarkPrice float            
)    
         
INSERT Into #DayMarkPrices                                                                                      
Select   
DayMarkPrice.FundID,                                                         
DayMarkPrice.Symbol,                                                          
0,                                                          
DayMarkPrice.FinalMarkPrice                                                                                                               
From PM_DayMarkPrice DayMarkPrice                                                          
Inner Join V_SymbolAUEC ON DayMarkPrice.Symbol = V_SymbolAUEC.Symbol                                                          
Inner Join #AUECYesterDates AUECDates ON AUECDates.AUECID = V_SymbolAUEC.AUECID                                                        
Where Datediff(d,DayMarkPrice.Date, AUECDates.YESTERDAYBIZDATE) = 0     
  
--select * from #DayMarkPrices  
  
Create Table #SecMasterDataTempTable                                  
(                                                 
  AUECID int,                                                                                                                                                                                                                                              
  TickerSymbol Varchar(100),                        
  Multiplier Float  
)  
INSERT INTO #SecMasterDataTempTable  
SELECT  
AUECID ,                                                            
TickerSymbol,  
Multiplier  
From V_SecMasterData    
  
------------------------------------------Region Unrealized PNL-------------------------------------  
Create Table #UnRealizedPNLData                  
(                        
 FundName varchar(100),  
 FundID int,        
 Symbol varchar(100),  
 Quantity float,    
 AvgPrice float,        
 MarkPrice float,  
 SideMultiplier int,  
 Multiplier int,   
 UnrealizedPNL float  
)    
INSERT INTO #UnRealizedPNLData  
(  
 FundName,  
 FundID,        
 Symbol,  
 Quantity,    
 AvgPrice,        
 MarkPrice,  
 SideMultiplier,  
 Multiplier  
)  
Select   
CF.FundName as FundName,  
PT.FundID as FundID,                                                                                                                                                                                                                                          
PT.Symbol as Symbol ,                                                                                                        
PT.TaxLotOpenQty as Quantity ,                                                                                                                                                                
PT.AvgPrice as AvgPrice ,   
ISNULL(MP.TodayMarkPrice,0) as MarkPrice,                                                                                                                                                                                                                      
ISNULL(dbo.GetSideMultiplier(PT.OrderSideTagValue),0) AS SideMultiplier,  
ISNULL(SM.Multiplier,0) as Multiplier                                                                                                                                                                                                                  
from PM_Taxlots PT      
INNER JOIN T_CompanyFunds CF on (CF.CompanyFundID = PT.FundID AND CF.FundName = @Fund)                                                            
INNER JOIN  T_Group G on G.GroupID=PT.GroupID   
LEFT OUTER JOIN #DayMarkPrices MP on MP.FundID = PT.FundID AND PT.Symbol = MP.Symbol  
LEFT JOIN #SecMasterDataTempTable SM ON SM.TickerSymbol = PT.Symbol                                                                    
WHERE               
taxlot_PK in                                                                                             
(                                                                                                                                            
 Select max(taxlot_PK) from PM_Taxlots PM                                                   
 Inner join  T_Group G on G.GroupID=PM.GroupID                                            
 inner join T_AUEC AUEC on AUEC.AUECID = G.AUECID                                                                                                
 inner join @AUECDatesTable AUECDates on AUEC.AUECID = AUECDates.AUECID                                                                        
 where Datediff(d,PM.AUECModifiedDate,AUECDates.CurrentAUECDate) >= 0                           
 group by taxlotid                                                                                                                     
)                            
and TaxLotOpenQty<>0  order by taxlotid    
  
Update #UnRealizedPNLData  
SET UnrealizedPNL = Quantity * SideMultiplier * Multiplier * (MarkPrice - AvgPrice)   
                                    
SELECT             
@SymbolUNRealizedPNL = ISNULL(SUM(UnrealizedPNL),0)                                                       
from #UnRealizedPNLData  
GROUP BY FundID,Symbol   
HAVING Symbol = @Symbol                       
  
SELECT             
@FundUNRealizedPNL = ISNULL(SUM(UnrealizedPNL),0)                                                       
from #UnRealizedPNLData  
GROUP BY FundID      
----------------------------------------------End of region unrealized PNL--------------------------------------  
    
  
  
----------------------------------------------Region Realized PNL---------------------------------------------  
Create Table #RealizedPNLData                  
(                        
 FundName varchar(100),  
 FundID int,        
 Symbol varchar(100),  
 ClosedQty float,    
 OpenPrice float,        
 ClosingPrice float,  
 SideMultiplier int,  
 Multiplier int,   
 RealizedPNL float  
)    
INSERT INTO #RealizedPNLData  
(  
 FundName,  
 FundID,        
 Symbol,  
 ClosedQty,  
 OpenPrice,  
 ClosingPrice,  
 SideMultiplier,  
 Multiplier  
)  
  
Select   
CF.FundName,  
PT.FundID as FundID,                                                                                                                                                                                                
PT.Symbol as Symbol,   
PTC.ClosedQty,                                                                                                                                                                                                                
PT.AvgPrice as OpenPrice, --/ IsNull(SplitTab.SplitFactor,1) as OpenPrice ,                                                                                                          
PT1.AvgPrice as ClosingPrice,   
CASE   
WHEN(PTC.PositionSide='Short') THEN -1  
ELSE 1  
END as SideMultiplier,                                                                                                                                                                                                                                                                                                                                                                                
ISNULL(SM.Multiplier, 0) AS Multiplier                                                                                                                                                                                                                         
                
FROM PM_TaxlotClosing  PTC                        
INNER Join PM_Taxlots PT on ( PTC.PositionalTaxlotID=PT.TaxlotID  and  PTC. TaxLotClosingId = PT.TaxLotClosingId_Fk)                                                                        
INNER Join PM_Taxlots PT1 on (PTC.ClosingTaxlotID=PT1.TaxlotID  and  PTC. TaxLotClosingId = PT1.TaxLotClosingId_Fk)                         
INNER JOIN T_CompanyFunds CF on (CF.CompanyFundID = PT.FundID AND CF.FundName = @Fund)     
INNER Join T_Group G on G.GroupID = PT.GroupID                                                                                                            
INNER Join T_Group G1 on G1.GroupID = PT1.GroupID          
LEFT JOIN #SecMasterDataTempTable SM ON SM.TickerSymbol = PT.Symbol                                                              
Where                                                                               
DateDiff(d,@StartDate,PTC.AUECLocalDate) >=0                                                                                                 
and  DateDiff(d,PTC.AUECLocalDate,@EndDate)>=0                                                                                      
and  PTC.ClosingMode<>7     
  
Update #RealizedPNLData  
SET RealizedPNL = ClosedQty * SideMultiplier * Multiplier * (ClosingPrice - OpenPrice)   
                                    
SELECT             
@SymbolRealizedPNL = ISNULL(SUM(RealizedPNL),0)                                                       
from #RealizedPNLData  
GROUP BY FundID,Symbol   
HAVING Symbol = @Symbol                       
  
SELECT             
@FundRealizedPNL = ISNULL(SUM(RealizedPNL),0)                                                       
from #RealizedPNLData  
GROUP BY FundID      
----------------------------------------------End of region realized PNL--------------------------------------  
Drop Table #UnRealizedPNLData,#RealizedPNLData,#SecMasterDataTempTable,#DayMarkPrices          
END   
    
  
