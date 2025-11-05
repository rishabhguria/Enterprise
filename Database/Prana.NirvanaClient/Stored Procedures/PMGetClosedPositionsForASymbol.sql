
      
      
CREATE PROCEDURE [dbo].[PMGetClosedPositionsForASymbol]                                                             
(                                                                                                                                
  @ToAllAUECDatesString varchar(max),--in Historical All auec date remain same                                                                            
  @FromAllAUECDatesString varchar(max),                                                                                                                                                                                  
  @FundIds VARCHAR(MAX),        
  @Symbol Varchar(100),  
  @GroupID Varchar(30)                                                                                                               
)                                                                                                                                
As                                                                                                                                    
                                                                            
Begin                                                                                                             
--http://www.sqlusa.com/bestpractices/training/scripts/parametersniffing/
 Declare @Local_ToAllAUECDatesString VARCHAR(MAX) 
 Declare @Local_FromAllAUECDatesString VARCHAR(MAX)                                                         
 Declare @Local_FundIds VARCHAR(MAX)               
 Declare @Local_Symbol Varchar(100)              
 Declare @Local_GroupID Varchar(30) 
 set @Local_ToAllAUECDatesString=@ToAllAUECDatesString                                                      
 set @Local_FromAllAUECDatesString=@FromAllAUECDatesString                 
 set @Local_FundIds=@FundIds            
 set @Local_Symbol=@Symbol
 set @Local_GroupID=@GroupID
                                                                                                
Declare @ToAUECDatesTable Table        
(        
 AUECID int,        
 CurrentAUECDate DateTime        
)        
                                                                                                
Insert Into @ToAUECDatesTable         
Select * From dbo.GetAllAUECDatesFromString(@Local_ToAllAUECDatesString)                                                                                                                
                                                                    
Declare @FromAUECDatesTable Table        
(        
 AUECID int,        
 CurrentAUECDate DateTime        
)        
                                                                                                
Insert Into @FromAUECDatesTable         
Select * From dbo.GetAllAUECDatesFromString(@Local_FromAllAUECDatesString)                          
          
Create Table #Funds         
(        
 FundID int        
)                                                    
        
If (@Local_FundIds is NULL Or @Local_FundIds = '')                                                    
 Insert into #Funds                                                    
 Select         
 CompanyFundID as FundID         
 From T_CompanyFunds Where IsActive=1                                                  
Else                                                    
 Insert into #Funds                                                    
 Select Items as FundID         
 From dbo.Split(@Local_FundIds,',')              
                
Create Table #SecurityMasterTemp                
(                
 TickerSymbol varchar(50),                
 Multiplier float                
)                
              
Insert into #SecurityMasterTemp              
Select         
TickerSymbol,         
Multiplier         
From V_SecMasterData
Where TickerSymbol=@Local_Symbol                  
              
