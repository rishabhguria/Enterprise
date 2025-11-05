
CREATE procedure P_GetSymbolListCollectionByUserID
(@userID int )
as
select SymbolListID,symbolListName,symbolListCollection
from  T_SymbolList 
where 
userID=@userID

