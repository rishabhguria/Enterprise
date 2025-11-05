create procedure BT_GetMaxTradedBasketID
as
select max(TradedBasketID) from T_BTTradedBasket