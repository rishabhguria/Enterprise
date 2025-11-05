CREATE proc P_BTRenamePortfolioBasket
(
@savedBasketID varchar(200),
@savedBasketName varchar(50)
)
as 
update T_BTSavedBaskets
set
SavedBasketName = @savedBasketName
where
SavedBasketID = @savedBasketID
