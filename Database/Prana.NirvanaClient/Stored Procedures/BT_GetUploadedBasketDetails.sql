
CREATE procedure [dbo].[BT_GetUploadedBasketDetails]

(
@basketID varchar(50)
)
as

select UB.UpLoadedBasketID,UB.UploadedBasketName,UB.TemplateID,UB.AssetID,UB.UnderLyingID,
UB.UserID,TL.Columns,UB.BenchMark,UB.HasWaves,isnull(TB.TradedBasketID ,'')  as TradedBasketID,TB.TradingAccountID
from T_BTUploadedBaskets  as UB 
join T_BTTemplateList as TL on TL.TemplateID=UB.TemplateID
left join T_BTTradedBasket as TB on TB.BasketID=UB.BasketID
where UB.BasketID=@basketID