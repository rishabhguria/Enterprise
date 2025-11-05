

CREATE procedure [dbo].[P_BTGetBasketGroups] (                            
@AllAUECDatesString varchar(8000),                            
@allocationType int                            
)                            
                            
as                            
                            
select distinct BG.GroupID,BG.ListID,BG.TradingAccountID,BG.AllocatedQty,BG.ExeQty,BG.TotalQty,BG.AUECID ,BG.UserID,BG.AssetID,                            
BG.UnderLyingId,BG.AllocationType,BG.StateID,G.CounterpartyId  ,G.VenueId, BG.AUECLocaldate                         
from BT_BasketGroups As BG left outer join  T_Group G on  BG.GroupId =   G.basketGroupid                        
inner join T_AUEC AUEC on AUEC.AUECID = BG.AUECID        
inner join GetAllAUECDatesFromString(@AllAUECDatesString) As AUECDates on AUECDates.AUECID = BG.AUECID                                                          
where (G.CumQty>0 and G.StateID=2     
and DATEDIFF(d,G.AUECLocalDate,AUECDates.CurrentAUECDate)=0 and AllocationType=@allocationType and BG.AssetID >=0 )             
or (BG.AssetID >=0 and AllocationType=@allocationType and BG.StateID=1 and     
(datediff(dd,BG.AUECLocalDate,AUECDates.CurrentAUECDate)>=0))  
-- or (G.CumQty>0 and G.StateID=2   
--and DATEDIFF(d,G.AUECLocalDate,AUECDates.CurrentAUECDate)>=0 and AllocationType=@allocationType)
