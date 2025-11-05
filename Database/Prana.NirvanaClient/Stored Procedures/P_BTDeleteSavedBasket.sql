CREATE proc [dbo].[P_BTDeleteSavedBasket]
( @savedBasketID varchar(200)   )
AS

Delete  from T_BTSavedBaskets where SavedBasketID = @savedBasketID


