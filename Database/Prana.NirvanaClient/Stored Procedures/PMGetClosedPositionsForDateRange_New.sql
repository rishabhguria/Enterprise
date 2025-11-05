            
CREATE PROCEDURE [dbo].[PMGetClosedPositionsForDateRange_New]                                                               
(                                                                                                                                  
  @ToAllAUECDatesString varchar(max),--in Historical All auec date remain same                                                                              
  @FromAllAUECDatesString varchar(max),                                                                                                                                                                                    
  @FundIds VARCHAR(MAX),        
  @AssetIds VARCHAR(MAX),        
  @Symbols VARCHAR(MAX),        
  @CustomConditions VARCHAR(MAX),        
  @ClosingDateType int                                                                                                                 
)                                                                                                                                  
As                                                                                                                                      
                                                                              
Begin                                                                                                               
      
Declare @Local_ToAllAUECDatesString varchar(max)      
Declare @Local_FromAllAUECDatesString varchar(max)      
Declare @Local_FundIds varchar(max)      
Declare @Local_AssetIds varchar(max)      
Declare @Local_Symbols varchar(max)      
Declare @Local_CustomConditions varchar(max)      
      
Set @Local_ToAllAUECDatesString = @ToAllAUECDatesString      
Set @Local_FromAllAUECDatesString = @FromAllAUECDatesString      
Set @Local_FundIds = @FundIds      
Set @Local_AssetIds = @AssetIds      
Set @Local_Symbols = @Symbols      
Set @Local_CustomConditions = @CustomConditions      
                                                                                                  
Declare @ToAUECDatesTable Table(AUECID int,CurrentAUECDate DateTime)                                                                                                                  
                                                                                                  
Insert Into @ToAUECDatesTable Select * From dbo.GetAllAUECDatesFromString(@Local_ToAllAUECDatesString)                                                                                                                  
                                                                    
                                                                      
Declare @FromAUECDatesTable Table(AUECID int,CurrentAUECDate DateTime)                                                                                                                  
                                                                                                  
Insert Into @FromAUECDatesTable Select * From dbo.GetAllAUECDatesFromString(@Local_FromAllAUECDatesString)                            
              
            
Create Table #Funds (FundID int)                                                      
 if (@Local_FundIds is NULL or @Local_FundIds = '')                                                      
 Insert into #Funds                                                      
 Select CompanyFundID as FundID from T_CompanyFunds Where IsActive=1                                                     
 else                                                      
 Insert into #Funds                                                      
 Select Items as FundID from dbo.Split(@Local_FundIds,',')                
        
        
 Create Table #AssetClass (AssetID int)                                                    
 if (@Local_AssetIds is NULL or @Local_AssetIds = '')                                                    
 Insert into #AssetClass            
 Select AssetID from T_Asset                                                    
 else               
 Insert into #AssetClass                                                    
 Select Items as AssetID from dbo.Split(@Local_AssetIds,',')                                                    
                                                    
        
          
 Create Table #Symbols (Symbol varchar(100))          
 if (@Local_Symbols is NOT NULL AND @Local_Symbols <> '')            
 INSERT INTO  #Symbols           
 SELECT ITEMS AS Symbol FROM dbo.Split(@Local_Symbols, ',')          
                  
Create Table #SecurityMasterTemp                  
(                  
  TickerSymbol varchar(100),                  
 Multiplier float,
UnderlyingSymbol varchar(200),
BloombergSymbol varchar(200) ,             
SEDOLSymbol varchar(20),        
LeadCurrencyId	INT,
VsCurrencyId	INT     
                
                  
)                  
                
Insert into      #SecurityMasterTemp                
                
select TickerSymbol, Multiplier,UnderlyingSymbol,BloombergSymbol,SEDOLSymbol, LeadCurrencyID, VsCurrencyID from V_SecMasterData                   
                
Create Table #PM_Taxlots                
(              
Symbol varchar(100),                
OrderSideTagValue char(1),                
AvgPrice float,                
FundID int,                
Level2ID int,                
ClosedTotalCommissionandFees float,                
PositionTag int,                
TaxLotClosingId_Fk uniqueidentifier,                
TaxlotID varchar(50),                
GroupID varchar(50),              
TradeAttribute1 varchar(200),
TradeAttribute2 varchar(200),
TradeAttribute3 varchar(200),
TradeAttribute4 varchar(200),
TradeAttribute5 varchar(200),
TradeAttribute6 varchar(200),
AdditionalTradeAttributes varchar(max)
)                
                
                
Insert into #PM_Taxlots                
                
select                
                
