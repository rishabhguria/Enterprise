        
CREATE function [dbo].[F_GetPositions_New]                                                                                              
(                                                                                                                                                                        
 @ToAllAUECDatesString VARCHAR(MAX),        
 @AssetIds VARCHAR(MAX),        
 @FundIds VARCHAR(MAX)                                                                                                                                                            
)                                                                 
                                                            
RETURNS @PositionTable Table                                                                                      
 (                                                                                                                                              
  TaxLotID  varchar(50),                                                                                                                                                                                                      
  AUECLocalDate datetime,                                                                                                               
  SideID char(1)  ,                                                                                                       
  Symbol varchar(200),                                                                                                                                                                                  
  OpenQuantity float,                                                                                                                                                                                   
  AveragePrice float,                                                                                                                                                                                                                             
  FundID int,                                                                                                                                
  AssetID int,                                                                                                                 
  UnderLyingID int,                                                                                                                              
  ExchangeID int,                                                                                                                                
  CurrencyID int,                                                                                                                                                                                                       
  AUECID int,                                                                                                                                                                                                                  
  TotalCommissionandFees float,                                                                                          
  Multiplier float,                                                                                                                                                                                                                                      
  SettlementDate datetime,                                  
LeadCurrencyID int,                                                                                                                                                          
  VsCurrencyID int,                          
  ExpirationDate datetime,                                                                                                                            
  Description varchar(max),                  
  Level2ID int,            
  NotionalValue float,                                                                    
  BenchMarkRate float,                                        
  Differential float,                                                              
  OrigCostBasis float,                                                                        
  DayCount int,                                           
  SwapDescription varchar(max),                                                                       
  FirstResetDate datetime,                                                            
  OrigTransDate datetime,                                                                                            
  IsSwapped bit,                                                                                      
  AllocationDate DateTime,                                                                                              
  GroupID Varchar(50),                                                                              
  PositionTag int,                                                                       
  FXRate float,                                                    
  FXConversionMethodOperator varchar(5),                                                                      
  CompanyName varchar(500),                                                                      
  UnderlyingSymbol varchar(50),                                                    
  Delta float,                                                  
  PutOrCall Varchar(5),                                                
  IsGrPreAllocated bit,                                                
  GrCumQty float,                                                
  GrAllocatedQty float,                                                
  GrQuantity float ,                                      
 Taxlot_Pk  bigint,                                               
  ParentRow_Pk  bigint ,                        
  StrikePrice float,                      
UserID int,                      
CounterPartyID int,                  
CorpActionID uniqueidentifier,        
Coupon float ,            
  IssueDate datetime,            
  MaturityDate datetime,            
  FirstCouponDate datetime,            
  CouponFrequencyID int,            
  AccrualBasisID int,            
  BondTypeID int,            
  IsZero bit,        
  ProcessDate datetime,        
  OriginalPurchaseDate datetime,        
IsNDF bit,      
FixingDate Datetime ,     
IDCOSymbol varchar(50),    
OSISymbol varchar(50),    
SEDOLSymbol varchar(50),    
CUSIPSymbol varchar(50)    
                                     
 )                                                              
                                                                                                                                        
As                                                                                                                                                     
Begin                                                                                                                                                            
                                                                                                                                         
 Declare @AUECDatesTable Table(AUECID int,CurrentAUECDate DateTime)                                                                                                                                                        
                                                                                                                                         
 Insert Into @AUECDatesTable Select * From dbo.GetAllAUECDatesFromString(@ToAllAUECDatesString)              
      
 Declare @AssetClass Table(AssetID int)            
 if (@AssetIds is NULL or @AssetIds = '')            
 Insert into @AssetClass            
 Select AssetID from T_Asset            
 else             
 Insert into @AssetClass            
 Select Items as AssetID from dbo.Split(@AssetIds,',')            
            
 Declare @Funds Table(FundID int)            
 if (@FundIds is NULL or @FundIds = '')            
 Insert into @Funds            
 Select CompanyFundID as FundID from T_CompanyFunds Where IsActive=1           
 else            
 Insert into @Funds            
 Select Items as FundID from dbo.Split(@FundIds,',')                                                                                                                                                   
                                                                                                                                         
 -- Get UTC Date. UTC Date is stored corresponding to AUECID 0 in @AUECDatesTable                                                                                                                                                        
