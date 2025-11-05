

-- =============================================            
-- Author:  <Vinod Nayal>            
-- Create date: <23/10/2006>            
-- Description: Returns all Traded Basket Details           
--modified by Abhishek As TradedBasketID is not casting in int      
-- Usage: EXEC BT_GetUnallocatedBasketDetails 15,'3-MARCH-2008 05:00:01 AM',0,1  
-- =============================================            
CREATE procedure [dbo].[BT_GetUnallocatedBasketDetails] (            
@userID int,            
@date Datetime,            
@lastID varchar(50),            
@allocationType int             
)            
as             
select TradedBasketID            
from T_BTTradedBasket as TB            
left join            
(            
select distinct BasketID,BG.GroupID,AllocationType from BT_GroupsBaskets as GB             
join BT_BasketGroups as BG on GB.GroupID=BG.GroupID            
where  AllocationType=@allocationType And dbo.GetFormattedDatePart(BG.AUECLocaldate) <= dbo.GetFormattedDatePart(@date)           
)            
as Temp on TB.TradedBasketID=Temp.BasketID            
where Temp.GroupID is null         
and CAST(TB.TradedBasketID as bigint)>CAST(@lastID as bigint)            
and dbo.GetFormattedDatePart(serverReceiveTime) <= dbo.GetFormattedDatePart(@date)  
order by TradedBasketID
