CREATE proc P_GetSavedBaskets
as
Select SavedBasketID,SavedBasketName 
from T_BTSavedBaskets Order By SavedBasketName