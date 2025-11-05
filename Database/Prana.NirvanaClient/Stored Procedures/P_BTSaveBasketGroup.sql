

-- =============================================  
-- Author:  <Vinod Nayal>  
-- Create date: <30/10/2006>  
-- Description: Saves Basket Group  
-- =============================================  
CREATE Procedure [dbo].[P_BTSaveBasketGroup] (  
@groupID varchar(50),  
@addedDate datetime,  
@allocationType int,  
@userID int ,  
@AssetID int ,  
@UnderLyingID int,  
@AllocatedQty float,  
@ExeQty float,  
@TotalQty float,  
@AUECID int,  
@ListID varchar(50),  
@AllocationDate datetime,  
@StateID int,  
@tradingAccountID int,
@AUECLocalDate datetime   
)  
as  
  
insert into BT_BasketGroups(GroupID,AddedDate,AllocationType,UserID,AssetID,UnderLyingID,AllocatedQty,ExeQty,TotalQty,AUECID,ListID,AllocationDate,StateID,TradingAccountID,AUECLocalDate)   
values(@groupID,@addedDate,@allocationType,@userID,@AssetID,@UnderLyingID,@AllocatedQty,@ExeQty,@TotalQty,@AUECID,@ListID,@AllocationDate,@StateID,@tradingAccountID,@AUECLocalDate)
