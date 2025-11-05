

    
                      
CREATE Procedure [dbo].[P_GetTransactions_All]                                                                                                  
(         
 @FromDate datetime,    
@ToDate datetime,  
@ACAFundsxml ntext                                                                                                                                                      
)                                                                     
As                                                                                                                                                          
Begin        
  
  
  
DECLARE @handle int      
  
exec sp_xml_preparedocument @handle OUTPUT, @ACAFundsxml  
create table #Temp_ACAFunds                                                                                                                                                  
(                                                                                                                                                  
  FundID varchar(max)                                                                                                                                        
)     
    
  
    
insert into #Temp_ACAFunds  
  
select text      
   
FROM  OPENXML(@handle, '//ArrayOfString//string',2)             
  
where text is not null                                                                                                                                                             
    
    
  Select                                                                                                       
 VT.TaxLotID as TaxLotID,                                                                                                                                                                                                  
 G.AUECLocalDate as TradeDate,          
 G.OriginalPurchaseDate,          
 G.ProcessDate,                                                                                                           
 VT.OrderSideTagValue as SideID, -- WE INSERT 0 FOR BUY SIDE AND 1 FOR SELL SHORT IN PM_NETPOSITIONS                                                                                                                                                           
 VT.Symbol as Symbol ,                                                                                                    
 VT.TaxLotQty as OpenQuantity ,                                                                                                      
 VT.AvgPrice as AveragePrice ,                                                                                         
 VT.FundID as FundID,                                                                                                                 
 G.AssetID as AssetID,                                                             
 G.UnderLyingID as UnderLyingID,                                                                                                                          
 G.ExchangeID as ExchangeID,                                                                         
 G.CurrencyID as CurrencyID,                                                                                                                                                
 G.AUECID as AUECID ,                                                                                                  
 VT.TotalExpenses as TotalCommissionandFees,--this is open commission and closed commission sum is not necessarily equals to total commission                       
 isnull(V_SecMasterData.Multiplier,1) as Multiplier,                                           
 G.SettlementDate as SettlementDate,                                            
 V_SecMasterData.LeadCurrencyID,                               
 V_SecMasterData.VsCurrencyID,                           
 isnull(V_SecMasterData.ExpirationDate,'1/1/1800') as ExpirationDate,                                                 
 G.Description as Description,                                                                                                                 
 VT.Level2ID as Level2ID,                                                                                                    
 isnull( (VT.TaxLotQty * SW.NotionalValue / G.Quantity) ,0) as NotionalValue,                                                                                                                
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
 IsNull(V_SecMasterData.StrikePrice,0) as StrikePrice,                          
 G.UserID,                          
 G.CounterPartyID,                                
 V_SecMasterData.Coupon,            
V_SecMasterData.IssueDate,            
V_SecMasterData.MaturityDate,            
V_SecMasterData.FirstCouponDate,            
V_SecMasterData.CouponFrequencyID,            
V_SecMasterData.AccrualBasisID,            
V_SecMasterData.BondTypeID,            
V_SecMasterData.IsZero                                                                      
 from V_Taxlots VT                                                                                                                     
 Inner join  T_Group G on G.GroupID=VT.GroupID                                                   
 Left outer  join  T_SwapParameters SW on G.GroupID=SW.GroupID                                                
 Left Outer Join V_SecMasterData ON VT.Symbol = V_SecMasterData.TickerSymbol                                                    
  
                       
Where VT.FundID in                                      
 (                                                                                  
   select Convert(int,FundID) as FundID from #Temp_ACAFunds
 )                                                                                         
and VT.taxlotId not in (select distinct TaxlotId from PM_taxlots where PositionTag in (3,5))

 and Datediff(d,VT.AUECLocalDate,@FromDate) <= 0 

  and Datediff (d,VT.AUECLocalDate,@ToDate) >=0  

                                                                          
       exec sp_xml_removedocument @handle                                                             
                                                         
RETURN;                                                                        
End     