Create Table #PM_Taxlots              
(              
Symbol varchar(50),              
OrderSideTagValue char(10),              
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
Select              
Symbol,              
OrderSideTagValue ,              
AvgPrice ,              
PM_Taxlots.FundID ,                    
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
Inner Join #Funds On #Funds.FundID = PM_Taxlots.FundID   
Where PM_Taxlots.Symbol = @Local_Symbol  
  
  
                                      
Select Distinct                                                           
PTC.PositionalTaxlotID,                                                    
PTC.ClosingTaxlotID,                                                            
PT.Symbol as Symbol,                                                           
PT.OrderSideTagValue as PositionSideID,                               
PT1.OrderSideTagValue as ClosingSideID,                                                            
G.ProcessDate as PositionTradeDate,                                                            
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
SM.Multiplier as Multiplier,                                                          
PT.PositionTag as OpeiningPositionTag,                                                          
PT1.PositionTag as ClosingPositionTag,                                                  
PTC.ClosedQty  ,                      
isnull(SW.NotionalValue*((PTC.ClosedQty)/G.CumQty) ,0) as PositionNotionalValue,                                                              
isnull(SW.BenchMarkRate,0) as PositionBenchMarkRate,                                                                  
isnull(SW.Differential,0) as PositionDifferential,                                                                      
isnull(SW.OrigCostBasis,0) as PositionOrigCostBasis,                                                                          
isnull(SW.DayCount,0) as PositionDayCount,                                             
SW.SwapDescription AS PositionalSwapDescription,                                                    
SW.FirstResetDate as PositionFirstResetDate,                                                                      
SW.OrigTransDate as PositionOrigTransDate,                                                             
isnull(SW1.NotionalValue*((PTC.ClosedQty)/G1.CumQty),0) as NotionalValue,                                                                      
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
PTC.PositionSide,                
PTC.ClosingAlgo,
CASE PTC.IsCopyTradeAttrbsPrefUsed WHEN 1 THEN PT.TradeAttribute1 ELSE '' END AS TrafeAttribute1,
CASE PTC.IsCopyTradeAttrbsPrefUsed WHEN 1 THEN PT.TradeAttribute2 ELSE '' END AS TrafeAttribute2,
CASE PTC.IsCopyTradeAttrbsPrefUsed WHEN 1 THEN PT.TradeAttribute3 ELSE '' END AS TrafeAttribute3,
CASE PTC.IsCopyTradeAttrbsPrefUsed WHEN 1 THEN PT.TradeAttribute4 ELSE '' END AS TrafeAttribute4,
CASE PTC.IsCopyTradeAttrbsPrefUsed WHEN 1 THEN PT.TradeAttribute5 ELSE '' END AS TrafeAttribute5,
CASE PTC.IsCopyTradeAttrbsPrefUsed WHEN 1 THEN PT.TradeAttribute6 ELSE '' END AS TrafeAttribute6,
CASE PTC.IsCopyTradeAttrbsPrefUsed WHEN 1 THEN PT.AdditionalTradeAttributes ELSE '{}' END AS AdditionalTradeAttributes
                                                            
From PM_TaxlotClosing  PTC                                                            
Inner Join #PM_Taxlots PT on ( PTC.PositionalTaxlotID=PT.TaxlotID  and  PTC. TaxLotClosingId = PT.TaxLotClosingId_Fk)                                                       
Inner Join #PM_Taxlots PT1 on (PTC.ClosingTaxlotID=PT1.TaxlotID  and  PTC. TaxLotClosingId = PT1.TaxLotClosingId_Fk)                                                          
Inner Join T_Group G on G.GroupID = PT.GroupID                                                            
Inner Join T_Group G1 on G1.GroupID = PT1.GroupID                                                           
Inner Join T_AUEC AUEC on AUEC.AUECID=G.AUECID                                          
--LEFT OUTER JOIN #SecurityMasterTemp SM ON PT.Symbol = SM.TickerSymbol                                                                
INNER JOIN #SecurityMasterTemp SM ON PT.Symbol = SM.TickerSymbol                                                                  
Inner Join @ToAUECDatesTable ToAUECDatesTable on ToAUECDatesTable.AUECID=AUEC.AUECID                                                            
Inner Join @FromAUECDatesTable FromAUECDatesTable on FromAUECDatesTable.AUECID=AUEC.AUECID                                                            
Left Outer Join T_SwapParameters SW on SW.GroupID=G.GroupID                                                            
Left Outer Join T_SwapParameters SW1 on SW1.GroupID=G1.GroupID            
Inner Join #Funds funds on PT.FundId=funds.FundId                                                          
Where
--Datediff(d,FromAUECDatesTable.CurrentAUECDate,PTC.AUECLocalDate) >= 0                                                            
 --And Datediff(d,PTC.AUECLocalDate,ToAUECDatesTable.CurrentAUECDate) >= 0        
 --And 
 PT.Symbol = @Local_Symbol And G1.GroupID  = @Local_GroupID                                
              
Drop Table #SecurityMasterTemp,#PM_Taxlots,#Funds                              
        
End     