Symbol,                
OrderSideTagValue ,                
AvgPrice ,                
FundID ,                
Level2ID ,                
ClosedTotalCommissionandFees ,                
PositionTag ,                
TaxLotClosingId_Fk,                
TaxlotID,                
GroupID,
TradeAttribute1,
TradeAttribute2,
TradeAttribute3,
TradeAttribute4,
TradeAttribute5,
TradeAttribute6,
AdditionalTradeAttributes
                
                
From PM_Taxlots             
        
        
CREATE TABLE #Temp_Positions              
(        
PositionalTaxlotID varchar(50),                                                      
ClosingTaxlotID varchar(50),                                                              
Symbol varchar(100),                                                             
PositionSideID nchar(10),                                                              
ClosingSideID  nchar(10),                                                              
PositionTradeDate datetime,                                                              
ClosingTradeDate datetime, --now closing taxlot Trade date is cloisng date                                                              
OpenPrice float ,                                                              
ClosingPrice float ,                                                              
FundID int,                                      
Level2ID int,                                                            
AssetID int,                                                            
UnderLyingID int,                                                            
ExchangeID int,                                                            
CurrencyID int ,                                                            
PositionalTaxlotCommission float,                                                              
ClosingTaxlotCommission float,                                                              
ClosingMode int,                                                             
Multiplier float,                                                            
OpeiningPositionTag int ,                                                            
ClosingPositionTag int,                                                    
ClosedQty float,                        
PositionNotionalValue float,                                                         
PositionBenchMarkRate float,                                                                    
PositionDifferential float,                                                                        
PositionOrigCostBasis float,                                                                            
PositionDayCount int,                                               
PositionalSwapDescription varchar(500),                      
PositionFirstResetDate datetime,                                                                        
PositionOrigTransDate datetime,                                                               
NotionalValue float,                    
BenchMarkRate float,                                                                        
Differential float,                                                                        
OrigCostBasis float,                                                                              
DayCount int,                                                                        
ClosingSwapDescription varchar(500),                                                      
FirstResetDate datetime,                                                                        
OrigTransDate datetime ,                                                            
IsSwapped bit,                                                            
ClosingIsSwapped bit,                                                        
TaxLotClosingId_Fk uniqueidentifier,                                        
PositionSide nchar(20),                
ClosingAlgo int ,        
UnderlyingSymbol varchar(200),
BloombergSymbol varchar(200) ,             
SEDOLSymbol varchar(20) ,       
LeadCurrencyId	INT,
VsCurrencyId	INT,                      
TradeAttribute1 varchar(200),
TradeAttribute2 varchar(200),
TradeAttribute3 varchar(200),
TradeAttribute4 varchar(200),
TradeAttribute5 varchar(200),
TradeAttribute6 varchar(200),
AdditionalTradeAttributes varchar(max)
        
        
        
        
)                   
        
        
INSERT INTO #Temp_Positions                                                                  
            
select                                                     
distinct                                                             
PTC.PositionalTaxlotID,                                                      
PTC.ClosingTaxlotID,                                                              
PT.Symbol as Symbol,                                                             
PT.OrderSideTagValue as PositionSideID,                                                              
PT1.OrderSideTagValue as ClosingSideID,   
CASE @ClosingDateType
WHEN 0
THEN G.AUECLocalDate
WHEN 1
THEN G.ProcessDate
WHEN 2
THEN G.OriginalPurchaseDate                                                              
END as PositionTradeDate,    --https://jira.nirvanasolutions.com:8443/browse/CI-4780                                                          
PTC.AUECLocalDate as ClosingTradeDate, --now closing taxlot Trade date is cloisng date                                                              
PTC.OpenPrice as OpenPrice ,                                                              
PTC.ClosePrice as ClosingPrice ,                                                              
PT.FundID as FundID,                                      
PT.Level2ID as Level2ID,                                                            
G.AssetID,                                                            
G.UnderLyingID,                                                            
G.ExchangeID,                                                            
G.CurrencyID,                                                            
PT.ClosedTotalCommissionandFees as PositionalTaxlotCommission,                                                              
PT1.ClosedTotalCommissionandFees as ClosingTaxlotCommission,                                                              
PTC.ClosingMode as ClosingMode,                                                             
SM.Multiplier as  Multiplier,              
PT.PositionTag as OpeiningPositionTag,                                                            
PT1.PositionTag as ClosingPositionTag,                                                    
PTC.ClosedQty  ,                        
CASE G.CumQty   
WHEN 0
THEN 0
ELSE   
isnull(SW.NotionalValue*((PTC.ClosedQty)/G.CumQty) ,0)   
END as PositionNotionalValue,                                                                
isnull(SW.BenchMarkRate,0) as PositionBenchMarkRate,                                                                    
isnull(SW.Differential,0) as PositionDifferential,                                                                        
isnull(SW.OrigCostBasis,0) as PositionOrigCostBasis,                                                                            
isnull(SW.DayCount,0) as PositionDayCount,                                               
SW.SwapDescription AS PositionalSwapDescription,                                                      
SW.FirstResetDate as PositionFirstResetDate,                                                                        
SW.OrigTransDate as PositionOrigTransDate,                                                               
CASE G.CumQty   
WHEN 0
THEN 0
ELSE   
isnull(SW1.NotionalValue*((PTC.ClosedQty)/G1.CumQty),0)   
END as NotionalValue,                                                                        
isnull(SW1.BenchMarkRate,0) as BenchMarkRate,                                                                        
isnull(SW1.Differential,0) as Differential,                                                                        
isnull(SW1.OrigCostBasis,0) as OrigCostBasis,                                                                              
isnull(SW1.DayCount,0) as DayCount,                                                                        
SW1.SwapDescription AS ClosingSwapDescription,                                                      
SW1.FirstResetDate as FirstResetDate,                                                                        
SW1.OrigTransDate as OrigTransDate ,                                                            
G.IsSwapped,                                                            
G1.IsSwapped ,                                                        
PT.TaxLotClosingId_Fk ,                                        
PTC.PositionSide ,                  
PTC.ClosingAlgo,   
SM.UnderlyingSymbol,
SM.BloombergSymbol,
SM.SEDOLSymbol,
SM.LeadCurrencyId,
SM.VsCurrencyId,	     
CASE PTC.IsCopyTradeAttrbsPrefUsed WHEN 1 THEN PT.TradeAttribute1 ELSE '' END AS TrafeAttribute1,
CASE PTC.IsCopyTradeAttrbsPrefUsed WHEN 1 THEN PT.TradeAttribute2 ELSE '' END AS TrafeAttribute2,
CASE PTC.IsCopyTradeAttrbsPrefUsed WHEN 1 THEN PT.TradeAttribute3 ELSE '' END AS TrafeAttribute3,
CASE PTC.IsCopyTradeAttrbsPrefUsed WHEN 1 THEN PT.TradeAttribute4 ELSE '' END AS TrafeAttribute4,
CASE PTC.IsCopyTradeAttrbsPrefUsed WHEN 1 THEN PT.TradeAttribute5 ELSE '' END AS TrafeAttribute5,
CASE PTC.IsCopyTradeAttrbsPrefUsed WHEN 1 THEN PT.TradeAttribute6 ELSE '' END AS TrafeAttribute6,
CASE PTC.IsCopyTradeAttrbsPrefUsed WHEN 1 THEN PT.AdditionalTradeAttributes ELSE '{}' END AS AdditionalTradeAttributes
                                                                                                                   
