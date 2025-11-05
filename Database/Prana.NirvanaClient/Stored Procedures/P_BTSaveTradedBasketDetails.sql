






CREATE procedure [dbo].[P_BTSaveTradedBasketDetails]
(
@basketID varchar(50),
@TradedbasketID varchar(50),
@userID int ,
@serverReceiveTime datetime,
@tradingAccID int

)
as

if(( select count(*) from T_BTTradedBasket where TradedbasketID=@TradedbasketID)=0)
insert into  T_BTTradedBasket(basketID,TradedbasketID,userID,serverReceiveTime,TradingAccountID) 
values(@basketID,@TradedbasketID,@userID,
@serverReceiveTime,@tradingAccID)







