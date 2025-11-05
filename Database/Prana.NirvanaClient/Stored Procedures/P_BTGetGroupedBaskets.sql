

-- =============================================      
-- Author:  <Vinod Nayal>      
-- Create date: <23/10/2006>      
-- Description: Retrieves Grouped BasketIDS      
-- =============================================      
CREATE procedure [dbo].[P_BTGetGroupedBaskets] (      
@groupId varchar(50),      
@allotionType int,    
@AllAUECDatesString  varchar(8000)      
)      
as      
select basketID from BT_GroupsBaskets  as GB      
join BT_BasketGroups as BG on GB.GroupID=BG.GroupID 
inner join T_AUEC AUEC on AUEC.AUECID = BG.AUECID    
inner join GetAllAUECDatesFromString(@AllAUECDatesString) As AUECDates on AUECDates.AUECID = BG.AUECID           
where GB.GroupID=@groupID and BG.AllocationType=@allotionType     
 and dbo.GetFormattedDatePart(BG.AUECLocalDate) <= dbo.GetFormattedDatePart(AUECDates.CurrentAUECDate)  
order by GB.GroupID