from PM_TaxlotClosing  PTC                                                              
Inner Join #PM_Taxlots PT on ( PTC.PositionalTaxlotID=PT.TaxlotID  and  PTC. TaxLotClosingId = PT.TaxLotClosingId_Fk)                                                         
Inner Join #PM_Taxlots PT1 on (PTC.ClosingTaxlotID=PT1.TaxlotID  and  PTC. TaxLotClosingId = PT1.TaxLotClosingId_Fk)          
        
 LEFT OUTER  JOIN   #Symbols ON (PT.Symbol = #Symbols.Symbol AND  @Local_Symbols <> '')           
Inner Join #Funds funds on PT.FundId=funds.FundId                                                           
Inner Join T_Group G on G.GroupID = PT.GroupID          
 Inner join #AssetClass on G.AssetID = #AssetClass.AssetID                                                                
 Inner Join T_Group G1 on G1.GroupID = PT1.GroupID                                                             
Inner Join T_AUEC AUEC on AUEC.AUECID=G.AUECID                                            
 LEFT OUTER JOIN #SecurityMasterTemp SM ON PT.Symbol = SM.TickerSymbol     
Inner Join @ToAUECDatesTable ToAUECDatesTable on ToAUECDatesTable.AUECID=AUEC.AUECID                                                              
Inner Join @FromAUECDatesTable FromAUECDatesTable on FromAUECDatesTable.AUECID=AUEC.AUECID                                                              
Left Outer Join  T_SwapParameters SW on SW.GroupID=G.GroupID                                                              
Left Outer Join  T_SwapParameters SW1 on SW1.GroupID=G1.GroupID              
                                                           
WHERE ((@Local_Symbols <> '' AND #Symbols.Symbol is not null) or (@Local_Symbols = ''))          
AND                                                                    
Datediff(d,FromAUECDatesTable.CurrentAUECDate,PTC.AUECLocalDate) >= 0            
 AND                                                     
          
Datediff(d,PTC.AUECLocalDate,ToAUECDatesTable.CurrentAUECDate) >= 0                              
                
        
        
DECLARE @sqlCommand VARCHAR(MAX)              
SET @sqlCommand  = 'SELECT * FROM #Temp_Positions WHERE 1=1'         
        
IF(@Local_CustomConditions <>'')          
BEGIN        
 SELECT @sqlCommand = @sqlCommand + @Local_CustomConditions        
EXEC (@sqlCommand)          
END        
              
ELSE           
        
  BEGIN        
  SELECT * FROM #Temp_Positions        
END        
        
                
drop table  #SecurityMasterTemp,#PM_Taxlots,#Temp_Positions,#Symbols,#Funds,#AssetClass                          
                                                                    
                                                
End 

