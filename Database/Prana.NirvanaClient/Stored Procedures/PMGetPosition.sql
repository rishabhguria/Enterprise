
--as it is Fill method is same as of UDF F_GetPositions so order needs to be same with that.        
--Abhishek Mehta        
CREATE Proc PMGetPosition                                                                        
(                                                                                                                                                  
@parentRowPk bigint                                                                                                                           
)                                           
                                                                                                                  
As                                                                                                                               
Begin                                                                                                                                      
declare @TaxlotID  varchar(50)        
set @TaxlotID = (select taxlotID from   PM_taxlots where   taxlot_pk= @parentRowPk)                                                                                                                   
       
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
null,        
null,                                                                                                                  
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
 G.AUECLocalDate as AUECLocalDate,                                                              
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
 PT.ParentRow_Pk,    
 V_SecMasterData.StrikePrice                                           
 from PM_Taxlots PT                                                                                           
 Inner join  T_Group G on G.GroupID=PT.GroupID                                                                                      
 Left outer  join  T_SwapParameters SW on G.GroupID=SW.GroupID                                                                                                           
 LEFT OUTER JOIN V_SecMasterData ON PT.Symbol = V_SecMasterData.TickerSymbol                                            
 Where                                             
 taxlot_PK in (select max(taxlot_pk) from PM_taxlots where taxlotid = @TaxlotID group by TaxlotID)                                                                        
                                                                         
End 

