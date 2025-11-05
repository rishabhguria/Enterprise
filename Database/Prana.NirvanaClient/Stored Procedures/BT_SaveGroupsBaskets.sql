-- =============================================
-- Author:		<Vinod Nayal>
-- Create date: <30/10/2006>
-- Description:	 Saves Groups and Basket details
-- =============================================

create procedure BT_SaveGroupsBaskets
(
@groupID varchar(50),
@basketID varchar(50)
)
as
insert into BT_GroupsBaskets (GroupID,BasketID) values(@groupID,@basketID)