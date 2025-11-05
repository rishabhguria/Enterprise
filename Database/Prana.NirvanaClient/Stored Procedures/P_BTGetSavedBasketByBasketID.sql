CREATE proc [dbo].[P_BTGetSavedBasketByBasketID]
(
@savedBasketID varchar(200)
)
as 
select SavedBasketID,SavedBasketName,TemplateID,AssetID,UnderLyingID,UserID,BenchMark from
T_BTSavedBaskets where SavedBasketID = @savedBasketID 






