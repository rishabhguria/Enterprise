                
CREATE Procedure [dbo].[P_GetTransactions_DailyCalc]                                                                                            
(                                                                                                                                                                      
 @ToAllAUECDatesString VARCHAR(MAX)                                                                                                                                                  
)                                                               
As                                                                                                                                                   
Begin                                                                                                                                                          
                                                                                                                                       
 Declare @AUECDatesTable Table(AUECID int,CurrentAUECDate DateTime)                                                                                                                                                      
                                                                                                                                       
 Insert Into @AUECDatesTable Select * From dbo.GetAllAUECDatesFromString(@ToAllAUECDatesString)                                                                                                                                                      
                                                                                                                                       
 -- Get UTC Date. UTC Date is stored corresponding to AUECID 0 in @AUECDatesTable                                                                                                                                                      
 Select                                                                                                 
 PT.TaxLotID as TaxLotID,                                                                                                                                                                                            
 G.AUECLocalDate as TradeDate,    
 G.OriginalPurchaseDate,    
 G.ProcessDate,                                                                                                     
 PT.OrderSideTagValue as SideID, -- WE INSERT 0 FOR BUY SIDE AND 1 FOR SELL SHORT IN PM_NETPOSITIONS    
        
 PT.Symbol as Symbol ,                                                                                              
 PT.TaxLotOpenQty as OpenQuantity ,                                                                                                
 PT.AvgPrice as AveragePrice ,                                                                                   
 PT.FundID as FundID,                                                                                                           
 G.AssetID as AssetID,                                                       
 G.UnderLyingID as UnderLyingID,                                                                                                                    
 G.ExchangeID as ExchangeID,                                                                   
 G.CurrencyID as CurrencyID,                                                                                                                                          
 G.AUECID as AUECID ,                                                                                            
 PT.OpenTotalCommissionandFees as TotalCommissionandFees,--this is open commission and closed commission sum is not necessarily equals to total commission                 
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
 CATaxlots.CorpActionID,      
 V_SecMasterData.Coupon,      
V_SecMasterData.IssueDate,      
V_SecMasterData.MaturityDate,      
V_SecMasterData.FirstCouponDate,      
V_SecMasterData.CouponFrequencyID,      
V_SecMasterData.AccrualBasisID,      
V_SecMasterData.BondTypeID,      
V_SecMasterData.IsZero                                                                
 from PM_Taxlots PT                                                                                                               
 Inner join  T_Group G on G.GroupID=PT.GroupID                                             
 Left outer  join  T_SwapParameters SW on G.GroupID=SW.GroupID                                            
 Left Outer Join V_SecMasterData ON PT.Symbol = V_SecMasterData.TickerSymbol                                              
 Left Outer Join PM_CorpActionTaxlots CATaxlots on  PT.Taxlot_PK = CATaxlots.FKId                              
 Where taxlot_PK in                                
 (                                                                            
  Select max(taxlot_PK) from PM_Taxlots                                                                                                 
  Inner join  T_Group G on G.GroupID=PM_Taxlots.GroupID                                                                    
  inner join T_AUEC AUEC on AUEC.AUECID = G.AUECID                                                                              
  inner join @AUECDatesTable AUECDates on AUEC.AUECID = AUECDates.AUECID                                                                  
  where Datediff(d,PM_Taxlots.AUECModifiedDate,AUECDates.CurrentAUECDate) = 0                      
  group by taxlotid                                                     
 )                                                                             
 and TaxLotOpenQty<>0 AND G.AssetID in ('3','8') --3=Future,8=FixedIncome                                                           
RETURN;                                                              
End 