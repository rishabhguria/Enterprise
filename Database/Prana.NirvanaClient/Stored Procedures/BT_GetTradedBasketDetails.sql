


-- =============================================
-- Author:		<Vinod Nayal>
-- Create date: <30/10/2006>
-- Description:	Get Traded Basket Details
-- =============================================

CREATE procedure [dbo].[BT_GetTradedBasketDetails] (
@tradedBasketID varchar(50)
)
as
select UB.UpLoadedBasketName,SB.TemplateID,SB.AssetID,SB.UnderLyingID,
TB.UserID,UB.BasketID,serverReceiveTime,UB.UpLOadedBasketID,TB.TradingAccountID
from T_BTUploadedBaskets as UB 
join T_BTTradedBasket as TB on UB.BasketID=TB.BasketID
join T_BTSavedBaskets as SB on SB.SavedBasketID=UB.UpLoadedBasketID
where TB.TradedBasketID=@tradedBasketID

--select Sum(Quantity) from V_TradedOrders  group by ListID having ListID=@tradedBasketID