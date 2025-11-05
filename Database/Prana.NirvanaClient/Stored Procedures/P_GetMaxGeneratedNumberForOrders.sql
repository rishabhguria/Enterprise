

 --Author	: Gaurav
 --Date		: 07 dec 12
 --Description: Picks up the max id from T_Order. This id is further used to generate the new distinct ids.
CREATE procedure [dbo].[P_GetMaxGeneratedNumberForOrders]  
as  
select max(cast(ParentClOrderID as numeric )) from T_Order