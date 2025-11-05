



CREATE  procedure [dbo].[P_BTGetTradedBasketIDS]

(

@upLoadedbasketID varchar(50),

@userID int,

@date Datetime

 

)

as

if(@upLoadedbasketID !='')

begin

      select distinct TB.BasketID,TB.TradedBasketID,serverReceiveTime ,UploadedBasketName
      from T_BTTradedBasket  as TB 
      join T_BTUploadedBaskets as UB on TB.BasketID=UB.BasketID 
	  join T_CompanyUserTradingAccounts as CTA on CTA.TradingAccountID = TB.TradingAccountID
      where UB.UpLoadedBasketID=@upLoadedbasketID      
      and CTA.CompanyUserID=@userID

      and  
datediff(day,@date, serverReceiveTime )  = 0

end

else

begin

      select distinct TB.BasketID,TB.TradedBasketID,serverReceiveTime ,UploadedBasketName

      from T_BTTradedBasket  as TB 

      join T_BTUploadedBaskets as UB on TB.BasketID=UB.BasketID 
      join T_CompanyUserTradingAccounts as CTA on CTA.TradingAccountID = TB.TradingAccountID
      where     CTA.CompanyUserID=@userID    and  
datediff(day,@date, serverReceiveTime )  =0
end




