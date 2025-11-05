


CREATE procedure [dbo].[P_BTGetAllUploadedBasketIDs]
(
@date datetime,
@userID int
)
as
select BasketID from T_BTTradedBasket as TB
join T_CompanyUserTradingAccounts as CTA on CTA.TradingAccountID = TB.TradingAccountID
where     CTA.CompanyUserID=@userID
and datediff(dd,serverReceiveTime,@date) =0
