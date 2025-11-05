CREATE PROCEDURE [dbo].[PMGetFundOpenPositionsForDailyCalc]                                              
(                                                                                                                    
 @ToAllAUECDatesString VARCHAR(MAX)                                              
 --@symbol varchar(50)                                                            
)                                                                                                                    
As                                                                                                                        
Begin                                                                                                        
                                                                                     
 Create Table #PositionTable        
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
  FactSetSymbol VARCHAR(100),
  ActivSymbol VARCHAR(100),
  BloombergSymbolWithExchangeCode VARCHAR(200),
  AdditionalTradeAttributes VARCHAR(MAX)
 )         
                                                                                                
Insert Into #PositionTable EXEC P_GetPositions @ToAllAUECDatesString           
                                  
--3=Future,8=FixedIncome
                                                                                                    
Select * from #PositionTable where #PositionTable.AssetID in ('3','8')
End 