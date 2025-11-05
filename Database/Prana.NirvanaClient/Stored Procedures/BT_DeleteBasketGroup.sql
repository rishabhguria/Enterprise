



-- =============================================
-- Author:		<Vinod Nayal>
-- Create date: <30/10/2006>
-- Description:	Delete Basket Group
-- =============================================
CREATE Procedure [dbo].[BT_DeleteBasketGroup]
(
@groupID varchar(50)

)
as
delete from BT_GroupsBaskets where GroupID=@groupID
delete from BT_BasketGroups where GroupID=@groupID
delete from T_BTFundAllocation where GroupID=@groupID
delete from T_BTStrategyAllocation where GroupID=@groupID




