




CREATE procedure [dbo].[BT_GetUploadedBasketIDS]
(
  @userID int,
  @date datetime
)

as
select UB.BasketID,UB.UploadedBasketName,UB.TimeOfSave ,null as ServerReceiveTime,UB.UserID,CU.ShortName
from T_BTUpLoadedBaskets as UB 
join T_CompanyUser as CU on CU.UserID=UB.UserID
where BasketID not in (select BasketID from T_BTTradedBasket )
and DATEDIFF(d,TimeOfSave,@date) = 0
union 
select TB.BasketID,UB.UploadedBasketName,UB.TimeOfSave,TB.ServerReceiveTime,TB.UserID,CU.ShortName
from T_BTTradedBasket as TB join T_BTUpLoadedBaskets as UB on UB.BasketID=TB.BasketID
join T_CompanyUserTradingAccounts as CTA on TB.TradingAccountID=CTA.TradingAccountID 
join T_CompanyUser as CU on CU.UserID=TB.UserID
where CTA.CompanyUserID=@userID   
and DATEDIFF(d,TimeOfSave,@date) = 0




