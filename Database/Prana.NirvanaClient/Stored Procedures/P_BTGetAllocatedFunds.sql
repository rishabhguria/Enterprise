  
CREATE Procedure [dbo].[P_BTGetAllocatedFunds] (    
 @AllAUECDatesString varchar(8000)    
    
)    
as    
    
select     
 Distinct     
 FA.GroupID,    
 FA.FundID,    
 FA.AllocatedQty,    
 FA.Percentage,        
 FA.AllocationID,
BTC.Commission,  
 BTC.Fees    
 from     
 T_BTFundAllocation  as FA    
join BT_BasketGroups as G on FA.GroupID=G.GroupID   
left outer join   
(Select Sum(FAC.Commission) as commission ,Sum(Fees) as fees , G.basketgroupID as ID ,FAL.FundID As FID from    
T_FundAllocationCommission FAC left outer join T_FundAllocation FAL   
on FAC.AllocationID_Fk= FAL.AllocationID  
left outer join T_Group G on G.GroupID = FAL.GroupID 
group by G.BasketGRoupID,FAL.FundID )   
 as BTC on BTC.ID = G.GroupID and BTC.FID = FA.FundID 
 inner join T_AUEC AUEC on AUEC.AUECID = G.AUECID    
inner join GetAllAUECDatesFromString(@AllAUECDatesString) As AUECDates on AUECDates.AUECID = G.AUECID                                                      
    
where     
 DATEDIFF(d,G.AddedDate,AUECDates.CurrentAUECDate) = 0  