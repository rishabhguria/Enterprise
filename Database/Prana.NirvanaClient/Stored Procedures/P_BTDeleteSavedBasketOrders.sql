CREATE proc [P_BTDeleteSavedBasketOrders]
(
@savedBasketID varchar(200)
)
AS
Declare @total int 
Select @total = Count(*)
From T_BTSavedBasketOrders
where SavedBasketID = @savedBasketID

if (@total > 0)
begin
Delete from T_BTSavedBasketOrders
where SavedBasketID = @savedBasketID
end