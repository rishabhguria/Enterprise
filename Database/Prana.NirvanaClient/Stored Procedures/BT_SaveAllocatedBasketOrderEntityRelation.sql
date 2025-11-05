CREATE procedure BT_SaveAllocatedBasketOrderEntityRelation
(
@basketGroupID varchar(50),
@EntityID varchar(50)
)
as

insert into BT_AllocatedBasket_OrderEntityRelation(BasketGroupID,EntityID)
 values(@basketGroupID,@EntityID)
