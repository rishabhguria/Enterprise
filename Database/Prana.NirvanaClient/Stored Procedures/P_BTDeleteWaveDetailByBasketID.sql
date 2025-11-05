create Procedure P_BTDeleteWaveDetailByBasketID
(

@basketID varchar(50)

)
as

delete from T_BTwave where basketID=@basketID