INSERT @PositionTable                                                                                                                                                    
 Select                                                                                                   
 PT.TaxLotID as TaxLotID,                                                                                                                                                                                              
 G.AUECLocalDate as TradeDate,                                    
 PT.OrderSideTagValue as SideID, -- WE INSERT 0 FOR BUY SIDE AND 1 FOR SELL SHORT IN PM_NETPOSITIONS                                                                                          
 PT.Symbol as Symbol ,                                                                                                
 PT.TaxLotOpenQty as TaxLotOpenQty ,                                                                                             
 PT.AvgPrice as AvgPrice ,                                                                                     
 PT.FundID as FundID,                                                                                                             
 G.AssetID as AssetID,                                                         
 G.UnderLyingID as UnderLyingID,                                                                                                                      
 G.ExchangeID as ExchangeID,                                                                     
 G.CurrencyID as CurrencyID,                                                                                                                                            
 G.AUECID as AUECID ,                                                                                              
 PT.OpenTotalCommissionandFees,--this is open commission and closed commission sum is not necessarily equals to total commission                                                                    
 isnull(V_SecMasterData.Multiplier,1) as Multiplier,                                                                                                                                                
 G.SettlementDate as SettlementDate,                                                                                                                                        
 V_SecMasterData.LeadCurrencyID,                            
 V_SecMasterData.VsCurrencyID,                          
 isnull(V_SecMasterData.ExpirationDate,'1/1/1800') as ExpirationDate,                                                          
 G.Description as Description,                                                                                                             
 PT.Level2ID as Level2ID,                                                                                                
 isnull( (PT.TaxLotOpenQty * SW.NotionalValue / G.Quantity) ,0) as NotionalValue,                  
 isnull(SW.BenchMarkRate,0) as BenchMarkRate,                                                                           
 isnull(SW.Differential,0) as Differential,                                                              
 isnull(SW.OrigCostBasis,0) as OrigCostBasis,                                                                         
 isnull(SW.DayCount,0) as DayCount,                                                                                                            
 isnull(SW.SwapDescription,'') as SwapDescription,                                                                                                            
 SW.FirstResetDate as FirstResetDate,                                                                                   
 SW.OrigTransDate as OrigTransDate,                                                                                        
 G.IsSwapped as IsSwapped,                                                                                     
 G.AllocationDate as AUECLocalDate,                                                                                    
 G.GroupID,                                                                                  
 PT.PositionTag,                                                                            
 G.FXRate,                                                                            
 G.FXConversionMethodOperator,                                                                       
 isnull(V_SecMasterData.CompanyName,'') as CompanyName,           
 isnull(V_SecMasterData.UnderlyingSymbol,'') as UnderlyingSymbol,                                                    
 IsNull(V_SecMasterData.Delta,1) as Delta,                                                  
 IsNull(V_SecMasterData.PutOrCall,'') as PutOrCall,                                
 G.IsPreAllocated,                                                
 G.CumQty,                                                
 G.AllocatedQty,                              
 G.Quantity,                                      
 PT.taxlot_Pk,                                      
 PT.ParentRow_Pk ,                        
 IsNull(V_SecMasterData.StrikePrice,0) as StrikePrice,                      
 G.UserID,                    
 G.CounterPartyID,                  
 CATaxlots.CorpActionID ,       
 V_SecMasterData.Coupon,                  
V_SecMasterData.IssueDate,                  
V_SecMasterData.MaturityDate,                  
V_SecMasterData.FirstCouponDate,                  
V_SecMasterData.CouponFrequencyID,                  
V_SecMasterData.AccrualBasisID,                  
V_SecMasterData.BondTypeID,                  
V_SecMasterData.IsZero,            
G.ProcessDate,              
G.OriginalPurchaseDate,        
V_SecMasterData.IsNDF,        
V_SecMasterData.FixingDate ,    
V_SecMasterData.IDCOSymbol ,    
V_SecMasterData.OSISymbol,     
V_SecMasterData.SEDOLSymbol,     
V_SecMasterData.CUSIPSymbol          
                                                                   
 from PM_Taxlots PT                                         
 Inner join  T_Group G on G.GroupID=PT.GroupID                                               
 Left outer  join  T_SwapParameters SW on G.GroupID=SW.GroupID                                              
 Left outer  join V_SecMasterData ON PT.Symbol = V_SecMasterData.TickerSymbol                     
 Left outer  join PM_CorpActionTaxlots CATaxlots on  PT.Taxlot_PK = CATaxlots.FKId              
  Inner join @AssetClass as AssetClass on G.AssetID = AssetClass.AssetID             
 Inner join @Funds as funds on PT.FundID = funds.FundID                         
 Where taxlot_PK in                                         
 (                                                                                        
  Select max(taxlot_PK) from PM_Taxlots                                                                                                             
  Inner join  T_Group G on G.GroupID=PM_Taxlots.GroupID                                                                                
  inner join T_AUEC AUEC on AUEC.AUECID = G.AUECID                                                                                          
  inner join @AUECDatesTable AUECDates on AUEC.AUECID = AUECDates.AUECID                     
  where Datediff(d,PM_Taxlots.AUECModifiedDate,AUECDates.CurrentAUECDate) >= 0                                  
  group by taxlotid                                                                 
 )                                             
 and TaxLotOpenQty<>0  order by taxlotid                                                                    
RETURN;                                                                          
End 